using CourseHub.API.Realtime.Services.Stream.Dtos;
using CourseHub.API.Realtime.Services.Stream;
using Microsoft.AspNetCore.SignalR;
using CourseHub.API.Realtime.Services;

namespace CourseHub.API.Realtime.Hubs;

public partial class AppHub
{
    public void SendOffers(OfferMessage[] offers, bool isRenegotiation)
    {
        Parallel.ForEach(offers, offer =>
        {
            Clients.Client(offer.Peer).SendAsync(StreamEvents.OfferReceived.ToString(), Context.ConnectionId, offer.Offer, isRenegotiation);
        });
    }

    public async Task SendAnswer(string offerer, string answer)
    {
        await Clients.Client(offerer).SendAsync(StreamEvents.AnswerReceived.ToString(), Context.ConnectionId, answer);
    }

    public async Task SendICECandidate(string receiver, string candidate)
    {
        await Clients.Client(receiver).SendAsync(StreamEvents.ICECandidateReceived.ToString(), Context.ConnectionId, candidate);
    }





    public async Task JoinRoom(string roomId, ParticipantExtraInfo info)
    {
        Room? room = ConnectionsHandler.FindRoom(roomId);
        Guid? clientId = ConnectionsHandler.GetUserId(Context);
        Participant participant = new(clientId, Context.ConnectionId, info);

        if (room == null)
        {
            ConnectionsHandler.CreateRoom(roomId, participant);
#pragma warning disable CS4014
            Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            Clients.Caller.SendAsync(
                StreamEvents.RoomCreated.ToString(), roomId);
#pragma warning restore CS4014
            return;
        }

        bool addSucceed = ConnectionsHandler.TryAddRoomParticipant(participant, room);
        if (addSucceed)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
#pragma warning disable CS4014
            Clients.Caller.SendAsync(StreamEvents.RoomInfoReceived.ToString(), room);
            Clients.OthersInGroup(roomId).SendAsync(
                StreamEvents.RoomJoined.ToString(), roomId, participant);
#pragma warning restore CS4014
        }
    }

	/*public async Task SendRoom(string roomId, StreamMessage message, [FromServices] ChatService chatService)
    {
    }*/

	public async Task LeaveRoom(string roomId)
    {
        Room? room = ConnectionsHandler.FindRoom(roomId);
        if (room == null)
            return;
        ConnectionsHandler.RemoveRoomParticipant(room, Context.ConnectionId);
        await Clients.OthersInGroup(room.Id).SendAsync(
            StreamEvents.ParticipantLeft.ToString(), room.Id, Context.ConnectionId);
    }
}
