using System.Reflection;
using Autofac;
using CommonUtil.AiGcUtil;

namespace WebApi.Config;

public class AutofacModuleRegister : Autofac.Module
{
    //注册接口和实现之间的关系
    protected override void Load(ContainerBuilder builder)
    {
        //通过反射
        Assembly interfaceAssembly = Assembly.Load("Interface");
        Assembly serviceAssembly = Assembly.Load("Service");
        builder.RegisterAssemblyTypes(interfaceAssembly, serviceAssembly).AsImplementedInterfaces();
        //ai请求策略注入
        builder.RegisterType<AiRequestProcessor>().AsSelf().SingleInstance();
        //Ai策略工厂注入
        builder.RegisterType<AiRequestStrategyFactory>().AsSelf().SingleInstance();
        //讯飞API实现注册
        builder.RegisterType<SparkAiRequestStrategy>().AsSelf().InstancePerDependency();
    }
}