using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace eHealth.Date.DAL.Common
{
    public class ImgFile
    {
        /// <summary>
        /// C:\tongl\AndroidApp\NewImg
        /// </summary>
        public static string WebRootPath
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["ServerImgFilePath"].ToString(); }
        }
        public static string HttpWebRootPath
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["HttpImgFilePath"].ToString(); }
        }
        public static string WebImgPath
        {
            get { return Path.Combine(WebRootPath, "ImgFile"); }
        }
        public static string WebImgDoctorPath
        {
            get { return Path.Combine(WebRootPath, "ImgDoctor"); }
        }
        public static string HttpWebImgPath
        {
            get { return string.Format("{0}/{1}/", HttpWebRootPath, "ImgFile"); }
        }
        public static string HttpWebImgDoctorPath
        {
            get { return string.Format("{0}/{1}/", HttpWebRootPath, "ImgDoctor"); }
        }
        public static bool IsHaveImg(string id, string type, out string imgName)
        {
            imgName = Lenovo.Tool.UrlEncode.Encrypt(id) + "." + type;
            return System.IO.File.Exists(Path.Combine(WebImgPath, imgName));
        }
        public static bool IsHaveImgDoctor(string id, string type, out string imgName)
        {
            imgName = Lenovo.Tool.UrlEncode.Encrypt(id) + "." + type;
            return System.IO.File.Exists(Path.Combine(WebImgDoctorPath, imgName));
        }

    }
}
