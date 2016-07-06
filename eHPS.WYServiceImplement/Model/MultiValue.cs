//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 温附一多值表
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/5 13:26:37
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.WYServiceImplement.Model
{

    /// <summary>
    /// 温附一多值表
    /// </summary>
    public class MultiValue
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string MC { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string LB { get; set; }

        //代码
        public string DM { get; set; }
    }
}
