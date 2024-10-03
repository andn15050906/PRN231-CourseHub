namespace CourseHub.API.Realtime.Services.Stream;

public enum StreamEvents : byte
{
    // follow client-defined event's name
    RoomCreated,
    RoomInfoReceived,
    RoomJoined,
    ParticipantLeft,
    OfferReceived,
    AnswerReceived,
    ICECandidateReceived,
    RoomMessageReceived,

    // RoomMessageReceived sub-events
    VideoOff
    //public const string VideoOn = "VideoOn";
}