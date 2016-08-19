//===================================================================================
// 北京联想智慧医疗信息技术有限公司 & 上海研发中心
//===================================================================================
// 服务器IIS 配置工具类
//
//
//===================================================================================
// .Net Framework 4.5
// CLR版本： 4.0.30319.42000
// 创建人：  Jay
// 创建时间：2016/8/19 17:27:40
// 版本号：  V1.0.0.0
//===================================================================================




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Microsoft.Win32;

namespace eHPS.Common
{
    /// <summary>
    /// 服务器IIS 配置工具类
    /// </summary>
    public class IISHelper
    {
        public static string InetDir = string.Empty;
        public static string AdminMessage = "权限不足，请以管理员方式打开当前应用程序";
        public static RegistryKey HKLM = null;
        public static  RegistryKey IIS = null;
        public static int IISVersion = 0;

        public static string Net2Dir = string.Empty;
        public static string Net4Dir = string.Empty;
        public static string Net264Dir = string.Empty;
        public static string Net464Dir = string.Empty;

        public static bool Net264Exist = false;
        public static bool Net464Exist = false;
        public static bool Net2Exist = false;
        public static bool Net4Exist = false;

        /// <summary>
        /// 验证IIS 是否安装，版本
        /// </summary>
        /// <returns></returns>
        public static String LocateIIS()
        {
            var indicate = String.Empty;
            try
            {

                HKLM = Registry.LocalMachine;

                IIS = HKLM.OpenSubKey("Software\\Microsoft\\InetStp");
                if (IIS == null)
                {
                    indicate = "IIS 没有安装!";
                    
                }
                IISVersion = Convert.ToInt16(IIS.GetValue("MajorVersion"));
                if (IISVersion < 7)
                {
                    indicate = "需要 IIS 版本 7 以上";

                }

                if (IISVersion == 7)
                    InetDir = IIS.GetValue("InstallPath").ToString();
            }
            catch
            {
                indicate = AdminMessage;
                
            }

            return indicate;
        }

        /// <summary>
        /// 验证当前服务器是否安装.net framework
        /// </summary>
        /// <returns></returns>
        public static  String  VerifyNetFramwork()
        {
            var indicate = String.Empty;
            String windir = System.Environment.GetEnvironmentVariable("windir");
            Net2Dir = windir + "\\Microsoft.NET\\Framework\\v2.0.50727";
            Net4Dir = windir + "\\Microsoft.NET\\Framework\\v4.0.30319";
            Net2Exist = Directory.Exists(Net2Dir);
            Net4Exist = Directory.Exists(Net4Dir);
            Net264Dir = windir + "\\Microsoft.NET\\Framework64\\v2.0.50727";
            Net464Dir = windir + "\\Microsoft.NET\\Framework64\\v4.0.30319";
            Net264Exist = Directory.Exists(Net264Dir);
            Net464Exist = Directory.Exists(Net464Dir);

            if (!Net2Exist && !Net4Exist)
            {
                indicate = "当前服务器没有安装.Net Framework";
            }

            if (!Net4Exist)
            {
                indicate = "部署环境需.Net Framework 4 以上";
            }
            return indicate;

        }
    }
}
