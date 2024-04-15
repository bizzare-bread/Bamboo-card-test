using System.Text.Json.Serialization;

namespace HackerNews.Models;

public class Story
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("by")]
    public string By { get; set; }
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("score")]
    public int Score { get; set; }
    
    [JsonPropertyName("time")]
    public int Time { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [JsonPropertyName("descendants")]
    public int Descendants { get; set; }
    
    [JsonPropertyName("kids")]
    public IEnumerable<int> Kids { get; set; }
    
    [JsonPropertyName("type")]
    public string Type { get; set; }
}