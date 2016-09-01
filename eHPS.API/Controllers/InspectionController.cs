//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 检验检查单对外服务
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
using System.Web.Http.Description;

namespace eHPS.API.Controllers
{
    /// <summary>
    /// 检验检查单对外服务
    /// </summary>
    [RoutePrefix("Inspection")]//,Authorize
    public class InspectionController : ApiController
    {

        private IInspection inspectionService;

        public InspectionController(IInspection inspectionService)
        {
            this.inspectionService = inspectionService;
        }


        /// <summary>
        /// 根据用户标识获取检查检验报告详情
        /// </summary>
        /// <param name="patientId">患者标识</param>
        /// <returns></returns>
        [Route("InspectionDetail"),HttpPost, ResponseType(typeof(List<InspectionReportDetail>))]
        public List<InspectionReportDetail> GetInspectionReportDetailByPatientId([FromBody]string patientId)
        {
            return inspectionService.GetInspectionReportDetailByPatientId(patientId);
        }



    }
}
