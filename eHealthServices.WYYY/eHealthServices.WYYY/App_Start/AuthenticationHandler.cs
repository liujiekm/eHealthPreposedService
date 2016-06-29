using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Xml;

namespace eHealthServices.WYYY.App_Start
{
    public class AuthenticationHandler : System.Net.Http.DelegatingHandler
    {
        /// <summary>
        /// 获取AppIDSecret
        /// </summary>
        /// <param name="appID"></param>
        /// <returns></returns>
        public string FindSecret(string appID)
        {
            string sec = System.Configuration.ConfigurationManager.AppSettings["AppIDSecret"].ToString();
            return sec;
        }

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            //var routeData = request.GetRouteData().Values;
            //if (IsUnAuthentication(routeData))//如果不用过滤
            //    return base.SendAsync(request, cancellationToken);
            try
            {
                IEnumerable<string> AppIDs;
                if (!request.Headers.TryGetValues("appid", out AppIDs))
                    return base.SendAsync(request, cancellationToken);
                IEnumerable<string> Secrets;
                if (!request.Headers.TryGetValues("secret", out Secrets))
                    return base.SendAsync(request, cancellationToken);
                IEnumerable<string> Timestamps;
                if (!request.Headers.TryGetValues("timestamp", out Timestamps))//20150528093456
                    return base.SendAsync(request, cancellationToken);
                if (AppIDs == null || Secrets == null || Timestamps == null || string.IsNullOrWhiteSpace(AppIDs.FirstOrDefault()) || string.IsNullOrWhiteSpace(Secrets.FirstOrDefault()) || string.IsNullOrWhiteSpace(Timestamps.FirstOrDefault()))
                    return base.SendAsync(request, cancellationToken);
                DateTime dt;
                if (!DateTime.TryParseExact(Timestamps.FirstOrDefault(), "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out dt))
                    return base.SendAsync(request, cancellationToken);
                var ExpiredTime = System.Configuration.ConfigurationSettings.AppSettings["ExpiredTime"].ToString();//权限过期时间
                if (Math.Abs((DateTime.Now.ToUniversalTime() - dt).TotalMinutes) > Convert.ToInt32(ExpiredTime))//前后时间相差不能超过15分钟
                    return base.SendAsync(request, cancellationToken);
                string appid = AppIDs.FirstOrDefault();
                if (appid != System.Configuration.ConfigurationManager.AppSettings["AppID"].ToString())//如果医院id不一致直接返回
                    return base.SendAsync(request, cancellationToken);
                string sec = FindSecret(appid);//获取明文secret
                if (string.IsNullOrWhiteSpace(sec))
                    return base.SendAsync(request, cancellationToken);
                string secret = string.Format("{0}{1}{2}", appid, sec, Timestamps.FirstOrDefault());
                secret = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(secret, "MD5");//md5加密（大写）
                if (secret != Secrets.FirstOrDefault())
                    return base.SendAsync(request, cancellationToken);

                //var appid = "222";

                if (!string.IsNullOrWhiteSpace(appid))//set user
                {
                    Thread.CurrentPrincipal = HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(appid), null);
                }
                return base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return base.SendAsync(request, cancellationToken);
            }
        }

    }
}