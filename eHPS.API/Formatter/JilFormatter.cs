//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & Net 开发组
//=================================================================================== 
// 基于Jil类库，改变API默认的JSON格式花器
// 
// 
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  liu
// 创建时间：2016/4/6 17:15:51
// 版本号：  V1.0.0.0
//===================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jil;
using System.Net.Http.Formatting;
using System.Text;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Net;

namespace eHPS.API.Formatter
{
    public class JilFormatter : MediaTypeFormatter
    {
        private readonly Options _jilOptions;
        public JilFormatter()
        {
            _jilOptions = new Options(dateFormat: DateTimeFormat.ISO8601);
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));

            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
            SupportedEncodings.Add(new UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true));
        }
        public override bool CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.FromResult(this.DeserializeFromStream(type, readStream));
        }


        private object DeserializeFromStream(Type type, Stream readStream)
        {
            try
            {
                using (var reader = new StreamReader(readStream))
                {
                    MethodInfo method = typeof(JSON).GetMethod("Deserialize", new Type[] { typeof(TextReader), typeof(Options) });
                    MethodInfo generic = method.MakeGenericMethod(type);
                    return generic.Invoke(this, new object[] { reader, _jilOptions });
                }
            }
            catch
            {
                return null;
            }

        }


        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            var streamWriter = new StreamWriter(writeStream);
            JSON.Serialize(value, streamWriter, _jilOptions);
            streamWriter.Flush();
            return Task.FromResult(writeStream);
        }
    }

}