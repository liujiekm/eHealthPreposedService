using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Linq;
using eHealthServices.WYYY.Areas.HelpPage.ModelDescriptions;
using eHealthServices.WYYY.Areas.HelpPage.Models;
using eHealthServices.WYYY.Models;

namespace eHealthServices.WYYY.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";

        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public ActionResult Index(string appid, string secret, string timestamp)
        {
            #region 身份验证
            if (HttpContext.Session["Hospital"] == null && (string.IsNullOrWhiteSpace(appid) || string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(timestamp)))
            { 
                //跳转到
                return Redirect(Url.Content("~/Help/UnLogin"));
            }
            else if (HttpContext.Session["Hospital"] == null)//有身份直接跳转
            {
                DateTime dt;
                if (!DateTime.TryParseExact(timestamp, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out dt))
                    return Redirect(Url.Content("~/Help/UnLogin"));
                var ExpiredTime = System.Configuration.ConfigurationSettings.AppSettings["ExpiredTime"].ToString();//权限过期时间
                if (Math.Abs((DateTime.Now.ToUniversalTime() - dt).TotalMinutes) > Convert.ToInt32(ExpiredTime))//前后时间相差不能超过15分钟
                    return Redirect(Url.Content("~/Help/UnLogin"));
                int hid;
                if(!int.TryParse(appid,out hid))
                    return Redirect(Url.Content("~/Help/UnLogin"));
                using (var db = new eHealthServices.WYYY.Areas.HelpPage.Models.ehealthdbEntities()) {
                    var temp = db.EH_Hospital.Where(a => a.HID == hid).FirstOrDefault();
                    if(temp==null)
                        return Redirect(Url.Content("~/Help/UnLogin"));
                    var hospital = new Hospital()
                    {
                        HID = hid,
                        Mode = temp.Mode.HasValue ? temp.Mode.Value : 0,
                        Secret = temp.Secret,
                        ServiceURL = temp.ServiceURL,
                        ShortName = temp.ShortName
                    };
                    string MD5secret = string.Format("{0}{1}{2}", appid, hospital.Secret, timestamp);
                    MD5secret = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(MD5secret, "MD5");//md5加密（大写）
                    if (secret != MD5secret)
                        return Redirect(Url.Content("~/Help/UnLogin"));
                    HttpContext.Session["Hospital"] = hospital;
                    ViewBag.MyHospital = HttpContext.Session["Hospital"] as Hospital;
                }
            }
            else {
                ViewBag.MyHospital = HttpContext.Session["Hospital"] as Hospital;
            }
            #endregion

            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

     

        public ActionResult Api(string apiId)
        {
            #region 身份验证
            if (HttpContext.Session["Hospital"] == null)
                return Redirect(Url.Content("~/Help/UnLogin"));
            else {
                ViewBag.MyHospital = HttpContext.Session["Hospital"] as Hospital;
            }
            #endregion

            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }


        public ActionResult UnLogin()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult TestApi(RequestTestApi param) {
           
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            if (string.IsNullOrWhiteSpace(param.Method))
            {
                result.Data = new {data="调用方法为空！" };
            }
            else {
                if (HttpContext.Session["Hospital"] == null)
                    result.Data = new { data = "请先登录！" };
                else
                {
                    var xml = string.IsNullOrWhiteSpace(param.XML) ? string.Empty : System.Web.HttpUtility.UrlDecode(param.XML);
                    var hospital =HttpContext.Session["Hospital"] as Hospital;
                    //hospital.ServiceURL += param.Method;
                    var dataXml=eHealthServices.WYYY.Areas.HelpPage.App_Start.RestSharpHelper.GetXML(new Hospital() { HID = hospital.HID, Mode = hospital.Mode, Secret = hospital.Secret, ServiceURL = hospital.ServiceURL + param.Method, ShortName = hospital.ShortName }, xml) ;

                    result.Data = new { data =System.Web.HttpUtility.HtmlEncode(dataXml) };
                }
            }

            return result;
        }


        public ActionResult ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }
    }
}