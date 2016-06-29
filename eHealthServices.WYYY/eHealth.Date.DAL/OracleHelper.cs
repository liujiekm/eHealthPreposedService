using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace eHealth.Date.DAL
{
    #region 数据库操作类
    /// <summary>
    /// 数据库操作类
    /// </summary>
    internal partial class OracleHelper
    {
        private static object objlock = new object();
        public static string ConnString{ get{return System.Configuration.ConfigurationSettings.AppSettings["oracleConn"];}}
        /// <summary>
        /// 获取数据库连接实体
        /// </summary>
        public static System.Data.OracleClient.OracleConnection Instant
        {
            get
            {
                if (System.Web.HttpContext.Current.Items["_oracleConn"] == null)
                {
                    lock (objlock)
                    {
                        if (System.Web.HttpContext.Current.Items["_oracleConn"] == null)
                        {
                            var temp = new System.Data.OracleClient.OracleConnection(ConnString);
                            System.Web.HttpContext.Current.Items["_oracleConn"] = temp;
                        }
                    }
                }
                var instant = System.Web.HttpContext.Current.Items["_oracleConn"] as System.Data.OracleClient.OracleConnection;
                if (instant.State != System.Data.ConnectionState.Open)
                    instant.Open();
                return instant;
            }
        }
        /// <summary>
        /// 返回序列
        /// </summary>
        /// <param name="vSequenceName"></param>
        /// <returns></returns>
        public static long GetNextValue(string vSequenceName)
        {
            var command = Instant.CreateCommand();
            command.CommandText = string.Format("select {0}.nextval from dual", vSequenceName);
            object lo_obj = command.ExecuteScalar();
            return lo_obj == null ? -1 : Convert.ToInt64(lo_obj);
        }

        public static long GetNextValue(string vSequenceName, OracleCommand command)
        {
            command.CommandText = string.Format("select {0}.nextval from dual", vSequenceName);
            object lo_obj = command.ExecuteScalar();
            return lo_obj == null ? -1 : Convert.ToInt64(lo_obj);
        }


        public static IList<T> GetDataItems<T>(OracleCommand command) where T : class, new()
        {
            var result = new List<T>();
            var reader = command.ExecuteReader();
            try
            {
                if (!reader.HasRows)
                {
                    reader.Close();
                    return result;
                }
                Type typeFromHandle = typeof(T);
                var inner = from i in Enumerable.Range(1, reader.FieldCount)
                            select new
                            {
                                Name = reader.GetName(i - 1).ToUpper(),
                                Index = i - 1
                            };
                var enumerable = typeFromHandle.GetProperties().Join(inner, x => x.Name.ToUpper(), x => x.Name, (x, y) => new { y.Index, x });//无法匹配，匹配结果为null
                
                while (reader.Read())
                {
                    T t = Activator.CreateInstance<T>();
                    foreach (var current in enumerable)
                    {
                        if (reader[current.Index].GetType() != typeof(DBNull))
                        {
                            if (current.x.PropertyType.IsGenericType && current.x.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                current.x.SetValue(t, new NullableConverter(current.x.PropertyType).ConvertFromString(reader[current.Index].ToString()), null);
                            }
                            else
                                current.x.SetValue(t, Convert.ChangeType(reader[current.Index], current.x.PropertyType), null);
                        }
                    }
                    result.Add(t);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
            }
            return result;
        }
    }
    #endregion
}
