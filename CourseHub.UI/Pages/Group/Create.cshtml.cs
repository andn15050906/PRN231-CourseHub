using CourseHub.Core.RequestDtos.Social.ConversationDtos;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Helpers;
using CourseHub.UI.Services.Contracts.SocialServices;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourseHub.Core.Models.User.UserModels;
using Microsoft.AspNetCore.Mvc;
using CourseHub.UI.Services.Contracts.UserServices;
using CourseHub.Core.RequestDtos.User.UserDtos;

namespace CourseHub.UI.Pages.Group;

public class CreateModel : PageModel
{
    private readonly IConversationApiService _conversationApiService;

    public UserFullModel Client { get; set; }
    public List<Core.Models.Social.ConversationModel> Conversations { get; set; }
    public Dictionary<Guid, UserModel> RelatedUsers { get; set; }




    [BindProperty]
    public CreateConversationDto Dto { get; set; }



    public CreateModel(IConversationApiService conversationApiService)
    {
        _conversationApiService = conversationApiService;
    }



    public async Task<IActionResult> OnGet([FromServices] IUserApiService userApiService)
    {
        Client = await HttpContext.GetClientData();
        if (Client is null)
            return Redirect(Global.PAGE_SIGNIN);


        QueryConversationDto dto = new()
        {
            ByClient = true
        };
        var conversations = await _conversationApiService.GetAsync(dto, HttpContext);
        Conversations = conversations.Items;

        /*List<Guid> relatedUserIds = new();
        foreach (var conversation in Conversations)
            foreach (var member in conversation.Members)
                relatedUserIds.Add(member.CreatorId);*/
        var relatedUsers = await userApiService.GetPagedAsync(new QueryUserDto(), HttpContext);
        RelatedUsers = relatedUsers.Items.ToDictionary(_ => _.Id);

        foreach (var conversation in Conversations)
        {
            if (conversation.Title == string.Empty)
            {
                var otherMember = conversation.Members.FirstOrDefault(_ => _.CreatorId != Client.Id);
                RelatedUsers.TryGetValue(otherMember.CreatorId, out var user);

                if (user is null)
                    continue;
                conversation.Title = user.FullName;

                if (string.IsNullOrEmpty(conversation.AvatarUrl))
                    conversation.AvatarUrl = user.AvatarUrl;
            }
        }

        return Page();
    }

    public async Task<IActionResult> OnPost([FromServices] IConversationApiService conversationApiService)
    {
        var result = await conversationApiService.CreateAsync(Dto, HttpContext);
        if (result.IsSuccessStatusCode)
            return Redirect(Request.Path);

        TempData[Global.ALERT_STATUS] = false;
        TempData[Global.ALERT_MESSAGE] = "Cannot create conversation!";
        return Redirect(Request.Path);
    }
}
