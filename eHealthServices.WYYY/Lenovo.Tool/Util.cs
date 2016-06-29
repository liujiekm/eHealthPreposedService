using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Security;

namespace Lenovo.Tool
{
    public class Util
    {

      

        //public Util()
        //{
        //    //
        //    // TODO: 在此处添加构造函数逻辑
        //    //
        //} 

        /// <summary>
        /// 返回字符串的长度（包括汉字），汉字长度为2
        /// </summary>
        /// <param name="yourstring">源字符串</param>
        /// <returns>字符串</returns>
        public static int f_length(string yourstring)
        {
            if (string.IsNullOrEmpty(yourstring)) return 0;
            int len = yourstring.Length;
            byte[] sarr = System.Text.Encoding.Default.GetBytes(yourstring);
            return sarr.Length;
        }

        /// <summary>
        /// 从左边开始取len长个几个字符串
        /// </summary>
        /// <param name="yourstring">源字符串</param>
        /// <param name="len">指定长度</param>
        /// <returns>字符串</returns>
        public static string f_left(string yourstring, int ai_len)
        {
            if (ai_len <= 0) return string.Empty;
            if (string.IsNullOrEmpty(yourstring)) return string.Empty;
            int length = f_length(yourstring);
            if (length <= ai_len)
                return yourstring;
            else
            {
                int tmp = 0;
                int len = 0;
                int okLen = 0;
                int li_asc;
                string ls_return = string.Empty;
                for (int i = 0; i < length; i++)
                {
                    //获取asc码
                    li_asc = Asc(yourstring.Substring(i, 1));
                    if (li_asc > 127)
                        tmp += 2;
                    else
                        len += 1;
                    okLen += 1;
                    if (tmp + len == ai_len)
                    {
                        ls_return = yourstring.Substring(0, okLen);
                        break;
                    }
                    else if (tmp + len > ai_len)
                    {
                        ls_return = yourstring.Substring(0, okLen - 1);
                        break;
                    }
                }
                return ls_return;
            }
        }
        /// <summary>
        /// 判断字符串是否未数字
        /// </summary>
        /// <param name="as_source">源字符串</param>
        /// <returns>true 是数字型 ，false 非数字型</returns>
        public static bool IsNumber(string as_source)
        {
            foreach (char lc_char in as_source.ToCharArray())
            {
                if (!char.IsNumber(lc_char)) return false;
            }
            return true;
        }
        /// <summary>
        /// 从指定文字开始取len长的字符串，从1开始计数
        /// </summary>
        /// <param name="yourstring">源字符串</param>
        /// <param name="start">开始的位置</param>
        /// <param name="len">指定长度</param>
        /// <returns>字符串</returns>
        public static string f_mid(string yourstring, int start, int len)
        {
            if (len <= 0 || start < 0) return string.Empty;
            if (string.IsNullOrEmpty(yourstring)) return string.Empty;
            if (yourstring.Length < start) return string.Empty;
            if (yourstring.Length <= start + len - 1)
                return yourstring.Substring(start - 1);
            else
                return yourstring.Substring(start - 1, len);
        }

        /// <summary>
        /// 从指定文字开始取字符串，从1开始计数
        /// </summary>
        /// <param name="yourstring">源字符串</param>
        /// <param name="start">开始的位置</param>
        /// <param name="len">指定长度</param>
        /// <returns>字符串</returns>
        public static string f_mid(string yourstring, int start)
        {
            int len = yourstring.Length;
            return f_mid(yourstring, start, len);
        }

        /// <summary>
        /// 从右边开始取指定len长的字符串
        /// </summary>
        /// <param name="yourstring">源字符串</param>
        /// <param name="len">指定长度</param>
        /// <returns>字符串</returns>
        public static string f_right(string yourstring, int len)
        {
            if (len <= 0) return string.Empty;
            if (string.IsNullOrEmpty(yourstring)) return string.Empty;
            if (yourstring.Length <= len)
                return yourstring;
            else
                return yourstring.Substring(yourstring.Length - len, len);
        }
        /// <summary>
        /// 截取指定长度的中英文混合字符串
        /// </summary>
        /// <param name="as_source">元字符串</param>
        /// <param name="l"></param>
        /// <param name="endStr"></param>
        /// <returns></returns>
        public static string f_getsubstring(string as_source, int ai_length, string endStr)
        {
            string temp = as_source.Substring(0, (as_source.Length < ai_length + 1) ? as_source.Length : ai_length + 1);
            byte[] encodedBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(temp);

            string outputStr = "";
            int count = 0;

            for (int i = 0; i < temp.Length; i++)
            {
                if ((int)encodedBytes[i] == 63)
                    count += 2;
                else
                    count += 1;

                if (count <= ai_length - endStr.Length)
                    outputStr += temp.Substring(i, 1);
                else if (count > ai_length)
                    break;
            }

            if (count <= ai_length)
            {
                outputStr = temp;
                endStr = "";
            }

            outputStr += endStr;

            return outputStr;
        }
        public static char[] chinese = new char[] { '〇', '一', '二', '三', '四', '五', '六', '七', '八', '九', '十' };
        // private string ConvertDateToChinese()
        //{
        //    chinese = new char[] {'〇','一','二','三','四','五','六','七','八','九','十'};
        //}

