namespace API.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using API.Clients.HackerNews;
	using API.DTO;

	public static class MappingExtensions
	{

		public static IReadOnlyCollection<StoryDTO> ToDto(this IEnumerable<Story> stories)
		{
			if (stories == null)
			{
				return Array.Empty<StoryDTO>();
			}

			return stories.Where(o => o != null).Select(o => o.ToDto()).ToArray();
		}

		public static StoryDTO ToDto(this Story story)
		{
			if (story == null)
			{
				return null;
			}

			return new StoryDTO
			{
				Title = story.Title,
				Uri = story.Url,
				PostedBy = story.By,
				Time = DateTimeOffset.FromUnixTimeSeconds(story.Time),
				Score = story.Score,
				CommentCount = story.Descendants
			};
		}
	}
}
