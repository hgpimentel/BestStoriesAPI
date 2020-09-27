namespace API.Tests
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using API.Caching;
	using API.Clients.HackerNews;
	using API.DTO;
	using API.Services;
	using Moq;
	using NUnit.Framework;

	public class StoriesServiceTests
	{
		private Mock<IHackerNewsClient> hackerNewsClientMock;
		private Mock<ICacheService> cacheServiceMock;
		private const int size = 2;
		private const int cacheTimeInMinutes = 1;

		[SetUp]
		public void Setup()
		{
			hackerNewsClientMock = new Mock<IHackerNewsClient>();
			cacheServiceMock = new Mock<ICacheService>();
		}

		[Test]
		public async Task GetTopStories_CacheHit_ReturnsStories()
		{
			//arrange
			cacheServiceMock.Setup(x => x.Get(It.IsAny<string>()))
				.Returns(GetStories());

			//act
			var stories = await GetService().GetTopStoriesAsync();

			//assert
			Assert.IsTrue(stories.Count() == 2);
			cacheServiceMock.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
			cacheServiceMock.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<object>(), cacheTimeInMinutes), Times.Never);
			hackerNewsClientMock.Verify(x => x.GetTopStoriesAsync(It.IsAny<int?>()), Times.Never);
			hackerNewsClientMock.Verify(x => x.GetStoryAsync(It.IsAny<int>()), Times.Never);
		}

		[Test, TestCase(null), TestCase(new int[0])]
		public async Task GetTopStories_CacheMissAndNoStoriesIds_ReturnsEmptyArray(IReadOnlyList<int> sts)
		{
			//arrange
			cacheServiceMock.Setup(x => x.Get(It.IsAny<string>()))
				.Returns(null);
			hackerNewsClientMock.Setup(x => x.GetTopStoriesAsync(It.IsAny<int?>()))
				.ReturnsAsync(sts);

			//act
			var stories = await GetService().GetTopStoriesAsync();

			//assert
			Assert.IsTrue(stories.Count() == 0);
			cacheServiceMock.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
			cacheServiceMock.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<object>(), cacheTimeInMinutes), Times.Never);
			hackerNewsClientMock.Verify(x => x.GetTopStoriesAsync(It.IsAny<int?>()), Times.Once);
			hackerNewsClientMock.Verify(x => x.GetStoryAsync(It.IsAny<int>()), Times.Never);
		}

		[Test]
		public async Task GetTopStories_CacheMissAndHasStoriesIdsAndNoStories_ReturnsEmptyArray()
		{
			//arrange
			cacheServiceMock.Setup(x => x.Get(It.IsAny<string>()))
				.Returns(null);
			hackerNewsClientMock.Setup(x => x.GetTopStoriesAsync(It.IsAny<int?>()))
				.ReturnsAsync(new int[] { 1, 2 });
			hackerNewsClientMock.Setup(x => x.GetStoryAsync(It.IsAny<int>()))
				.ReturnsAsync(null as Story);

			//act
			var stories = await GetService().GetTopStoriesAsync();

			//assert
			Assert.IsTrue(stories.Count() == 0);
			cacheServiceMock.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
			cacheServiceMock.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<object>(), cacheTimeInMinutes), Times.Never);
			hackerNewsClientMock.Verify(x => x.GetTopStoriesAsync(It.IsAny<int?>()), Times.Once);
			hackerNewsClientMock.Verify(x => x.GetStoryAsync(It.IsAny<int>()), Times.Exactly(2));
		}

		[Test]
		public async Task GetTopStories_CacheMissAndHasStoriesIdsAndNoStories_ReturnsStories()
		{
			//arrange
			cacheServiceMock.Setup(x => x.Get(It.IsAny<string>()))
				.Returns(null);
			hackerNewsClientMock.Setup(x => x.GetTopStoriesAsync(It.IsAny<int?>()))
				.ReturnsAsync(new int[] { 1, 2 });
			hackerNewsClientMock.Setup(x => x.GetStoryAsync(It.IsAny<int>()))
				.ReturnsAsync(new Story { Title = "TEST1" });

			//act
			var stories = await GetService().GetTopStoriesAsync();

			//assert
			Assert.IsTrue(stories.Count() == 2);
			cacheServiceMock.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
			cacheServiceMock.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<object>(), cacheTimeInMinutes), Times.Once);
			hackerNewsClientMock.Verify(x => x.GetTopStoriesAsync(It.IsAny<int?>()), Times.Once);
			hackerNewsClientMock.Verify(x => x.GetStoryAsync(It.IsAny<int>()), Times.Exactly(2));
		}

		private StoriesService GetService() =>
			new StoriesService(hackerNewsClientMock.Object, cacheServiceMock.Object, size, cacheTimeInMinutes);

		private StoryDTO[] GetStories() =>
			new StoryDTO[]
			{
				new StoryDTO { Title = "TEST1"},
				new StoryDTO { Title = "TEST2"},
			};
	}
}