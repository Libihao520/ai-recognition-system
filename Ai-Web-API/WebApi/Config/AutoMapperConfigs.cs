using AutoMapper;
using Model.Dto.Role;
using Model.Dto.TestPapers;
using Model.Dto.User;
using Model.Dto.Yolo;
using Model.Entitys;
using Model.Enum;

namespace WebApi.Config;

public class AutoMapperConfigs : Profile
{
    public AutoMapperConfigs()
    {
        //左往右
        //用户
        CreateMap<Users, UserRes>();
        //yolo
        CreateMap<Yolotbs, YoloPkqRes>();

        CreateMap<YoloDetectionPutReq, Yolotbs>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PhotosId, opt => opt.Ignore());

        CreateMap<Yolotbs, YoloPkqEditRes>();

        //练题模块
        CreateMap<TestPapers, SingleChoice>()
            .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.Topic))
            .ForMember(dest => dest.options,
                opt => opt.MapFrom(src => new List<string>() { src.Choice1, src.Choice2, src.Choice3, src.Choice4 }));

        CreateMap<TestPapers, MultipleChoice>()
            .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.Topic))
            .ForMember(dest => dest.options,
                opt => opt.MapFrom(src => new List<string>() { src.Choice1, src.Choice2, src.Choice3, src.Choice4 }));

        CreateMap<TestPapers, TrueFalse>()
            .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.Topic))
            .ForMember(dest => dest.options,
                opt => opt.MapFrom(src => new List<string>() { src.Choice1, src.Choice2 }));

        //RoleManagement
        CreateMap<Users, RoleRes>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => EnumConvert.ConvertRoleNameToString(src.Role)));
    }
}