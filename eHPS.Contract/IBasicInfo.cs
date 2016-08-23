//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 医院基础数据
// 科室、院区、人员信息等
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/28 15:56:58
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
    /// 医院基础数据
    /// 科室、院区、人员信息等
    /// </summary>
    public interface IBasicInfo
    {
        /// <summary>
        /// 获得医院内的科室组织结构
        /// </summary>
        /// <param name="areaId">院区标识</param>
        /// <returns>当前院区内的科室组织结构</returns>
        List<Organization> GetDepts(string areaId);



        /// <summary>
        /// 获得科室下医生信息
        /// </summary>
        /// <param name="deptId">科室标识</param>
        /// <returns>科室下医生信息清单</returns>
        List<Doctor> GetDoctors(string deptId);



        /// <summary>
        /// 根据姓名或者拼音查询医生信息
        /// 
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="spelling">拼音</param>
        /// <returns>查询获取的医生信息清单</returns>
        List<Doctor> GetDoctors(string name, string spelling);




        /// <summary>
        /// 获取科室名称
        /// </summary>
        /// <param name="deptId">科室标识</param>
        /// <returns>科室名称</returns>
        string GetDeptName(string deptId);



        /// <summary>
        ///  根据医生标示获取医生信息
        /// </summary>
        /// <param name="doctorId">医生标识</param>
        /// <returns>医生信息</returns>
        Doctor GetDoctorById(String doctorId);



        /// <summary>
        /// 根据患者就诊卡获取患者基本信息
        /// </summary>
        /// <param name="patientId">患者标识</param>
        /// <returns>患者信息</returns>
        Patient GetPatientInfo(string patientId);

        /// <summary>
        /// 根据患者注册的手机号码获取患者基本信息
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns>患者信息清单</returns>
        List<Patient> GetPatientInfoByMobile(string mobile);


        /// <summary>
        /// 获取挂号费用
        /// </summary>
        /// <param name="diagnosisTypeId">诊疗类型</param>
        /// <param name="jobTitleId">挂牌工种</param>
        /// <returns>挂号费用</returns>
        decimal GetRegisteredAmount(string diagnosisTypeId, string jobTitleId);

    }
}
