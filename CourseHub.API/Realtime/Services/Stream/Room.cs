namespace CourseHub.API.Realtime.Services.Stream;

/// <summary>
/// Managed using ConnectionsHandler
/// </summary>
public class Room
{
    public string Id { get; }
    public List<Participant> Participants { get; } = new List<Participant>();
    public string HostConnectionId { get; private set; }

    public const int CAPACITY = 8;

    public Room(string id, Participant host)
    {
        Id = id;
        Participants.Add(host);
        HostConnectionId = host.ConnectionId;
    }

    public bool TryAddParticipant(Participant participant)
    {
        if (Participants.Find(_ => _.ConnectionId == participant.ConnectionId) != null)
            return false;
        if (Participants.Count + 1 > CAPACITY)
            return false;
        Participants.Add(participant);
        return true;
    }

    public void RemoveParticipant(string connectionId)
    {
        Participant? target = Participants.Find(_ => _.ConnectionId == connectionId);
        if (target != null)
        {
            Participants.Remove(target);
            if (target.ConnectionId == HostConnectionId && Participants.Count > 0)
                HostConnectionId = Participants[0].ConnectionId;
            //else should be removed afterward
        }
    }

    public bool IsEmpty()
    {
        return Participants.Count == 0;
    }
}