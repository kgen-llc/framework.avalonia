using System.Text.Json.Serialization;

namespace KGen.Framework.Telemetry;

[JsonSerializable(typeof(TelemetryPageViewPayload))]
internal partial class TelemetryJsonContext : JsonSerializerContext
{
}
