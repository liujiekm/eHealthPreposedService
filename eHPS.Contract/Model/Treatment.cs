//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 患者一次诊疗活动模型，包含（医疗数据信息、费用信息）
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/18 10:57:11
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
    /// 患者一次诊疗活动模型，包含（医疗数据信息、费用信息）
    /// </summary>
    public class Treatment
    {

        /// <summary>
        /// 当前医院在当前平台的医院标识
        /// </summary>
        public String AppId { get; set; }


        /// <summary>
        /// 诊疗活动标识
        /// </summary>
        public String TreatmentId { get; set; }

        /// <summary>
        /// 科室标识
        /// </summary>
        public String DeptdId { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public String DeptName { get; set; }



        /// <summary>
        /// 医生标识
        /// </summary>
        public String  DoctorId { get; set; }



        /// <summary>
        /// 医生姓名
        /// </summary>
        public String DoctorName { get; set; }


        /// <summary>
        /// 主诉
        /// </summary>
        public String Complaint { get; set; }

        /// <summary>
        /// 患者标识
        /// </summary>
        public String PatientId { get; set; }


        /// <summary>
        /// 患者姓名
        /// </summary>
        public String PatientName { get; set; }

        /// <summary>
        /// 患者诊断信息
        /// </summary>
        public List<Diagnostics> Diagnostics { get; set; }


        /// <summary>
        /// 患者订单信息
        /// </summary>
        public List<Order> Orders { get; set; }
    }
}
