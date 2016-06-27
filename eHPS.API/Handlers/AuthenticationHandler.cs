//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// HTTP 摘要算法 认证Handler
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
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using System.IdentityModel.Tokens;

namespace eHPS.API.Handlers
{
    public class AuthenticationHandler:DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
        {
            HttpRequestHeaders headers = request.Headers;
            if (headers.Authorization != null)
            {
                var header = new Header(headers.Authorization.Parameter, request.Method.Method);

                if (Nonce.IsValid(header.Nonce, header.NounceCounter))
                {
                    var uzkey = ConfigurationManager.AppSettings["UZKey"];
                    var uzsecret = ConfigurationManager.AppSettings["UZSecret"];

                    var hash1 = HashHelper.GetMD5(String.Format("{0}:{1}:{2}", uzkey, header.Realm, uzsecret));
                    var hash2 = HashHelper.GetMD5(String.Format("{0}:{1}", header.Method, header.Uri));

                    var computedResponse = HashHelper.GetMD5(String.Format("{0}:{1}:{2}:{3}:{4}:{5}",hash1,header.Nonce,header.NounceCounter,header.Cnonce,"auth",hash2));

                    if(String.CompareOrdinal(header.Response, computedResponse)==0)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,header.UserName),
                            new Claim(ClaimTypes.AuthenticationMethod,AuthenticationMethods.Password)
                        };

                        ClaimsPrincipal principal = new ClaimsPrincipal(new[] { new ClaimsIdentity(claims,"Digest")});

                        Thread.CurrentPrincipal = principal;
                        if(HttpContext.Current !=null)
                        {
                            HttpContext.Current.User = principal;
                        }

                    }


                }


            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Digest", Header.UnauthorizedResponseHeader("lenovohit.com").ToString()));
            }
            return response;
        }
    }
}