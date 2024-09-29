using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.Social.ConversationDtos;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Helpers;
using CourseHub.UI.Services.Contracts.SocialServices;
using CourseHub.UI.Services.Contracts.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourseHub.Core.Models.Social;
using CourseHub.Core.RequestDtos.Social.ChatMessageDtos;
using CourseHub.UI.Services.Implementations.UserServices;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.Core.RequestDtos.Course.CourseDtos;
using CourseHub.UI.Helpers.Utils;

namespace CourseHub.UI.Pages.Group;

public class ConversationModel : PageModel
{
    public UserFullModel Client { get; set; }
    public Dictionary<Guid, UserMinModel> RelatedUsers { get; set; }
    // Conversations that user is member
    public List<Core.Models.Social.ConversationModel> Conversations { get; set; }
    public Core.Models.Social.ConversationModel? CurrentConversation { get; set; }
    public List<Guid> MemberIds { get; set; }
    public List<Guid> Admins { get; set; }
    public List<ChatMessageModel> Messages { get; set; }

    public string ReportPath { get; set; }
    public string InviteMemberPath { get; set; }
    public string DeleteMessagePath { get; set; }

    [BindProperty]
    public UpdateConversationDto UpdateDto { get; set; }



    public async Task<IActionResult> OnGet(
        [FromQuery] Guid id,
        [FromQuery] Guid newTarget,
        [FromServices] IUserApiService userApiService,
        [FromServices] IConversationApiService conversationApiService,
        [FromServices] IChatMessageApiService chatMessageApiService)
    {
        if (id == default && newTarget == default)
            return Redirect(Global.PAGE_404);

        Client = await HttpContext.GetClientData();
        if (Client is null)
            return Redirect(Global.PAGE_SIGNIN);

        if (id == default && newTarget != default)
        {
            try
            {
                CreateConversationDto createDto = new()
                {
                    OtherParticipants = new() { newTarget }
                };
                var result = await conversationApiService.CreateAsync(createDto, HttpContext);
                if (result.IsSuccessStatusCode)
                {
                    string? createdConversation = await result.Content.ReadFromJsonAsync<string>();
                    return Redirect(Request.Path + "?id=" + createdConversation);
                }
            }
            catch
            {
                return Redirect(Global.PAGE_404);
            }
        }



        QueryConversationDto dto = new()
        {
            ByClient = true
        };
        var conversations = await conversationApiService.GetAsync(dto, HttpContext);
        Conversations = conversations.Items;



        CurrentConversation = conversations.Items.FirstOrDefault(_ => _.Id == id);
        CurrentConversation ??= await conversationApiService.GetAsync(id, HttpContext);
        if (CurrentConversation is null)
            return Redirect(Global.PAGE_404);



        MemberIds = new();
        Admins = new();
        foreach (var conversation in Conversations)
            foreach (var member in conversation.Members)
            {
                //MemberIds.Add(member.CreatorId);
                if (member.IsAdmin)
                    Admins.Add(member.CreatorId);
            }

        // CurrentConversation
        if (CurrentConversation.IsPrivate)
        {
            var members = CurrentConversation.Members.Select(_ => _.CreatorId);
            MemberIds.AddRange(members);
            Admins.AddRange(members);
        }
        else
        {
            foreach (var member in CurrentConversation.Members)
            {
                MemberIds.Add(member.CreatorId);
                if (member.IsAdmin)
                    Admins.Add(member.CreatorId);
            }
        }

        var allUsers = await userApiService.GetAllMinAsync(HttpContext);
        RelatedUsers = allUsers.ToDictionary(_ => _.Id);



        var apiServerPath = Configurer.GetApiClientOptions().ApiServerPath;
        ReportPath = apiServerPath + "/api/notifications";
        InviteMemberPath = apiServerPath + "/api/notifications/multiple";
        DeleteMessagePath = apiServerPath + "/api/chatmessages";

        if (CurrentConversation is null)
        {
            Messages = new();
            return Page();
        }



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

        QueryChatMessageDto queryChatMessageDto = new()
        {
            ConversationId = id
        };
        var messages = await chatMessageApiService.GetAsync(queryChatMessageDto, HttpContext);
        Messages = messages.Items;

        Client.AvatarUrl = UserApiService.GetAvatarApiUrl(Client.AvatarUrl, Client.Id);
        return Page();
    }

    public async Task<IActionResult> OnPostEdit([FromServices] IConversationApiService conversationApiService)
    {
        if (!ModelState.IsValid)
        {
            return Redirect(Request.Path + "?id=" + UpdateDto.Id);
        }
        if (UpdateDto.Avatar is not null)
        {
            var file = UpdateDto.Avatar.File;
            if (file is null)
                return Reload();
            if (!ResourceHelper.IsImage(file))
                return Reload();
        }



        var response = await conversationApiService.UpdateAsync(UpdateDto, HttpContext);
        // Alert
        return Reload();
    }

    private IActionResult Reload()
    {
        return Redirect(Request.Path + "?id=" + UpdateDto.Id);
    }
}
