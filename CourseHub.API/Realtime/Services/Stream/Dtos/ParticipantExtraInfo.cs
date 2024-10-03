namespace CourseHub.API.Realtime.Services.Stream.Dtos;

public class ParticipantExtraInfo
{
    public string? FullName { get; init; }
    public string? Avatar { get; init; }

    public ParticipantExtraInfo(string? fullName, string? avatar)
    {
        FullName = fullName;
        Avatar = avatar;
    }
}
