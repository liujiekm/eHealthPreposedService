using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lenovo.CodeBuild
{
    public class Make
    {  
        public static string Do(string nameSpace, string path)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                string newPath = string.Format("{0}_temp-{1}.dll", path, DateTime.Now.Ticks.ToString());
                File.Copy(path, newPath);
                Assembly assembly = Assembly.LoadFile(newPath);
                var publicClass = assembly.GetExportedTypes(); //获取所有公共实体类
                sb.AppendLine("using System;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.Linq;");
                sb.AppendLine("using System.Text;");
                sb.AppendLine("using System.Threading.Tasks;");
                sb.AppendLine("using System.Data.OracleClient;");
                sb.AppendLine("using System.ComponentModel;");
                sb.Append("namespace ").AppendLine(nameSpace).AppendLine("{");
                sb.AppendLine(BuildOracleHelper.CreateCode());
                foreach (var p in publicClass)
                {
                    sb.AppendLine(DoClass(p));
                }
                return sb.AppendLine("}").ToString();
            }
            catch (Exception ex)
            {
                return sb.Clear().AppendLine(ex.Message).AppendLine(string.Empty).AppendLine(ex.StackTrace).ToString();
            } 
        }


        #region 私有方法
        private static string DoClass(Type p) {
            MemberInfo classInfo = p;
            StringBuilder sb = new StringBuilder();
            var attrs = System.Attribute.GetCustomAttributes(classInfo, typeof(System.Attribute)).ToList();
            if (attrs == null || attrs.Count <= 0)
                return string.Empty;
            Attribute.TableAttribute table = null;
            bool isPartial = false;//是否有自动生成的方法
            attrs.ForEach(a =>
            {
                if (table == null && a.GetType() == typeof(Attribute.TableAttribute))
                {
                    table = a as Attribute.TableAttribute;
                }
                else if (!isPartial && a is Attribute.IBaseMaker)
                {
                    isPartial = true;
                }
            });
            if (table == null)
                return string.Empty;
            if (isPartial)
                sb.AppendLine(DoMethod(table, attrs, p));
            return sb.ToString();
        }

        private static string DoMethod(Attribute.TableAttribute table, List<System.Attribute> attrs, Type p)
        {
            string className = p.Name.StartsWith("m_") ? p.Name.Substring(2, p.Name.Length - 2) : p.Name;//设置类名
            StringBuilder attrBuilder = new StringBuilder();
            //todo 实现该类
            attrBuilder.Append("    #region 注释：").Append(p.FullName).AppendLine(table.Description);
            attrBuilder.AppendLine("    /// <summary>");
            attrBuilder.AppendLine("    /// 模板生成");
            attrBuilder.Append("    /// CreateBy 童岭 ").AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            attrBuilder.Append("    /// ").AppendLine(table.Description);
            attrBuilder.AppendLine("    /// </summary>");
            attrBuilder.Append("    public partial class dl_").AppendLine(className);
            attrBuilder.AppendLine("    {");
            foreach (var attr in attrs) //遍历每一个特性
            {
                if (attr is Attribute.ILenovoMaker) //如果其实现了ICodeBuildMaker 接口
                {
                    attrBuilder.AppendLine((attr as Attribute.ILenovoMaker).Do(table, p)); //添加方法
                }
            }
            return attrBuilder.AppendLine("    }").AppendLine("    #endregion").ToString();
        }

        #endregion
    }
}
