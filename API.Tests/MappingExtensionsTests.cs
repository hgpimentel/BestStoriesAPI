namespace API.Tests
{
	using System;
	using API.Clients.HackerNews;
	using API.Extensions;
	using NUnit.Framework;

	public class MappingExtensionsTests
	{
		private Story story;

		[SetUp]
		public void Setup()
		{
			this.story = new Story
			{
				Title = "TEST1",
				By = "AUTHOR",
				Descendants = 1,
				Score = 1,
				Time = 1000000,
				Type = "story",
				Url = "URL"
			};
		}

		[Test]
		public void StoryDto_IsNull_ReturnsNul()
		{
			//arrange act
			var dto = (null as Story).ToDto();

			//assert
			Assert.IsNull(dto);
		}

		[Test]
		public void StoryDto_IsNotNull_ReturnsNul()
		{
			//arrange act
			var dto = this.story.ToDto();

			//assert
			Assert.AreEqual(story.Title, dto.Title);
			Assert.AreEqual(story.By, dto.PostedBy);
			Assert.AreEqual(story.Descendants, dto.CommentCount);
			Assert.AreEqual(story.Score, dto.Score);
			Assert.AreEqual(DateTimeOffset.FromUnixTimeSeconds(story.Time), dto.Time);
			Assert.AreEqual(story.Url, dto.Uri);
		}

		[Test]
		public void StoriesDto_CacheHit_ReturnsStories()
		{
			//arrange 
			var stories = new Story[] { story };

			//act
			var dto = stories.ToDto();

			//assert
			Assert.IsTrue(stories.Length == dto.Count);
		}
	}
}
