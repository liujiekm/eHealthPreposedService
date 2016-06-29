using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lenovo.Tool.File
{
   public class PathHelper
    {
        /// <summary>
        /// 获取服务器根物理路径  bin的上一个目录
        /// </summary>
        public static string WebRootPath
        {
            get { return HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath); }
        }
    }
}
