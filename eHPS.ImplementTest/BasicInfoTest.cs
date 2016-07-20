using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eHPS.WYServiceImplement;
using eHPS.Contract;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using eHPS.CrossCutting.NetFramework.ExceptionHandler;
using eHPS.CrossCutting.NetFramework.Logging;
using eHPS.CrossCutting.Logging;
using System.Collections.Generic;

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

            basicService = container.Resolve<IBasicInfo>();
            var areaId = "01";
            var result = basicService.GetDepts(areaId);

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void Is_Nlog_Functional()
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            logger.Error("what fuck");


        }

        private void ModifyRef(List<String> items)
        {
            items.Add("kobe");
            items.Add("jack");
        }

        [TestMethod]
        public void Wheather_Need_Ref()
        {
            var items = new List<String>();
            items.Add("michael");
            items.Add("jordan");

            ModifyRef(items);

            Assert.AreEqual(4, items.Count);


        }
    }
}
