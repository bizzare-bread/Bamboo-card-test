using HackerNews.Models;
using HackerNews.Models.Request;

namespace HackerNews;

public partial class HackerNewsClient
{
    public async Task<CallResult<IEnumerable<int>>> GetBestStoryIds(CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<int>>($"{HackerNewsApiUrl}/beststories.json", cancellationToken);
    }

    public async Task<CallResult<Story>> GetStoryDetails(GetStoryDetailsRequest request,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<Story>($"{HackerNewsApiUrl}/item/{request.Id}.json", cancellationToken);
    }
}