        /// <summary>
        /// 将数字表示的年月日转化为中文数字的年月日
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertMethod(string date)
        {
            // Define stringBuilder variable  for write out the chinese of date
            StringBuilder strb = new StringBuilder();

            //define regex regularExpressins variable
            Regex regex = new Regex(@"(\d{2}|\d{4})(-|/)(\d{1}|\d{2})(-|/)(\d{1}|\d{2})");
            if (regex.IsMatch(date))
            {
                string[] str = null;
                if (date.Contains("-"))
                {
                    str = date.Split('-');
                }
                else
                {
                    if (date.Contains("/"))
                    {
                        str = date.Split('/');

                    }
                }
                //Convert year as follow
                for (int i = 0; i < str[0].Length; i++)
                {
                    strb = strb.Append(chinese[int.Parse(str[0][i].ToString())]);
                }
                strb.Append("年");

                #region  //convert month
                int monthod = int.Parse(str[1]);
                int MN1 = monthod / 10;
                int MN2 = monthod % 10;


                if (MN1 > 0)
                {
                    strb.Append(chinese[10]);
                }
                if (MN2 != 0)
                {
                    strb.Append(chinese[MN2]);
                }

                strb.Append("月");

                #endregion
                #region //convert day
                int day = int.Parse(str[2]);
                int day1 = day / 10;
                int day2 = day % 10;

                if (day1 > 1)
                {
                    strb.Append(chinese[day1]);


                }
                if (day1 > 0)
                {
                    strb.Append(chinese[10]);
                }
                if (day2 > 0)
                {
                    strb.Append(chinese[day2]);
                }

                strb.Append("日");

                #endregion


            }
            return strb.ToString();
        }

