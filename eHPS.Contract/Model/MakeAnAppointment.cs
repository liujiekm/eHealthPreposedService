//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 预约行为
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/6 14:39:33
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
    /// 预约行为
    /// </summary>
    public class MakeAnAppointment
    {

        /// <summary>
        /// 患者标识（就诊卡）
        /// </summary>
        public String PatientId { get; set; }

        /// <summary>
        /// 排班标识
        /// </summary>
        public String ArrangeId { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime? AppointTime { get; set; }


        /// <summary>
        /// 预约序号
        /// </summary>
        public Int32? AppointSequence { get; set; }

        /// <summary>
        /// 预约号描述
        /// 如果没有预约到时间点则显示 预约序号
        /// 如果预约到时间点则显示预约序号+$+预约时间点
        /// </summary>
        public String ArrangeIndicate { get; set; }



        /// <summary>
        /// 患者姓名
        /// </summary>
        public String PatientName { get; set; }

        /// <summary>
        /// 患者手机号码
        /// </summary>
        public String  Mobile { get; set; }


        /// <summary>
        /// 患者身份证
        /// </summary>
        public String  PatientIdCard { get; set; }


    }
}
