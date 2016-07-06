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
        public void Verify_DiagnosisService_Functionality()
        {
            var patientId = "0000003001778193";

            var diagnosisService = container.Resolve<IDiagnosis>();

            var result = diagnosisService.GetDiagnosisRecord(patientId);

            Assert.AreEqual(5, result.Count);
        }
    }
}
