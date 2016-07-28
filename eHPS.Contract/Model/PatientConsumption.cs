//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 推送服务推送的用户待支付项目
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/28 15:51:44
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
    /// 推送服务推送的用户待支付项目
    /// </summary>
    public class PatientConsumption
    {

        /// <summary>
        /// 当前医院在当前平台的医院标识
        /// </summary>
        public String AppId { get; set; }



        public List<TreatmentActivityInfo> TreatmentActivityInfos { get; set; }

        

        /// <summary>
        /// 患者标识
        /// </summary>
        public String PatientId { get; set; }


        /// <summary>
        /// 患者姓名
        /// </summary>
        public String PatientName { get; set; }

        


       
    }
}
