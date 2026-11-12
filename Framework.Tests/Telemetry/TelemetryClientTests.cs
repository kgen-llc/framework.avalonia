using System.Text.Json;
using KGen.Framework.Telemetry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Tests.Telemetry;

[TestClass]
public sealed class TelemetryClientTests
{
    [TestMethod]
    public void CreatePageViewPayloadSerializesExpectedFieldsAndValues()
    {
        var payload = TelemetryClient.CreatePageViewPayload("My Product", "Campaign Page");
        var json = JsonSerializer.Serialize(payload, TelemetryJsonContext.Default.TelemetryPageViewPayload);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        var fieldNames = new[] { "name", "url", "domain" };

        CollectionAssert.AreEqual(
            fieldNames,
            root.EnumerateObject().Select(property => property.Name).ToArray(),
            StringComparer.Ordinal);
        Assert.AreEqual("pageview", root.GetProperty("name").GetString(), StringComparer.Ordinal);
        Assert.AreEqual("app://kgen-llc.com/My%20Product/Campaign%20Page", root.GetProperty("url").GetString(), StringComparer.Ordinal);
        Assert.AreEqual("kgen-llc.com", root.GetProperty("domain").GetString(), StringComparer.Ordinal);
    }
}
