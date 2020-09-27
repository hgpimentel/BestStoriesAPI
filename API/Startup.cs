namespace BestStoriesAPI
{
	using API.Caching;
	using API.Clients.HackerNews;
	using API.Services;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Caching.Memory;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;

	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			
			services.AddSingleton<IHackerNewsClient>(sp =>
				new HackerNewsClient(
					Configuration.GetValue<string>("HackerNewsApiEndpoint"),
					Configuration.GetValue<string>("HackerNewsApiVersion"),
					sp.GetRequiredService<ILogger<HackerNewsClient>>()));

			services.AddSingleton<ICacheService, MemoryCacheService>();
			
			services.AddScoped<IStoriesService>(sp =>
				new StoriesService(
					sp.GetRequiredService<IHackerNewsClient>(),
					sp.GetRequiredService<ICacheService>(),
					Configuration.GetValue<int>("TopStoriesSize"),
					Configuration.GetValue<int>("TopStoriesCacheTimeInMinutes")
			));

			services.AddOptions<MemoryCacheOptions>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			
			app.UseMvc();
		}
	}
}
