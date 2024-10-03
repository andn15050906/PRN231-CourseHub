using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Models.Social;
using Microsoft.Extensions.Caching.Memory;

namespace CourseHub.UI.Services.Cache;

public class CacheService
{
	private readonly IMemoryCache _memoryCache;

	//private readonly TimeSpan _expire = TimeSpan.FromMinutes(5);

	private const string CATEGORIES = "Categories";



	public CacheService(IMemoryCache memoryCache)
	{
		_memoryCache = memoryCache;
	}






	public void Set(List<Category> categories)
	{
		_memoryCache.Set(CATEGORIES, categories);
	}

	public bool TryGetCategories(out List<Category>? categories)
	{
		return _memoryCache.TryGetValue(CATEGORIES, out categories);
	}



	public void SetConversation(ConversationModel conversation)
	{
		_memoryCache.Set(conversation.Id, conversation/*, _expire*/);
	}

	public bool TryGetConversation(Guid id, out ConversationModel? conversation)
	{
		return _memoryCache.TryGetValue(id, out conversation);
	}

	public void RemoveConversation(Guid id)
	{
		_memoryCache.Remove(id);
	}
}
