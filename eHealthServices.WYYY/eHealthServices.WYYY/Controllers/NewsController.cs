using eHealth.Date.DAL;
using eHealthServices.WYYY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace eHealthServices.WYYY.Controllers
{
    public partial class HospitalController : ApiController
    {
        /// <summary>
        /// 模块：医院动态
        /// 【分页】获取新闻列表 如果没有参数默认返回第一页，前十条信息
        /// </summary>
        /// <param name="Page">请求新闻列表参数Page=第几页 Rows=一页多少行，从0页开始</param>
        /// <param name="Rows">请求新闻列表参数Rows=一页多少行</param>
        /// <returns>新闻列表</returns>
        [HttpPost]
        public NewList GetNews(NewsParam param)
        {
            try
            {
                // param = ConverToT<NewsParam>();
                param = param == null ? new NewsParam() { Page = 0, Rows = 10 } : param;
                if (param.Rows <= 0)
                    param.Rows = 10;
                var result = new NewList();
                var dl=new dl_oa_xw();
                //从数据库获取数据
                var list = dl.GetXWPage(param.Rows, param.Page);
                //将数据装入response
                var temp = new List<SimpleNew>();
                foreach (var item in list)
                {
                    temp.Add(new SimpleNew() { ID = item.ID.ToString(), Auther = item.ZZ, CreateTime = item.TJSJ, Title = item.BT, ImgURL=dl.GetNewsOneImgURL(item.ID) });

                } 
                result.Data = temp;
                //result.Data = new List<SimpleNew>() { new SimpleNew() { ID = param.Rows.ToString() +"|"+ param.Page.ToString(), Auther = list.Count.ToString(), CreateTime = DateTime.Now, Title = "title" } };
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new NewList() { HasError = 1, ErrorMessage = ex.Message };
            }
        }
        /// <summary>
        /// 模块：医院动态
        /// 获取新闻详细信息
        /// </summary>
        /// <param name="id">ID为新闻的id</param>
        /// <returns>新闻详细信息</returns>
        [HttpPost]
        public ResponseFullNew GetNew(NewParam id)
        {
            // id = ConverToT<NewParam>();
            if (id == null)
                return new ResponseFullNew() { HasError = 1, ErrorMessage = "参数不匹配！" };
            if (string.IsNullOrWhiteSpace(id.ID))
                return new ResponseFullNew() { HasError = 1, ErrorMessage = "id为空！" };
            try
            {
                var news = new dl_oa_xw().GetXWByID(Convert.ToInt64(id.ID));
                if (news == null)
                    return new ResponseFullNew() {  HasError=1, ErrorMessage="无法查询该条新闻！"};
                return new ResponseFullNew()
                {
                    Data = new FullNew()
                    {
                        Auther = news.ZZ,
                        CreateTime = news.TJSJ,
                        Title = news.BT,
                        Content = GetWZNRHTML(news)
                    }
                };
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new ResponseFullNew() { HasError = 1, ErrorMessage = ex.Message };
            }
        }
        /// <summary>
        /// 获取新闻网页
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        private string GetWZNRHTML(NewModel news)
        {
            string content = "<html xmlns='http://www.w3.org/1999/xhtml'><head>"
                +"<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>"
            +"<title>"+news.BT+"</title>"
            + "<style>p {text-indent: 2em;text-align: left;}img{display: block; width: 94%; margin: 10px 0px 10px 3%;}</style></head>"
            +"<body>"
            +"<div style='text-align:center;background-color:white'>";
            foreach (var item in news.IMG)
            {
                content += "<img src='"+item.Url+"'/><br />";
            }
            content += news.WZNR + "</div></body></html>";
            return content;
        }
    }
}
