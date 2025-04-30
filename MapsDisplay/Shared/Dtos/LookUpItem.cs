using System.Text.Json.Serialization;

namespace Shared.Dtos
{
    public record GeometryDto(
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("coordinates")] object? Coordinates
    );
}