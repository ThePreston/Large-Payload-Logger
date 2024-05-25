using System.Text.Json.Serialization;

namespace Microsoft.LargePayloadLogger.API.Models
{
    public class LargePayload
    {
        [JsonPropertyName("payload")]
        public string Payload { get; set; } = string.Empty;

        [JsonPropertyName("payloadType")]
        public string PayloadType { get; set; } = string.Empty;

        [JsonPropertyName("sessionKey")]
        public string SessionKey { get; set; } = string.Empty;

    }
}