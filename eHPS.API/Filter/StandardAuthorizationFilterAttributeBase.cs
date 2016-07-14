//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// HTTP 基本身份验证以及摘要验证 抽象基类
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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace eHPS.API.Filter
{
    public abstract class StandardAuthorizationFilterAttributeBase:AuthorizationFilterAttribute
    {
        /// <summary>
        /// 标示是否发起验证
        /// </summary>
        private readonly bool _issueChallenge;

        private static bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }


        protected StandardAuthorizationFilterAttributeBase(bool issureChallenge)
        {
            this._issueChallenge = issureChallenge;
        }


        /// <summary>
        /// 获取经过验证的用户
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected abstract string GetAuthenticatedUser(HttpActionContext actionContext);

        /// <summary>
        ///用户是否授权过
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        protected abstract bool IsUserAuthorized(string userName);

        /// <summary>
        /// 获取未经授权的response header信息
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected abstract AuthenticationHeaderValue GetUnauthorizedResponseHeader(HttpActionContext actionContext);



        /// <summary>
        /// 认证失败的response
        /// </summary>
        /// <param name="actionContext"></param>
        private void Challenge(HttpActionContext actionContext)
        {
            if(!_issueChallenge)
            {
                return;
            }

            var headerValue = GetUnauthorizedResponseHeader(actionContext);
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);

            response.Headers.WwwAuthenticate.Add(headerValue);

            actionContext.Response = response;


        }



        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //开启HTTPS
            //if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            //{
            //    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
            //    {
            //        ReasonPhrase = "HTTPS Required"
            //    };
            //    return;
            //}





            if (IsAuthenticated)
            {
                return;
            }

            var userName = GetAuthenticatedUser(actionContext);
            if(String.IsNullOrWhiteSpace(userName))
            {
                Challenge(actionContext);
                return;
            }

            var isUserAuthorized = IsUserAuthorized(userName);
            if(!isUserAuthorized)
            {
                Challenge(actionContext);
                return;
            }


            SetIdentity(actionContext, userName);

        }




        /// <summary>
        /// 验证通过过 设置上下文的Principla信息
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="userName"></param>
        protected virtual void SetIdentity(HttpActionContext actionContext,string userName)
        {
            var identity = new GenericIdentity(userName);
            var principal = new GenericPrincipal(identity, new string[0]);

            Thread.CurrentPrincipal = principal;

            if(HttpContext.Current!=null)
            {
                HttpContext.Current.User = principal;
            }


            var context = actionContext.Request.GetRequestContext();
            context.Principal = principal;

        }

    }
}