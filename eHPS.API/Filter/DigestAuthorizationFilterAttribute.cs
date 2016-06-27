//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// HTTP 摘要验证具体实现类
// 
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/27 10:10:53
// 版本号：  V1.0.0.0
//===================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHPS.API.Filter
{

    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Method,AllowMultiple =false)]
    public class DigestAuthorizationFilterAttribute : DigestAuthorizationFilterAttributeBase
    {
        public DigestAuthorizationFilterAttribute(bool issureChallenge=true) : base(issureChallenge)
        {
        }

        protected override string GetPassword(string userName)
        {
            return userName;
        }

        protected override bool IsUserAuthorized(string userName)
        {
            return true;
        }
    }
}