//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 基础信息服务，包含：
// 医院科室信息，科室人员信息等
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
    /// 基础信息服务，包含：
    /// 医院科室信息，科室人员信息等
    /// </summary>
    [RoutePrefix("BasicInfo")]//,Authorize
    public class BasicInfoController : ApiController
    {

        private IBasicInfo basicInfoService;

        public BasicInfoController(IBasicInfo basicInfoService)
        {
            this.basicInfoService = basicInfoService;
        }




        /// <summary>
        /// 获得医院内的科室组织结构
        /// </summary>
        /// <param name="areaId">院区标识</param>
        /// <returns></returns>
        [Route("Depts"), HttpPost,ResponseType(typeof(List<Organization>))]
        public List<Organization> GetDepts([FromBody]string areaId)
        {
            return basicInfoService.GetDepts(areaId);
        }




        /// <summary>
        /// 获得科室下医生信息
        /// </summary>
        /// <param name="deptId">科室标识</param>
        /// <returns></returns>
        [Route("Doctors"), HttpPost, ResponseType(typeof(List<Doctor>))]
        public List<Doctor> GetDoctors([FromBody]string deptId)
        {
            return basicInfoService.GetDoctors(deptId);
        }

        /// <summary>
        /// 查询医生信息
        /// </summary>
        /// <param name="model">查询变量</param>
        /// <returns></returns>
        [Route("SearchDoctors"), HttpPost, ResponseType(typeof(List<Doctor>))]
        public List<Doctor> GetDoctors([FromBody]DoctorSearchModel model)
        {
            return basicInfoService.GetDoctors(model.Name,model.Spelling);
        }








        /// <summary>
        ///  根据医生标示获取医生信息
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        [Route("Doctor"), HttpPost, ResponseType(typeof(Doctor))]
        public Doctor GetDoctorById([FromBody]string doctorId)
        {
            return basicInfoService.GetDoctorById(doctorId);
        }



        /// <summary>
        /// 根据患者就诊卡获取患者基本信息
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [Route("Patient"), HttpPost, ResponseType(typeof(Patient))]
        public Patient GetPatientInfo([FromBody]string patientId)
        {
            return basicInfoService.GetPatientInfo(patientId);
        }



        /// <summary>
        /// 根据患者注册的手机号码获取患者基本信息
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns></returns>
        [Route("Patients"),HttpPost, ResponseType(typeof(List<Patient>))]
        public List<Patient> GetPatientInfoByMobile([FromBody]string mobile)
        {
            return basicInfoService.GetPatientInfoByMobile(mobile);
        }

    }
}
