using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Reflection;

using eHPS.CrossCutting.Adapter;
using eHPS.CrossCutting.NetFramework.Adapter;
using eHPS.CrossCutting.NetFramework.Logging;
using eHPS.CrossCutting.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using System.Configuration;
using eHPS.CrossCutting.NetFramework.ExceptionHandler;


namespace eHPS.API.DependencyBootstrap
{
    public static class ContainerBootstrap
    {

        public static void Configure(IUnityContainer container)
        {


            
            container.LoadConfiguration();



            //注册AutoMapper Adapter Factory
            container.RegisterType<ITypeAdapterFactory, AutomapperTypeAdapterFactory>(
               new ContainerControlledLifetimeManager());






            //.RegisterTypes(AllClasses.FromAssemblies(Assembly.GetExecutingAssembly()),
            //               WithMappings.FromAllInterfacesInSameAssembly,
            //               WithName.Default,
            //               WithLifetime.ContainerControlled)

            // 通过拦截注入日志 异常 缓存处理
            //.RegisterType<IExpenseRepository, ExpenseRepository>(new Interceptor<VirtualMethodInterceptor>(),
            //                                                     new InterceptionBehavior<PolicyInjectionBehavior>());



            //设定日志类
            LoggerFactory.SetCurrent(new eHPSNLogFactory());



            

            var typeAdapterFactory = container.Resolve<ITypeAdapterFactory>();
            TypeAdapterFactory.SetCurrent(typeAdapterFactory);

            

            #region 注入异常日志记录功能

            //container.RegisterType<IApplicationService, ApplicationService>(
            //    new Interceptor<InterfaceInterceptor>(), 
            //        new InterceptionBehavior<LoggingInterceptionBehavior>()
            //    );
            ////启用Policy Injection来进行Interception必须对容器进行扩展
            //container.AddNewExtension<Interception>();


            #endregion
        }
    }
}