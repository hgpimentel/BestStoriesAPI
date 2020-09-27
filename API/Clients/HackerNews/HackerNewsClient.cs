namespace API.Clients.HackerNews
{
	using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Logging;

	public class HackerNewsClient : HttpClient, IHackerNewsClient
	{
		private readonly string endpoint;
		private readonly string version;
		private readonly ILogger logger;

		public HackerNewsClient(string endpoint, string version, ILogger<HackerNewsClient> logger) : base()
		{
			this.endpoint = endpoint;
			this.version = version;
			this.logger = logger;
		}

		public Task<IReadOnlyList<int>> GetTopStoriesAsync(int? size) =>
			GetAsync<IReadOnlyList<int>>(GetTopStoriesResouce(size));

		public Task<Story> GetStoryAsync(int id) =>
			GetAsync<Story>($"item/{id}.json");

		private static string GetTopStoriesResouce(int? size) =>
			size.HasValue
			? $"beststories.json?orderBy=%22$key%22&limitToFirst={size}"
			: "beststories.json";

		private async Task<T> GetAsync<T>(string resource)
		{
			var url = $"{GetEndpointUrl()}/{resource}";
			try
			{
				var response = await this.GetAsync(url);
				if (response.IsSuccessStatusCode)
				{
					return await response.Content.ReadAsAsync<T>();
				}
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex, $"Error accessing {url}.");
			}

			return default;
		}

		private string GetEndpointUrl() => $"{endpoint}/{version}";
	}
}
