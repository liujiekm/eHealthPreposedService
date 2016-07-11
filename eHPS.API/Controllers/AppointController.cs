//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 预约对外服务
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/24 16:51:43
// 版本号：  V1.0.0.0
//===================================================================================

using eHPS.Contract;
using eHPS.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eHPS.API.Controllers
{

    /// <summary>
    /// 对外预约服务
    /// </summary>
    [RoutePrefix("Appointment"),Authorize]
    public class AppointmentController : ApiController
    {
        private IBasicInfo basicInfoService;
        private IAppointment appointmentService;

        public AppointmentController(IBasicInfo basicInfoService,IAppointment appointmentService)
        {
            this.basicInfoService = basicInfoService;
            this.appointmentService = appointmentService;
        }


        [Route("DoctorAppoint")]
        public List<BookableDoctor> GetBookableInfo(String doctorId, DateTime? startTime, DateTime? endTime)
        {
            return appointmentService.GetBookableInfo(doctorId, startTime, endTime);
        }




    }
}
