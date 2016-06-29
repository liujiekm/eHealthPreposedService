using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHealthServices.WYYY.Models
{
    /// <summary>
    /// 就诊人信息，医院请求时，如果需要身份，则改对象为完整的就诊人信息
    /// </summary>
    public class PatientInfo
    {
        /// <summary>
        /// 就诊人id
        /// </summary>
        public long PID { get; set; }
        /// <summary>
        /// 患者真实姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 1男，2女，3未知
        /// </summary>
        public string SEX { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 就诊卡
        /// </summary>
        public string HospitalCard { get; set; }
        /// <summary>
        /// 医院预留手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 传递的id
        /// </summary>
        public string KeyID { get; set; }
        /// <summary>
        /// 预约时间，有实现‘获取时间点列表’接口则此字段值为预约时间，否则为空
        /// </summary>
        public DateTime AppTime { get; set; }
        /// <summary>
        /// 预约序号，有实现‘获取时间点列表‘接口则此字段值为预约序号，否则为空
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 备注，有实现‘获取时间点列表’接口则此字段值为接口返回值‘备注’字段内容，否则为空
        /// </summary>
        public string Remark { get; set; }
    }







    ///// <summary>
    ///// 就诊人信息，医院请求时，如果需要身份，则改对象为完整的就诊人信息
    ///// </summary>
    //public class PatientInfo
    //{
    //    /// <summary>
    //    /// 就诊人id
    //    /// </summary>
    //    public long PID { get; set; }
    //    /// <summary>
    //    /// 患者真实姓名
    //    /// </summary>
    //    public string Name { get; set; }
    //    /// <summary>
    //    /// 1男，2女，3未知
    //    /// </summary>
    //    public string SEX { get; set; }

    //    /// <summary>
    //    /// 身份证
    //    /// </summary>
    //    public string IDCard { get; set; }
    //    /// <summary>
    //    /// 就诊卡
    //    /// </summary>
    //    public string HospitalCard { get; set; }
    //    /// <summary>
    //    /// 医院预留手机号
    //    /// </summary>
    //    public string PhoneNumber { get; set; }

    //    ///// <summary>
    //    ///// 传递的id
    //    ///// </summary>
    //    //public string KeyID { get; set; }
    //    /// <summary>
    //    /// 预约时间，有实现‘获取时间点列表’接口则此字段值为预约时间，否则为空
    //    /// </summary>
    //    public DateTime AppTime { get; set; }
    //    /// <summary>
    //    /// 预约序号，有实现‘获取时间点列表‘接口则此字段值为预约序号，否则为空
    //    /// </summary>
    //    public int Index { get; set; }
    //    ///// <summary>
    //    ///// 备注，有实现‘获取时间点列表’接口则此字段值为接口返回值‘备注’字段内容，否则为空
    //    ///// </summary>
    //    //public string Remark { get; set; }
    //}
}