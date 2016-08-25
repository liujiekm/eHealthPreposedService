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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Microsoft.Web.Administration;
using Microsoft.Win32;
using System.Threading;

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



        /// <summary>
        /// 优化web服务器设置
        /// </summary>
        public static void OptimizeWebServer()
        {
            if (!TuneMachineConfig() || !TuneASPNETConfig())
            {
                return;
            }
            Tune();
        }


        private static string[] SetRegistryEntry(Dictionary<string, int> entries)
        {
            List<string> cmds = new List<string>();

            foreach (string k in entries.Keys)
            {
                string position = k.Split(':')[0];
                string parameter = k.Split(':')[1];
                int value = Convert.ToInt32(entries[k]);

                //reg add HKLM\System\CurrentControlSet\Services\TcpIp\Parameters /v TcpTimedWaitDelay /t REG_DWORD /d 30 /f
                cmds.Add("reg add HKLM\\" + position + " /v " + parameter + " /t REG_DWORD /d " + value.ToString() + " /f");
            }

            return cmds.ToArray();
        }



        /// <summary>
        /// 修改服务器本地MachingConfig配置
        /// </summary>
        /// <returns></returns>
        public static bool TuneMachineConfig()
        {
            List<string> frameworks = new List<string>();
            if (Net2Exist) frameworks.Add(Net2Dir + "\\Config\\");
            if (Net4Exist) frameworks.Add(Net4Dir + "\\Config\\");
            if (Net264Exist) frameworks.Add(Net264Dir + "\\Config\\");
            if (Net464Exist) frameworks.Add(Net464Dir + "\\Config\\");

            foreach (string dir in frameworks)
            {
                try
                {
                    File.Copy(dir + "machine.config", dir + "machine.config.bak." + DateTime.Now.ToFileTimeUtc().ToString());
                }
                catch
                {
                    //MessageBox.Show(Loading.AdminMessage);
                    return false;
                }
                StreamReader rd = new StreamReader(dir + "machine.config", Encoding.Default);
                string config = rd.ReadToEnd();
                rd.Close();

                int posweb = config.IndexOf("<system.web>");
                if (posweb > 0)
                {
                    posweb = config.IndexOf("<processModel", posweb);
                    int endpos = config.IndexOf("/>", posweb);
                    string left = config.Substring(0, posweb);
                    string right = config.Substring(endpos + 2, config.Length - (endpos + 2));
                    config = left
                        + "<processModel autoConfig=\"true\" minIoThreads=\"30\" maxWorkerThreads=\"100\" maxIoThreads=\"100\" minWorkerThreads=\"100\" requestQueueLimit=\"Infinite\" />"
                        + right;

                    StreamWriter wr = new StreamWriter(dir + "machine.config", false);
                    wr.Write(config);
                    wr.Close();
                }
            }

            frameworks.Clear();
            return true;
        }


        /// <summary>
        /// 修改asp.net config配置
        /// </summary>
        /// <returns></returns>
        public static bool TuneASPNETConfig()
        {
            List<string> frameworks = new List<string>();
            if (Net2Exist) frameworks.Add(Net2Dir + "\\");
            if (Net4Exist) frameworks.Add(Net4Dir + "\\");
            if (Net264Exist) frameworks.Add(Net264Dir + "\\");
            if (Net464Exist) frameworks.Add(Net464Dir + "\\");

            foreach (string dir in frameworks)
            {
                try
                {
                    File.Copy(dir + "Aspnet.config", dir + "Aspnet.config.bak." + DateTime.Now.ToFileTimeUtc().ToString());
                }
                catch
                {
                    //MessageBox.Show(Loading.AdminMessage);
                    return false;
                }
                StreamReader rd = new StreamReader(dir + "Aspnet.config", Encoding.Default);
                string config = rd.ReadToEnd();
                rd.Close();

                bool found = config.Contains("</configuration>");
                found = found && !config.Contains("maxConcurrentRequestsPerCPU");
                if (found)
                {
                    if (!config.Contains("</system.web>"))
                        config = config.Replace("</configuration>", "<system.web><applicationPool maxConcurrentRequestsPerCPU=\"5000\" maxConcurrentThreadsPerCPU=\"0\" requestQueueLimit=\"25000\" /></system.web></configuration>");
                    else
                        config = config.Replace("</system.web>", "<applicationPool maxConcurrentRequestsPerCPU=\"5000\" maxConcurrentThreadsPerCPU=\"0\" requestQueueLimit=\"25000\" /></system.web>");

                    StreamWriter wr = new StreamWriter(dir + "Aspnet.config", false);
                    wr.Write(config);
                    wr.Close();
                }
            }

            frameworks.Clear();

            return true;
        }


        static bool _processWorking = false;

        /// <summary>
        /// 运行CMD命令行
        /// </summary>
        /// <param name="cmds"></param>
        internal static void RunMSDOS(string[] cmds)
        {
            if (_processWorking)
                return;

            _processWorking = true;

            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            Process msdos = new Process { StartInfo = psi, EnableRaisingEvents = true };

            msdos.Exited += new EventHandler(MSDOSExited);
            msdos.Start();
            foreach (string cmd in cmds)
                msdos.StandardInput.WriteLine(cmd);


            msdos.StandardInput.WriteLine("exit");

            WaitMSDOS();
            
        }

        internal static void WaitMSDOS()
        {
            while (true)
            {
                if (!_processWorking)
                    break;
                Thread.Sleep(100);
                
            }
        }

        static void MSDOSExited(object sender, EventArgs e)
        {
            _processWorking = false;
        }


        /// <summary>
        /// 修改本机注册表信息
        /// </summary>
        public static  void Tune()
        {
            Dictionary<string, int> cmds = new Dictionary<string, int>();
            cmds.Add("System\\CurrentControlSet\\Services\\HTTP\\Parameters:EnableAggressiveMemoryUsage", 1);//1
            cmds.Add("System\\CurrentControlSet\\Services\\HTTP\\Parameters:EnableCopySend", 1);//1
            cmds.Add("System\\CurrentControlSet\\Services\\TcpIp\\Parameters:TcpTimedWaitDelay", 30);//30
            cmds.Add("System\\CurrentControlSet\\Services\\HTTP\\Parameters:MaxConnections", 65535);//65535

            if (Net2Exist)
            {
                cmds.Add("SOFTWARE\\Microsoft\\ASP.NET\\2.0.50727.0:MaxConcurrentRequestsPerCPU", 0);//0
                cmds.Add("SOFTWARE\\Microsoft\\ASP.NET\\2.0.50727.0:MaxConcurrentThreadsPerCPU", 0);//0 
            }

            if (Net4Exist)
            {
                cmds.Add("SOFTWARE\\Microsoft\\ASP.NET\\4.0.30319.0:MaxConcurrentRequestsPerCPU", 0);//0
                cmds.Add("SOFTWARE\\Microsoft\\ASP.NET\\4.0.30319.0:MaxConcurrentThreadsPerCPU", 0);//0
            }

            string[] cmd = SetRegistryEntry(cmds);
            RunMSDOS(cmd);
        }







        /// <summary>
        /// 创建应用程序池
        /// </summary>
        /// <param name="poolname"></param>
        /// <param name="enable32BitOn64"></param>
        /// <param name="mode"></param>
        /// <param name="runtimeVersion"></param>
        public  static void CreateAppPool(string poolname, bool enable32BitOn64, ManagedPipelineMode mode, string runtimeVersion = "v4.0")
        {
            using (ServerManager serverManager = new ServerManager())
            {
                ApplicationPool newPool = serverManager.ApplicationPools.Add(poolname);

                newPool.ManagedRuntimeVersion = runtimeVersion;
                newPool.Enable32BitAppOnWin64 = enable32BitOn64;
                newPool.ManagedPipelineMode = mode;

                newPool.AutoStart = true;
                newPool.StartMode=StartMode.AlwaysRunning;
                newPool.QueueLength = 65535;
                newPool.ProcessModel.ShutdownTimeLimit=new TimeSpan(110);
                newPool.ProcessModel.MaxProcesses = 100;
                
                
                serverManager.CommitChanges();
            }
        }

        /// <summary>
        /// 优化IIS 应用程序池设置
        /// </summary>
        /// <param name="pool"></param>
        public static void OptimizeAppliactionPool(ApplicationPool pool)
        {
            pool.AutoStart = true;
            pool.StartMode = StartMode.AlwaysRunning;
            pool.QueueLength = 65535;
            pool.ProcessModel.ShutdownTimeLimit = TimeSpan.FromSeconds(110);
            pool.ProcessModel.MaxProcesses = 100;
        }



    }
}
