namespace API.Clients.HackerNews
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IHackerNewsClient
	{
		Task<IReadOnlyList<int>> GetTopStoriesAsync(int? size);

		Task<Story> GetStoryAsync(int id);
	}
}
