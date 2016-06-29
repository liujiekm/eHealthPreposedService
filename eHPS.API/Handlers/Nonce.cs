//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// HTTP 摘要算法 Nonce类实现
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/6/27 10:10:53
// 版本号：  V1.0.0.0
//===================================================================================

using eHPS.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace eHPS.API.Handlers
{
    public  class Nonce
    {
        //线程安全的nonces
        private static ConcurrentDictionary<string, Tuple<int, DateTime>> nonces = new ConcurrentDictionary<string, Tuple<int, DateTime>>();



        /// <summary>
        /// 创建nonce随机数
        /// </summary>
        /// <returns></returns>
        public static string  Generate()
        {
            byte[] bytes = new byte[16];

            using (var rngProvider = new RNGCryptoServiceProvider())
            {
                rngProvider.GetBytes(bytes);
            }

            string nonce = HashHelper.GetMD5(bytes);

            //插入nonce值以及过期时间
            nonces.TryAdd(nonce, new Tuple<int, DateTime>(0, DateTime.Now.AddMinutes(10)));

            return nonce;

        }


        /// <summary>
        /// 在服务器端验证随机数
        /// </summary>
        /// <param name="nonce"></param>
        /// <param name="nonceCount"></param>
        /// <returns></returns>
        public static bool IsValid(string nonce,string nonceCount)
        {
            Tuple<int, DateTime> cachedNonce = null;
            //每个nonce只允许使用一次
            nonces.TryRemove(nonce, out cachedNonce);

            if(cachedNonce !=null)
            {
                //nonce 的count必须比记录（内存中）的大
                if(Int32.Parse(nonceCount)>cachedNonce.Item1)
                {
                    //nonce 有过期时间
                    if(cachedNonce.Item2>DateTime.Now)
                    {
                        //更新nonce 递增
                        //nonces[nonce] = new Tuple<int, DateTime>(Int32.Parse(nonceCount)+1, cachedNonce.Item2);
                        return true;
                    }
                }
            }
            return false;
        }



    }
}