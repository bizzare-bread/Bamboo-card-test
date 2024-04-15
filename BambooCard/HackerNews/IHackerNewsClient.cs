using HackerNews.Models;
using HackerNews.Models.Request;

namespace HackerNews;

public interface IHackerNewsClient
{
    #region Story

    Task<CallResult<IEnumerable<int>>> GetBestStoryIds(CancellationToken cancellationToken);
    
    Task<CallResult<Story>> GetStoryDetails(GetStoryDetailsRequest request,
        CancellationToken cancellationToken = default);
    
    #endregion
}