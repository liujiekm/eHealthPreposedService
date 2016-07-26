using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using eHPS.Contract;
using Microsoft.Practices.Unity.InterceptionExtension;
using eHPS.CrossCutting.NetFramework.ExceptionHandler;
using eHPS.CrossCutting.Logging;
using eHPS.CrossCutting.NetFramework.Logging;
using eHPS.WYServiceImplement;
using System.Collections.Generic;

namespace eHPS.ImplementTest
{
    [TestClass]
    public class PaymentTest
    {
        private IBasicInfo basicService;
        private IPayment paymentService;
        private UnityContainer container;

        [TestInitialize]
        public void Initialize()
        {
            container = new UnityContainer();

            container.RegisterType<IBasicInfo, BasicInfoService>(new Interceptor<InterfaceInterceptor>(),
        new InterceptionBehavior<LoggingInterceptionBehavior>());

            container.RegisterType<IPayment, PaymentService>(new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<LoggingInterceptionBehavior>());

            //设定日志类
            LoggerFactory.SetCurrent(new eHPSNLogFactory());



            container.AddNewExtension<Interception>();


        }


        [TestMethod]
        public void Verify_AwareOrderBooked_Functional()
        {
            paymentService = container.Resolve<IPayment>();

            var result = paymentService.AwareOrderBooked(new List<String> { "0000003001777361", "0000003001775739", "0000003001779855" });

            Assert.IsNotNull(result);
        }
    }
}
