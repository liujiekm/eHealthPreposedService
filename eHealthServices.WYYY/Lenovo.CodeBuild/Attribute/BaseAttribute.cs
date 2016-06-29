using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lenovo.CodeBuild.Attribute
{
    public abstract class BaseAttribute : System.Attribute
    {
        public string Description { get; set; }
        public string MethodName { get; set; }


        #region 公共方法
        /// <summary>
        /// 验证查询语句是否正确
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="model"></param>
        public void VerifySelectSql(string sql, string model)
        {
            var currentSql = sql.Trim().ToLower();
            if (!currentSql.StartsWith("select "))
                throw new Exception(string.Format("{0}方法sql语句错误，不存在select 关键字，请检查！原始sql语句为:{1},该错误所属实体类为：{2}", this.MethodName, sql, model));
            currentSql = currentSql.Substring(6, currentSql.Length - 7).Trim();//去除开头的 select
            if (!currentSql.Contains(" from "))
                throw new Exception(string.Format("{0}方法sql语句错误，不存在 from 关键字，请检查！原始sql语句为:{1},该错误所属实体类为：{2}", this.MethodName, sql, model));

        }
        /// <summary>
        /// 获取参数 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<string> GetWhereParams(string sql)
        {
            Regex reg = new Regex(@":\w*");
            List<string> strs = new List<string>();
            if (reg.IsMatch(sql))
            {
                foreach (Match item in reg.Matches(sql))
                {
                    var temp = item.Value.Trim().Trim(':');
                    if (!strs.Contains(temp))
                        strs.Add(temp);
                }
            }
            return strs;
        }
        /// <summary>
        /// 获取方法参数 比如 string name,int age
        /// </summary>
        /// <param name="sqlParam"></param>
        /// <param name="p"></param>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        public string GetMethodParam(IList<string> sqlParam, Type p, string prefix = "")
        {
            List<string> list = new List<string>();
            foreach (var item in sqlParam)
            {
                var temp = p.GetProperties().ToList().FindAll(a => a.Name.ToUpper() == item.ToUpper());
                if (temp != null && temp.Count > 0)
                {
                    var tempType = temp.FirstOrDefault().PropertyType;
                    if (tempType.IsGenericType && tempType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        tempType = tempType.GetGenericArguments().FirstOrDefault();
                        list.Add(string.Format("{0}? {2}{1}", tempType.Name, temp.FirstOrDefault().Name, prefix));
                    }
                    else
                    {
                        list.Add(string.Format("{0} {2}{1}", tempType.Name, temp.FirstOrDefault().Name, prefix));
                    }
                }
            }
            return string.Join(",", list);
        }
        /// <summary>
        /// 获取oracle 参数
        /// </summary>
        /// <param name="format"></param>
        /// <param name="sqlParam"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public string GetOracleParam(string format, IList<string> sqlParam, Type p)
        {
            if (sqlParam == null || sqlParam.Count <= 0)
            {
                return string.Empty;
            }
            var pros = p.GetProperties().Select(a => a.Name).ToList();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("                    command.Parameters.Clear();");
            
            foreach (var item in sqlParam)
            {
                var pro = pros.FindAll(a => a.ToUpper() == item.ToUpper());
                if (pro != null && pro.Count > 0)
                {
                    sb.AppendLine(string.Format(format, item, pro.FirstOrDefault()));
                }
                else
                {
                    throw new Exception(string.Format("{0}对象的{1}方法{2}【sql】参数无法匹配！", p.FullName, this.MethodName, item));
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 验证 where语句是否正确 不能匹配嵌套where语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="model"></param>
        public void VerifyWhereSql(string sql, string model)
        {
            //var currentSql = sql.Trim().ToLower();
            //if (currentSql.Split('=').Length > 2 && !currentSql.Contains(" and "))
            //    throw new Exception(string.Format("{0}方法where语句错误，and关键字错误，请检查！原始sql语句为:{1},该错误所属实体类为：{2}", this.MethodName, sql, model));
            //var tempAnd = currentSql.Replace(" and ", "|").Split('|').Length + 1;
            //if (currentSql.Split('=').Length != tempAnd)
            //    throw new Exception(string.Format("{0}方法where语句错误，and关键字与=数理无法匹配，请检查！原始sql语句为:{1},该错误所属实体类为：{2}", this.MethodName, sql, model));

            

        }
        #endregion

    }
}
