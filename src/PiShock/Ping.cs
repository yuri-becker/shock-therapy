using Newtonsoft.Json;

namespace ShockTherapy.PiShock;

[JsonObject]
internal record Ping : IMessage
{
    public string MessageType => "PING";
}