//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 诊疗记录
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/5 10:28:48
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.Contract.Model
{

    /// <summary>
    /// 诊疗记录
    /// </summary>
    public class DiagnosisRecord
    {
        /// <summary>
        /// 诊疗记录id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 诊疗时间
        /// </summary>
        public DateTime DiagnosisTime { get; set; }
        /// <summary>
        /// 诊断结果
        /// </summary>
        public string DiagnosisInfo { get; set; }
        /// <summary>
        /// 病史信息
        /// </summary>
        public string MedicalHistory { get; set; }
        /// <summary>
        /// 体检信息
        /// </summary>
        public string Examination { get; set; }
        /// <summary>
        /// 化验信息
        /// </summary>
        public string Laboratory { get; set; }
    }
}
