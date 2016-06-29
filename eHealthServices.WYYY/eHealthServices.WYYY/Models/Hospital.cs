using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHealthServices.WYYY.Models
{
    public class Hospital
    {
        public int HID { get; set; }
        public string Secret { get; set; }
        /// <summary>
        /// 1简单模式（数据托管在云端），2开发者模式（医院提供webservice），3高级开发者模式
        /// </summary>
        public int Mode { get; set; }
        public string ShortName { get; set; }

        public string ServiceURL { get; set; }
    }
}