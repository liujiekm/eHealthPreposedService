using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;

namespace eHealthServices.WYYY.Models
{


#region 预约

    #region request
    public class DoAppointmentParam : PatientInfo
    {
        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime AppTime { get; set; }
        /// <summary>
        /// 预约序号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    public class AppointmentHistoryParam
    {
        /// <summary>
        /// 可选 就诊卡号
        /// </summary>
        public string Card { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }

    }
    /*
    public class DoAppointmentParam : RequestDoctor
    {
        /// <summary>
        /// 预约排班id
        /// </summary>
        public string AppointmentID { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string DepName { get; set; }


        /// <summary>
        /// 可选 就诊卡号
        /// </summary>
        public string Card { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime? AppointmentTime { get; set; }
        
    }*/

    public class UnAppointmentParam
    {
        public string AppointmentID { get; set; }
    }
    public class AppointmentSignParam
    {
        public string AppointmentID { get; set; }
        /// <summary>
        /// 病人编号  可选，无卡预约签到用
        /// </summary>
        public string CardID { get; set; }
    }

    #endregion

    #region response
    /// <summary>
    /// 简单的实体
    /// </summary>
    public class Simple
    {
        /// <summary>
        /// 科室id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string Title { get; set; }

    }
    public class AppointmentHistory
    {/// <summary>
        /// *预约状态：1预约成功；2等待；3已经呼叫；4已经进入；5已经完成；9已经放弃
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 门诊类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 收费金额
        /// </summary>
        public string Charge { get; set; }
        /// <summary>
        /// 可预约方式：1，有卡预约；2，根据姓名和手机号进行预约；3，完整信息进行预约：姓名、手机号、身份证号
        /// </summary>
        public int AppWay { get; set; }
        /// <summary>
        /// *预约排班id
        /// </summary>
        public string ScheduleID { get; set; }
        /// <summary>
        /// *医生姓名
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string DepName { get; set; }
        /// <summary>
        /// *医生代码
        /// </summary>
        public string DoctorCode { get; set; }

        /// <summary>
        /// 可选 就诊卡号
        /// </summary>
        public string Card { get; set; }
        /// <summary>
        /// *用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// *预约序号
        /// </summary>
        public int? AppointmentNo { get; set; }
        /// <summary>
        /// *预约时间
        /// </summary>
        public DateTime AppointmentTime { get; set; }
        /// <summary>
        /// 医院预约id
        /// </summary>
        public string AppointmentCode { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StateName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        
    }

    public class ResponseAppointmentHistory : Models.BaseModel
    {
        /// <summary>
        /// 预约历史记录
        /// </summary>
        public List<AppointmentHistory> Data { get; set; }
    }

    public class AppointmentState
    {
        /// <summary>
        /// 大于0就是成功，否则失败
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 如果失败，失败原因
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 若成功，预约记录信息
        /// </summary>
        public AppointmentHistory Appointment{get;set;}
    }

    public class ResponseSignInfo : Models.BaseModel
    {
        /// <summary>
        /// 签到小票信息
        /// </summary>
        public SignInInfo Data { get; set; }
    }
    public class SignInInfo
    {
        /// <summary>
        /// 科室名称
        /// </summary>
        public string DepName { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 诊室名称
        /// </summary>
        public string RoomName { get; set; }
        /// <summary>
        /// 诊室位置
        /// </summary>
        public string RoomSite { get; set; }
        /// <summary>
        /// 院区名称
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 预约序号（第几位就诊）
        /// </summary>
        public int OrderIndex { get; set; }
        /// <summary>
        /// 签到时间（可选）
        /// </summary>
        public DateTime? SignTime { get; set; }
        /// <summary>
        /// 预约时间（可选）
        /// </summary>
        public DateTime? OrderTime { get; set; }
    }

    public class ResponseAppointmentState : Models.BaseModel
    {
        /// <summary>
        /// 预约结果数据
        /// </summary>
        public AppointmentState Data { get; set; }
    }

    public class ResponseAppointment : Models.BaseModel
    {
        /// <summary>
        /// 医生多科室排班列表
        /// </summary>
        public List<DepartmentSchediling> Data { get; set; }
    }

    public class DepartmentSchediling : Simple
    {
        /// <summary>
        /// 排班列表
        /// </summary>
        public List<SchedulingInfo> Data { get; set; }
    }

    public class ResponseAppointmentTime : Models.BaseModel
    {
        /// <summary>
        /// 时间点列表
        /// </summary>
        public List<AppointmentTime> Data { get; set; }
    }
   
    /// <summary>
    /// 具体的可预约数 预约方式精确到每个排版是有用的，不同排版类型的预约方式有很多种
    /// </summary>
    public class SchedulingInfo
    {
        /// <summary>
        /// 排班id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 是否可以预约 1可以预约，2已经预满，3已经过期，4其他原因不可预约
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 门诊类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 收费金额
        /// </summary>
        public string Charge { get; set; }
        /// <summary>
        /// 可预约方式：1，有卡预约；2，根据姓名和手机号进行预约；3，完整信息进行预约：姓名、手机号、身份证号
        /// </summary>
        public int AppWay { get; set; }

        /// <summary>
        /// 有无时间点列表,0无，1有（需要实现获取时间点接口：AppointmentTimes）
        /// </summary>
        public int HaveAppTimeList { get; set; }

    }
    /// <summary>
    /// 时间点信息
    /// </summary>
    public class AppointmentTime
    {
        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime AppTime { get; set; }
        /// <summary>
        /// 预约序号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否可预约 1可预约，0不可预约（界面显示为灰色，用户不可以进行预约）
        /// </summary>
        public int IsUseable { get; set; }
    }


    /// <summary>
    /// 医生简介 --预约的
    /// </summary>
    public class DoctorAppiontment : DoctorBase
    {
        /// <summary>
        /// 是否可以预约 1可以预约，2已经预满，3已经过期，4其他原因不可预约
        /// </summary>
        public int State { get; set; }

    }

    public class ResponseDoctorAppiontment : Models.BaseModel
    {
        /// <summary>
        /// 医生列表
        /// </summary>
        public List<DoctorAppiontment> Data { get; set; }
    }

    #endregion

#endregion
}