using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lenovo.CodeBuild.Attribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TableAttribute : System.Attribute
    {
        public string Description { get; private set; }
        public string TableName { get; private set; }

        public string BelongToDBUser { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="description"></param>
        /// <param name="belongToDBUser">默认为空，有个默认数据库用户</param>
        public TableAttribute(string tableName, string description, string belongToDBUser="")
        {
            this.TableName = tableName;
            this.BelongToDBUser = belongToDBUser;
            this.Description = description;
        }
    }
}
