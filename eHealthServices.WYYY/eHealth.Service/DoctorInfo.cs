using eHealth.Date.DAL;
using eHealth.Date.DAL.Common;
using eHealth.Date.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eHealth.Service
{
    public class MyComparintByR_RYK : IEqualityComparer<m_r_ryk>
    {
        public bool Equals(m_r_ryk x, m_r_ryk y)
        {
            if (x == null && y == null)
                return false;
            return x.XM == y.XM;
        }
        public int GetHashCode(m_r_ryk obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
    public class DoctorInfo
    {

        private const string _headUrl = "http://www.wzhospital.cn/wyyyweb/yszp/";
        /// <summary>
        /// 获取医生信息列表
        /// </summary>
        /// <param name="bmid"></param>
        /// <returns></returns>
        private IList<m_r_ryk> GetDoctorByBMID(IList<m_yyfz_zkfj_wh> bmid)
        {
            dl_r_ryk dlryk = new dl_r_ryk();
            List<m_r_ryk> result = new List<m_r_ryk>();

            var dl_pic = new dl_i_ysxx();//this nr_i_ysxx
            string[] picTypes = null;
            string trueName = string.Empty;
            foreach (m_yyfz_zkfj_wh m in bmid)
            {
                if (!m.BMID.HasValue) continue;
                IList<m_r_ryk> list = dlryk.GetDoctorByBMID(m.BMID.Value);
                if (list != null && list.Count > 0)
                {
                    foreach (m_r_ryk imgRow in list)
                    {
                        //根据医生工种代码获取医生职位
                        if (Convert.ToInt32(imgRow.GZDM) > Convert.ToInt32(imgRow.GZDM2) && imgRow.GZDM2 != "0000")
                        {
                            imgRow.GZDM = imgRow.GZDM2;
                        }
                        if (string.IsNullOrWhiteSpace(imgRow.ZW))
                        {
                            switch (imgRow.GZDM)
                            {
                                case "0014": imgRow.ZW = "住院医师";
                                    break;
                                case "0013": imgRow.ZW = "主治医师";
                                    break;
                                case "0012": imgRow.ZW = "副主任医师";
                                    break;
                                case "0011": imgRow.ZW = "主任医师";
                                    break;
                                case "0010": imgRow.ZW = "市级名中医";
                                    break;
                                default: imgRow.ZW = "";
                                    break;
                            }
                        }
                        //获取医生专科
                        var zk = dlryk.GetZKByRykid(imgRow.ID);
                        if (zk!=null)
                        imgRow.ZKID = zk.ZKID;
                        //获取医生头像
                        var picName = dl_pic.GetDoctorPicByName(imgRow.XM.Trim());
                        if (picName != null && !string.IsNullOrWhiteSpace(picName.YSZP) && picName.YSZP.Contains("."))//有图片
                        {
                            picTypes = picName.YSZP.Split('.');
                            if (!ImgFile.IsHaveImgDoctor(picName.ID + picTypes[0], picTypes[1], out trueName))
                            {
                                WebResponse response = null;
                                Stream stream = null;
                                try
                                {
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_headUrl + string.Join(".", picTypes));
                                    response = request.GetResponse();
                                    stream = response.GetResponseStream();
                                    if (!response.ContentType.ToLower().StartsWith("text/") && stream != null)
                                    {
                                        Image image = Image.FromStream(stream);
                                        Lenovo.Tool.File.Thumbnail.MakeThumbnailImage(image, Path.Combine(ImgFile.WebImgDoctorPath, trueName), 55, 70);
                                    }
                                }
                                catch (Exception err)
                                {
                                    Lenovo.Tool.Log4NetHelper.Error(err);
                                }
                            }
                            imgRow.URL = ImgFile.HttpWebImgDoctorPath + trueName;//使用医生的照片存放地址
                        }
                        else
                        {
                            string imgName;
                            if (!ImgFile.IsHaveImgDoctor(imgRow.ID.ToString(), "jpg", out imgName))
                            {
                                m_r_ryk rypic = new dl_r_ryk().GetPICByID2(imgRow.ID);
                                if (rypic != null && rypic.RYPIC != null && rypic.RYPIC.Length > 0)
                                {
                                    byte[] img = (byte[])rypic.RYPIC;
                                    MemoryStream ms = new MemoryStream(img, 0, img.Length);
                                    var image = Image.FromStream(ms);
                                    Lenovo.Tool.File.Thumbnail.MakeThumbnailImage(image, Path.Combine(ImgFile.WebImgDoctorPath, imgName), 100, 0);
                                }
                            }
                            imgRow.URL = ImgFile.HttpWebImgDoctorPath + imgName;//使用医生的照片存放地址
                        }
                        //imgRow.URL = Common.ImgFile.HttpWebImgDoctorPath + imgName;//使用医生的照片存放地址
                    }
                    result.AddRange(list);
                }
            }

            return result.OrderBy(p => p.GZDM).Distinct(new MyComparintByR_RYK()).ToList();

        }

        /// <summary>
        /// 获取医生照片，60分钟缓存
        /// </summary>
        /// <param name="xm"></param>
        /// <param name="rykid"></param>
        /// <returns></returns>
        private string GetDoctorPic(string xm, long rykid)
        {
            var url = Lenovo.Tool.CacheHelper.Get<string>(string.Format("rykGetDoctorPic{0}{1}", xm, rykid));
            if (!string.IsNullOrWhiteSpace(url))
                return url;
            var dl_pic = new dl_i_ysxx();//this nr_i_ysxx
            string[] picTypes = null;
            string trueName = string.Empty;
            var picName = dl_pic.GetDoctorPicByName(xm);
            if (picName != null && !string.IsNullOrWhiteSpace(picName.YSZP) && picName.YSZP.Contains("."))//有图片
            {
                picTypes = picName.YSZP.Split('.');
                if (!ImgFile.IsHaveImgDoctor(picName.ID + picTypes[0], picTypes[1], out trueName))
                {
                    WebResponse response = null;
                    Stream stream = null;
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_headUrl + string.Join(".", picTypes));
                        response = request.GetResponse();
                        stream = response.GetResponseStream();
                        if (!response.ContentType.ToLower().StartsWith("text/") && stream != null)
                        {
                            Image image = Image.FromStream(stream);
                            Lenovo.Tool.File.Thumbnail.MakeThumbnailImage(image, Path.Combine(ImgFile.WebImgDoctorPath, trueName), 55, 70);
                        }
                    }
                    catch (Exception err)
                    {
                        Lenovo.Tool.Log4NetHelper.Error(err);
                    }
                }
                url = ImgFile.HttpWebImgDoctorPath + trueName;//使用医生的照片存放地址
            }
            else
            {
                string imgName;
                if (!ImgFile.IsHaveImgDoctor(rykid.ToString(), "jpg", out imgName))
                {
                    m_r_ryk rypic = new dl_r_ryk().GetPICByID2(rykid);
                    if (rypic != null && rypic.RYPIC != null && rypic.RYPIC.Length > 0)
                    {
                        byte[] img = (byte[])rypic.RYPIC;
                        MemoryStream ms = new MemoryStream(img, 0, img.Length);
                        var image = Image.FromStream(ms);
                        Lenovo.Tool.File.Thumbnail.MakeThumbnailImage(image, Path.Combine(ImgFile.WebImgDoctorPath, imgName), 100, 0);
                        url = ImgFile.HttpWebImgDoctorPath + imgName;//使用医生的照片存放地址
                    }
                    else
                    {
                        url = "";
                    }
                }
                else
                {
                    url = ImgFile.HttpWebImgDoctorPath + imgName;//使用医生的照片存放地址
                }
            }
            // return url;
            if (url == null)
                url = string.Empty;
            Lenovo.Tool.CacheHelper.Insert(string.Format("rykGetDoctorPic{0}{1}", xm, rykid), url, 24 * 60);
            return url;
        }

        /// <summary>
        /// 根据rykid获取医生信息
        /// </summary>
        /// <param name="rykid"></param>
        /// <returns></returns>
        public m_r_ryk getDoctorByRykid(long rykid)
        {
            dl_r_ryk dlryk = new dl_r_ryk();
            //获取医生信息
            m_r_ryk imgRow = dlryk.GetDoctorByRykid(rykid);
            if (imgRow == null) return null;
            //根据医生工种代码获取医生职位
            if (Convert.ToInt32(imgRow.GZDM) > Convert.ToInt32(imgRow.GZDM2) && imgRow.GZDM2 != "0000")
            {
                imgRow.GZDM = imgRow.GZDM2;
            }
            if (string.IsNullOrWhiteSpace(imgRow.ZW))
            {
                switch (imgRow.GZDM)
                {
                    case "0014": imgRow.ZW = "住院医师";
                        break;
                    case "0013": imgRow.ZW = "主治医师";
                        break;
                    case "0012": imgRow.ZW = "副主任医师";
                        break;
                    case "0011": imgRow.ZW = "主任医师";
                        break;
                    case "0010": imgRow.ZW = "市级名中医";
                        break;
                    default: imgRow.ZW = "";
                        break;
                }
            }
            //获取医生专科
            var zk = dlryk.GetZKByRykid(imgRow.ID);
            imgRow.ZKID = zk.ZKID;
            //获取医生头像
            imgRow.URL= GetDoctorPic(imgRow.XM, imgRow.ID);
            return imgRow;
        }

        /// <summary>
        /// 根据搜索内容获取医生信息
        /// </summary>
        /// <param name="hz"></param>
        /// <returns></returns>
        public IList<m_r_ryk> GetDoctorByHZ(string hz)
        {
            dl_r_ryk dlryk = new dl_r_ryk();
            //获取医生信息
            IList<m_r_ryk> list = new dl_r_ryk().GetDoctorByHZ("%" + hz + "%", "%" + hz + "%");
            if (list != null && list.Count > 0)
            {
                foreach (m_r_ryk imgRow in list)
                {
                    //根据医生工种代码获取医生职位
                    if (Convert.ToInt32(imgRow.GZDM) > Convert.ToInt32(imgRow.GZDM2) && imgRow.GZDM2 != "0000")
                    {
                        imgRow.GZDM = imgRow.GZDM2;
                    }
                    if (string.IsNullOrWhiteSpace(imgRow.ZW))
                    {
                        switch (imgRow.GZDM)
                        {
                            case "0014": imgRow.ZW = "住院医师";
                                break;
                            case "0013": imgRow.ZW = "主治医师";
                                break;
                            case "0012": imgRow.ZW = "副主任医师";
                                break;
                            case "0011": imgRow.ZW = "主任医师";
                                break;
                            case "0010": imgRow.ZW = "市级名中医";
                                break;
                            default: imgRow.ZW = "";
                                break;
                        }
                    }
                    //获取医生专科
                    var zk = dlryk.GetZKByRykid(imgRow.ID);
                    if(zk!=null)
                        imgRow.ZKID = zk.ZKID;
                    //获取医生头像
                    imgRow.URL = GetDoctorPic(imgRow.XM, imgRow.ID);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取医生列表，根据部门代码
        /// </summary>
        /// <param name="dm"></param>
        /// <returns></returns>
        public IList<m_r_ryk> GetDoctorByDM(string dm)
        {
            var list = Lenovo.Tool.CacheHelper.Get<IList<m_r_ryk>>("dl_r_ryk_GetDoctorByBMID_" + dm);
            if (list == null)
            {
                list = GetDoctorByBMID(new dl_yyfz_zkfj_wh().GetBMIDListByDM(dm));
                Lenovo.Tool.CacheHelper.Insert("dl_r_ryk_GetDoctorByBMID_" + dm, list, 120);
            }
            return list;// GetDoctorByBMID(new dl_mapp_yyfz_zkfj_wh().GetBMIDListByDM(dm));
        }
    }
}
