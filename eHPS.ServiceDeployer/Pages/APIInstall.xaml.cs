using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Reflection;
using eHPS.Common;

namespace eHPS.ServiceDeployer.Pages
{
    /// <summary>
    /// Interaction logic for APIInstall.xaml
    /// </summary>
    public partial class APIInstall : UserControl
    {
        public APIInstall()
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
                    var contractUrl = Environment.CurrentDirectory+@"\Contract\eHPS.Contract.dll";

                    if (File.Exists(contractUrl))
                    {
                        var contractTypes = Assembly.LoadFrom(contractUrl).GetExportedTypes().Where(t=>t.IsInterface);
                        var impTypes = Assembly.LoadFrom(impDll.ToString()).GetExportedTypes();
                        var contractImp = new Dictionary<String,String>();
                        if (ConfigHelper.VerifyImplement(contractTypes, impTypes,out contractImp))
                        {
                            validInfo = "验证通过！";
                        }
                        else
                        {
                            var unImps = contractImp.Where((item) => String.IsNullOrEmpty(item.Value)).Select(p => p.Key).ToList();
                            validInfo = String.Join(",",unImps)+" 未实现！";
                        }
                    }
                    else
                    {
                        validInfo = "内部错误 Contract  403！";
                    }
                }
                else
                {
                    validInfo = "请选择dll文件！";
                }
            }
            else
            {
                validInfo = "请选择具体实现类库！";
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
            var configUrl = Environment.CurrentDirectory + @"\APIDeploy\Web.config";
            var webserviceUrl = "";
            var result = ConfigHelper.ConfigUnityConfig(configUrl,contractUrl,impDll.ToString(),webserviceUrl);
            if (result == "")
            {
                //拷贝实现文件夹中的类库到API 的bin目录
                foreach (var file in this.FileView.ItemsSource)
                {
                    if (file.ToString().EndsWith(".dll"))
                    {
                        var destFile = Environment.CurrentDirectory + @"\APIDeploy\bin\" +
                                   file.ToString()
                                       .Substring(file.ToString().LastIndexOf("\\", StringComparison.Ordinal) + 1);

                        File.Copy(file.ToString(), destFile);
                    }
                }

                this.BuildInfo.Content = "配置成功！";
            }
            else
            {
                this.BuildInfo.Content = result;
            }

        }



        private void DeployApi_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
