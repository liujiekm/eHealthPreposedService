using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using eHPS.CrossCutting.NetFramework.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHPS.API.DependencyBootstrap
{
    public class EHABConfig
    {
        public static void Configure()
        {
            //通过指定配置文件加载EHAB配置信息
            //IConfigurationSource config = new FileConfigurationSource("",true);
            //ExceptionPolicyFactory factory = new ExceptionPolicyFactory(config);
            //ExceptionManager exManager = factory.CreateManager();

            //通过webconfig文件加载EHAB配置信息
            //ExceptionHandlingSettings section = (ExceptionHandlingSettings)ConfigurationManager.GetSection(ExceptionHandlingSettings.SectionName);
            //ExceptionManager exManager = section.BuildExceptionManager();

            //异常正常传递策略
            var propogatePolicyEntry = new ExceptionPolicyEntry(typeof(Exception), PostHandlingAction.NotifyRethrow, new List<IExceptionHandler>() {
                new LoggingHandler()
            });
            //异常替换策略
            //服务层调用Application时执行本策略
            var replacePolicyEntry = new ExceptionPolicyEntry(typeof(Exception), PostHandlingAction.ThrowNewException, new List<IExceptionHandler>()
            {
                new LoggingHandler(),
                new ReplaceHandler("",typeof(Exception))

            });

            //异常包装策略
            var wrapPolicyEntry = new ExceptionPolicyEntry(typeof(Exception), PostHandlingAction.ThrowNewException, new List<IExceptionHandler>()
            {
                new LoggingHandler(),
                new WrapHandler("",typeof(Exception))

            });

            //异常传递策略
            var policies = new List<ExceptionPolicyDefinition>();
            policies.Add(new ExceptionPolicyDefinition(
              "Propogate Policy", new List<ExceptionPolicyEntry>() { propogatePolicyEntry }));
            policies.Add(new ExceptionPolicyDefinition(
              "Replace Policy", new List<ExceptionPolicyEntry>() { replacePolicyEntry }));
            policies.Add(new ExceptionPolicyDefinition(
              "Wrap Policy", new List<ExceptionPolicyEntry>() { wrapPolicyEntry }));

            ExceptionManager exManager = new ExceptionManager(policies);


            
            ExceptionPolicy.SetExceptionManager(exManager);

            
        }
    }
}