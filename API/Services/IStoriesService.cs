namespace API.Services
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using API.DTO;

	public interface IStoriesService
	{
		Task<IEnumerable<StoryDTO>> GetTopStoriesAsync();
	}
}
