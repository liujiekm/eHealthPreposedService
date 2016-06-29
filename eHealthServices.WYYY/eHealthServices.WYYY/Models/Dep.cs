using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHealthServices.WYYY.Models
{
    public class Dep
    {
        /// <summary>
        /// 科室id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 子科室
        /// </summary>
        public List<Dep> Childrens { get; set; }
    }
    public class HospitalDepNavigation
    {
        /// <summary>
        /// 导航id（可不填）
        /// </summary>
        public long NID { get; set; }
        /// <summary>
        /// 医院id（可不填）
        /// </summary>
        public long HID { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string DName { get; set; }
        /// <summary>
        /// 位置信息
        /// </summary>
        public string Navigation { get; set; }
    }
}