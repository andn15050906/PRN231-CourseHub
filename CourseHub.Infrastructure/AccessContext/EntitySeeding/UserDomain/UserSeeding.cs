using CourseHub.Core.Entities.UserDomain;
using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Infrastructure.AccessContext.Shared;
using System.Globalization;

namespace CourseHub.Infrastructure.AccessContext.EntitySeeding.UserDomain;

internal class UserSeeding : DomainSeedingBase<User>
{
    internal override List<User> seedValues => new()
    {
        new User(new("0e322eb7-422d-4fc6-bfc4-8797538bec3c"), "Trần Thị Hương", "SkyDiver7")
        {
            UserName = "SkyDiver7",
            Email = "SkyDiver7@gmail.com",
            Role = Role.SysAdmin,
            IsVerified = true,
            Bio = "Một giáo viên dạy ngoại ngữ với hơn 20 năm kinh nghiệm giảng dạy. Tôi có niềm đam mê mãnh liệt với việc truyền đạt kiến thức ngôn ngữ và văn hóa đến các học viên của mình.",
            DateOfBirth = DateTime.ParseExact("31/12/2002", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Phone = "0311202002",

            AvatarUrl = string.Empty,
            Token = string.Empty,
            RefreshToken = string.Empty
        },
        new User(new("1e512cf4-856b-4dd8-94b1-521c71276d8a"), "Lê Văn Đức", "TigerLion9")
        {
            UserName = "TigerLion9",
            Email = "TigerLion9@gmail.com",
            Role = Role.Admin,
            Bio = string.Empty,
            DateOfBirth = DateTime.ParseExact("10/02/2005", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Phone = "0123456789",

            AvatarUrl = string.Empty,
            Token = string.Empty,
            RefreshToken = string.Empty
        },
        new User(new("ac9d2a6c-8e7f-4d1e-96e3-8995c192b262"), "Phạm Thanh Mai", "MoonStar8")
        {
            UserName = "MoonStar8",
            Email = "MoonStar8@gmail.com",
            Role = Role.Admin,
            Bio = string.Empty,
            DateOfBirth = DateTime.ParseExact("05/07/1998", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Phone = "0987654321",

            AvatarUrl = string.Empty,
            Token = string.Empty,
            RefreshToken = string.Empty
        },
        new User(new ("55ac3b6e-48c4-4ef0-86e8-17129b0f1676"), "Hoàng Minh Phương", "Snow1234")
        {
            UserName = "Snow1234",
            Email = "Snow1234@gmail.com",
            Role = Role.Learner,
            Bio = string.Empty,
            DateOfBirth = DateTime.ParseExact("10/08/2014", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Phone = "0765432109",

            AvatarUrl = string.Empty,
            Token = string.Empty,
            RefreshToken = string.Empty
        },
        new User(new ("7a7bb1cd-25f9-4d44-8f8e-9a8d10d2eb98"), "Vũ Thị Thuỳ Linh", "FireIce6")
        {
            UserName = "FireIce6",
            Email = "FireIce6@gmail.com",
            Role = Role.Instructor,
            Bio = string.Empty,
            DateOfBirth = DateTime.ParseExact("03/05/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture),
            Phone = "0345678901",

            AvatarUrl = string.Empty,
            Token = string.Empty,
            RefreshToken = string.Empty
        }
    };
}
