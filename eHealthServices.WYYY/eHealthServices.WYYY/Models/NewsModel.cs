using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHealthServices.WYYY.Models
{

    #region request参数
    /// <summary>
    /// 获取新闻列表参数
    /// </summary>
    public class NewsParam
    {
        /// <summary>
        /// 第几页 从0页开始
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 一页多少行
        /// </summary>
        public int Rows { get; set; }
    }

    public class NewParam
    {
        public string ID { get; set; }
    }
    #endregion

    #region response 参数
    /// <summary>
    /// 返回简单的新闻结构
    /// </summary>
    public class SimpleNew
    {
        /// <summary>
        /// 新闻id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Auther { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 预览图地址
        /// </summary>
        public string ImgURL { get; set; }
    }
    /// <summary>
    /// 完整新闻结构
    /// </summary>
    public class FullNew : SimpleNew
    {
        /// <summary>
        /// 原文链接
        /// </summary>
        public string ResourceLink { get;set;}
        /// <summary>
        /// 新闻内容（网页格式：图片文字拼接）
        /// </summary>
        public string Content { get; set; }


    }

    public class ResponseFullNew : Models.BaseModel
    {
        /// <summary>
        /// 新闻详细信息
        /// </summary>
        public FullNew Data { get; set; }
    }
    public class NewList : Models.BaseModel
    {
        /// <summary>
        /// 新闻列表
        /// </summary>
        public List<SimpleNew> Data { get; set; }

    }
    #endregion

}