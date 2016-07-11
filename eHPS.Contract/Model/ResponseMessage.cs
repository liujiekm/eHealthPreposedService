//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 执行命令的返回信息：
// 比如 预约、支付、
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/11 16:53:01
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.Contract.Model
{
    public class ResponseMessage<T> where T:class
    {

        /// <summary>
        /// 命令执行是否哟错误
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        public T Body { get; set; }
    }
}
