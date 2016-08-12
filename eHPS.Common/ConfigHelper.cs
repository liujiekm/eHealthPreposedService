//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 操作webconfig以及appconfig文件，动态添加修改config内容的帮助类
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/8/12 13:44:55
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace eHPS.Common
{
    /// <summary>
    /// 操作webconfig以及appconfig文件，动态添加修改config内容的帮助类
    /// </summary>
    public class ConfigHelper
    {

        public static void GetPath()
        {
            var url = Environment.CurrentDirectory;
        }

        private void Modify()
        {
            
            var configuration = WebConfigurationManager.OpenWebConfiguration("~");
            var section = (ConnectionStringsSection)configuration.GetSection("connectionStrings");
            section.ConnectionStrings["MyConnectionString"].ConnectionString = "Data Source=...";
            configuration.Save();
        }
    }
}
