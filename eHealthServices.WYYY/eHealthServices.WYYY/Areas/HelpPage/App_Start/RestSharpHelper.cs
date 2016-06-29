using eHealthServices.WYYY.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHealthServices.WYYY.Areas.HelpPage.App_Start
{
    public class RestSharpHelper
    {

        public static string GetXML(Hospital hospital, string xml) {
            var surl=hospital.ServiceURL;
            Uri uri = new Uri(surl);
            if (hospital == null || string.IsNullOrWhiteSpace(hospital.ServiceURL) || hospital.Mode == 1)
                return null;
            RestClient client = new RestClient(string.Format("{0}://{1}", uri.Scheme, uri.Authority));
            var request = new RestRequest(uri.AbsolutePath.Trim('/'), Method.POST);
            request.AddHeader("Accept", "application/xml");

            #region 身份加密
            request.AddHeader("appid", hospital.HID.ToString());
            string timestamp = DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss");
            request.AddHeader("timestamp", timestamp);
            string secret = string.Format("{0}{1}{2}", hospital.HID, hospital.Secret, timestamp);
            secret = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(secret, "MD5");//md5加密（大写）
            request.AddHeader("secret", secret);
            #endregion

            const string contentType = "text/xml;charset=UTF-8";// "text/plain;charset=UTF-8";
            IRestResponse response;
            try
            {
                request.AddParameter(contentType, xml, ParameterType.RequestBody);//设置xml格式的参数
                response = client.Execute(request);
                return response.Content;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}