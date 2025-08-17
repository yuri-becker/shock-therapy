using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BepInEx.Logging;
using Newtonsoft.Json;
using ShockTherapy.Config;

namespace ShockTherapy.PiShock;

public class Client : IDisposable
{
    private readonly ManualLogSource _logger = new(typeof(Client).FullName);

    private readonly ShockTherapyConfig _config;
    private readonly CancellationTokenSource _cancellationToken = new();
    private readonly HttpClient _httpClient = new();
    private readonly ClientWebSocket _webSocket;

    private uint? _userId;

    public Client(ShockTherapyConfig config)
    {
        Logger.Sources.Add(_logger);

        _config = config;
        _webSocket = new();
        Task.Run(async () => _userId = await FetchUserId());
        ConnectToWebSocket()
            .ContinueWith(_ => SendMessage(new Ping()));
    }

    private async Task ConnectToWebSocket()
    {
        var uri = new Uri(
            _config.PiShock.WebSocketEndpoint,
            $"/v2?Username={_config.PiShock.Username}&ApiKey={_config.PiShock.ApiKey}"
        );
        var maskedUrl = Regex.Replace(uri.ToString(), @"(&ApiKey=)([\w\d-]+)$", m => m.Groups[1] + "***", RegexOptions.None);
        _logger.LogDebug($"Connecting to WebSocket at URL {maskedUrl} .");
        await _webSocket.ConnectAsync(uri, _cancellationToken.Token);
        _logger.LogInfo($"Connected to PiShock WebSocket. State: {_webSocket.State}");

        _ = Task.Run(async () =>
        {
            while (_webSocket.State == WebSocketState.Open)
            {
                using var stream = new MemoryStream();
                try
                {
                    WebSocketReceiveResult result;
                    do
                    {
                        var buffer = WebSocket.CreateClientBuffer(1024, 8);
                        result = await _webSocket.ReceiveAsync(buffer, _cancellationToken.Token);
                        stream.Write(buffer.Array ?? [], buffer.Offset, buffer.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(stream.ToArray());
                        _logger.LogDebug($"Received message: {message}");
                    }
                    else
                    {
                        _logger.LogDebug("Received non-text message.");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogDebug($"Could not read websocket message: {e}");
                }
            }
        }, _cancellationToken.Token);
    }

    private async Task<uint> FetchUserId()
    {
        var uri = new Uri(
            _config.PiShock.AuthEndpoint,
            $"/Auth/GetUserIfAPIKeyValid?apikey={_config.PiShock.ApiKey}&username={_config.PiShock.Username}"
        );
        _logger.LogDebug($"Querying PiShock Auth endpoint.");
        var response = await _httpClient.GetAsync(uri)
            .ContinueWith(it => it.Result.EnsureSuccessStatusCode())
            .ContinueWith(it => it.Result.Content.ReadAsStringAsync())
            .Unwrap()
            .ContinueWith(it => JsonConvert.DeserializeObject<AuthResponse>(it.Result));
        _logger.LogDebug($"Get Response {response}");
        return response!.UserId;
    }

    private async Task SendMessage(IMessage message)
    {
        var serialized = JsonConvert.SerializeObject(message);
        _logger.LogDebug($"Sending message {serialized}.");
        var asBytes = Encoding.UTF8.GetBytes(serialized);
        await _webSocket.SendAsync(asBytes, WebSocketMessageType.Text, true, _cancellationToken.Token);

    }

    public async Task Shock(Mode mode, byte intensity, float duration)
    {
        if (_userId is null)
        {
            _logger.LogInfo("Redis client is not initialized yet, skipping shock.");
            return;
        }

        var location = new Publish.Location()
        {
            UserId = _userId,
            AuthType = _config.Shockers.IsShareCode ? Publish.AuthType.ShareCode : Publish.AuthType.Api, Hold = false,
            Origin = "Rhythm Doctor (ShockTherapy)",
        };

        var message = new Publish()
        {
            PublishCommands = _config.Shockers.Shockers.Select(shocker => new Publish.Command()
            {
                Target = _config.Shockers.Channel(),
                Body = new Publish.Body()
                {
                    Shocker = shocker,
                    ShockMode = mode,
                    Duration = Convert.ToUInt32(duration * 1000f),
                    Intensity = intensity,
                    Location = location
                }
            }).ToList()
        };

        await SendMessage(message);
    }

    public void Dispose()
    {
        _cancellationToken.Cancel();
        _webSocket.Dispose();
    }
}