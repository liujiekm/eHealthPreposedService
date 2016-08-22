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
        public static String DeploySite(String siteName, BindingProtocol protocol,Int32 port, String deployFolderUrl)
        {
            var indicate = String.Empty;
            using (ServerManager iisManager = new ServerManager())
            {
                if (!IsSiteExist(siteName))
                {
                    var site = iisManager.Sites.Add(siteName, protocol.ToString(), "*:" + port + ":", deployFolderUrl);
                    //"d:\\MySite"
                    //创建应用程序池
                    ApplicationPool newPool = iisManager.ApplicationPools.Add(siteName);
                    newPool.ManagedRuntimeVersion = "v4.0";
                    newPool.Enable32BitAppOnWin64 = false;
                    newPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                    site.ApplicationDefaults.ApplicationPoolName = siteName;
                    //网站下所有应用程序的应用程序池都设置为新创建的newPool

                    foreach (var item in site.Applications)
                    {
                        item.ApplicationPoolName = siteName;
                    }

                    iisManager.CommitChanges();
                }
                else
                {
                    indicate = "该网站已存在，请重新输入网站名称";
                }
            }

            return indicate;
        }

        public static String DeployApplication(String siteName,String applicationName,String deployFolderUrl)
        {
            var indicate = String.Empty;
            using (ServerManager iisManager = new ServerManager())
            {
                if (IsSiteExist(siteName))
                {
                    if (!IsApplicationExist(siteName, applicationName))
                    {
                        var application = iisManager.Sites[siteName].Applications.Add("/" + applicationName, deployFolderUrl);
                        //"d:\\MyApp"

                        //创建应用程序池
                        ApplicationPool newPool = iisManager.ApplicationPools.Add(applicationName);
                        newPool.ManagedRuntimeVersion = "v4.0";
                        newPool.Enable32BitAppOnWin64 = false;
                        newPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                        application.ApplicationPoolName = applicationName;

                        iisManager.CommitChanges();
                    }
                    else
                    {
                        indicate = "该网站下应用程序已存在！";
                    }

                }
                else
                {
                    indicate = "该网站不存在！";
                }
            }

            return indicate;
        }

        /// <summary>
        /// 获取当前服务器IIS下面的网站
        /// </summary>
        /// <returns></returns>
        public static  List<String> GetCurrentIisSite()
        {
            using (ServerManager iisManager = new ServerManager())
            {
                return iisManager.Sites.Select(s => s.Name + " ：" + s.Bindings.Select(b => b.EndPoint.Port.ToString()).FirstOrDefault()).ToList();
            }
                
        }


        /// <summary>
        /// 当前服务器IIS中是否已经包含指定名称的网站
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public  static bool IsSiteExist(String siteName)
        {
            using (ServerManager iisManager = new ServerManager())
            {
                return iisManager.Sites.Any(s => s.Name == siteName);
            }
                
        }



        /// <summary>
        /// 指定网站下面的应用程序是否存在
        /// </summary>
        /// <param name="siteName">网站名称</param>
        /// <param name="applicationName">应用程序名称</param>
        /// <returns></returns>
        private static bool IsApplicationExist(String siteName,String applicationName)
        {
            using (ServerManager iisManager = new ServerManager())
            {
                if (IsSiteExist(siteName))
                {
                    return iisManager.Sites[siteName].Applications.Any(a => a.Path == "/" + applicationName);
                }
                else
                {
                    return false;
                }
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
