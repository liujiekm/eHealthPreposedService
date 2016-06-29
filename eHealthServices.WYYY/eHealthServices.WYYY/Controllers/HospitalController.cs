using eHealthServices.WYYY.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;

namespace eHealthServices.WYYY.Controllers
{


    [EnableCors("*", "*", "*")]
    [Authorize]
    public partial class HospitalController : ApiController
    {
        private static string WebRootPath
        {
            get { return System.Web.HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath); }
        }
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T ConverToT<T>() where T:class
        {
            try
            {
                System.IO.Stream s = System.Web.HttpContext.Current.Request.InputStream;
                if (s == null || s.Length <= 0)
                    return null;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                var postStr = System.Text.Encoding.UTF8.GetString(b);
                using (StringReader sr = new StringReader(postStr))
                {
                    var xz = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    T t = xz.Deserialize(sr) as T;
                    return t;
                }
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return null;
            }
        }

        ///// <summary>
        ///// 这是一个demo
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public Models.ResponseDemo Demo() { 
        //    //根据该方法的使用情况，判断是否需要参数
        //    var param = ConverToT<RequestDemo>();
        //    if (param == null)
        //        return new Models.ResponseDemo() { HasError = 1, ErrorMessage = "参数为null！" };

        //     //根据param.Name参数来写业务逻辑

        //    return new ResponseDemo() {  Data=null};

        //}

        /// <summary>
        /// 模块：医院导航
        /// </summary>
        /// <returns>医院导航科室位置列表</returns>
        [HttpPost]
        public Models.ResponseHospitalNavigation HospitalNavigation()
        {
            var data = new List<HospitalDepNavigation>();
            XmlDocument doc = new XmlDocument();
            var path = System.IO.Path.Combine(WebRootPath, "File\\FloorInfo.xml");
            doc.Load(path);
            XmlNode xn = doc.SelectSingleNode("FLOORS");
            XmlNodeList xnl = xn.ChildNodes;
            foreach (XmlNode xn1 in xnl)
            {
                XmlElement xe = (XmlElement)xn1;
                data.Add(new HospitalDepNavigation()
                {
                    DName = xe.InnerText,
                    Navigation = xe.GetAttribute("name").ToString()
                });
            }
            return new ResponseHospitalNavigation() { Data = data };
        }

    }
}
