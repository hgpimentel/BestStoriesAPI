namespace API.Caching
{
	public interface ICacheService
	{
		void Add(string key, object item, int cacheTime);

		object Get(string key);
	}
}
