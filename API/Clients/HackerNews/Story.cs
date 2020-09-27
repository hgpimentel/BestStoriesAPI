﻿namespace API.Clients.HackerNews
{
	public class Story
	{
		public int Id { get; set; }

		public string By { get; set; }

		public int Descendants { get; set; }

		public int[] Kids { get; set; }

		public string Url { get; set; }

		public string Title { get; set; }

		public long Time { get; set; }

		public int Score { get; set; }

		public string Type { get; set; }
	}
}
