//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 时间操作工具类
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/11 14:15:19
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.Common
{
    /// <summary>
    /// 时间操作工具类
    /// </summary>
    public class DateHelper
    {
        public static Int64 GetSeconds(DateTime dt1, DateTime dt2)
        {
            TimeSpan ts = dt2 - dt1;
            return ts.Seconds;
        }
    }
}
