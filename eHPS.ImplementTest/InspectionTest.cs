using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eHPS.CrossCutting.Logging;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using eHPS.CrossCutting.NetFramework.ExceptionHandler;
using eHPS.CrossCutting.NetFramework.Logging;
using eHPS.Contract;
using eHPS.WYServiceImplement;

namespace eHPS.ImplementTest
{
    [TestClass]
    public class InspectionTest
    {

        private IInspection inspectionService;
        private UnityContainer container;

        [TestInitialize]
        public void Initialize()
        {
            container = new UnityContainer();

            container.RegisterType<IInspection, InspectionService>(new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<LoggingInterceptionBehavior>());

            //设定日志类
            LoggerFactory.SetCurrent(new eHPSNLogFactory());



            container.AddNewExtension<Interception>();


        }

        //[TestMethod]
        //public void Verify_Command_Could_Query()
        //{
        //    var patientId = "59622015000091";


        //    inspectionService = container.Resolve<IInspection>();
        //    var result = inspectionService.GetInspectionReportByPatientId(patientId);

        //    Assert.AreEqual("尿常规", result[0].InspectionName);

        //}

        [TestMethod]
        public void Verify_Inspection_Query()
        {
            var patientId = "0000003001777900";
            inspectionService = container.Resolve<IInspection>();
            var result = inspectionService.GetInspectionReportDetailByPatientId(patientId);
            Assert.IsNotNull(result);
        }
    }
}
