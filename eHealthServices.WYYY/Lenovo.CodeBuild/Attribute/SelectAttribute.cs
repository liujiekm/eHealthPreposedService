using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lenovo.CodeBuild.Attribute
{
     [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class SelectAttribute:BaseAttribute,ILenovoMaker
    {
        public string Sql { get; set; }
        public bool IsMult { get; set; }

        public SelectAttribute(string methodName, string sql, string description, bool isMult = true)
        {
            this.Description = description;
            this.Sql = sql;
            this.MethodName = methodName;
            this.IsMult = isMult;
        }

        public string Do(TableAttribute tableAttribute, Type p)
        {
            try
            {
                this.VerifySelectSql(this.Sql, p.FullName);
                StringBuilder sb = new StringBuilder();
                var param = this.GetWhereParams(this.Sql);
                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// 查询方法，生成时间："+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.AppendLine("        /// "+this.Description);
                sb.AppendLine("        /// </summary>");
                foreach (var item in param)
                {
                    sb.AppendLine(string.Format("        /// <param name=\"{0}\">查询方法所需参数{0}</param>", item));
                }
                sb.AppendLine(string.Format("        /// <returns>返回{0}实体</returns>",p.FullName));
                sb.AppendLine(string.Format("        public {0} {1}({2})",this.IsMult?string.Format("IList<{0}>",p.FullName):p.FullName,this.MethodName,this.GetMethodParam(param,p,string.Empty)));
                sb.AppendLine("        {");
                sb.AppendLine("            try");
                sb.AppendLine("            {");
                sb.AppendLine("                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){").AppendLine("                    conn.Open();");
                sb.AppendLine("                    var command = conn.CreateCommand();");
                sb.AppendLine(string.Format("                    command.CommandText = \"{0}\";", this.Sql));
                sb.AppendLine(this.GetOracleParam("                    command.Parameters.Add(new OracleParameter(\":{0}\",{1}));", param.ToList(), p));
                sb.AppendLine(string.Format("                    var result = OracleHelper.GetDataItems<{0}>(command);", p.FullName));
                sb.AppendLine(string.Format("                    return result{0};", this.IsMult ? string.Empty : ".FirstOrDefault()"));
                sb.AppendLine("                }");
                sb.AppendLine("            }");
                sb.AppendLine("            catch (Exception ex)");
                sb.AppendLine("            {");
                sb.AppendLine("                Lenovo.Tool.Log4NetHelper.Error(ex);");
                sb.AppendLine(string.Format("                return new {0}();",this.IsMult?string.Format("List<{0}>",p.FullName):p.FullName));
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
