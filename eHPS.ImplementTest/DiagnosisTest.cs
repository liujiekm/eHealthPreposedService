using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using eHPS.Contract;
using eHPS.WYServiceImplement;
using eHPS.CrossCutting.NetFramework.ExceptionHandler;
using eHPS.CrossCutting.Logging;
using eHPS.CrossCutting.NetFramework.Logging;

namespace eHPS.ImplementTest
{
    [TestClass]
    public class DiagnosisTest
    {

        private IDiagnosis diagnosisService;
        private UnityContainer container;

        [TestInitialize]
        public void Initialize()
        {
            container = new UnityContainer();

            container.RegisterType<IDiagnosis, DiagnosisService>(new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<LoggingInterceptionBehavior>());

            //设定日志类
            LoggerFactory.SetCurrent(new eHPSNLogFactory());



            container.AddNewExtension<Interception>();


        }

        [TestMethod]
        public void Verify_OnlineDiagnosis_Functionality()
        {
            var diagnosisService = container.Resolve<IDiagnosis>();
            var result = diagnosisService.MakeADiagnosis("0000003001777361", "000000300177736111111", "8798", "15", "just get out here 111");
            Assert.AreEqual(0, result.HasError);
        }

        [TestMethod]
        public void Verify_DiagnosisService_Functionality()
        {
            var patientId = "0000003001778193";

            var diagnosisService = container.Resolve<IDiagnosis>();

            var result = diagnosisService.GetDiagnosisHistory(patientId);

            Assert.AreEqual(5, result.Count);
        }
    }
}
