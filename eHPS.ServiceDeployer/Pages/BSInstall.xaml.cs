using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using eHPS.Common;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace eHPS.ServiceDeployer.Pages
{
    /// <summary>
    /// Interaction logic for BSInstall.xaml
    /// </summary>
    public partial class BSInstall : UserControl
    {
        /// <summary>
        /// 实现类库是否验证通过
        /// </summary>
        private bool IsImplementValid = false;


        /// <summary>
        /// 根据实现类库，部署文件是否配置通过
        /// </summary>
        private bool IsConfigured = false;


        public BSInstall()
        {
            InitializeComponent();

            //检测服务器环境Net Framework,IIS版本等
            var netFrameworkIndicate = IISHelper.VerifyNetFramwork();
            if (!String.IsNullOrEmpty(netFrameworkIndicate))
            {
                MessageBox.Show(netFrameworkIndicate);


                return;
            }
            var iisIndicate = IISHelper.LocateIIS();
            if (!String.IsNullOrEmpty(iisIndicate))
            {
                MessageBox.Show(iisIndicate);
                return;
            }








        }

        private void OpenFile_OnClick(object sender, RoutedEventArgs e)
        {

            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Multiselect = false

            };
            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {

                string filename = dialog.FileName;
                FileNameTextBox.Text = filename;

                //    //获取当前目录中的所有文件
                var files = Directory.GetFiles(filename, "*", SearchOption.TopDirectoryOnly);
                this.FileView.ItemsSource = files;

            }
            

            Window wid = Window.GetWindow(this);

            //MessageBox.Show(wid.ToString());

            #region OpenFileDialog

            //// 创建 OpenFileDialog 
            //Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            //{
            //    DefaultExt = ".dll",
            //    Filter = "Class Library (.dll)|*.dll"
            //};
            //// 设置默认的文件类型
            //bool? result = dlg.ShowDialog();
            //if (result == true)
            //{
            //    string filename = dlg.FileName;
            //    FileNameTextBox.Text = filename;
            //    //获取当前目录中的所有文件
            //    var files = Directory.GetFiles(filename.Substring(0,filename.LastIndexOf("\\", StringComparison.Ordinal)),"*",SearchOption.TopDirectoryOnly);
            //    var dataSource = new ObservableCollection<String>();
            //    this.FileView.ItemsSource = files;
            //}

            #endregion
        }


        /// <summary>
        /// 验证类库是否都实现了具体的接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            //获得选中的dll
            var impDll = this.FileView.SelectedItem;
            var validInfo = String.Empty;
            //验证是否dll
            if (impDll != null)
            {
                if (impDll.ToString().EndsWith(".dll"))
                {
                    var contractUrl = Environment.CurrentDirectory + @"\Contract\eHPS.Contract.dll";

                    if (File.Exists(contractUrl))
                    {
                        var contractTypes = Assembly.LoadFrom(contractUrl).GetExportedTypes().Where(t => t.IsInterface);
                        var impTypes = Assembly.LoadFrom(impDll.ToString()).GetExportedTypes();
                        var contractImp = new Dictionary<String, String>();
                        if (ConfigHelper.VerifyImplement(contractTypes, impTypes, out contractImp))
                        {
                            validInfo = "验证通过！";

                            this.Build.IsEnabled = true;
                            this.Build.Background = new SolidColorBrush(Color.FromArgb(100, 0, 111, 255));
                            IsImplementValid = true;
                        }
                        else
                        {
                            var unImps = contractImp.Where((item) => String.IsNullOrEmpty(item.Value)).Select(p => p.Key).ToList();
                            validInfo = String.Join(",", unImps) + " 未实现！";
                            IsImplementValid = false;
                            this.Build.IsEnabled = false;
                            this.Build.Background = new SolidColorBrush(Colors.Gray);
                        }
                    }
                    else
                    {
                        validInfo = "内部错误 Contract  403！";
                        IsImplementValid = false;
                        this.Build.IsEnabled = false;
                        this.Build.Background = new SolidColorBrush(Colors.Gray);
                    }
                }
                else
                {
                    validInfo = "请选择dll文件！";
                    IsImplementValid = false;
                    this.Build.IsEnabled = false;
                    this.Build.Background = new SolidColorBrush(Colors.Gray);
                }
            }
            else
            {
                validInfo = "请选择具体实现类库！";
                IsImplementValid = false;
                this.Build.IsEnabled = false;
                this.Build.Background = new SolidColorBrush(Colors.Gray);
            }
            this.VerifyInfo.Content = validInfo;
        }


        /// <summary>
        /// 把实现类库加入API 部署包，并修改Config中实现类的配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Build_Click(object sender, RoutedEventArgs e)
        {
            var contractUrl = Environment.CurrentDirectory + @"\Contract\eHPS.Contract.dll";
            var impDll = this.FileView.SelectedItem;
            var configUrl = Environment.CurrentDirectory + @"\BSDeploy\App.config";
            var webserviceUrl = "";
            var result = ConfigHelper.ConfigUnityConfig(configUrl, contractUrl, impDll.ToString(), webserviceUrl);
            if (result == "")
            {
                //拷贝实现文件夹中的类库到API 的bin目录
                foreach (var file in this.FileView.ItemsSource)
                {
                    if (file.ToString().EndsWith(".dll"))
                    {
                        var destFile = Environment.CurrentDirectory + @"\BSDeploy\" +
                                   file.ToString()
                                       .Substring(file.ToString().LastIndexOf("\\", StringComparison.Ordinal) + 1);

                        File.Copy(file.ToString(), destFile, true);
                    }
                }

                this.BuildInfo.Content = "配置成功！";
                this.Deploy.Background = new SolidColorBrush(Color.FromArgb(100, 0, 111, 255));
                this.Deploy.IsEnabled = true;

                this.IsConfigured = true;

            }
            else
            {
                this.BuildInfo.Content = result;
                this.IsConfigured = false;
                this.Deploy.IsEnabled = false;
                this.Deploy.Background = new SolidColorBrush(Colors.Gray);
            }

        }



        /// <summary>
        /// 新建网站或者应用程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Deploy_Click(object sender, RoutedEventArgs e)
        {
            var serviceName = this.WindowsServiceName.Text;
            if (!Regex.IsMatch(serviceName, @"[a-zA-Z]"))
            {
                MessageBox.Show("请输入英文字符！");
                return;
            }
            var serviceUrl = Environment.CurrentDirectory + @"\BSDeploy\eHPS.BackgroundService.exe";
            var result = DeployHelper.InstallWindowService(serviceUrl, serviceName, serviceName);
            MessageBox.Show(result == "" ? "部署成功" : result);
        }
    }
}
