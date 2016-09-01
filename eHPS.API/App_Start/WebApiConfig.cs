using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using eHPS.API.Formatter;
using eHPS.API.LogAttribute;
using eHPS.API.Resolver;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using eHPS.API.Handlers;
using eHPS.API.Filter;
using eHPS.API.DependencyBootstrap;

namespace eHPS.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            // 将 Web API 配置为仅使用不记名令牌身份验证。
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            //启用跨域
            config.EnableCors();
            //增加摘要算法验证


            //config.Filters.Add(new DigestAuthorizationFilterAttribute());


            //替换默认的JSON序列化器JSON.NET为Jil
            config.Formatters.Clear();
            config.Formatters.Insert(0, new JilFormatter());

            //压缩payload
            //config.Filters.Add(new DeflateCompressionAttribute());

            //增加错误日志记录
            config.Filters.Add(new ElmahErrorAttribute());


            //设置Dependency Resolver
            var container = new UnityContainer();
            ContainerBootstrap.Configure(container);
            config.DependencyResolver = new UnityResolver(container);

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
