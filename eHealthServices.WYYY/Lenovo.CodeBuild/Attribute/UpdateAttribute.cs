using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lenovo.CodeBuild.Attribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class UpdateAttribute:BaseAttribute,ILenovoMaker
    {
        public string WhereSql { get; set; }
        public List<string> UpdateFields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName">方法名</param>
        /// <param name="updateFields">需要更新的字段 例如：yhid,yhxm</param>
        /// <param name="whereSql">where后的语句 比如：name=:name and age=20 and sex=:sex</param>
        /// <param name="description">描述</param>
        public UpdateAttribute(string methodName, string updateFields, string whereSql, string description)
        {
            this.Description = description;
            this.MethodName = methodName;
            this.WhereSql = whereSql;
            if (!string.IsNullOrWhiteSpace(updateFields)) {
                this.UpdateFields = updateFields.Split(',').ToList();
            }
        }
        public string Do(TableAttribute tableAttribute, Type p)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.WhereSql))
                    throw new Exception("where 不能为空！");
                if (this.UpdateFields == null || this.UpdateFields.Count <= 0)
                    throw new Exception("update 不能为空");
                this.VerifyWhereSql(this.WhereSql, p.FullName);
                var tempPro = p.GetProperties().Select(a => a.Name.ToUpper()).ToList();
                foreach (var field in this.UpdateFields)
                {
                    if (!tempPro.Contains(field.ToUpper()))
                    {
                        throw new Exception(string.Format("{0}对象中不存在updateFields中的{1}字段，请核对！", p.FullName, field));
                    }
                }
                StringBuilder sb = new StringBuilder();
                var param = this.GetWhereParams(this.WhereSql);
                sb.AppendLine("        /// <summary>").
                   AppendLine("        /// 修改方法，生成时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).
                   AppendLine("        /// " + this.Description).
                   AppendLine("        /// </summary>");
                foreach (var pro in this.UpdateFields)
                {
                    sb.AppendLine(string.Format("        /// <param name=\"update{0}\">update{0}</param>", pro));
                }
                foreach (var par in param)
                {
                    sb.AppendLine(string.Format("        /// <param name=\"where{0}\">where{0}</param>", par));
                }
                sb.AppendLine("        /// <returns></returns>");
                string whereParams = this.GetMethodParam(param, p, "where");
                string updateParams = this.GetMethodParam(this.UpdateFields, p, "update");
                var tempParam = string.Join(",", new[] { updateParams, whereParams });
                sb.AppendLine(string.Format("        public int {0}({1})", this.MethodName, tempParam));

                sb.AppendLine("        {");
                sb.AppendLine("            try");
                sb.AppendLine("            {");
                sb.AppendLine("                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){").AppendLine("                    conn.Open();");
                sb.AppendLine("                    var command = conn.CreateCommand();");
                sb.Append(this.GetOracleParam("                    command.Parameters.Add(new OracleParameter(\":update{0}\",update{1}));", this.UpdateFields, p));
                sb.Append(this.GetOracleParam("                    command.Parameters.Add(new OracleParameter(\":{0}\",where{1}));", param, p).Replace("command.Parameters.Clear();", string.Empty));

                sb.AppendLine(string.Format("                    command.CommandText = \"update {0} set {1} where {2}\";", tableAttribute.TableName, this.GetUpdateSql(p), this.WhereSql));
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
        /// 返回update字符串 比如：age=:updateage,name=:updatename
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetUpdateSql(Type p)
        {
            List<string> updateStr = new List<string>();
            var tempTypePro = p.GetProperties().Select(a => a.Name.ToUpper()).ToList();
            foreach (var item in this.UpdateFields)
            {
                if (tempTypePro.Contains(item.ToUpper()))
                {
                    updateStr.Add(string.Format("{0}=:update{0}", item));
                }
                else
                {
                    throw new Exception(string.Format("{0}对象中不存在updateFields中的{1}字段，请核对！", p.FullName, item));
                }
            }
            return string.Join(",", updateStr);
        }
    }
}
