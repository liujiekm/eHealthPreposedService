//===================================================================================
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
    public interface IAppointment
    {


        /// <summary>
        /// 获取医生可预约信息
        /// </summary>
        /// <param name="deptId">医生标识</param>
        /// <param name="startTime">排班开始时间</param>
        /// <param name="endTime">排班结束时间</param>
        /// <returns></returns>
        List<BookableDoctor> GetBookableInfo(String doctorId,DateTime? startTime,DateTime? endTime);



        //List<BookableDoctor> GetBookableInfo(String doctorId, DateTime? startTime, DateTime? endTime);




        /// <summary>
        /// 获取患者的预约历史
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        List<BookHistory> GetAppointmentHistory(String patientId);



        /// <summary>
        /// 预约
        /// </summary>
        /// <param name="obj"></param>
        void MakeAnAppointment(MakeAnAppointment appointment);



        /// <summary>
        /// 取消指定预约
        /// </summary>
        /// <param name="obj"></param>
        void CancelTheAppointment(String apponintId);



        /// <summary>
        /// 推送可预约的医生信息
        /// </summary>
        /// <returns></returns>
        List<BookableDoctor> PushBookableDoctors();


    }
}
