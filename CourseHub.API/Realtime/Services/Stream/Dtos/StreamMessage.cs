namespace CourseHub.API.Realtime.Services.Stream.Dtos;

public class StreamMessage
{
    public string Event { get; set; }               // StreamEvents
    public string? Data { get; set; }
}