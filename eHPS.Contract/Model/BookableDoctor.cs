//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 可预约的医生信息模型
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/6 11:20:39
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
    /// 可预约的医生信息模型
    /// </summary>
    public class BookableDoctor
    {

        /// <summary>
        /// 医生标识
        /// </summary>
        public String DocotorId { get; set; }


        /// <summary>
        /// 医生姓名
        /// </summary>
        public String DoctorName { get; set; }

        /// <summary>
        /// 头像图片链接
        /// </summary>
        public String PhotoUrl { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// 科室标识
        /// </summary>
        public String DeptId { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public String  DeptName { get; set; }

        /// <summary>
        /// 职位标识
        /// </summary>
        public String JobTitleId { get; set; }


        /// <summary>
        /// 职位名称
        /// </summary>
        public String JobTitle { get; set; }


        ///// <summary>
        ///// 可预约号数
        ///// </summary>
        //public Int32 BookableNum { get; set; }


        /// <summary>
        /// 已使用号源数
        /// </summary>
        public Int32 UsedBookNum { set; get; }

        /// <summary>
        /// 号源总数
        /// </summary>
        public Int32 SumBookNum { get; set; }



        /// <summary>
        /// 剩余号源
        /// </summary>
        public String UnusedBookNum {
            get
            {
                if(SumBookNum - UsedBookNum<=0)
                {
                    return "";
                }
                else
                {
                    return (SumBookNum - UsedBookNum).ToString();
                }
                
            }
        }

        /// <summary>
        /// 诊疗类型
        /// 1 普通、2 专家
        /// </summary>
        public String DiagnosisType { get; set; }

        /// <summary>
        /// 排班标识
        /// </summary>
        public String ArrangeId { get; set; }



        


        /// <summary>
        /// 排班开始时间
        /// </summary>
        public DateTime ArrangeStartTime { get; set; }

        /// <summary>
        /// 排班结束时间
        /// </summary>
        public DateTime ArrangeEndTime { get; set; }


        ///// <summary>
        ///// 是否专家
        ///// </summary>
        //public Boolean IsExpert { get; set; }


        

        /// <summary>
        /// 挂号金额
        /// </summary>
        public Decimal RegisteredAmount { get; set; }


        private AppointmentState _appointmentState;

        /// <summary>
        /// 医生可预约状态
        /// </summary>       
        [JilDirective(TreatEnumerationAs =typeof(Int32))]
        public AppointmentState AppointmentState {
            get {
                if(UsedBookNum>= SumBookNum)
                {
                    this._appointmentState=  AppointmentState.FullyBooked;
                }
                else
                {
                    this._appointmentState = AppointmentState.Bookable;
                }
                return this._appointmentState;

            }
            set
            {
                this._appointmentState = value;
            }
        }


        /// <summary>
        /// 当前排班下的可预约的时间点
        /// 如果实施医院不支持预约到时间点 则为空
        /// </summary>
        public List<BookableTimePoint> BookableTimePoints { get; set; }




        /// <summary>
        /// 备注说明
        /// 专病信息等
        /// </summary>
        public string Remark { get; set; }


        ///// <summary>
        ///// 是否专病
        ///// </summary>
        //public Boolean IsSpecialDisease { get; set; }



        ///// <summary>
        ///// 专病说明
        ///// </summary>
        //public String SpecialDiseaseState { get; set; }


    }

    /// <summary>
    /// 医生可预约排班状态
    /// </summary>
    public enum AppointmentState
    {

        /// <summary>
        /// 1可以预约
        /// </summary>
        Bookable = 1,

        /// <summary>
        /// 2已经预满
        /// </summary>
        FullyBooked = 2,

        /// <summary>
        /// 3已经过期
        /// </summary>
        Expired = 3,


        /// <summary>
        /// 4其他原因不可预约
        /// </summary>
        OtherReason = 4
    }


}
