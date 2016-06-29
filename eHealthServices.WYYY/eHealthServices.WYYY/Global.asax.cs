using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml;
using System.Xml.Serialization;

namespace eHealthServices.WYYY
{

    #region xml序列化时去除命名空间
    public class CustomNamespaceXmlFormatter : XmlMediaTypeFormatter
    {
        //public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        //{
        //    try
        //    {
        //        if (type == null)
        //        {
        //            return ReadFromStreamAsync(type, readStream, content, formatterLogger);
        //        }
        //        if (readStream == null)
        //        {
        //            return ReadFromStreamAsync(type, readStream, content, formatterLogger);
        //        }
        //        Task<object> result;
        //        try
        //        {
        //            result = Task.FromResult<object>(this.ReadFromStream(type, readStream, content, formatterLogger));
        //        }
        //        catch (Exception exception)
        //        {
        //            return ReadFromStreamAsync(type, readStream, content, formatterLogger);
        //        }
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ReadFromStreamAsync(type, readStream, content, formatterLogger);
        //    }
        //}

        //private object ReadFromStream(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        //{
        //    HttpContentHeaders httpContentHeaders = (content == null) ? null : content.Headers;
        //    if (httpContentHeaders != null && httpContentHeaders.ContentLength == 0L)
        //    {
        //        return MediaTypeFormatter.GetDefaultValueForType(type);
        //    }
        //    object deserializer = this.GetDeserializer(type, content);
        //    object result;
        //    try
        //    {
        //        using (XmlReader xmlReader = this.CreateXmlReader(readStream, content))
        //        {
        //            XmlSerializer xmlSerializer = deserializer as XmlSerializer;
        //            if (xmlSerializer != null)
        //            {
        //                result = xmlSerializer.Deserialize(xmlReader);
        //            }
        //            else
        //            {
        //                XmlObjectSerializer xmlObjectSerializer = deserializer as XmlObjectSerializer;
        //                if (xmlObjectSerializer == null)
        //                {
        //                   //抛出异常

        //                }
        //                result = xmlObjectSerializer.ReadObject(xmlReader);
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        if (formatterLogger == null)
        //        {
        //            throw;
        //        }
        //        formatterLogger.LogError(string.Empty, exception);
        //        result = MediaTypeFormatter.GetDefaultValueForType(type);
        //    }
        //    return result;
        //}

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            try
            {
                var xns = new XmlSerializerNamespaces();
                foreach (var attribute in type.GetCustomAttributes(true))
                {
                    var xmlRootAttribute = attribute as XmlRootAttribute;
                    if (xmlRootAttribute != null)
                    {
                        xns.Add(string.Empty, xmlRootAttribute.Namespace);
                    }
                }

                if (xns.Count == 0)
                {
                    xns.Add(string.Empty, string.Empty);
                }

                var task = Task.Factory.StartNew(() =>
                {
                    var serializer = new XmlSerializer(type);
                    serializer.Serialize(writeStream, value, xns);
                });

                return task;
            }
            catch (Exception)
            {
                return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
            }
        }
    }
    #endregion
    public class WebApiApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            #region xml序列化
            var config = GlobalConfiguration.Configuration;
            config.Formatters.Clear();
            config.Formatters.Add(new CustomNamespaceXmlFormatter() { UseXmlSerializer = true });//返回的对象一定不能是接口，而要是可以new的class，不然无法序列化

           
            #endregion

            #region 系统日志
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~") + @"\log4net.config"));
            #endregion

            //解决RestSharp请求https匿名证书
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    var req = System.Web.HttpContext.Current.Request;
        //    if (req.HttpMethod == "OPTIONS")//过滤options请求，用于js跨域
        //    {
        //        Response.StatusCode = 200;
        //        Response.SubStatusCode = 200;
        //        Response.End();//tongl1
        //    }
        //}

    }
}
