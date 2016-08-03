//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 患者在医院交易的账户信息
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/8/3 13:43:51
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
    /// 患者在医院交易的账户信息
    /// </summary>
    public class TradingAccount
    {

        /// <summary>
        /// 当前账号是否可用
        /// </summary>
        public Boolean Avaliable { get; set; }


        /// <summary>
        /// 患者标识
        /// </summary>
        public String PatientId
        { get; set; }

        /// <summary>
        /// 当前账号可用余额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
