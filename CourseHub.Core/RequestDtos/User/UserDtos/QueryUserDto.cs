namespace CourseHub.Core.RequestDtos.User.UserDtos;

public class QueryUserDto
{
    public short PageIndex { get; set; }    // from 0
    public byte PageSize { get; set; } = 20;

    public string? Name { get; set; }       // suggest
}