        /// <summary>
        /// 按固定数\随机数加密或解密
        /// </summary>
        /// <param name="as_pass"></param>
        /// <param name="ai_mod">ai_mod = 0 固定数加密,ai_mod = 1 随机数加密,ai_mod = 2 解密</param>
        /// <returns>加／解密后的字符串</returns>
        public static string f_passwd2(string as_pass, int ai_mod)
        {
            if (as_pass.Length > 23)
                return "-";
            int li_i, li_len, li_pos;
            string ls_cpu = "", ls_tmp = "", ls_ret = "";
            string[,] ls_pass = new string[14, 2];
            if (ai_mod == 0 || ai_mod == 1)
            {
                if (string.IsNullOrEmpty(as_pass)) return "";
                as_pass = f_left(as_pass, 10);
                if (ai_mod == 1)
                {
                    ls_cpu = DateTime.Now.ToString("mmss");
                }
                else
                {
                    ls_cpu = f_left(System.Convert.ToString(System.Convert.ToDouble(f_length(as_pass)) / 0.000412), 4);     //不能改变
                }
                li_len = f_length(as_pass);
                ls_ret = as_pass + ls_cpu;
                for (li_i = 1; li_i <= 4; li_i++)
                {
                    li_pos = System.Convert.ToInt32(f_mid(ls_cpu, li_i, 1));
                    li_pos = li_pos % (li_len + 4);
                    if (li_pos == 0) li_pos = 1;
                    ls_tmp = ls_tmp + f_mid(ls_ret, li_pos, 1);
                    ls_pass[li_pos - 1, 1] = "1";
                    li_pos = (9 - li_pos) % (li_len + 4);
                    if (li_pos == 0) li_pos = 1;
                    ls_pass[li_pos - 1, 1] = "1";
                    ls_tmp = ls_tmp + f_mid(ls_ret, li_pos, 1);
                }
                for (li_i = 1; li_i <= li_len + 4; li_i++)
                {
                    if (ls_pass[li_i - 1, 1] == "1") continue;
                    ls_tmp = ls_tmp + f_mid(ls_ret, li_i, 1);
                }
                ls_ret = System.Convert.ToString(li_len - 1) + ls_cpu + ls_tmp;
                return ls_ret;
            }
            else if (ai_mod == 2)
            {
                if (string.IsNullOrEmpty(as_pass)) return "";
                li_len = System.Convert.ToInt32(f_mid(as_pass, 1, 1)) + 1;
                ls_cpu = f_mid(as_pass, 2, 4);
                ls_ret = f_mid(as_pass, 6, f_length(as_pass));
                for (li_i = 1; li_i <= 4; li_i++)
                {
                    li_pos = System.Convert.ToInt32(f_mid(ls_cpu, li_i, 1));
                    li_pos = li_pos % (li_len + 4);
                    if (li_pos == 0) li_pos = 1;
                    ls_pass[li_pos - 1, 1] = "1";
                    ls_pass[li_pos - 1, 0] = f_mid(ls_ret, 2 * li_i - 1, 1);
                    li_pos = (9 - li_pos) % (li_len + 4);
                    li_pos = li_pos % (li_len + 4);
                    if (li_pos == 0) li_pos = 1;
                    ls_pass[li_pos - 1, 1] = "1";
                    ls_pass[li_pos - 1, 0] = f_mid(ls_ret, 2 * li_i, 1);
                }
                li_pos = 9;
                for (li_i = 1; li_i <= li_len; li_i++)
                {
                    if (ls_pass[li_i - 1, 1] == "1")
                    {
                        //null 
                    }
                    else
                    {
                        ls_pass[li_i - 1, 0] = f_mid(ls_ret, li_pos, 1);
                        li_pos++;
                    }
                    ls_tmp = ls_tmp + ls_pass[li_i - 1, 0];
                }
                return ls_tmp;
            }
            else
            {
                return as_pass;
            }
        }
        /// <summary>
        /// 加密或解密
        /// </summary>
        /// <param name="as_pass">加／解密前的字符串</param>
        /// <param name="ai_mod">ai_mod = １加密,其他解密</param>
        /// <returns>加／解密后的字符串</returns>
        public static string f_passwd(string as_pass, int ai_mode)
        {
            if (string.IsNullOrEmpty(as_pass)) return "";
            if (ai_mode == 1)
            {
                char[] sarr = as_pass.ToCharArray();
                string strAll = "";
                for (int i = 0; i < sarr.Length; i++)
                {
                    if (i == 0)
                        strAll = String.Concat(strAll, Asc(sarr[0].ToString()));
                    else if (i == 2)
                        strAll = String.Concat(strAll, Asc(sarr[2].ToString()));
                    else
                        strAll = String.Concat(strAll, sarr[i].ToString());
                }
                return strAll;
            }
            else
            {
                switch (as_pass.Length)
                {
                    case 1: return "";
                    case 2: return Char(int.Parse(as_pass.Substring(0, 2)));
                    case 3: return Char(int.Parse(as_pass.Substring(0, 2))) + as_pass.Substring(2, 1);
                    case 4: return "";
                    case 5: return Char(int.Parse(as_pass.Substring(0, 2))) + as_pass.Substring(2, 1) + Char(int.Parse(as_pass.Substring(3, 2)));
                    case 6: return "";
                    default: return Char(int.Parse(as_pass.Substring(0, 2))) + as_pass.Substring(2, 1) + Char(int.Parse(as_pass.Substring(3, 2))) + as_pass.Substring(5);
                }
            }
        }
        /// <summary>
        /// 将ＡＳＣ码转换为字符
        /// </summary>
        /// <param name="asciiCode">ＡＳＣ码</param>
        /// <returns>对应的字符</returns>
        public static string Char(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                throw new Exception("ASCII编码无效");
            }
        }
        /// <summary>
        /// 返回制定字符串的MD5码
        /// </summary>
        /// <param name="as_str">源字符串</param>
        /// <returns>对于的MD5码</returns>
        public static string f_Md5string(string as_str)
        {
            if (string.IsNullOrEmpty(as_str)) return as_str;
            return FormsAuthentication.HashPasswordForStoringInConfigFile(as_str, "MD5");
        }
        /// <summary>
        /// 将字符转换为ＡＳＣ码
        /// </summary>
        /// <param name="asciiCode">字符</param>
        /// <returns>对应的ＡＳＣ码</returns>
        public static int Asc(string character)
        {
            if (character.Length == 1)
            {
                byte[] bytes = System.Text.Encoding.Default.GetBytes(character);
                int intAsciiCode = (int)bytes[0];
                return (intAsciiCode);
            }
            else
            {
                throw new Exception("字符的长度不对");
            }

        }

        /// <summary>
        /// 返回于指定日期相隔second秒的日期
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="second1">秒数</param>
        /// <returns>日期型</returns>
        public static DateTime f_RelativeTime(DateTime dt, Int32 second1)
        {
            int days, hours, minutes, seconds, second;
            second = System.Math.Abs(second1);
            days = second / (24 * 3600);
            hours = (second - days * 24 * 3600) / 3600;
            minutes = (second - days * 24 * 3600 - hours * 3600) / 60;
            //2014-9-28 赵中美 方法逻辑错误
            //seconds = (second - days * 24 * 3600 - hours * 3600 - minutes * 60) / 60;
            seconds = (second - days * 24 * 3600 - hours * 3600 - minutes * 60);

            if (second1 < 0)
            {
                return dt.Subtract(new TimeSpan(days, hours, minutes, seconds));
            }
            else
            {
                return dt.AddSeconds(second1);
            }
        }

