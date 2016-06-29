//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// HTTP 摘要算法 身份凭证信息
// Authorization.Parameter 参数
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
using System.Text;
using System.Web;

namespace eHPS.API.Handlers
{
    public class Header
    {
        public Header() { }


        //客户端随机数
        public string Cnonce { get; private set; }

        //服务端随机数
        public string Nonce { get; private set; }
        public string Realm { get; private set; }
        public string UserName { get; private set; }
        public string Uri { get; private set; }
        public string Response { get; private set; }
        public string Method { get; private set; }
        public string NounceCounter { get; private set; }


        public Header(string header,string method)
        {
            string keyValuePairs = header.Replace("\"", String.Empty);

            foreach (string keyValuePair in keyValuePairs.Split(','))
            {
                int index = keyValuePair.IndexOf("=", StringComparison.Ordinal);
                string key = keyValuePair.Substring(0, index);
                string value = keyValuePair.Substring(index + 1);

                switch (key.Trim())
                {
                    case "username": this.UserName = value; break;
                    case "realm": this.Realm = value; break;
                    case "nonce": this.Nonce = value; break;
                    case "uri": this.Uri = value; break;
                    case "nc": this.NounceCounter = value; break;
                    case "cnonce": this.Cnonce = value; break;
                    case "response": this.Response = value; break;
                    case "method": this.Method = value; break;

                }
            }

            if(String.IsNullOrEmpty(this.Method))
            {
                this.Method = method;
            }


        }


        /// <summary>
        /// 使用这个属性处理程序生成 nonce 打包在WWW-Authenticate头,
        /// 作为401响应的一部分,会生成一个随机数key发送给客户端
        /// </summary>
        public static Header UnauthorizedResponseHeader(string realm)
        {
                return new Header()
                {
                    Realm = realm,
                    Nonce = eHPS.API.Handlers.Nonce.Generate()

                };
            
        }

        public override string ToString()
        {
            StringBuilder header = new StringBuilder();
            header.AppendFormat("realm=\"{0}\"", Realm);
            header.AppendFormat(",nonce=\"{0}\"", Nonce);
            header.AppendFormat(",qop=\"{0}\"", "auth");
            return header.ToString();
        }


    }
}