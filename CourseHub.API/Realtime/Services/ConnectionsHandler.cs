using CourseHub.API.Realtime.Services.Stream;
using Microsoft.AspNetCore.SignalR;

namespace CourseHub.API.Realtime.Services;

public class ConnectionsHandler
{
    private static readonly Dictionary<string, Guid?> _connection_user = new();

    private static readonly Dictionary<string, Participant> _connection_participant = new();
    private static readonly List<Room> _rooms = new();






    public static void Connected(HubCallerContext context)
    {
        if (context.UserIdentifier == null || !Guid.TryParse(context.UserIdentifier, out Guid clientId))
        {
            _connection_user.Add(context.ConnectionId, null);
            return;
        }
        _connection_user.Add(context.ConnectionId, clientId);
    }

    public static void Disconnected(string connection)
    {
        _connection_user.Remove(connection);
    }

    public static Guid? GetUserId(HubCallerContext context)
    {
        _connection_user.TryGetValue(context.ConnectionId, out Guid? userId);
        return userId;
    }






    public static Room? FindRoom(string roomId) => _rooms.Find(_ => _.Id == roomId);

    public static Participant? FindParticipant(string connectionId)
    {
        if (_connection_participant.TryGetValue(connectionId, out Participant? participant))
            return participant;
        return null;
    }

    public static Room CreateRoom(string roomId, Participant host)
    {
        //Room room = new (GenerateRoomId(), host);
        Room room = new(roomId, host);
        _rooms.Add(room);
        host.SetRoom(roomId);
        _connection_participant.Add(host.ConnectionId, host);
        return room;
    }

    public static bool TryAddRoomParticipant(Participant guest, Room room)
    {
        if (!room.TryAddParticipant(guest))
            return false;
        guest.SetRoom(room.Id);
        _connection_participant.Add(guest.ConnectionId, guest);
        return true;
    }

    public static void RemoveRoomParticipant(Room room, string guestConnectionId)
    {
        room.RemoveParticipant(guestConnectionId);
        _connection_participant.Remove(guestConnectionId);

        if (room.IsEmpty())
            _rooms.Remove(room);
    }






    public static Dictionary<string, Participant> GetParticipants() => _connection_participant;

    public static List<Room> GetRooms() => _rooms;
}
