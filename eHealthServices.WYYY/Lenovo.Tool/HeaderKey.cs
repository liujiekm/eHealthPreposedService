using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lenovo.Tool
{
   public class HeaderKey
    {  /// <summary>
        /// 数据库用户
        /// </summary>
       public static string cDataBaseStr = System.Configuration.ConfigurationSettings.AppSettings["app_news"].ToString();
        public static string GetCurrentDBUser(string userName)
        {
            return string.IsNullOrWhiteSpace(cDataBaseStr) ? userName : cDataBaseStr;
        }
    }
}
