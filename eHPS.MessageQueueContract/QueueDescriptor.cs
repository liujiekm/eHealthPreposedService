//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 消息队列约定的队列名称以及队列内部传递的参数格式
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/8/1 17:36:08
// 版本号：  V1.0.0.0
//===================================================================================




using eHPS.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.MessageQueueContract
{
    /// <summary>
    /// 消息队列约定的队列名称以及队列内部传递的参数格式
    /// </summary>
    public class QueueDescriptor
    {

        /// <summary>
        /// 患者待收费项目队列描述
        /// </summary>
        public static readonly Tuple<String,Type> AwareOrderBooked = new Tuple<String, Type>("AwareOrderBooked",typeof(List<PatientConsumption>));


    }
}
