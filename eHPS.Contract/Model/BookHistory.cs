//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 患者预约历史
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/6 14:26:21
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jil;

namespace eHPS.Contract.Model
{
    /// <summary>
    /// 患者预约历史
    /// </summary>
    public class BookHistory
    {



        /// <summary>
        /// 患者标识
        /// </summary>
        public String  PatientId { get; set; }

        /// <summary>
        /// 排班标识
        /// </summary>
        public String ArrangeId { get; set; }


        /// <summary>
        /// 预约标识
        /// </summary>
        public String AppointId { get; set; }

        /// <summary>
        /// 预约医生标识
        /// </summary>
        public String DoctorId { get; set; }



        /// <summary>
        /// 预约医生姓名
        /// </summary>
        public String  DoctorName { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime AppointTime { set; get; }


        /// <summary>
        /// 预约序号
        /// </summary>
        public Int32 AppointSequence { set; get; }



        [JilDirective(TreatEnumerationAs =typeof(Int32))]
        public AppointState AppointState { get; set; }


        /// <summary>
        /// 注意事项
        /// </summary>
        public String Attention { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 挂号收费金额
        /// </summary>
        public Decimal RegisteredAmount { get; set; }


        /// <summary>
        ///患者姓名
        /// </summary>
        public String PatientName { get; set; }


        /// <summary>
        /// 患者手机号
        /// </summary>
        public String PatientMobile { get; set; }



        /// <summary>
        /// 患者身份证
        /// </summary>
        public String  PatientIdCard { get; set; }




        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime {  get; set; }


    }




    /// <summary>
    /// 患者预约状态
    /// </summary>
    public enum AppointState
    {
        /// <summary>
        /// 预约中
        /// </summary>
        Appointing=1,


        /// <summary>
        /// 挂号中
        /// </summary>
        Registering=2,

        /// <summary>
        /// 等待叫号
        /// </summary>
        WaitingForCall=3,

        /// <summary>
        /// 正在就诊
        /// </summary>
        InTreatment=4,

        /// <summary>
        /// 就诊完成
        /// </summary>
        TreatmentFinished =5,


        /// <summary>
        /// 已经取消
        /// </summary>
        AlreadyCanceled=9


    }


}
