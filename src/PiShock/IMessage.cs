using Newtonsoft.Json;

namespace ShockTherapy.PiShock;

[JsonObject]
internal interface IMessage
{
    [Newtonsoft.Json.JsonProperty("Operation")]
    public string MessageType { get; }
}