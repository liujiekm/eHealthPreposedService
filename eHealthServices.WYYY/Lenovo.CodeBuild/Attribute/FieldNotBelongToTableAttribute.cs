using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lenovo.CodeBuild.Attribute
{
    /// <summary>
    /// 表示该字段不属于该表--在Add时
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FieldNotBelongToTableAttribute:System.Attribute
    {
    }
}
