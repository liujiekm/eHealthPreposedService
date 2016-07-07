using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using eHPS.CrossCutting.Logging;
using Microsoft.Practices.Unity.InterceptionExtension;
using eHPS.Contract;
using eHPS.WYServiceImplement;
using eHPS.CrossCutting.NetFramework.ExceptionHandler;
using eHPS.CrossCutting.NetFramework.Logging;

namespace eHPS.ImplementTest
{
    [TestClass]
    public class AppointmentTest
    {

        private IAppointment appointmentService;
        private UnityContainer container;

        [TestInitialize]
        public void Initialize()
        {
            container = new UnityContainer();

            container.RegisterType<IAppointment, AppointmentService>(new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<LoggingInterceptionBehavior>());

            //设定日志类
            LoggerFactory.SetCurrent(new eHPSNLogFactory());



            container.AddNewExtension<Interception>();


        }


        [TestMethod]
        public void Get_Dept_BookableInfo()
        {
            var appointmentService = container.Resolve<IAppointment>();

            var deptId = "33";
            var result = appointmentService.GetBookableInfo(deptId, new DateTime(2015,1,1),new DateTime(2016,1,1));


            Assert.AreEqual(122,result.Count);
            
        }
    }
}
