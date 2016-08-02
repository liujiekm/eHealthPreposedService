//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 可预约的时间点信息
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/11 13:09:41
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
    /// 可预约的时间点信息
    /// </summary>
    public class BookableTimePoint
    {

        /// <summary>
        /// 可预约的预约序号
        /// </summary>
        public Int32 AppointSequence { set; get; }

        /// <summary>
        /// 可预约的时间点
        /// </summary>
        public DateTime AppointTime { get; set; }




        /// <summary>
        /// 预约号描述
        /// 如果没有预约到时间点则显示 预约序号
        /// 如果预约到时间点则显示预约序号+$+预约时间点
        /// </summary>
        public String ArrangeIndicate {

            get {

                if(AppointTime!=null)
                {
                    return AppointSequence + "$" + AppointTime.ToUniversalTime();
                }
                else
                {
                    return AppointSequence.ToString();
                }
                
            }
        }


    }
}