        /// <summary>
        /// 计算两个日期的差，单位：秒
        /// </summary>
        /// <param name="dt1">日期1</param>
        /// <param name="dt2">日期2</param>
        /// <returns>整型</returns>
        public static Int64 f_SecondsAfter(DateTime dt1, DateTime dt2)
        {
            TimeSpan ts = dt2 - dt1;
            return (Int64)ts.TotalSeconds;
        }

        /// <summary>
        /// 计算年龄
        /// </summary>
        /// <param name="dt">指定日期</param>
        /// <param name="show">true的话，后面带有单位</param>
        /// <returns></returns>
        public static string f_GetNL(DateTime dt, bool showword)
        {
            TimeSpan ts = new TimeSpan();
            ts = DateTime.Today - dt;
            if (ts.Days < 100)
                return ts.Days.ToString() + (showword ? "天" : "");
            else if (ts.Days < 365)
                return Convert.ToString(Convert.ToInt32(ts.Days / 30.5)) + (showword ? "月" : "");
            else
                return Convert.ToString(Convert.ToInt32(ts.Days / 365.25)) + (showword ? "岁" : "");
            //Int64 timespan = f_SecondsAfter(dt, DateTime.Today);
            //if (timespan < 24 * 3600 * 30.56)
            //{
            //    return Convert.ToString(Convert.ToInt32(timespan / (24 * 3600))) + (showword?"天":"");
            //}
            //else if (timespan < 24 * 3600 * 30.56 * 12)
            //{
            //    return Convert.ToString(Convert.ToInt32(timespan / (24 * 3600 * 30.56))) + (showword?"月":"");
            //}
            //else
            //{
            //    return Convert.ToString(Convert.ToInt32(timespan / (24 * 3600 * 30.56 * 12))) + (showword?"岁":"");
            //}
        }
        /// <summary>
        /// 获取两个日期差，单位：满一天则为天，否则为小时
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="showword"></param>
        /// <returns></returns>
        public static string f_TimeSpan(DateTime dt1, DateTime dt2, bool showword)
        {
            TimeSpan ts = new TimeSpan();
            ts = dt2 - dt1;
            if (ts.Days < 1)
            {
                return (ts.Hours).ToString() + (showword ? "小时" : "");
            }
            else
                return (ts.Days).ToString() + (showword ? "天" : "");
        }
        /// <summary>
        /// 计算两个日期间年龄
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="showword"></param>
        /// <returns></returns>
        public static string f_GetNL(DateTime dt1, DateTime dt2, bool showword)
        {
            TimeSpan ts = new TimeSpan();
            ts = dt2 - dt1;
            if (ts.Days < 100)
                return ts.Days.ToString() + (showword ? "天" : "");
            else if (ts.Days < 365)
                return Convert.ToString(Convert.ToInt32(ts.Days / 30.5)) + (showword ? "月" : "");
            else
                return Convert.ToString(Convert.ToInt32(ts.Days / 365.25)) + (showword ? "岁" : "");
            //Int64 timespan = f_SecondsAfter(dt1, dt2);
            //if (timespan < 24 * 3600 * 30.56)
            //{
            //    return Convert.ToString(Convert.ToInt32(timespan / (24 * 3600))) + (showword ? "天" : "");
            //}
            //else if (timespan < 24 * 3600 * 30.56 * 12)
            //{
            //    return Convert.ToString(Convert.ToInt32(timespan / (24 * 3600 * 30.56))) + (showword ? "月" : "");
            //}
            //else
            //{
            //    return Convert.ToString(Convert.ToInt32(timespan / (24 * 3600 * 30.56 * 12))) + (showword ? "岁" : "");
            //}
        }
        /// <summary>
        /// 计算年限
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="showword"></param>
        /// <returns></returns>
        public static string f_GetNX(DateTime dt1, DateTime dt2, bool showword)
        {
            TimeSpan ts = new TimeSpan();
            ts = dt2 - dt1;
            if (ts.Days < 365)
                return "未满1年";
            else
                return Convert.ToString(Convert.ToInt32(ts.Days / 365.25)) + (showword ? "年" : "");

        }
        /// <summary>
        /// 判断字符串是否为Date格式（没有时分秒的DateTime）
        /// </summary>
        /// <param name="as_date"></param>
        /// <returns></returns>
        public static bool isDate(string as_date)
        {
            try
            {
                DateTime.Parse(as_date + " 00:00:00");
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 判断字符串是否为DateTime格式
        /// </summary>
        /// <param name="as_datetime"></param>
        /// <returns></returns>
        public static bool isDateTime(string as_datetime)
        {
            try
            {
                DateTime.Parse(as_datetime);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 把字定字符串替换掉，代替f_replace
        /// </summary>
        /// <param name="yourstring">源字符串</param>
        /// <param name="sour">要替换的源字符串</param>
        /// <param name="desc">要替换的目的字符串</param>
        /// <returns>字符串</returns>
        public static string f_Replace(string yourstring, string sour, string desc)
        {
            if (string.IsNullOrEmpty(yourstring)) return string.Empty;
            return yourstring.Replace(sour, desc);
        }

        /// <summary>
        /// 取指定字符串在母串的位置,从1开始计数
        /// </summary>
        /// <param name="yourstring">源字符串</param>
        /// <param name="str">要查找的字符串</param>
        /// <returns>整型</returns>
        public static int f_Pos(string yourstring, string str)
        {
            if (string.IsNullOrEmpty(yourstring)) return 0;
            return yourstring.IndexOf(str) == -1 ? 0 : yourstring.IndexOf(str) + 1;
        }


        /// <summary>
        /// 返回指定时间，指定格式的字符串形式
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="df">格式</param>
        /// <returns>字符串</returns>
        public static string DateTimeString(DateTime dt, dateFormat df)
        {
            string ls_datetime;
            switch (df)
            {
                case dateFormat.LongTime:
                    ls_datetime = dt.ToLongTimeString();
                    break;
                case dateFormat.LongDate:
                    ls_datetime = dt.ToLongDateString();
                    break;
                case dateFormat.ShortDate:
                    ls_datetime = dt.ToShortDateString();
                    break;
                case dateFormat.ShortTime:
                    ls_datetime = dt.ToShortTimeString();
                    break;
                case dateFormat.DateTime:
                    ls_datetime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    //ls_datetime = DateTimeString(dt, dateFormat.ShortDate) + " " + DateTimeString(dt, dateFormat.LongTime);
                    break;
                case dateFormat.DateTimeChinese:
                    ls_datetime = "";
                    break;
                default:
                    ls_datetime = "";
                    break;
            }
            return ls_datetime;
        }

        /// <summary>
        /// 返回指定日期的具体某个部分
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="df">要返回的指定内容</param>
        /// <returns>字符串</returns>
        public static int DateTimeInt(DateTime dt, dateFormat df)
        {
            int li_datetime;
            switch (df)
            {
                case dateFormat.Second:
                    li_datetime = dt.Second;
                    break;
                case dateFormat.Minute:
                    li_datetime = dt.Minute;
                    break;
                case dateFormat.Hour:
                    li_datetime = dt.Hour;
                    break;
                case dateFormat.Day:
                    li_datetime = dt.Day;
                    break;
                case dateFormat.WeekDay:
                    li_datetime = Convert.ToInt32(dt.DayOfWeek);
                    break;
                case dateFormat.Month:
                    li_datetime = dt.Month;
                    break;
                case dateFormat.Year:
                    li_datetime = dt.Year;
                    break;
                default:
                    li_datetime = -1;
                    break;
            }
            return li_datetime;
        }

        /// <summary>
        /// 多参数生成字符串
        /// </summary>
        /// <param name="as_parm">添加本参数之前的由多个参数组成的字符串</param>
        /// <param name="as_name">参数名称</param>
        /// <param name="as_value">参数值</param>
        /// <returns>添加本参数之后的由多个参数组成的字符串</returns>
        public static string f_addparm(string as_parm, string as_name, string as_value)
        {
            if (as_name == null || as_name.Trim() == "")
            {
                return as_parm;
            }
            string ls_rtn;
            if (as_parm == null || as_parm.Trim() == "")
            {
                ls_rtn = as_name + "=" + as_value;
            }
            else
            {
                if (as_value == null || as_value.Trim() == "") as_value = "";
                ls_rtn = as_parm + "&" + as_name + "=" + as_value;
            }
            return ls_rtn;
        }

        /// <summary>
        /// 设置参数的值，有则修改，没有则添加（By孙桢）
        /// </summary>
        public static string f_setparm(string as_parm, string as_name, string as_value)
        {
            string ls_value = null, ls_first;
            int li_pos1, li_pos2;
            li_pos1 = as_parm.IndexOf("?" + as_name + "=");
            if (li_pos1 == -1)
            {
                li_pos1 = as_parm.IndexOf("&" + as_name + "=");
                ls_first = "&";
            }
            else
            {
                ls_first = "?";
            }
            if (li_pos1 >= 0)
            {
                li_pos1 = li_pos1 + as_name.Length + 2;
                li_pos2 = as_parm.IndexOf("&", li_pos1);
                if (li_pos2 > 0)
                {
                    ls_value = as_parm.Substring(li_pos1, li_pos2 - li_pos1);
                    ls_value = f_Replace(as_parm, ls_first + as_name + "=" + ls_value + "&", ls_first + as_name + "=" + as_value + "&");
                }
                else
                {
                    ls_value = as_parm.Substring(li_pos1);
                    ls_value = f_Replace(as_parm, ls_first + as_name + "=" + ls_value, ls_first + as_name + "=" + as_value);
                }
            }
            else
            {
                ls_value = f_addparm(as_parm, as_name, as_value);
            }
            return ls_value;
        }

        /// <summary>
        /// 移除某个参数（By孙桢）
        /// </summary>
        public static string f_removeparm(string as_parm, string as_name)
        {
            string ls_value = null, ls_first;
            int li_pos1, li_pos2;
            li_pos1 = as_parm.IndexOf("?" + as_name + "=");
            if (li_pos1 == -1)
            {
                li_pos1 = as_parm.IndexOf("&" + as_name + "=");
                ls_first = "&";
            }
            else
            {
                ls_first = "?";
            }
            if (li_pos1 >= 0)
            {
                li_pos1 = li_pos1 + as_name.Length + 2;
                li_pos2 = as_parm.IndexOf("&", li_pos1);
                if (li_pos2 > 0)
                {
                    ls_value = as_parm.Substring(li_pos1, li_pos2 - li_pos1);
                    if (ls_first == "?")
                        ls_value = f_Replace(as_parm, ls_first + as_name + "=" + ls_value + "&", "?");
                    else
                        ls_value = f_Replace(as_parm, ls_first + as_name + "=" + ls_value + "&", "&");
                }
                else
                {
                    ls_value = as_parm.Substring(li_pos1);
                    ls_value = f_Replace(as_parm, ls_first + as_name + "=" + ls_value, "");
                }
            }
            else
            {
                ls_value = as_parm;
            }
            return ls_value;
        }

        /// <summary>
        /// 取多参数组成的字符串中的某个参数
        /// </summary>
        /// <param name="as_parm">多参数组成的字符串中</param>
        /// <param name="as_name">要取的参数名称</param>
        /// <returns>字符串中没有该参数则返回NULL,否则返回该参数的value</returns>
        public static string f_getparm(string as_parm, string as_name)
        {
            string ls_value = null;
            int li_pos1, li_pos2;
            li_pos1 = as_parm.IndexOf(as_name) + as_name.Length + 1;//参数第一个字符的位置加上参数的长度再加上"＝"为1的长度
            //2014-9-28 赵中美 方法逻辑错误
            //if (li_pos1 < 0) return ls_value;
            if (li_pos1 < as_name.Length + 1) return ls_value;

            li_pos2 = as_parm.IndexOf("&", li_pos1);
            if (li_pos2 > 0)
            {
                ls_value = as_parm.Substring(li_pos1, li_pos2 - li_pos1);
            }
            else
            {
                ls_value = as_parm.Substring(li_pos1);
            }
            return ls_value;
        }

       

        ///// <summary>
        ///// 往页面添加JS或CSS
        ///// </summary>
        //public static void SetHeader(Page CustomPage, bool JScript, string FilePath)
        //{
        //    StringBuilder StrHeader = new StringBuilder();
        //    Literal HeaderLit = new Literal();
        //    StrHeader.Append(Environment.NewLine);
        //    if (JScript)
        //        StrHeader.Append("\t<script language=\"javascript\" src=\"" + FilePath + "\" type=\"text/javascript\"></script>" + Environment.NewLine);
        //    else
        //        StrHeader.Append("\t<link href=\"" + FilePath + "\" rel=\"stylesheet\" type=\"text/css\" />" + Environment.NewLine);
        //    HeaderLit.Text = StrHeader.ToString();
        //    CustomPage.Header.Controls.Add(HeaderLit);
        //}

        ///// <summary>
        ///// 往页面添加JS或CSS
        ///// </summary>
        //public static void SetHeader(Page CustomPage, bool JScript, string FilePath, string uniqueKey)
        //{
        //    bool lb_find = false;
        //    //如果已经注册过，就不再注册
        //    for (int i = 0; i < CustomPage.Header.Controls.Count; i++)
        //    {
        //        if (CustomPage.Header.Controls[i].ID == uniqueKey) return;
        //    }

        //    StringBuilder StrHeader = new StringBuilder();
        //    Literal HeaderLit = new Literal();
        //    HeaderLit.ID = uniqueKey;
        //    StrHeader.Append(Environment.NewLine);
        //    if (JScript)
        //        StrHeader.Append("\t<script language=\"javascript\" src=\"" + FilePath + "\" type=\"text/javascript\"></script>" + Environment.NewLine);
        //    else
        //        StrHeader.Append("\t<link href=\"" + FilePath + "\" rel=\"stylesheet\" type=\"text/css\" />" + Environment.NewLine);
        //    HeaderLit.Text = StrHeader.ToString();
        //    CustomPage.Header.Controls.Add(HeaderLit);
        //}

        ///// <summary>
        ///// 在服务器端弹出警告信息
        ///// </summary>
        ///// <param name="msg"></param>
        //public static void Alert(string msg)
        //{
        //    Page page = (Page)HttpContext.Current.CurrentHandler;
        //    if (page != null)
        //        page.ClientScript.RegisterStartupScript(typeof(Page), "alert", string.Format("alert('{0}');", msg), true);
        //    else
        //        System.Web.HttpContext.Current.Response.Write("<script>alert('" + msg + "');</script>");
        //}
        ///// <summary>
        ///// 在服务器端弹出警告信息,然后关闭本页面
        ///// </summary>
        ///// <param name="msg">警告信息内容</param>
        //public static void AlertAndClose(string msg)
        //{
        //    Page page = (Page)HttpContext.Current.CurrentHandler;
        //    if (page != null)
        //        page.ClientScript.RegisterStartupScript(typeof(Page), "alert", string.Format("alert('{0}');window.close();", msg), true);
        //    else
        //        System.Web.HttpContext.Current.Response.Write("<script>alert('" + msg + "');window.close();</script>");
        //}

        ///// <summary>
        ///// 子窗口在服务器端弹出警告信息,然后关闭窗口,同时父窗口跳转到指定页面
        ///// </summary>
        ///// <param name="msg">警告信息内容</param>
        ///// <param name="href">跳转到指定页面</param>
        //public static void AlertAndCloseGoTo(string msg, string href)
        //{
        //    Page page = (Page)HttpContext.Current.CurrentHandler;
        //    if (page != null)
        //        page.ClientScript.RegisterStartupScript(typeof(Page), "alert", string.Format("alert('{0}');window.opener.location.href='{1}';window.close();", msg, href), true);
        //    else
        //        System.Web.HttpContext.Current.Response.Write("<script>alert('" + msg + "');window.opener.location.href='" + href + "';window.close();</script>");
        //}

        ///// <summary>
        ///// 在服务器端弹出警告信息,同时跳转到指定页面
        ///// </summary>
        ///// <param name="msg">警告信息内容</param>
        ///// <param name="href">跳转到指定页面</param>
        //public static void AlertAndGoTo(string msg, string href)
        //{
        //    Page page = (Page)HttpContext.Current.CurrentHandler;
        //    if (page != null)
        //        page.ClientScript.RegisterStartupScript(typeof(Page), "alert", string.Format("alert('{0}');location.href='{1}';", msg, href), true);
        //    else
        //        System.Web.HttpContext.Current.Response.Write("<script>alert('" + msg + "');location.href='" + href + "';</script>");
        //}

        ///// <summary>
        ///// 在服务器端弹出警告信息,同时指定框架跳转到指定页面
        ///// </summary>
        ///// <param name="msg">警告信息内容</param>
        ///// <param name="frame">指定框架</param>
        ///// <param name="href">跳转到指定页面</param>
        //public static void AlertAndGoTo(string msg, string frame, string href)
        //{
        //    Page page = (Page)HttpContext.Current.CurrentHandler;
        //    if (page != null)
        //        page.ClientScript.RegisterStartupScript(typeof(Page), "alert", string.Format("alert('{0}');{1}.location.href='{2}';", msg, frame, href), true);
        //    else
        //        System.Web.HttpContext.Current.Response.Write("<script>alert('" + msg + "');" + frame + ".location.href='" + href + "';</script>");
        //}

        ///// <summary>
        ///// 在服务器端弹出警告信息,同时刷新父窗口
        ///// </summary>
        ///// <param name="msg">警告信息内容</param>
        //public static void AlertAndReload(string msg)
        //{
        //    Page page = (Page)HttpContext.Current.CurrentHandler;
        //    if (page != null)
        //        page.ClientScript.RegisterStartupScript(typeof(Page), "alert", string.Format("alert('{0}');window.close();window.opener.location.reload();", msg), true);
        //    else
        //        System.Web.HttpContext.Current.Response.Write("<script>alert('" + msg + "');window.close();window.opener.location.reload();</script>");
        //}

        ///// <summary>
        ///// 在服务器端弹出警告信息,同时刷新本页面
        ///// </summary>
        ///// <param name="msg">警告信息内容</param>
        //public static void AlertAndRefresh(string msg)
        //{
        //    Page page = (Page)HttpContext.Current.CurrentHandler;
        //    if (page != null)
        //        page.ClientScript.RegisterStartupScript(typeof(Page), "alert", string.Format("alert('{0}');location.reload();", msg), true);
        //    else
        //        System.Web.HttpContext.Current.Response.Write("<script>alert('" + msg + "');location.reload();</script>");
        //}

        ///// <summary>
        ///// 在服务器端弹出警告信息,同时刷新指定框架
        ///// </summary>
        ///// <param name="msg">警告信息内容</param>
        ///// <param name="frame">指定框架</param>
        //public static void AlertAndReload(string msg, string frame)
        //{
        //    Page page = (Page)HttpContext.Current.CurrentHandler;
        //    if (page != null)
        //        page.ClientScript.RegisterStartupScript(typeof(Page), "alert", string.Format("alert('{0}');{1}.location.reload();", msg, frame), true);
        //    else
        //        System.Web.HttpContext.Current.Response.Write("<script>alert('" + msg + "');" + frame + ".location.reload();</script>");
        //}

        /// <summary>
        /// 判断对象是否为空或数据库空值；用于判断ExecuteScalar的返回值，null表示找不到记录，DBNull表示数据库空值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrDBNull(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return true;
            else
                return false;
        }


        /**/
        /// <summary> 
        /// 转换人民币大小金额 
        /// </summary> 
        /// <param name="num">金额</param> 
        /// <returns>返回大写形式</returns> 
        public static string CmycurD(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字 
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
            string str3 = "";    //从原num值中取出的值 
            string str4 = "";    //数字的字符串形式 
            string str5 = "";  //人民币大写金额形式 
            int i;    //循环变量 
            int j;    //num的值乘以100的字符串长度 
            string ch1 = "";    //数字的汉语读法 
            string ch2 = "";    //数字位的汉字读法 
            int nzero = 0;  //用来计算连续的零值是几个 
            int temp;            //从原num值中取出的值 

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数 
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式 
            j = str4.Length;      //找出最高位 
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

            //循环取出每一位需要转换的值 
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值 
                temp = Convert.ToInt32(str3);      //转换为数字 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整” 
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }

        /**/
        /// <summary> 
        /// 一个重载，将字符串先转换成数字在调用CmycurD(decimal num) 
        /// </summary> 
        /// <param name="num">用户输入的金额，字符串形式未转成decimal</param> 
        /// <returns></returns> 
        public static string CmycurD(string numstr)
        {
            try
            {
                decimal num = Convert.ToDecimal(numstr);
                return CmycurD(num);
            }
            catch
            {
                return "金额出错，非数字形式！";
            }
        }
        /// <summary>
        /// 获取日其中的星期，中文表示
        /// </summary>
        /// <param name="adt"></param>
        /// <returns></returns>
        public static string GetWeekByDateTime(DateTime adt)
        {
            string ls_week = adt.DayOfWeek.ToString();
            switch (ls_week)
            {
                case "Monday":
                    ls_week = "星期一";
                    break;
                case "Tuesday":
                    ls_week = "星期二";
                    break;
                case "Wednesday":
                    ls_week = "星期三";
                    break;
                case "Thursday":
                    ls_week = "星期四";
                    break;
                case "Friday":
                    ls_week = "星期五";
                    break;
                case "Saturday":
                    ls_week = "星期六";
                    break;
                case "Sunday":
                    ls_week = "星期日";
                    break;
                default:
                    break;
            }
            return ls_week;
        }
        /// <summary>
        /// 替换字符串中所有的数字
        /// </summary>
        /// <param name="as_source"></param>
        /// <returns></returns>
        public static string ReplaceNum(string as_source)
        {
            //if (string.IsNullOrEmpty(as_source)) return as_source;
            //for (int i = 0; i <= 9; i++)
            //{
            //    if (!char.IsNumber(as_source.Substring(as_source.Length - 1, 1), 0)) break;
            //    as_source = as_source.Replace(i.ToString(), "");
            //}
            //return as_source;
            if (string.IsNullOrEmpty(as_source)) return as_source;
            for (int i = 0; i <= 9; i++)
            {
                as_source = as_source.Replace(i.ToString(), "");
            }
            return as_source;
        }


        ///// <summary> 
        ///// 合并GridView中某列相同信息的行（单元格） 
        ///// </summary> 
        ///// <param name="GridView1">GridView</param> 
        ///// <param name="cellNum">第几列</param> 
        //public static void GroupRows(GridView GridView1, int cellNum)
        //{
        //    int i = 0, rowSpanNum = 1;
        //    while (i < GridView1.Rows.Count - 1)
        //    {
        //        GridViewRow gvr = GridView1.Rows[i];
        //        for (++i; i < GridView1.Rows.Count; i++)
        //        {
        //            GridViewRow gvrNext = GridView1.Rows[i];
        //            if (gvr.Cells[cellNum].Text == gvrNext.Cells[cellNum].Text)
        //            {
        //                gvrNext.Cells[cellNum].Visible = false;
        //                rowSpanNum++;
        //            }
        //            else
        //            {
        //                gvr.Cells[cellNum].RowSpan = rowSpanNum;
        //                rowSpanNum = 1;
        //                break;
        //            }
        //            if (i == GridView1.Rows.Count - 1)
        //            {
        //                gvr.Cells[cellNum].RowSpan = rowSpanNum;
        //            }
        //        }
        //    }
        //}

    }

    #region 孙桢加的一个枚举类型
    public enum dateFormat
    {
        LongTime = 0,
        ShortTime = 1,
        LongDate = 2,
        ShortDate = 3,
        Second = 4,
        Minute = 5,
        Hour = 6,
        Day = 7,
        WeekDay = 8,
        Month = 9,
        Year = 10,
        DateTime = 11,
        DateTimeChinese = 12,
    }
    #endregion
}
