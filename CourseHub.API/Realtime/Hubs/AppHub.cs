using CourseHub.API.Realtime.Services;
using CourseHub.API.Realtime.Services.Messaging;
using CourseHub.API.Realtime.Services.Stream;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Services.Domain.SocialServices.Contracts;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CourseHub.API.Realtime.Hubs;

public partial class AppHub : Hub
{
    private readonly IConversationService _conversationService;
    private readonly IChatMessageService _chatMessageService;
    private readonly IAppLogger _logger;

    public AppHub(IConversationService conversationService, IChatMessageService chatMessageService, IAppLogger logger)
    {
        _conversationService = conversationService;
        _chatMessageService = chatMessageService;
        _logger = logger;
    }



    public override Task OnConnectedAsync()
    {
        _logger.Inform("Connected: " + Context.ConnectionId);

        ConnectionsHandler.Connected(Context);

        return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        base.OnDisconnectedAsync(exception);

        ConnectionsHandler.Disconnected(Context.ConnectionId);

        Participant? participant = ConnectionsHandler.FindParticipant(Context.ConnectionId);
        if (participant != null)
            // Is participant
            LeaveRoom(participant.RoomId);
    }






    // Send to the client with specified clientId
    public async Task SendClient(string id, Message message)
    {
        await Clients.Client(id).SendAsync(message.Callback, message);
    }

    // Send to the user with specified identifier
    public async Task SendUser(string id, Message message)
    {
        await Clients.User(id).SendAsync(message.Callback, message);
    }

    // Send to clients with specified clientIds
    public async Task SendClientGroup(string[] ids, Message message)
    {
        await Clients.Clients(ids).SendAsync(message.Callback, message);
    }

    // Send to users with specified identifiers
    public async Task SendUserGroup(string[] ids, Message message)
    {
        await Clients.Users(ids).SendAsync(message.Callback, message);
    }

    // Send to all others
    public async Task SendOthers(Message message)
    {
        await Clients.Others.SendAsync(message.Callback, message);
    }



    private Guid GetClientId()
    {
        if (Guid.TryParse(Context.UserIdentifier, out var clientId))
            return clientId;
        return default;
    }
}
