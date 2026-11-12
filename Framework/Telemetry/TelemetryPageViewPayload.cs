using System.Text.Json.Serialization;

namespace KGen.Framework.Telemetry;

internal sealed record TelemetryPageViewPayload(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("domain")] string Domain);
