using AutoMapper;
using Model.Entitys;
using Model.User;

namespace WebApi.Config;

public class AutoMapperConfigs : Profile
{
    public AutoMapperConfigs()
    {
        //用户
        CreateMap<Users, UserAdd>();
        CreateMap<UserAdd, Users>();
        
    }
}