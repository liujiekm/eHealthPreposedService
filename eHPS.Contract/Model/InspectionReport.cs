//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 检查、检验报告
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/1 17:26:02
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
    /// 检查、检验报告总览
    /// </summary>
    public class InspectionReport
    {

        /// <summary>
        /// 报告标识
        /// </summary>
        //public string BGBS { get; set; }
        public string ReportId { get; set; }

        //public string BRBH { get; set; }


        /// <summary>
        /// 病人就诊卡标识
        /// </summary>
        public string PatientId { get; set; }


        //public string BRXM { get; set; }


        /// <summary>
        /// 病人姓名
        /// </summary>
        public string PatientName { get; set; }




        //public DateTime BGSJ { get; set; }


        /// <summary>
        /// 出具报告时间
        /// </summary>
        public DateTime ReportTime { get; set; }


        //public string JCLX { get; set; }

        /// <summary>
        /// 检查检验类型中文名称
        /// </summary>
        public string InspectionTypeName { get; set; }

        //public string JCLX1 { get; set; }

        /// <summary>
        /// 检查检验类型代码
        /// </summary>
        public string InspectionTypeCode { get; set; }

        /// <summary>
        /// 检查检验详细名称
        /// </summary>
        public string InspectionName { get; set; }
        //public string JYMC { get; set; }
    }
}
