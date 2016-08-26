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

using System.IO;

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
        /// <returns>
        /// 接口与实现类的键值对，可能有的接口没有实现类（当然调用方法会控制）
        /// </returns>
        public static bool VerifyImplement(IEnumerable<Type> contractTypes, IEnumerable<Type> implementTypes, out Dictionary<String, String> contractImp)
        {

            contractImp = new Dictionary<String, String>();
            //var contractAssembly = Assembly.LoadFile(contractAssemblyUrl);
            //var implementAssembly = Assembly.LoadFile(implementAssemblyUrl);
            //var contractTypes = contractAssembly.GetExportedTypes().Where(t=>t.IsInterface);
            //var implementTypes = implementAssembly.GetExportedTypes();
            var impCount = 0;
            foreach (var contractType in contractTypes)
            {
                contractImp.Add(contractType.Name, "");
                foreach (var impType in implementTypes)
                {
                    if (contractType.IsAssignableFrom(impType))
                    {
                        contractImp[contractType.Name]=impType.Name;
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
        /// service中的App.config 地址 http://192.168.1.233/webservice/n_webservice.asmx
        /// </param>
        /// <param name="impConfigUrl">用户自定义实现类库的配置文件</param>
        /// <param name="contractAssemblyUrl">接口类库地址</param>
        /// <param name="implementAssemblyUrl">实现类库地址</param>
        /// <param name="webserviceUrl">HIS暴露的webservice服务地址</param>
        public static  String ConfigUnityConfig(String configUrl,String impConfigUrl, String contractAssemblyUrl,String implementAssemblyUrl)
        {
            var indicate = String.Empty;
            //Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //var webconfigUrl = @"\App.config";
            //var configuration = WebConfigurationManager.OpenWebConfiguration(configUrl) as Configuration;
            //var container = GetContainer(configUrl, configuration);

            var contractAssembly = Assembly.LoadFrom(contractAssemblyUrl);
            var implementAssembly = Assembly.LoadFrom(implementAssemblyUrl);
            var contractTypes = contractAssembly.GetExportedTypes().Where(t => t.IsInterface);
            var implementTypes = implementAssembly.GetExportedTypes();
            //接口与实现类的键值对
            Dictionary<String, String> contractImp;
            //检测实现类是否都实现了Contract
            //VerifyImplement(contractTypes, implementTypes, out contractImp);
            if (VerifyImplement(contractTypes, implementTypes, out contractImp))
            {
                //var section = (UnityConfigurationSection)configuration.GetSection("unity");
                var ignoreAssembilies = new List<String> { "eHPS.Contract", "eHPS.CrossCutting.NetFramework" };
                var ignoreNamesapces = new List<String> { "eHPS.Contract", "eHPS.CrossCutting.NetFramework", "eHPS.CrossCutting.NetFramework.ExceptionHandler" };
                if (File.Exists(configUrl))
                {
                    var root = XElement.Load(configUrl);
                    

                    #region unity 配置修改
                    //config 文件中unity标签
                    var unityElement = root.Element("unity");
                    //config 文件中unity标签下的assembly 标签
                    var assemblyElements = unityElement.Elements("assembly");
                    //config 文件中unity标签下的namespace 标签
                    var namespaceElements = unityElement.Elements("namespace");
                    //config 文件中unity标签下的container 标签
                    var containerElement = unityElement.Element("container");
                    //config 文件中unity标签下的container 标签中的register
                    var registerElements = containerElement.Elements("register");

                    //删除config中存在的实现类的assembly以及namespace
                    assemblyElements.Where(a => !ignoreAssembilies.Contains((String)a.Attribute("name"))).Remove();
                    namespaceElements.Where(r => !ignoreNamesapces.Contains((String)r.Attribute("name"))).Remove();
                    //增加现有实现类的assembly以及namespace
                    var currentAssembly = new XElement("assembly", new XAttribute("name", implementAssembly.GetName().Name));
                    var currentNamespace = new XElement("namespace", new XAttribute("name", implementAssembly.GetName().Name));

                    containerElement.AddBeforeSelf(currentAssembly);
                    containerElement.AddBeforeSelf(currentNamespace);
                    //替换 register mapTo attribute
                    foreach (var element in registerElements)
                    {
                        foreach (var contractType in contractTypes)
                        {
                            if ((String)element.Attribute("type") == contractType.Name)
                            {
                                element.Attribute("mapTo").SetValue(contractImp[contractType.Name]);
                                break;
                            }
                        }
                    }

                    #endregion

                    #region appsetting 配置 连接字符串配置  web service 配置  
                    var appSettings = root.Element("appSettings");
                    var connectionSettings = root.Element("connectionStrings");
                    if (File.Exists(impConfigUrl))
                    {
                        var impRoot = XElement.Load(impConfigUrl);

                        var impAppSettings = impRoot.Element("appSettings");
                        if (impAppSettings != null)
                        {
                            var impAppSettingsItem = impAppSettings.Elements("add");
                            appSettings.Add(impAppSettings);
                        }

                        var impConnectionStrings = impRoot.Element("connectionStrings");
                        if (impConnectionStrings != null)
                        {
                            connectionSettings.Add(impConnectionStrings.Elements("add"));
                        }


                        var impWebService = impRoot.Element("system.serviceModel");
                        if (impWebService != null)
                        {
                            root.Add(impWebService);
                        }

                    }

                   
                    

                    #endregion


                    root.Save(configUrl);
                }
                else
                {
                    indicate = "API配置文件未发现！";
                }

            }
            else
            {
                indicate = "类库验证未通过！";
            }

            return indicate;
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
