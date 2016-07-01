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
    public interface IBasicInfo
    {
        /// <summary>
        /// 获得医院内的科室组织结构
        /// </summary>
        /// <returns>
        /// Item1:科室唯一标识
        /// Item2:父节点科室唯一标识
        /// Item3:科室名称
        /// </returns>
        List<Tuple<String, String, String>> GetDepts();



        /// <summary>
        /// 获得所有医生信息
        /// </summary>
        /// <returns></returns>
        List<Doctor> GetDoctors();



        /// <summary>
        ///  根据医生标示获取医生信息
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        Doctor GetDoctorById(String doctorId);

    }
}
