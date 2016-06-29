using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lenovo.Tool.Oracle
{
    public class PageHelper
    {
        /*
       select * from t_xiaoxi where rowid in (select rid from (select rownum rn,rid from(select rowid rid,cid from t_xiaoxi  order by cid desc) where rownum<10000) where rn>9980) order by cid desc;
       */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">表名字</param>
        /// <param name="fields">需要获取的字段名s</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="whereStr"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex">分页索引，从0开始</param>
        /// <param name="isDesc">如何排序</param>
        /// <param name="pageIndex"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public static string GetPagingSql(string tableName, string fields, string orderField, string whereStr, int pageSize, int pageIndex, bool isDesc = true)
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
    }
}
