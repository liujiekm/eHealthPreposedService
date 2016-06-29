using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lenovo.CodeBuild.Attribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DeleteAttribute:BaseAttribute,ILenovoMaker 
    {
        public string WhereSql { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName">方法名</param>
        /// <param name="whereSql">where后的语句 比如：name=:name and age=20 and sex=:sex</param>
        /// <param name="description">描述</param>
        public DeleteAttribute(string methodName, string whereSql, string description)
        {
            this.Description = description;
            this.MethodName = methodName;
            this.WhereSql = whereSql;
        }
        public string Do(TableAttribute tableAttribute, Type p)
        {
            try
            {
                this.VerifyWhereSql(this.WhereSql, p.FullName);
                if (string.IsNullOrWhiteSpace(this.WhereSql))
                    throw new Exception("where 不能为空！");
                StringBuilder sb = new StringBuilder();
                var param = this.GetWhereParams(this.WhereSql);
                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// 删除方法 创建时间："+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.AppendLine("        /// " + this.Description);
                sb.AppendLine("        /// </summary>");
                foreach (var par in param)
                {
                    sb.AppendLine(string.Format("        /// <param name=\"where{0}\">删除条件{0}</param>", par));
                }
                sb.AppendLine("        /// <returns></returns>");
                sb.AppendLine(string.Format("        public int {0}({1})", this.MethodName, this.GetMethodParam(param, p, "where")));
                sb.AppendLine("        {");
                sb.AppendLine("            try");
                sb.AppendLine("            {");
                sb.AppendLine("                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){").AppendLine("                    conn.Open();");
                sb.AppendLine("                    var command = conn.CreateCommand();");
                sb.Append(this.GetOracleParam("                    command.Parameters.Add(new OracleParameter(\":{0}\", where{1}));", param, p));
                sb.AppendLine(string.Format("                    command.CommandText = \"delete from {0} where {1}\";", tableAttribute.TableName, this.WhereSql));
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
    }
}
