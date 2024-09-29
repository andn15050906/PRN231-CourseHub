namespace CourseHub.Core.RequestDtos.Social.ChatMessageDtos;

public class QueryChatMessageDto
{
    public short PageIndex { get; set; }    // from 0
    public byte PageSize { get; set; } = 20;

    public Guid ConversationId { get; set; }
}
