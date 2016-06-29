using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lenovo.CodeBuild.Attribute
{
    /// <summary>
    /// 获取 seq 序列
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FieldSeqAttribute : System.Attribute
    {
        public string Seq { get; set; }
        public FieldSeqAttribute(string seq)
        {
            this.Seq = seq;
        }
    }
}
