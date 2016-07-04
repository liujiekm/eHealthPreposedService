//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 检查、检验报告 详情
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/1 17:26:33
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
    /// 检查、检验报告 详情
    /// </summary>
    public class InspectionReportDetail
    {
        /// <summary>
        /// 报告标识
        /// </summary>
        public string ReportId { get; set; }


        /// <summary>
        /// 用户标识
        /// </summary>
        public string PatientId { get; set; }


        /// <summary>
        /// 用户姓名
        /// </summary>
        public string PatientName { get; set; }
        /// <summary>
        /// 报告时间
        /// </summary>
        public DateTime ReportTime { get; set; }
        /// <summary>
        /// 报告者
        /// </summary>
        public string Reporter { get; set; }


        /// <summary>
        /// 检查检验类型代码
        /// </summary>
        public string InspectionTypeCode { get; set; }


        /// <summary>
        /// 检查检验类型中文名称
        /// </summary>
        public string InspectionTypeName { get; set; }
        /// <summary>
        /// 检查标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 检查结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 检查结果诊断
        /// </summary>
        public string Diagnostic { get; set; }
        /// <summary>
        /// 检验结果
        /// </summary>
        public List<LaboratoryItemDetail> Details { get; set; }
    }
}
