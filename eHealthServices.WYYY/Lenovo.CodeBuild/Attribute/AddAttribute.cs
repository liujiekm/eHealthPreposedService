using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Lenovo.CodeBuild.Attribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AddAttribute:BaseAttribute,ILenovoMaker
    {

        public AddAttribute():this("Add","新增方法") { }

        public AddAttribute(string methodName, string description)
        {
            this.Description = description;
            this.MethodName = methodName;
        }

        public string Do(TableAttribute tableAttribute, Type p)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// 添加方法，生成时间："+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.AppendLine("        /// "+this.Description);
                sb.AppendLine("        /// </summary>");
                sb.AppendLine("        /// <param name=\"model\">添加对象</param>");
                sb.AppendLine("        /// <returns>大于0为添加成功</returns>");
                sb.AppendLine(string.Format("        public int {0}({1} model)",this.MethodName,p.FullName));
                sb.AppendLine("        {");
                sb.AppendLine("            try");
                sb.AppendLine("            {");
                var fields=GetAddFields(p);
                sb.AppendLine("                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){").AppendLine("                    conn.Open();");
                sb.AppendLine("                    var command = conn.CreateCommand();");
                sb.Append(this.GetOracleAddParam("                    if(model.{1}!=null) fields.Add(\"{0}\");if(model.{1}!=null) command.Parameters.Add(new OracleParameter(\":{0}\", model.{1}));", p));
                //sb.AppendLine(string.Format("                    command.CommandText = \"insert into {0}({1}) values(:{2})\";", tableAttribute.TableName, string.Join(",", fields), string.Join(",:", fields)));
                sb.AppendLine("                    command.CommandText = string.Format(\"insert into " + tableAttribute.TableName + "({0}) values(:{1})\",string.Join(\",\",fields),string.Join(\",:\",fields));");
                sb.AppendLine("                    return command.ExecuteNonQuery();");
                sb.AppendLine("                }");
                sb.AppendLine("            }");
                sb.AppendLine("            catch (Exception ex)");
                sb.AppendLine("            {");
                sb.AppendLine("                Lenovo.Tool.Log4NetHelper.Error(ex);");
                sb.AppendLine("                return 0;");
                sb.AppendLine("            }");
                sb.AppendLine("        }");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 获取 添加参数
        /// </summary>
        /// <param name="format"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetOracleAddParam(string format, Type p)
        {
            var pros = p.GetProperties().Select(a => a.Name).ToList();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("                    var fields = new List<string>();");
            sb.AppendLine("                    command.Parameters.Clear();");
            foreach (var item in p.GetProperties())
            {
                if (item.GetCustomAttributes(true).Count() > 0 && item.GetCustomAttributes(true).FirstOrDefault().GetType() == typeof(FieldSeqAttribute))
                {
                    MemberInfo classInfo = item;
                    var seq = System.Attribute.GetCustomAttribute(classInfo, typeof(FieldSeqAttribute)) as FieldSeqAttribute;
                    sb.AppendLine(string.Format("                    model.{0}= OracleHelper.GetNextValue(\"{1}\");", item.Name, seq.Seq));
                }

                if (item.GetCustomAttributes(true).Count() > 0 && item.GetCustomAttributes(true).FirstOrDefault().GetType() == typeof(FieldNotBelongToTableAttribute))
                {
                    //不是数据库中的字段
                }
                else
                {
                    sb.AppendLine(string.Format(format, item.Name, item.Name));
                }

            }
            return sb.ToString();
        }

        private IList<string> GetAddFields(Type p)
        {
            IList<string> fields = new List<string>();
            foreach (var item in p.GetProperties())
            {
                if (item.GetCustomAttributes(true).Count() > 0 && item.GetCustomAttributes(true).FirstOrDefault().GetType() == typeof(FieldNotBelongToTableAttribute))
                {
                    //不是数据库中的字段
                }
                else
                {
                    fields.Add(item.Name);
                }
            }
            return fields;
        }
    }
}
