using HackerNews.Models;

namespace BambooCard.Models;

public class GetBestStoriesDetailsResponse
{
    public List<HackerNewsStory> BestStoriesDetails { get; set; } = new();
}