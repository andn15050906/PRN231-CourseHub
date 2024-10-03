using CourseHub.Core.Models.User.UserModels;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.SocialServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Group;

public class CallModel : PageModel
{
    private readonly IConversationApiService _conversationApiService;



    public Guid ConversationId { get; set; }
    public Core.Models.Social.ConversationModel Conversation { get; set; }

    public UserFullModel? Client { get; set; }



    public CallModel(IConversationApiService conversationApiService)
    {
        _conversationApiService = conversationApiService;
	}

    public async Task<IActionResult> OnGet(Guid id)
    {
        Client = await HttpContext.GetClientData();
        if (Client is null)
            return Redirect(Global.PAGE_SIGNIN);

        ConversationId = id;
        Conversation = await _conversationApiService.GetAsync(id, HttpContext);
        if (Conversation is null)
            return Redirect(Global.PAGE_404);

        if (!Conversation.Members.Any(_ => _.CreatorId == Client.Id))
			return Redirect(Global.PAGE_403);

		return Page();
    }
}
