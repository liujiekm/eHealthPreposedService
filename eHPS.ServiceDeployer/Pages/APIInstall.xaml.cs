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

using System.IO;

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
        }

        private void OpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            // 创建 OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".dll",
                Filter = "Class Library (.dll)|*.dll"
            };

            // 设置默认的文件类型
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                FileNameTextBox.Text = filename;

                

                //获取当前目录中的所有文件
                var files = Directory.GetFiles(filename.Substring(0,filename.LastIndexOf("\\", StringComparison.Ordinal)));
                var dataSource = new ObservableCollection<String>();
                foreach (var file in files)
                {
                    dataSource.Add(file);
                }

                this.FileView.DataContext = dataSource;

            }
        }
    }
}
