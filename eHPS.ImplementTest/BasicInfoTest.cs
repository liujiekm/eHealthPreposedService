using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eHPS.WYServiceImplement;
using eHPS.Contract;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using eHPS.CrossCutting.NetFramework.ExceptionHandler;
using eHPS.CrossCutting.NetFramework.Logging;
using eHPS.CrossCutting.Logging;

namespace eHPS.ImplementTest
{
    [TestClass]
    public class BasicInfoTest
    {
        private IBasicInfo basicService;
        private UnityContainer container;

        [TestInitialize]
        public void Initialize()
        {
            container = new UnityContainer();

            container.RegisterType<IBasicInfo, BasicInfoService>( new Interceptor<InterfaceInterceptor>(), 
                    new InterceptionBehavior<LoggingInterceptionBehavior>());

            //设定日志类
            LoggerFactory.SetCurrent(new eHPSNLogFactory());



            container.AddNewExtension<Interception>();


        }



        [TestMethod]
        public void Get_All_Depts()
        {

            //basicService = container.Resolve<IBasicInfo>();
            //var result = basicService.GetDepts();

            //Assert.AreEqual(5, result.Count);
        }


        [TestMethod]
        public void Is_Nlog_Functional()
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            logger.Error("what fuck");


        }
    }
}
