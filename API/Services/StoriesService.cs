namespace API.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using API.Caching;
	using API.Clients.HackerNews;
	using API.DTO;
	using API.Extensions;

	public class StoriesService : IStoriesService
	{
		private readonly IHackerNewsClient hackerNewsClient;
		private readonly ICacheService cacheService;
		private readonly int topStoriesSize;
		private readonly int cacheTimeInMinutes;

		public StoriesService(IHackerNewsClient hackerNewsClient, ICacheService cacheService, int topStoriesSize, int cacheTimeInMinutes)
		{
			this.hackerNewsClient = hackerNewsClient;
			this.cacheService = cacheService;
			this.topStoriesSize = topStoriesSize;
			this.cacheTimeInMinutes = cacheTimeInMinutes;
		}

		public async Task<IEnumerable<StoryDTO>> GetTopStoriesAsync()
		{
			var key = GetTopStoriesCacheKey();

			if (cacheService.Get(key) is IReadOnlyCollection<StoryDTO> stories)
			{
				return stories;
			}

			var topStoriesIds = await hackerNewsClient.GetTopStoriesAsync(topStoriesSize);

			if (topStoriesIds == null || topStoriesIds.Count == 0)
			{
				return Enumerable.Empty<StoryDTO>();
			}

			stories = (await Task.WhenAll(topStoriesIds.Select(o => hackerNewsClient.GetStoryAsync(o)))).ToDto();

			if (stories.Count > 0)
			{
				cacheService.Add(key, stories, cacheTimeInMinutes);
			}

			return stories;
		}

		private string GetTopStoriesCacheKey() => $"topstories#{topStoriesSize}";
	}
}
