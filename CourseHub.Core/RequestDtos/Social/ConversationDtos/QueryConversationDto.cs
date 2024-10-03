namespace CourseHub.Core.RequestDtos.Social.ConversationDtos;

public class QueryConversationDto
{
    public short PageIndex { get; set; }    // from 0
    public byte PageSize { get; set; } = 20;

    public bool ByClient { get; set; }

    public string? Title { get; set; }
    public IEnumerable<Guid>? ConversationIds { get; set; }
}
