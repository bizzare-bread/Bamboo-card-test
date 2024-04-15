using System.Net;
using BambooCard.Abstract;
using BambooCard.Models;
using HackerNews;
using HackerNews.Models;
using HackerNews.Models.Request;

namespace BambooCard.Services;

public class HackerNews : IHackerNews
{
    const string HackerNewsApiUrl = "https://hacker-news.firebaseio.com/v0";
    
    private readonly ILogger<HackerNews> _logger;
    
    public HackerNews(ILogger<HackerNews> logger)
    {
        _logger = logger;
    }
    
    public async Task<GetBestStoriesDetailsResponse> GetBestStoriesDetails(int storiesQty,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Requested best stories details");

        var response = new GetBestStoriesDetailsResponse();

        if (storiesQty < 1)
        {
            return response;
        }

        var bestStoryIds = new List<int>();
        
        bestStoryIds = await GetBestStoriesIds(cancellationToken);

        if (!bestStoryIds.Any())
        {
            return response;
        }
        
        var tasks = bestStoryIds.Select(id => GetStoryDetails(id, cancellationToken));
        var stories = await Task.WhenAll(tasks);
        
        response.BestStoriesDetails = stories
            .OrderByDescending(s => s?.Score)
            .Take(storiesQty)
            .Where(s => s != null)
            .Select(s =>
            {
                HackerNewsStory newStory;
                newStory = new HackerNewsStory
                {
                    //This should be adjusted a bit, but 
                    Title = s.Title,
                    Url = s.Url,
                    PostedBy = s.By,
                    Score = s.Score,
                    CommentCount = s.Descendants
                };

                try
                {
                    newStory.Time = DateTimeOffset.FromUnixTimeSeconds(s.Time).UtcDateTime;
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, $"When trying to parse story Time: {s.Time}");
                    throw;
                }

                return newStory;
            }).ToList();
        
        return response;
    }
    
    private async Task<List<int>> GetBestStoriesIds(CancellationToken cancellationToken = default)
    {
        try
        {
            var hackerNewsClient = new HackerNewsClient();
            var bestStoryIdsResponse =
                await hackerNewsClient.GetBestStoryIds(cancellationToken);

            if (!bestStoryIdsResponse.Success)
            {
                _logger.LogWarning($"{nameof(HackerNewsClient)} GetBestStoryIds returned with code {bestStoryIdsResponse.Code}" +
                                   $"message {bestStoryIdsResponse.Message} data {bestStoryIdsResponse.Data}");
                return new List<int>();
            }

            return bestStoryIdsResponse.Data.ToList();
        }
        catch (Exception e)
        {
            // We have a lot of space to continue improving of our exception handling
            _logger.LogError(e, "When trying to GetBestStoryIds");
            return new List<int>();
        }
    }
    
    // Null-return method is not the best practice but useful in this case
    private async Task<Story?> GetStoryDetails(int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var hackerNewsClient = new HackerNewsClient();
            var bestStoryDetailsResponse =
                await hackerNewsClient.GetStoryDetails(
                    new GetStoryDetailsRequest
                    {
                        Id = id
                    },
                    cancellationToken);

            if (!bestStoryDetailsResponse.Success)
            {
                _logger.LogWarning($"{nameof(HackerNewsClient)} GetBestStoryIds returned with code {bestStoryDetailsResponse.Code}" +
                                   $"message {bestStoryDetailsResponse.Message} data {bestStoryDetailsResponse.Data}");
                    
                return null;
            }

            return bestStoryDetailsResponse.Data;
        }
        catch (Exception e)
        {
            // We have a lot of space to continue improving of our exception handling
            _logger.LogError(e, "When trying to GetStoryDetails");
            return null;
        }
    }
}