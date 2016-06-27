//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// Hash帮助类
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/27 10:10:53
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.Common
{
    public static class HashHelper
    {
        public static string GetMD5(string value, Encoding encoding = null)
        {

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = encoding.GetBytes(value);
            return GetMD5(bytes);
        }

        /// <summary>
        /// 获取字节数组的MD5哈希值
        /// </summary>
        public static string GetMD5(byte[] bytes)
        {

            StringBuilder sb = new StringBuilder();
            MD5 hash = new MD5CryptoServiceProvider();
            bytes = hash.ComputeHash(bytes);
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取字符串的SHA1哈希值
        /// </summary>
        public static string GetSHA1(string value, Encoding encoding = null)
        {
            StringBuilder sb = new StringBuilder();
            SHA1Managed hash = new SHA1Managed();

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] bytes = hash.ComputeHash(encoding.GetBytes(value));
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取字符串的Sha256哈希值
        /// </summary>
        public static string GetSHA256(string value, Encoding encoding = null)
        {


            StringBuilder sb = new StringBuilder();
            SHA256Managed hash = new SHA256Managed();

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] bytes = hash.ComputeHash(encoding.GetBytes(value));
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取字符串的Sha512哈希值
        /// </summary>
        public static string GetSHA512(string value, Encoding encoding = null)
        {


            StringBuilder sb = new StringBuilder();
            SHA512Managed hash = new SHA512Managed();

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = hash.ComputeHash(encoding.GetBytes(value));
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

    }
}
