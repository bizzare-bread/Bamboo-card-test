using System.Text.Json.Serialization;

namespace BambooCard.Models;

public class HackerNewsStory
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [JsonPropertyName("postedBy")]
    public string PostedBy { get; set; }
    
    [JsonPropertyName("time")]
    public DateTime? Time { get; set; }
    
    [JsonPropertyName("score")]
    public int Score { get; set; }
    
    [JsonPropertyName("commentCount")]
    public int CommentCount { get; set; }
}