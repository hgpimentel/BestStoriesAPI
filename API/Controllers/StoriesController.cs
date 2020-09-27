namespace BestStoriesAPI.Controllers
{
	using System.Threading.Tasks;
	using API.Services;
	using Microsoft.AspNetCore.Mvc;

	[Route("api/[controller]")]
	[ApiController]
	public class StoriesController : ControllerBase
	{
		private readonly IStoriesService storiesService;

		public StoriesController(IStoriesService storiesService)
		{
			this.storiesService = storiesService;
		}

		[HttpGet]
		public async Task<IActionResult> GetTopStories()
		{
			return Ok(await storiesService.GetTopStoriesAsync());
		}
	}
}
