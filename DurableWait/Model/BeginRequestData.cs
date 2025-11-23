using System.Text.Json.Serialization;

namespace DurableWait.Model;

public class BeginRequestData
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
}
