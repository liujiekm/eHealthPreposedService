using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHealthServices.WYYY.Models
{
    public class ResponseDep : BaseModel
    {
        /// <summary>
        /// 科室数据
        /// </summary>
        public List<Dep> Data { get; set; }

    }

    public class ResponseHospitalNavigation : BaseModel
    {
        /// <summary>
        /// 科室位置列表
        /// </summary>
        public List<HospitalDepNavigation> Data { get; set; }

    }
}