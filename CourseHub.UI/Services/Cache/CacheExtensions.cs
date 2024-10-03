namespace CourseHub.UI.Services.Cache;

public static class CacheExtensions
{
	public static IServiceCollection AddCacheService(this IServiceCollection services)
	{
		services
			.AddMemoryCache()
			.AddSingleton<CacheService>();
		return services;
	}
}
