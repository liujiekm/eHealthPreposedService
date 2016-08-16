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

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

using eHPS.Contract;
using System.Reflection;
using Microsoft.Practices.Unity.InterceptionExtension;
using eHPS.CrossCutting.NetFramework.ExceptionHandler;
using System.Xml.Linq;

namespace eHPS.Common
{
    /// <summary>
    /// 操作webconfig以及appconfig文件，动态添加修改config内容的帮助类
    /// </summary>
    public class ConfigHelper
    {

        public static void GetPath()
        {
            
        }

        /// <summary>
        /// 验证具体实现类是否都实现了eHPS.Contract类库中的接口
        /// </summary>
        /// <returns></returns>
        public static bool VerifyImplement(String contractAssemblyUrl,String implementAssemblyUrl)
        {
            var contractAssembly = Assembly.LoadFile(contractAssemblyUrl);
            var implementAssembly = Assembly.LoadFile(implementAssemblyUrl);
            var contractTypes = contractAssembly.GetExportedTypes().Where(t=>t.IsInterface); ;
            var implementTypes = implementAssembly.GetExportedTypes();
            var impCount = 0;
            foreach (var contractType in contractTypes)
            {
                foreach (var impAssembly in implementTypes)
                {
                    if (contractType.IsAssignableFrom(impAssembly))
                    {
                        impCount++;
                        break;
                    }
                }
            }
            if (contractTypes.Count() == impCount)//实现类库都实现了定义的接口库eHPS.Contract
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 跟新Unity Config配置信息为当前最新实现
        /// </summary>
        /// <param name="configUrl">API中的webconfig或者windows
        /// service中的App.config 相对地址
        /// </param>
        /// <param name="contractAssemblyUrl">接口类库地址</param>
        /// <param name="implementAssemblyUrl">实现类库地址</param>
        public static  void ConfigUnityConfig(String configUrl, String contractAssemblyUrl,String implementAssemblyUrl)
        {
            //Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //var webconfigUrl = @"\App.config";
            var configuration = WebConfigurationManager.OpenWebConfiguration(configUrl) as Configuration;
            //var container = GetContainer(configUrl, configuration);

            var root = XElement.Load(configUrl);

            


            var contractAssembly = Assembly.LoadFile(contractAssemblyUrl);
            var implementAssembly = Assembly.LoadFile(implementAssemblyUrl);
            var contractTypes = contractAssembly.GetExportedTypes();
            var implementTypes = implementAssembly.GetExportedTypes();
            //检测实现类是否都实现了Contract
            VerifyImplement(contractAssemblyUrl, implementAssemblyUrl);
            //if (VerifyImplement(contractAssemblyUrl,implementAssemblyUrl))
            //{
                var section = (UnityConfigurationSection)configuration.GetSection("unity");
                #region 移除已实现的服务，增加自己实现的服务类库
                var ignoreAssembilies = new List<String> { "eHPS.Contract", "eHPS.CrossCutting.NetFramework" };
                var ignoreNamesapces = new List<String> { "eHPS.Contract", "eHPS.CrossCutting.NetFramework", "eHPS.CrossCutting.NetFramework.ExceptionHandler" };

                var currentAssemblies = section.Assemblies;
                for (int i = 0; i < currentAssemblies.Count; i++)
                {
                    if (!ignoreAssembilies.Contains(currentAssemblies[i].Name))
                    {
                        section.Assemblies.RemoveAt(i);
                        currentAssemblies = section.Assemblies;
                    }
                }

                var currentNamespaces = section.Namespaces;
                for (int i = 0; i < currentNamespaces.Count; i++)
                {
                    if (!ignoreNamesapces.Contains(currentNamespaces[i].Name))
                    {
                        section.Namespaces.RemoveAt(i);
                        currentNamespaces = section.Namespaces;
                    }
                }

                section.Assemblies.Add(new AssemblyElement() { Name = implementAssembly.FullName });
                section.Namespaces.Add(new NamespaceElement() { Name = implementAssembly.FullName });
                #endregion

                #region 增加Registry至Container
                var containers = section.Containers;
                foreach (var container in containers)
                {
                    foreach (var contractType in contractTypes)
                    {
                        if(container.Registrations.Any(t=>t.TypeName==contractType.Name))
                        {
                            container.Registrations.FirstOrDefault(t => t.TypeName == contractType.Name).MapToName = contractType.Name;
                        }
                    }
                }
                #endregion
                section.CurrentConfiguration.Save();
            //}          
        }



        /// <summary>
        /// 获取Unity 配置项信息
        /// </summary>
        /// <param name="configUrl"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static  IUnityContainer GetContainer(String configUrl,Configuration configuration)
        {
            //var map = new ExeConfigurationFileMap();
            //map.ExeConfigFilename = "web.config";
            //var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            //var configuration = WebConfigurationManager.OpenWebConfiguration(configUrl);
            var section = (UnityConfigurationSection)configuration.GetSection("unity");
            var container = new UnityContainer();
            container.LoadConfiguration(section);
            return container;

            
        }
    }
}
