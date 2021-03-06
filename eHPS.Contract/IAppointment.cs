﻿//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 预约服务接口
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/24 16:51:43
// 版本号：  V1.0.0.0
//===================================================================================

using eHPS.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace eHPS.Contract
{
    /// <summary>
    /// 预约服务接口
    /// </summary>
    public interface IAppointment
    {


        /// <summary>
        /// 获取医生可预约信息
        /// </summary>
        /// <param name="areaId">院区标识</param>
        /// <param name="deptId">科室标识</param>
        /// <param name="doctorId">医生标识</param>
        /// <param name="registerOrAppointment">标识是挂号还是预约</param>
        /// <param name="startTime">排班开始时间</param>
        /// <param name="endTime">排班结束时间</param>
        /// <returns>医生可预约信息</returns>
        List<BookableDoctor> GetBookableInfo(String areaId, String deptId, String doctorId,String registerOrAppointment,DateTime? startTime,DateTime? endTime);


        /// <summary>
        /// 获取患者的预约历史
        /// </summary>
        /// <param name="patientId">患者标识</param>
        /// <param name="mobile">患者手机</param>
        /// <returns>患者的预约历史</returns>
        List<BookHistory> GetAppointmentHistory(String patientId, String mobile);


        /// <summary>
        /// 预约 ,需包含如下逻辑
        /// 预约之前的判断条件，验证是否可以预约
        /// 进行预约操作
        /// </summary>
        /// <param name="appointment">发起预约对象模型</param>
        ResponseMessage<BookHistory> MakeAnAppointment(MakeAnAppointment appointment);


        #region 如果预约无需预约到精确时间点则无需实一下方法

        /// <summary>
        /// 如果预约无需预约到精确时间点则无需实现改方法
        /// 获得指定排班下面的可预约时间点清单
        /// </summary>
        /// <param name="arrangeId">排班标识</param>
        /// <returns>排班区间内的时间点划分</returns>
        List<BookableTimePoint> GetBookableTimePoint(String arrangeId);


        /// <summary>
        /// 如果预约无需预约到精确时间点则无需实现改方法
        /// 检查当前时间点或者预约序号是否被占用
        /// </summary>
        /// <param name="bookTime">预约时间点</param>
        /// <param name="bookSequence">预约序号</param>
        /// <param name="arrangeId">排班标识</param>
        /// <returns>当前时间点是否可预约</returns>
        bool IsTimePointBooked(DateTime? bookTime, Int32? bookSequence, string arrangeId);


        #endregion



        /// <summary>
        /// 取消指定预约
        /// </summary>
        /// <param name="apponintId">预约标识</param>
        /// <returns>取消预约操作的返回消息体</returns>
        ResponseMessage<string> CancelTheAppointment(String apponintId);






    }
}
