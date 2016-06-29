using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lenovo.CodeBuild.Attribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class SelectPagingAttribute : BaseAttribute, ILenovoMaker
    {
        /// <summary>
        /// where 语句
        /// </summary>
        public string WhereSql { get; set; }
        /// <summary>
        /// 查询字段
        /// </summary>
        public IList<string> SelectFields { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public IList<string> OrderFields { get; set; }
        /// <summary>
        /// 生成sql语句
        /// </summary>
        /// <param name="tableName">表名字</param>
        /// <param name="fields">需要获取的字段名s</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="whereStr"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex">分页索引，从0开始</param>
        /// <param name="isDesc">如何排序</param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        private string GetPagingSql(string tableName, string fields, string orderField, string whereStr, int pageSize, int pageIndex, bool isDesc = true)
        {
            return
                string.Format(
                    "select {1} from {0} where rowid in (select rid from (select rownum rn,rid from(select rowid rid,{2} from {0} where {6} order by {2} {3}) where rownum<{4}) where rn>{5}) order by {2} {3}",
                    tableName,
                    fields,
                    orderField,
                    isDesc ? "desc" : "asc",
                    ((pageIndex + 1) * pageSize + 1),
                    pageSize * pageIndex, whereStr);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="selectFields">例如：name,age</param>
        /// <param name="orderField">例如:name,age</param>
        /// <param name="whereStr">例如：name=:name and age=:age</param>
        /// <param name="description"></param>
        public SelectPagingAttribute(string methodName, string selectFields, string orderField, string whereStr, string description)
        {
            this.MethodName = methodName;
            this.Description = description;
            this.WhereSql = whereStr;
            this.SelectFields = selectFields.Trim().Trim(',').Split(',').ToList();
            this.OrderFields = orderField.Trim().Trim(',').Split(',').ToList();
        }

        public string Do(TableAttribute tableAttribute, Type p)
        {
            try
            {
                var pros = p.GetProperties().Select(a => a.Name.ToLower()).ToList();
                foreach (var item in this.SelectFields)//验证字段是否包含在实体对象中
                {
                    if (pros.FindAll(a => a == item.ToLower()).Count <= 0)
                    {
                        throw new Exception(string.Format("{0}对象中的{1}方法中查询结果有不包含{2}字段问题", p.FullName, this.MethodName, item));
                    }
                }
                return string.Format("{0}{1}", CreateCode(tableAttribute, p), CreateOutCountCode(tableAttribute, p));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string CreateOutCountCode(TableAttribute tableAttribute, Type p)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var param = this.GetWhereParams(this.WhereSql);
                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// 查询分页方法，生成时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.AppendLine("        /// " + this.Description);
                sb.AppendLine("        /// </summary>");
                foreach (var item in param)
                {
                    sb.AppendLine(string.Format("        /// <param name=\"where{0}\">查询分页方法所需参数{0}</param>", item));
                }
                sb.AppendLine("        /// <param name=\"pageSize\">每页大小</param>");
                sb.AppendLine("        /// <param name=\"pageIndex\">页索引，从零开始计数</param>");
                sb.AppendLine("        /// <param name=\"count\">记录总数</param>");
                sb.AppendLine("        /// <param name=\"isDesc\">是否正排，默认true最大的排在前面</param>");
                sb.AppendLine(string.Format("        /// <returns>返回{0}实体</returns>", p.FullName));
                var temp = this.GetMethodParam(param, p, "where");
                temp = string.IsNullOrWhiteSpace(temp) ? string.Empty : temp + ",";
                sb.AppendLine(string.Format("        public IList<{0}> {1}({2} int pageSize, int pageIndex,out int count, bool isDesc=true)", p.FullName, this.MethodName, temp));
                sb.AppendLine("        {");
                sb.AppendLine("            count = 0;");
                sb.AppendLine("            try");
                sb.AppendLine("            {");
                sb.AppendLine("                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){").AppendLine("                    conn.Open();");
                sb.AppendLine("                    var command = conn.CreateCommand();");
                sb.AppendLine(this.GetOracleParam("                    command.Parameters.Add(new OracleParameter(\":{0}\",where{1}));", param.ToList(), p));

                var countWhere = string.IsNullOrWhiteSpace(this.WhereSql) ? string.Empty : "where " + this.WhereSql;
                sb.AppendLine(string.Format("                    command.CommandText = string.Format(\"select count(*) from {0} {1}\");", tableAttribute.TableName, countWhere));
                sb.AppendLine("                    count = Convert.ToInt32(command.ExecuteScalar());");
                sb.AppendLine(string.Format("                    command.CommandText = string.Format(\"{0}\",\"{1}\",\"{2}\",\"{3}\",isDesc ? \"desc\" : \"asc\",((pageIndex + 1) * pageSize + 1),pageSize * pageIndex,{4});",
                    "select {1} from {0} where rowid in (select rid from (select rownum rn,rid from(select rowid rid,{2} from {0} {6} order by {2} {3}) where rownum<{4}) where rn>{5}) order by {2} {3}",
                    tableAttribute.TableName, string.Join(",", this.SelectFields), string.Join(",", OrderFields), string.IsNullOrWhiteSpace(this.WhereSql) ? "string.Empty" : string.Format("\"where {0}\"", this.WhereSql)));
               
                sb.AppendLine(string.Format("                    var result = OracleHelper.GetDataItems<{0}>(command);", p.FullName));
                sb.AppendLine("                    return result;");
                sb.AppendLine("                }");
                sb.AppendLine("            }");
                sb.AppendLine("            catch (Exception ex)");
                sb.AppendLine("            {");
                sb.AppendLine("                Lenovo.Tool.Log4NetHelper.Error(ex);");
                sb.AppendLine(string.Format("                return new List<{0}>();", p.FullName));
                sb.AppendLine("            }");
                sb.AppendLine("        }");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string CreateCode(TableAttribute tableAttribute, Type p)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var param = this.GetWhereParams(this.WhereSql);
                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// 查询分页方法，生成时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.AppendLine("        /// " + this.Description);
                sb.AppendLine("        /// </summary>");
                foreach (var item in param)
                {
                    sb.AppendLine(string.Format("        /// <param name=\"where{0}\">查询分页方法所需参数{0}</param>", item));
                }
                sb.AppendLine("        /// <param name=\"pageSize\">每页大小</param>");
                sb.AppendLine("        /// <param name=\"pageIndex\">页索引，从零开始计数</param>");
                sb.AppendLine("        /// <param name=\"isDesc\">是否正排，默认true最大的排在前面</param>");
                sb.AppendLine(string.Format("        /// <returns>返回{0}实体</returns>", p.FullName));
                var temp = this.GetMethodParam(param, p, "where");
                temp = string.IsNullOrWhiteSpace(temp) ? string.Empty : temp + ",";
                sb.AppendLine(string.Format("        public IList<{0}> {1}({2} int pageSize, int pageIndex, bool isDesc=true)", p.FullName, this.MethodName,temp));
                sb.AppendLine("        {");
                sb.AppendLine("            try");
                sb.AppendLine("            {");
                sb.AppendLine("                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString)){").AppendLine("                    conn.Open();");
                sb.AppendLine("                    var command = conn.CreateCommand();");
                sb.AppendLine(string.Format("                    command.CommandText = string.Format(\"{0}\",\"{1}\",\"{2}\",\"{3}\",isDesc ? \"desc\" : \"asc\",((pageIndex + 1) * pageSize + 1),pageSize * pageIndex,{4});",
                    "select {1} from {0} where rowid in (select rid from (select rownum rn,rid from(select rowid rid,{2} from {0} {6} order by {2} {3}) where rownum<{4}) where rn>{5}) order by {2} {3}",
                    tableAttribute.TableName, string.Join(",", this.SelectFields), string.Join(",", OrderFields), string.IsNullOrWhiteSpace(this.WhereSql) ? "string.Empty" : string.Format("\"where {0}\"", this.WhereSql)));
                sb.AppendLine(this.GetOracleParam("                    command.Parameters.Add(new OracleParameter(\":{0}\",where{1}));", param.ToList(), p));
                sb.AppendLine(string.Format("                    var result = OracleHelper.GetDataItems<{0}>(command);", p.FullName));
                sb.AppendLine("                    return result;");
                sb.AppendLine("                }");
                sb.AppendLine("            }");
                sb.AppendLine("            catch (Exception ex)");
                sb.AppendLine("            {");
                sb.AppendLine("                Lenovo.Tool.Log4NetHelper.Error(ex);");
                sb.AppendLine(string.Format("                return new List<{0}>();", p.FullName));
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
