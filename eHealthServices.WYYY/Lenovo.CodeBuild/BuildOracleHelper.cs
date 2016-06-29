using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lenovo.CodeBuild
{
    internal class BuildOracleHelper
    {
        /// <summary> 
        /// 创建OracleHelper类
        /// </summary>
        /// <returns></returns>
        public static string CreateCode() {
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine("    #region 数据库操作类");
            //sb.AppendLine("    /// <summary>");
            //sb.AppendLine("    /// 数据库操作类");
            //sb.AppendLine("    /// </summary>");
            //sb.AppendLine("    internal partial class OracleHelper");
            //sb.AppendLine("    {");
            //sb.AppendLine("        private static object objlock = new object();");
            //sb.AppendLine("        private static string ConnString");
            //sb.AppendLine("        {");
            //sb.AppendLine("            get");
            //sb.AppendLine("            {");
            //sb.AppendLine("                return System.Configuration.ConfigurationSettings.AppSettings[\"oracleConn\"];");
            //sb.AppendLine("            }");
            //sb.AppendLine("        }");
            //sb.AppendLine("        /// <summary>");
            //sb.AppendLine("        /// 获取数据库连接实体");
            //sb.AppendLine("        /// </summary>");
            //sb.AppendLine("        public static System.Data.OracleClient.OracleConnection Instant");
            //sb.AppendLine("        {");
            //sb.AppendLine("            get");
            //sb.AppendLine("            {");
            //sb.AppendLine("                if (System.Web.HttpContext.Current.Items[\"_oracleConn\"] == null)");
            //sb.AppendLine("                {");
            //sb.AppendLine("                    lock (objlock)");
            //sb.AppendLine("                    {");
            //sb.AppendLine("                        if (System.Web.HttpContext.Current.Items[\"_oracleConn\"] == null)");
            //sb.AppendLine("                        {");
            //sb.AppendLine("                            var temp = new System.Data.OracleClient.OracleConnection(ConnString);");
            //sb.AppendLine("                            System.Web.HttpContext.Current.Items[\"_oracleConn\"] = temp;");
            //sb.AppendLine("                        }");
            //sb.AppendLine("                    }");
            //sb.AppendLine("                }");
            //sb.AppendLine("                var instant = System.Web.HttpContext.Current.Items[\"_oracleConn\"] as System.Data.OracleClient.OracleConnection;");
            //sb.AppendLine("                if (instant.State != System.Data.ConnectionState.Open)");
            //sb.AppendLine("                    instant.Open();");
            //sb.AppendLine("                return instant;");
            //sb.AppendLine("            }");
            //sb.AppendLine("        }");
            //sb.AppendLine("        /// <summary>");
            //sb.AppendLine("        /// 放回序列");
            //sb.AppendLine("        /// </summary>");
            //sb.AppendLine("        /// <param name=\"vSequenceName\"></param>");
            //sb.AppendLine("        /// <returns></returns>");
            //sb.AppendLine("        public static long GetNextValue(string vSequenceName)");
            //sb.AppendLine("        {");
            //sb.AppendLine("            var command = Instant.CreateCommand();");
            //sb.AppendLine("            command.CommandText = string.Format(\"select {0}.nextval from dual\",vSequenceName);");
            //sb.AppendLine("            object lo_obj = command.ExecuteScalar();");
            //sb.AppendLine("            return lo_obj == null ? -1 : Convert.ToInt64(lo_obj);");
            //sb.AppendLine("        }");
            //sb.AppendLine("    ");
            //sb.AppendLine("        public static IList<T> GetDataItems<T>(OracleCommand command) where T : class, new()");
            //sb.AppendLine("        {");
            //sb.AppendLine("            var result = new List<T>();");
            //sb.AppendLine("            var reader = command.ExecuteReader();");
            //sb.AppendLine("            try");
            //sb.AppendLine("            {");
            //sb.AppendLine("                if (!reader.HasRows)");
            //sb.AppendLine("                {");
            //sb.AppendLine("                    reader.Close();");
            //sb.AppendLine("                    return result;");
            //sb.AppendLine("                }");
            //sb.AppendLine("                Type typeFromHandle = typeof(T);");
            //sb.AppendLine("                var inner = from i in Enumerable.Range(1, reader.FieldCount)");
            //sb.AppendLine("                            select new");
            //sb.AppendLine("                            {");
            //sb.AppendLine("                                Name = reader.GetName(i - 1),");
            //sb.AppendLine("                                Index = i - 1");
            //sb.AppendLine("                            };");
            //sb.AppendLine("                var enumerable = typeFromHandle.GetProperties().Join(inner, x => x.Name, x => x.Name, (x, y) => new { y.Index, x });");
            //sb.AppendLine("                while (reader.Read())");
            //sb.AppendLine("                {");
            //sb.AppendLine("                    T t = Activator.CreateInstance<T>();");
            //sb.AppendLine("                    foreach (var current in enumerable)");
            //sb.AppendLine("                    {");
            //sb.AppendLine("                        if (reader[current.Index].GetType() != typeof(DBNull))");
            //sb.AppendLine("                        {");
            //sb.AppendLine("                            if (current.x.PropertyType.IsGenericType && current.x.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))");
            //sb.AppendLine("                            {");
            //sb.AppendLine("                                current.x.SetValue(t, new NullableConverter(current.x.PropertyType).ConvertFromString(reader[current.Index].ToString()), null);");
            //sb.AppendLine("                            }");
            //sb.AppendLine("                            else");
            //sb.AppendLine("                                current.x.SetValue(t, Convert.ChangeType(reader[current.Index], current.x.PropertyType), null);");
            //sb.AppendLine("                        }");
            //sb.AppendLine("                    }");
            //sb.AppendLine("                    result.Add(t);");
            //sb.AppendLine("                }");
            //sb.AppendLine("            }");
            //sb.AppendLine("            catch (Exception ex)");
            //sb.AppendLine("            {");
            //sb.AppendLine("                throw ex;");
            //sb.AppendLine("            }");
            //sb.AppendLine("            finally");
            //sb.AppendLine("            {");
            //sb.AppendLine("                reader.Close();");
            //sb.AppendLine("            }");
            //sb.AppendLine("            return result;");
            //sb.AppendLine("        }");
            //sb.AppendLine("    }");
            //sb.AppendLine("    #endregion");
            return sb.ToString();
        }

    }
}
