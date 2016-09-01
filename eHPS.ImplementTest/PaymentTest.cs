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


            var result = paymentService.AwareOrderBooked(new List<String> { "0000003001775739","0000003001777361" });

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void Verify_GetPatientAvaliableAmount_Functional()
        {
            paymentService = container.Resolve<IPayment>();

            var patientId = "0000003001777361";
            var result = paymentService.GetPatientAvaliableAmount(patientId);

            Assert.IsNotNull(result.Amount);
        }





        [TestMethod]
        public void Verify_Pay_Register_Functional()
        {
            paymentService = container.Resolve<IPayment>(); ;

            var TradingId = "gh_86_290_1000";
            var AppointId = "27652";

            var Amount = "0";

            //{ AppointId = "27652", TradingId = "gh_86_290_1000", Amount = 0 }

            var result = paymentService.Recharge(TradingId, AppointId, Amount);

            Assert.AreEqual(0, result.HasError);
            Assert.IsNotNull(result.Body);
        }



        [TestMethod]
        public void Verify_Pay_Functional()
        {
            paymentService = container.Resolve<IPayment>(); ;

            var activityId = "284492";
            var amount = "703.56";

            var actualAmount = "100";

            var result = paymentService.Pay("111", activityId, amount,actualAmount);

            Assert.AreEqual(0, result.HasError);
            Assert.IsNotNull(result.Body);
        }
    }
}
