//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// HTTP 摘要验证 抽象基类
// 
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/27 10:10:53
// 版本号：  V1.0.0.0
//===================================================================================


using eHPS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Controllers;

namespace eHPS.API.Filter
{
    public abstract class DigestAuthorizationFilterAttributeBase : StandardAuthorizationFilterAttributeBase
    {

        private const string Scheme = "Digest";


        public DigestAuthorizationFilterAttributeBase(bool issureChallenge) : base(issureChallenge)
        {
        }

        protected override string GetAuthenticatedUser(HttpActionContext actionContext)
        {
            var auth = actionContext.Request.Headers.Authorization;
            if(auth==null||auth.Scheme!=Scheme)
            {
                return null;
            }

            var header = new Handlers.Header(actionContext.Request.Headers.Authorization.Parameter, actionContext.Request.Method.Method);

            if (!Handlers.Nonce.IsValid(header.Nonce, header.NounceCounter))
            {
                return null;
            }

            var password = GetPassword(header.UserName);

            //



            var hash1 = HashHelper.GetMD5(String.Format(
                 "{0}:{1}:{2}",
                 header.UserName,
                 header.Realm,
                 password));
        


             var hash2 = HashHelper.GetMD5(String.Format(
                 "{0}:{1}",
                 header.Method,
                 header.Uri));
        


             var computedResponse = HashHelper.GetMD5(String.Format(
                 "{0}:{1}:{2}:{3}:{4}:{5}",
                 hash1,
                 header.Nonce,
                 header.NounceCounter,
                 header.Cnonce,
                 "auth",
                 hash2));

             return header.Response.Equals(computedResponse, StringComparison.Ordinal)
 ? header.UserName
                 : null;

        }


        protected abstract string GetPassword(string userName);
        protected override AuthenticationHeaderValue GetUnauthorizedResponseHeader(HttpActionContext actionContext)
        {
            var host = actionContext.Request.RequestUri.DnsSafeHost;
            var header = Handlers.Header.UnauthorizedResponseHeader("lenovohit.com");

            var parameter = header.ToString();

            return new AuthenticationHeaderValue(Scheme, parameter);
        }


    }
}