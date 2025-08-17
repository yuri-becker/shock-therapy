using Newtonsoft.Json;

namespace ShockTherapy.PiShock;

[JsonObject]
public record AuthResponse
{
    [Newtonsoft.Json.JsonProperty]
    public uint UserId { get; set; }
}