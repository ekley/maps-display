using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class GeometryDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("coordinates")]
        public object Coordinates { get; set; }

    }
}
