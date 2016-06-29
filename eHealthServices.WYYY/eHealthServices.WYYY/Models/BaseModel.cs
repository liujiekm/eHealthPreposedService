using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHealthServices.WYYY.Models
{
    public class BaseModel
    {
        /// <summary>
        /// 0:没有错误；1：代表有错误
        /// </summary>
        public int HasError { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

    }
}