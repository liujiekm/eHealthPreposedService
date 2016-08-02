//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 平台服务调用返回类型
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/8/2 11:00:54
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.Contract.Model
{
    public class PlatformServiceResponse<T> where T : class
    {
        /// <summary>
        /// 命令执行是否哟错误
        /// 0 正确 1 有问题
        /// </summary>
        public Int32 HasError { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        public T Data { get; set; }
    }
}
