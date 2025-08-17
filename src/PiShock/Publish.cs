using System.Collections.Generic;
using Newtonsoft.Json;
using ShockTherapy.Utils;

namespace ShockTherapy.PiShock;

[JsonObject]
internal record Publish : IMessage
{
    // TODO: Unhappy with all these optionals here
    // TODO: Are inner types cool?

    public string MessageType => "PUBLISH";

    [Newtonsoft.Json.JsonProperty("PublishCommands")]
    public List<Command>? PublishCommands { get; set; }

    [JsonObject]
    public record Command
    {
        [Newtonsoft.Json.JsonProperty("Target")]
        public string? Target { get; set; }

        [Newtonsoft.Json.JsonProperty("Body")] public Body? Body { get; set; }
    }

    [JsonObject]
    public record Body
    {
        // Using safe defaults for things to avoid accidental intense shocks while testing.
        [Newtonsoft.Json.JsonProperty("id")] public uint? Shocker { get; set; }

        [Newtonsoft.Json.JsonProperty("m")]
        [JsonConverter(typeof(EnumJsonConverter<Mode>))]
        public Mode? ShockMode { get; set; }

        /// <summary>
        ///  0 ≤ Intensity ≤ 100
        /// </summary>
        [Newtonsoft.Json.JsonProperty("i")] public byte Intensity { get; set; } = 20;

        /// <summary>
        /// In Milliseconds
        /// </summary>
        [Newtonsoft.Json.JsonProperty("d")] public uint Duration { get; set; } = 200;


        /// <summary>
        /// Docs say "true or false, always set to true."
        /// </summary>
        [Newtonsoft.Json.JsonProperty("r")] public bool Repeating { get; } = true;

        [Newtonsoft.Json.JsonProperty("l")] public Location? Location { get; set; }
    }

    /// <summary>
    /// I actually have no idea what "l" stands for: https://docs.pishock.com/pishock/api-documentation/redis-api-documentation.html
    /// </summary>
    [JsonObject]
    public record Location
    {
        [Newtonsoft.Json.JsonProperty("u")] public uint? UserId { get; set; }

        [Newtonsoft.Json.JsonProperty("ty")]
        [JsonConverter(typeof(EnumJsonConverter<AuthType>))]
        public AuthType AuthType { get; set; }

        [Newtonsoft.Json.JsonProperty("w")] public bool Warning { get; set; }

        [Newtonsoft.Json.JsonProperty("h")] public bool Hold { get; set; }

        [Newtonsoft.Json.JsonProperty("o")] public string? Origin { get; set; }
    }


    public enum AuthType
    {
        [EnumConvert("sc")] ShareCode,
        [EnumConvert("api")] Api
    }
}