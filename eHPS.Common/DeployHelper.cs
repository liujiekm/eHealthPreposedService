//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 部署工具类
//  Web Site 
// Windows Service等
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/8/12 16:26:05
// 版本号：  V1.0.0.0
//===================================================================================




using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHPS.Common
{
    /// <summary>
    /// 部署工具类
    /// </summary>
    public class DeployHelper
    {


        public enum IISVersion
        {
            IIS6=0,
            IIS7=1
        }

        public enum SolutionDeployType
        {
            /// <summary>
            /// 网站
            /// </summary>
            Site = 0,

            /// <summary>
            /// 应用程序
            /// </summary>
            Application=1,

            /// <summary>
            /// 虚拟目录
            /// </summary>
            VirtualDirectory=2
        }

        public enum BindingProtocol
        {
            HTTP=0,
            HTTPS=1
        }

        ///// <summary>
        ///// 部署网站项目（IIS）
        ///// </summary>
        //public static  void DeployWebSolution(SolutionDeployType deployType,Int32 port,String deployFolderUrl,String siteName)
        //{
        //    ServerManager iisManager = new ServerManager();
        //    iisManager.Sites.Add("NewSite", "http", "*:8080:", "d:\\MySite");
        //    iisManager.CommitChanges();
        //}

        /// <summary>
        /// 部署网站
        /// </summary>
        /// <param name="siteName">网站名称</param>
        /// <param name="protocol">绑定协议</param>
        /// <param name="port">端口</param>
        /// <param name="deployFolderUrl">部署文件夹路径</param>
        public static void DeploySite(String siteName, BindingProtocol protocol,Int32 port, String deployFolderUrl)
        {
            ServerManager iisManager = new ServerManager();
            iisManager.Sites.Add(siteName, protocol.ToString(), "*:"+port+":", deployFolderUrl);//"d:\\MySite"
            iisManager.CommitChanges();
        }

        public static void DeployApplication(String siteName,String applicationName,String deployFolderUrl)
        {
            ServerManager iisManager = new ServerManager();
            iisManager.Sites[siteName].Applications.Add("/"+applicationName, deployFolderUrl);//"d:\\MyApp"
            iisManager.CommitChanges();
        }

        /// <summary>
        /// 获取当前服务器IIS下面的网站
        /// </summary>
        /// <returns></returns>
        private List<String> GetCurrentIisSite()
        {
            ServerManager iisManager = new ServerManager();
            return iisManager.Sites.Select(s => s.Name).ToList();
        }


        /// <summary>
        /// 当前服务器IIS中是否已经包含指定名称的网站
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        private bool IsSiteExist(String siteName)
        {
            ServerManager iisManager = new ServerManager();
            return iisManager.Sites.Any(s => s.Name == siteName);
        }



        /// <summary>
        /// 指定网站下面的应用程序是否存在
        /// </summary>
        /// <param name="siteName">网站名称</param>
        /// <param name="applicationName">应用程序名称</param>
        /// <returns></returns>
        private bool IsApplicationExist(String siteName,String applicationName)
        {
            ServerManager iisManager = new ServerManager();

            if(IsSiteExist(siteName))
            {
                return iisManager.Sites[siteName].Applications.Any(a => a.Path == "/" + applicationName);
            }
            else
            {
                return false;
            }

            
        }

        /// <summary>
        /// 安装部署服务项目（Windows Service）
        /// </summary>
        public static void InstallWindowService(String servicePath,String serviceName,String serviceDisplayName)
        {
            ServiceInstaller installer = new ServiceInstaller();
            installer.InstallService(servicePath, serviceName, serviceDisplayName);
        }

        /// <summary>
        /// 卸载windows service服务
        /// </summary>
        /// <param name="serviceName"></param>
        public static void UninstallWindowService(String serviceName)
        {
            ServiceInstaller installer = new ServiceInstaller();
            installer.UnInstallService(serviceName);
        }
    }
}
