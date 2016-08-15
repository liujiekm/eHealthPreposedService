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

        private void Modify(String configUrl)
        {
            //获取当前安装工具的执行目录
            //var url = Environment.CurrentDirectory;
            var configuration = WebConfigurationManager.OpenWebConfiguration(configUrl);
            if(configuration!=null)
            {
                var section = (ConnectionStringsSection)configuration.GetSection("connectionStrings");
                section.ConnectionStrings["MyConnectionString"].ConnectionString = "Data Source=...";
                configuration.Save();
            }

        }


        /// <summary>
        /// 验证具体实现类是否都实现了eHPS.Contract类库中的接口
        /// </summary>
        /// <returns></returns>
        //public  Tuple<String,String,String,String,String> VerifyImplement(String implementAssemblyUrl)
        //{

        //    return false;
        //}


        public static  void ConfigUnityConfig(String configUrl,String implementAssemblyUrl)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


            //C:\Jay\Workstation\Team\Projects\fangxin_eHealth\eHealthPreposedService\eHPS.API
            //var webconfigUrl = @"\eHPS.API\web.config";
            //var webconfiguration = WebConfigurationManager.OpenWebConfiguration(webconfigUrl);
            var container = GetContainer(configUrl, configuration);

            //动态加载实现类库
            Assembly assembly = Assembly.LoadFile(implementAssemblyUrl);
            //检测实现类是否都实现了Contract

            
            var contractTypes = new List<Type> { typeof(IBasicInfo), typeof(IAppointment), typeof(IDiagnosis), typeof(IInspection), typeof(IPayment) };

            var impCount = 0;
            foreach (var contractType in contractTypes)
            {
                foreach (var impAssembly in assembly.GetExportedTypes())
                {
                    if (contractType.IsAssignableFrom(impAssembly))
                    {
                        impCount++;
                        break;
                    }
                }
                        
            }
            if(contractTypes.Count==impCount)//实现类库都实现了定义的接口库eHPS.Contract
            {

            }


            var implementTypes = assembly.GetExportedTypes();
            var basicImplement = implementTypes.Where(t => typeof(IBasicInfo).IsAssignableFrom(t)).FirstOrDefault();

           
            var section = (UnityConfigurationSection)configuration.GetSection("unity");


            #region 移除已实现的服务，增加自己实现的服务类库
            var ignoreAssembilies = new List<String> { "eHPS.Contract", "eHPS.CrossCutting.NetFramework" };
            var ignoreNamesapces = new List<String> { "eHPS.Contract", "eHPS.CrossCutting.NetFramework", "eHPS.CrossCutting.NetFramework.ExceptionHandler" };

            var currentAssemblies = section.Assemblies;
            for (int i = 0; i < currentAssemblies.Count; i++)
            {
                if(!ignoreAssembilies.Contains(currentAssemblies[i].Name))
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

            section.Assemblies.Add(new AssemblyElement() { Name= basicImplement.AssemblyQualifiedName });
            section.Namespaces.Add(new NamespaceElement() { Name= basicImplement.Namespace });
            #endregion


            foreach (var c in section.Containers)
            {
                c.Registrations.FirstOrDefault(r => r.TypeName == typeof(IBasicInfo).Name).MapToName = basicImplement.Name;
            }
            section.CurrentConfiguration.Save();
            
        }

        private static  IUnityContainer GetContainer(String configUrl,Configuration configuration)
        {
            //var map = new ExeConfigurationFileMap();
            //map.ExeConfigFilename = "web.config";
            //var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            
            //var configuration = WebConfigurationManager.OpenWebConfiguration(configUrl);
            var section = (UnityConfigurationSection)configuration.GetSection("unity");
            var container = new UnityContainer();
            container.LoadConfiguration(section);
            //section.Containers["container"].Configure(container);

            
            return container;

            
        }
    }
}
