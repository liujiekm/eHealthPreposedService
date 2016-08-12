using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace eHPS.ImplementTest
{
    [TestClass]
    public class CommonTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //C:\Jay\Workstation\Team\Projects\fangxin_eHealth\eHealthPreposedService\eHPS.ImplementTest\bin\Debug
            var url = Environment.CurrentDirectory;

            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            //C:\Jay\Workstation\Team\Projects\fangxin_eHealth\eHealthPreposedService\eHPS.ImplementTest\bin\Debug
            var url2 = AppDomain.CurrentDomain.BaseDirectory;

            var url3 = Process.GetCurrentProcess().MainModule.FileName;



            var url4 = Directory.GetCurrentDirectory();
            Assert.IsNotNull(url);
            Assert.IsNotNull(url2);
            Assert.IsNotNull(url3);
        }
    }
}
