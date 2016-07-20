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

using eHPS.API.Models;
using eHPS.Contract;
using eHPS.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

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



        /// <summary>
        /// 获取医生可预约情况
        /// 排班情况等
        /// </summary>
        /// <param name="request">获取医生可预约情况的请求包装对象</param>
        /// <returns></returns>
        [Route("DoctorAppoint"),HttpPost,ResponseType(typeof(List<BookableDoctor>))]
        public List<BookableDoctor> GetBookableInfo([FromBody]BookableInfoRequest request)
        {
            return appointmentService.GetBookableInfo(request.areaId, request.doctorId, request.startTime, request.endTime);
        }


        /// <summary>
        /// 获取患者的预约历史
        /// </summary>
        /// <param name="request">获取医生预约历史的请求包装对象</param>
        /// <returns></returns>
        [Route("AppointmentHistory"),HttpPost, ResponseType(typeof(List<BookHistory>))]
        public List<BookHistory> GetAppointmentHistory([FromBody]AppointmentHistoryRequest request)
        {
            return appointmentService.GetAppointmentHistory(request.PatientId, request.Mobile);
        }


        /// <summary>
        /// 预约 ,需包含如下逻辑
        /// 预约之前的判断条件，验证是否可以预约
        /// 进行预约操作
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns></returns>
        [Route("DoAppointment"), HttpPost, ResponseType(typeof(ResponseMessage<BookHistory>))]
        public ResponseMessage<BookHistory> MakeAnAppointment([FromBody]MakeAnAppointment appointment)
        {
            return appointmentService.MakeAnAppointment(appointment);
        }



        /// <summary>
        /// 取消指定预约
        /// </summary>
        /// <param name="apponintId">预约标识</param>
        /// <returns></returns>
        [Route("CancelAppointment"), HttpPost]
        public ResponseMessage<string> CancelTheAppointment([FromBody]String apponintId)
        {
            return appointmentService.CancelTheAppointment(apponintId);
        }

    }
}
