using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Net.Http.Headers;

namespace eHPS.API.Test
{
    [TestClass]
    public class DigestTest
    {
        [TestMethod]
        public void Call_From_Client_Use_Digest()
        {
            var requestUri = new Uri("http://localhost:51797/BasicInfo/Depts"); 
            var credCache = new CredentialCache 
            { 
                { 
                    new Uri("http://localhost:51797/BasicInfo/Depts"),  
                    "Digest",  
                    new NetworkCredential("1", "csy","lenovohit.com") 
                } 
            }; 
            using (var clientHander = new HttpClientHandler 
            { 
                Credentials = credCache, 
                PreAuthenticate = true 
            }) 
            using (var httpClient = new HttpClient(clientHander)) 
            {
                
                var content = "01";
                var responseTask = httpClient.PostAsJsonAsync(requestUri.ToString(), content); 
                responseTask.Result.EnsureSuccessStatusCode();

                Assert.AreEqual("", responseTask.Result.Content.ReadAsStringAsync().Result);
            } 

        }



        [TestMethod]
        public void Make_Call_Use_Digest()
        {
            string msg = string.Empty;
            Request("http://localhost:51797/Test/Get/", "GET", "dman", "nothing", out msg);
            Assert.AreEqual("", msg);
        }



        private static string Request(string sUrl, string sMethod, string sEntity, string sContentType, out string sMessage)
        {
            try
            {
                sMessage = "";
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    client.Credentials = CreateAuthenticateValue(sUrl);
                    Uri url = new Uri(sUrl);
                    byte[] bytes = Encoding.UTF8.GetBytes(sEntity);
                    byte[] buffer;
                    switch (sMethod.ToUpper())
                    {
                        case "GET":
                            buffer = client.DownloadData(url);
                            break;
                        case "POST":
                            buffer = client.UploadData(url, "POST", bytes);
                            break;
                        default:
                            buffer = client.UploadData(url, "POST", bytes);
                            break;
                    }

                    return Encoding.UTF8.GetString(buffer);
                }
            }
            catch (WebException ex)
            {
                sMessage = ex.Message;
                var rsp = ex.Response as HttpWebResponse;
                var httpStatusCode = rsp.StatusCode;
                var authenticate = rsp.Headers.Get("WWW-Authenticate");

                return "";
            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
                return "";
            }
        }

        private static CredentialCache CreateAuthenticateValue(string sUrl)
        {
            CredentialCache credentialCache = new CredentialCache();
            credentialCache.Add(new Uri(sUrl), "Digest", new NetworkCredential("  blackbody  ", "  username password "));

            return credentialCache;
        }




        [TestMethod]
        public  void Test_WYService()
        {
            WYService.n_webserviceSoapClient client = new WYService.n_webserviceSoapClient();
            String patientId = "0000003001777361";
            String response = "";
            
            var result = client.f_get_data("getmzyz", ref patientId, ref response);

            Assert.IsNotNull("", response);
        }


        [TestMethod]
        public void Test_WebService()
        {
            Test.TestService.Service1SoapClient client = new TestService.Service1SoapClient();

            var result = client.HelloWorld();

            Assert.IsNotNull(result);


        }

    }
}
