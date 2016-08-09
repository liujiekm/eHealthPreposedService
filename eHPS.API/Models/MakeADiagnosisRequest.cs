//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 发起在线诊疗请求包装对象
// 
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/24 16:51:43
// 版本号：  V1.0.0.0
//===================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHPS.API.Models
{
    /// <summary>
    /// 发起在线诊疗请求包装对象
    /// </summary>
    public class MakeADiagnosisRequest
    {
        /// <summary>
        /// 患者标识
        /// </summary>
        public String PatientId { get; set; }

        /// <summary>
        /// 医生标识
        /// </summary>
        public String DoctorId { get; set; }


        /// <summary>
        /// 科室标识
        /// </summary>

        public String DeptId { get; set; }

        /// <summary>
        /// 主诉内容
        /// </summary>
        public String Complaint { get; set; }

        /// <summary>
        /// 患者在互联网医院平台的标识
        /// </summary>
        public String PId { get; set; }
    }
}