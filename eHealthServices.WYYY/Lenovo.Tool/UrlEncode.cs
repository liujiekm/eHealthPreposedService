using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lenovo.Tool
{
   public class UrlEncode
    {
        /// <summary>
        /// 编码加密 
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string Encrypt(string strKey)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(strKey);
            return HttpServerUtility.UrlTokenEncode(buffer);
        }
        /// <summary>
        /// 编码解密
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string Decrypt(string strKey)
        {
            byte[] buffer = HttpServerUtility.UrlTokenDecode(strKey);
            return System.Text.Encoding.UTF8.GetString(buffer);
        }
    }
}
