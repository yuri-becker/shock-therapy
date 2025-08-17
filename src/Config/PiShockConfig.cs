using System;
using BepInEx.Configuration;

namespace ShockTherapy.Config;

public class PiShockConfig(ConfigFile file)
{
    private readonly ConfigEntry<string> _username = file.Bind<string>(
        ConfigSections.PiShock,
        "Username",
        "",
        "Your PiShock username (you can see this on https://pishock.com/#/account)."
    );

    private readonly ConfigEntry<string> _apiKey = file.Bind<string>(
        ConfigSections.PiShock,
        "ApiKey",
        "",
        "Your API key (also obtainable at https://pishock.com/#/account)"
    );

    private readonly ConfigEntry<string> _authEndpoint = file.Bind<string>(
        ConfigSections.PiShock,
        "AuthEndpoint",
        "https://auth.pishock.com",
        "PiShock auth endpoint without trailing slash (usually not necessary to change)."
    );

    private readonly ConfigEntry<string> _websocketEndpoint = file.Bind<string>(
        ConfigSections.PiShock,
        "WebSocketEndpoint",
        "wss://broker.pishock.com",
        "PiShock WebSocket (usually not necessary to change)."
    );
    public string Username => _username.Value;
    public string ApiKey => _apiKey.Value;
    public Uri AuthEndpoint => new(_authEndpoint.Value);
    
    public Uri WebSocketEndpoint => new(_websocketEndpoint.Value);

}