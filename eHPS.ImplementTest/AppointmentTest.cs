using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using eHPS.CrossCutting.Logging;
using Microsoft.Practices.Unity.InterceptionExtension;
using eHPS.Contract;
using eHPS.WYServiceImplement;
using eHPS.CrossCutting.NetFramework.ExceptionHandler;
using eHPS.CrossCutting.NetFramework.Logging;
using Jil;
using eHPS.Contract.Model;

namespace eHPS.ImplementTest
{
    [TestClass]
    public class AppointmentTest
    {

        private IAppointment appointmentService;
        private IBasicInfo basicInfoService;
        private UnityContainer container;

        [TestInitialize]
        public void Initialize()
        {
            container = new UnityContainer();

            container.RegisterType<IAppointment, AppointmentService>(new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<LoggingInterceptionBehavior>());
            container.RegisterType<IBasicInfo, BasicInfoService>(new Interceptor<InterfaceInterceptor>(),
            new InterceptionBehavior<LoggingInterceptionBehavior>());


            //设定日志类
            LoggerFactory.SetCurrent(new eHPSNLogFactory());



            container.AddNewExtension<Interception>();


        }


        [TestMethod]
        public void Get_Dept_BookableInfo()
        {
            var appointmentService = container.Resolve<IAppointment>();
            var areaId = "01";
            var doctorId = "333";
            var deptId = "901";
            var result = appointmentService.GetBookableInfo(areaId,deptId,doctorId, new DateTime(2015,1,1),new DateTime(2016,1,1));

            var jiled = JSON.Serialize(result);



            
            Assert.AreEqual(122,result.Count);
            
        }


        [TestMethod]
        public void Make_An_Appointment()
        {
            var appointmentService = container.Resolve<IAppointment>();
            var makeAnAppointment = new MakeAnAppointment {
                 

            };
            var response = appointmentService.MakeAnAppointment(makeAnAppointment);

            Assert.AreEqual(true, response.HasError);
        }

        [TestMethod]
        public void Veriry_GetAppointmentHistory()
        {
            var appointmentService = container.Resolve<IAppointment>();

            var patientId = "0000003001777362";
            var mobile = "";
            var result = appointmentService.GetAppointmentHistory(patientId,mobile);

            Assert.IsNotNull(result);
        }
    }
}
