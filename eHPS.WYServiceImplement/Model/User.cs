﻿//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 系统内用户信息
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/7/4 16:02:40
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.WYServiceImplement.Model
{
    public class User
    {

        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserId { set; get; }


        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { set; get; }
    }
}
