using BambooCard.Models;
using HackerNews.Models;

namespace BambooCard.Abstract;

public interface IHackerNews
{
    Task<GetBestStoriesDetailsResponse> GetBestStoriesDetails(int storiesQty,
        CancellationToken cancellationToken);
}