using eHealth.Date.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.OracleClient;

namespace eHealth.Date.DAL
{
    public partial class dl_oa_xw
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public IList<eHealth.Date.Entity.m_oa_xw> GetXWPage(int pageSize, int pageIndex, bool isDesc = true)
        {
            try
            {
                string sql = System.Configuration.ConfigurationSettings.AppSettings["NewsSql"].ToString();
                IList<eHealth.Date.Entity.m_oa_xw> result=new List<eHealth.Date.Entity.m_oa_xw>();
                using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString))
                //using (var conn = new System.Data.OracleClient.OracleConnection("Data Source=tongl;User Id=oayh1;Password=oayhaaabbb;"))
                {
                    conn.Open();
                    var command = conn.CreateCommand();
                    StringBuilder sb = new StringBuilder();
                    // string.Format("select ID,BT,ZZ,TJSJ from (select  t.ID,t.BT,t.ZZ,t.TJSJ,rownum recno  from (select * from  oa_xw where  yxbz ='Y'  and ( xwlbid like  '1蠀%' or xwlbid like'蠁1蠀' ) and TGZT in ('5','7','8') and NWXS like '%1' order by tjsj desc )  t) where recno between {0} and {1}",pageIndex*pageSize,(pageIndex+1)*pageSize-1);
                    //sb.Append("select ID,BT,ZZ,TJSJ from (select  t.ID,t.BT,t.ZZ,t.TJSJ,rownum recno  from (select * from  oa_xw where  yxbz ='Y'  and ( xwlbid like  '1蠀%' or xwlbid like'蠁1蠀' ) and TGZT in ('5','7','8') and NWXS like '%1' order by tjsj desc )  t) where recno between ");
                    sb.Append("select ID,BT,ZZ,TJSJ from (select  t.ID,t.BT,t.ZZ,t.TJSJ,rownum recno  from (" + sql + ") and TGZT in ('5','7','8') and NWXS like '%1' order by tjsj desc )  t) where recno between ");
                    sb.Append(pageIndex * pageSize);
                    sb.Append(" and ");
                    sb.Append((pageIndex + 1) * pageSize - 1);



                    command.CommandText = sb.ToString();
                    //command.CommandText = string.Format("select ID,BT,ZZ,TJSJ from oa_xw where rownum<5");
                    OracleDataReader odr = command.ExecuteReader();
                    while (odr.Read()) { 
                        var xw=new m_oa_xw();
                        xw.ID = odr.GetInt64(0);
                        xw.BT = odr.GetString(1);
                        xw.ZZ = odr.GetString(2);
                        xw.TJSJ = odr.GetDateTime(3);
                        result.Add(xw);
                    }
                    //var result = OracleHelper.GetDataItems<eHealth.Date.Entity.m_oa_xw>(command);
                    //return result;
                }
                //Lenovo.Tool.Log4NetHelper.Error(result.Count);
                return result;
            }
            catch (Exception ex)
            {
                Lenovo.Tool.Log4NetHelper.Error(ex);
                return new List<eHealth.Date.Entity.m_oa_xw>();
            }
        }



        /// <summary>
        /// 获取单个新闻
        /// </summary>
        /// <param name="newID"></param>
        /// <returns></returns>
        public NewModel GetXWByID(long newID)
        {
            using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                var nxw = this;
                var ngl = new dl_oa_glfjb();//this nr_oa_glfjb
                var row = nxw.GetOAXWByID(newID);
                if (row == null)
                    return null;
                NewModel result = new NewModel();
                result.WZNR = row.WZNR;
                result.ZZ = row.ZZ;
                result.TJSJ = row.TJSJ;
                result.BT = row.BT;
                result.IMG = new List<m_oa_glfjb>();
                IList<m_oa_glfjb> imgList = ngl.GetGLByGLID(newID);
                if (imgList != null && imgList.Count > 0)
                {
                    foreach (m_oa_glfjb imgRow in imgList)
                    {
                        string imgName;
                        if (!Common.ImgFile.IsHaveImg(imgRow.FJID, imgRow.WJGS, out imgName))
                        {
                            command.CommandText = string.Format("select FJNR from OA_GLFJB where FJID={0}", imgRow.FJID);
                            var temp = OracleHelper.GetDataItems<FJNR>(command).FirstOrDefault();
                            if (temp != null)
                            {
                                byte[] img = temp.fjnr;
                                MemoryStream ms = new MemoryStream(img, 0, img.Length);
                                var  image =  Image.FromStream(ms);
                                Lenovo.Tool.File.Thumbnail.MakeThumbnailImage(image, Path.Combine(Common.ImgFile.WebImgPath, imgName), 800, 0);
                            }
                        }
                        imgRow.Url = Common.ImgFile.HttpWebImgPath + imgName;
                        result.IMG.Add(imgRow);//  .ImgUrlsList.Add(imgModel);
                    }
                }
                //result.IMG = imgList;
                return result;
            }
        }
        /// <summary>
        /// 获取新闻第一个图片的URL
        /// </summary>
        /// <param name="newID"></param>
        /// <returns></returns>
        public string GetNewsOneImgURL(long newID)
        {
            using (var conn = new System.Data.OracleClient.OracleConnection(OracleHelper.ConnString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                var ngl = new dl_oa_glfjb();
                IList<m_oa_glfjb> imgList = ngl.GetGLByGLID(newID);
                if (imgList != null && imgList.Count > 0)
                {
                    m_oa_glfjb imgRow = imgList[0];
                    string imgName;
                    if (!Common.ImgFile.IsHaveImg(imgRow.FJID, imgRow.WJGS, out imgName))
                    {
                        command.CommandText = string.Format("select FJNR from OA_GLFJB where FJID={0}", imgRow.FJID);
                        var temp = OracleHelper.GetDataItems<FJNR>(command).FirstOrDefault();
                        if (temp != null)
                        {
                            byte[] img = temp.fjnr;
                            MemoryStream ms = new MemoryStream(img, 0, img.Length);
                            var image = Image.FromStream(ms);
                            Lenovo.Tool.File.Thumbnail.MakeThumbnailImage(image, Path.Combine(Common.ImgFile.WebImgPath, imgName), 800, 0);
                        }
                    }
                    return Common.ImgFile.HttpWebImgPath + imgName;
                }
                return "";
            }
        }

        private string toHTML(List<m_oa_glfjb> IMG, string WZNR)
        {
            string html = "";

            return html;
        }
    }
    public class FJNR {
        public byte[] fjnr { get; set; }
    }

    public class NewModel
    {
        public IList<m_oa_glfjb> IMG { get; set; }
        public string BT { get; set; }
        public DateTime TJSJ { get; set; }
        public string WZNR { get; set; }
        public string ZZ { get; set; }
    }
    
}
