using AutoMapper;
using CourseHub.Core.Models.Payment;
using CourseHub.Core.Models.User.UserModels;

namespace CourseHub.Core.Services.Mappers.PaymentMappers;

public class BillMapperProfile
{
    public static readonly IConfigurationProvider ModelConfig = new MapperConfiguration(
        cfg =>
        {
            cfg.CreateMap<User, UserMinModel>();
            cfg.CreateMap<Bill, BillModel>();
        }
    );
}
