using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHealthServices.WYYY.Models
{
    #region request

    public class RequestDoctor
    {
        /// <summary>
        /// 医院医生代码
        /// </summary>
        public string DoctorCode { get; set; }

        public long UID { get; set; }
    }

    #endregion

    #region response

    public class ResponseDoctorBase : Models.BaseModel
    {
        /// <summary>
        /// 医生基本信息
        /// </summary>
        public DoctorBase Data { get; set; }
    }
    public class DoctorBase : RequestDoctor
    {
        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        public string HospitalID { get; set; }
         /// <summary>
        /// 1简单模式（数据托管在云端），2开发者模式（医院提供webservice），3高级开发者模式
        /// </summary>
        public int HospitalMode { get { return 2; } }
        public string HospitalName { get; set; }
        /// <summary>
        /// 科室名
        /// </summary>
        public string DepName { get; set; }
        /// <summary>
        /// 科室id
        /// </summary>
        public string DepID { get; set; }
        ///// <summary>
        ///// 关注人数
        ///// </summary>
        //public int FocusNumber { get; set; }
        /// <summary>
        /// 职称 主任医师 等
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// 头像位置
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 擅长
        /// </summary>
        public string Expert { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNum { get; set; }

    }

    public class ResponseDoctorInfo : Models.BaseModel
    {
        public DoctorBase Data { get; set; }
    }
    public class ResponseSearchDoctorBase : Models.BaseModel
    {
        /// <summary>
        /// 医生列表
        /// </summary>
        public List<DoctorBase> Data { get; set; }
    }

    #endregion
}