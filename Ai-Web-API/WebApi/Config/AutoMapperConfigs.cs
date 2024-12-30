using AutoMapper;
using Model.Dto.AiModel;
using Model.Dto.Role;
using Model.Dto.TestPaperManage;
using Model.Dto.TestPapers;
using Model.Dto.User;
using Model.Dto.Yolo;
using Model.Entities;
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

        // 题库模块
        CreateMap<TestPapersManage, TestPaperManageRes>();
        
        //成绩导出
        CreateMap<ReportCard, DownloadAchievementWordDto>();
        //只映射名称
        CreateMap<Users, DownloadAchievementWordDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<ReportCard, AchievementCenterRes>();
        //RoleManagement
        CreateMap<Users, RoleRes>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => EnumConvert.ConvertRoleNameToString(src.Role)));

        //RoleManagement
        CreateMap<AiModels, GetModelRes>()
            //desc目标对象--将源对象AiModels的CreateUserId属性的值作为目标对象GetModelRes的CreateName属性的值
            .ForMember(dest => dest.CreateName, opt => opt.MapFrom(sec => (sec.CreateUserId)));
    }
}