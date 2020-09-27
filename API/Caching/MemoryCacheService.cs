namespace API.Caching
{
	using System;
	using Microsoft.Extensions.Caching.Memory;
	using Microsoft.Extensions.Options;

	public class MemoryCacheService: MemoryCache, ICacheService
	{
		public MemoryCacheService(IOptions<MemoryCacheOptions> options) : base(options)
		{

		}

		public void Add(string key, object item, int cacheTime) =>
			this.Set(key, item, new TimeSpan(0, cacheTime, 0));

		public object Get(string key) =>
			this.Get<object>(key);
	}
}
