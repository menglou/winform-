using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace SkyCar.Coeus.BLL.Common
{
    public class BLLClientCheck
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage{get;set;}
        #region ---字符串长度验证--
        /// <summary>
        /// 验证字符串长度合法性
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="min">最小长度</param>
        /// <param name="max">最大长度</param>
        /// <returns>验证结果</returns>
        public bool StrLength(string str, int min, int max)
        {
            try
            {
                bool b = true;
                if (str.Length == 0)
                {
                    throw (new Exception("字符串不存在"));
                }
                int length = str.Length;
                if (length > max || length < min)
                {
                    b = false;
                }
                return b;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 验证字符串长度合法性
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">字符串长度</param>
        /// <returns>验证结果</returns>
        public bool strEqualLength(string str, int length)
        {
            try
            {
                if (str.Length == 0)
                {
                    ErrorMessage="字符串不存在";
                    return false;
                }
                if (str.Length == length)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
        //#region --时间验证--
        ///// <summary>
        ///// 是否为日期型字符串
        ///// </summary>
        ///// <param name="StrSource">日期字符串(2008-05-08)2008/05/08(2008.05.08)</param>
        ///// <returns>验证结果</returns>
        //public bool IsDate(string StrSource)
        //{
        //    bool b = DateTime.TryParse(StrSource,out BLLCom.GetCurStdDatetime());
        //    bool isDate = false;
        //    // yyyy/MM/dd  
        //    if (Regex.IsMatch(StrSource, "^(?<year>\\d{2,4})/(?<month>\\d{1,2})/(?<day>\\d{1,2})$"))
        //        isDate = true;
        //    // yyyy-MM-dd   
        //    else if (Regex.IsMatch(StrSource, "^(?<year>\\d{2,4})-(?<month>\\d{1,2})-(?<day>\\d{1,2})$"))
        //        isDate = true;
        //    // yyyy.MM.dd   
        //    else if (Regex.IsMatch(StrSource, "^(?<year>\\d{2,4})[.](?<month>\\d{1,2})[.](?<day>\\d{1,2})$"))
        //        isDate = true;
        //    return isDate;
        //}
        ///// <summary>
        ///// 是否为时间型字符串
        ///// </summary>
        ///// <param name="StrSource">时间字符串(15:00:00)</param>
        ///// <returns>验证结果</returns>
        //public bool IsTime(string StrSource)
        //{
        //    bool istime = false;
        //    if (Regex.IsMatch(StrSource, "^(?<hour>\\d{1,2}):(?<minute>\\d{1,2}):((?<second>\\d{1,2}))$"))
        //        istime = true;
        //    if (Regex.IsMatch(StrSource, "^(?<hour>\\d{1,2}).(?<minute>\\d{1,2}).((?<second>\\d{1,2}))$"))
        //        istime = true;
        //    return istime;
        //}
        ///// <summary>
        ///// 是否为日期+时间型字符串
        ///// </summary>
        ///// <param name="StrSource">2008-05-08(2008/01/01) 15:00:00</param>
        ///// <returns>验证结果</returns>
        //public bool IsDateTime(string StrSource)
        //{
        //    bool isDateTime = false;
        //    //去除空格
        //    StrSource = StrSource.Replace(" ", "|");
        //    if (Regex.IsMatch(StrSource, "^(?<year>\\d{2,4})-(?<month>\\d{1,2})-(?<day>\\d{1,2})|(?<hour>\\d{1,2}):(?<minute>\\d{1,2}):((?<second>\\d{1,2}))$"))
        //        isDateTime = true;
        //    else if (Regex.IsMatch(StrSource, "^(?<year>\\d{2,4})/(?<month>\\d{1,2})/(?<day>\\d{1,2})|(?<hour>\\d{1,2}):(?<minute>\\d{1,2}):((?<second>\\d{1,2}))$"))
        //        isDateTime = true;
        //    return isDateTime;
        //}
        ///// <summary>
        ///// 判断各类型的日期格式
        ///// </summary>
        ///// <param name="StrSource">日期型字符串</param>
        ///// <returns>验证结果</returns>
        //public bool IsDateOrTime(string StrSource)
        //{
        //    bool isDateTime = false;
        //    //yyyy-MM-dd xx:xx:xx
        //    if (Regex.IsMatch(StrSource, "^(?<year>\\d{2,4})-(?<month>\\d{1,2})-(?<day>\\d{1,2})|(?<hour>\\d{1,2}):(?<minute>\\d{1,2}):((?<second>\\d{1,2}))$"))
        //        isDateTime = true;
        //    //yyyy/mm/dd xx:xx:xx
        //    else if (Regex.IsMatch(StrSource, "^(?<year>\\d{2,4})/(?<month>\\d{1,2})/(?<day>\\d{1,2})|(?<hour>\\d{1,2}):(?<minute>\\d{1,2}):((?<second>\\d{1,2}))$"))
        //        isDateTime = true;
        //    // yyyy/MM/dd  
        //    else if (Regex.IsMatch(StrSource, "^(?<year>\\d{2,4})/(?<month>\\d{1,2})/(?<day>\\d{1,2})$"))
        //        isDateTime = true;
        //    // yyyy-MM-dd   
        //    else if (Regex.IsMatch(StrSource, "^(?<year>\\d{2,4})-(?<month>\\d{1,2})-(?<day>\\d{1,2})$"))
        //        isDateTime = true;
        //    // yyyy.MM.dd   
        //    else if (Regex.IsMatch(StrSource, "^(?<year>\\d{2,4})[.](?<month>\\d{1,2})[.](?<day>\\d{1,2})$"))
        //        isDateTime = true;
        //    // yyyy年MM月dd日  
        //    else if (Regex.IsMatch(StrSource, "^((?<year>\\d{2,4})年)?(?<month>\\d{1,2})月((?<day>\\d{1,2})日)?$"))
        //        isDateTime = true;
        //    // yyyy年MM月dd日  
        //    else if (Regex.IsMatch(StrSource, "^((?<year>\\d{2,4})年)?(正|一|二|三|四|五|六|七|八|九|十|十一|十二)月((一|二|三|四|五|六|七|八|九|十){1,3}日)?$"))
        //        isDateTime = true;

        //    // yyyy年MM月dd日  
        //    else if (Regex.IsMatch(StrSource, "^(零|〇|一|二|三|四|五|六|七|八|九|十){2,4}年((正|一|二|三|四|五|六|七|八|九|十|十一|十二)月((一|二|三|四|五|六|七|八|九|十){1,3}(日)?)?)?$"))
        //        isDateTime = true;
        //    // yyyy年  
        //    //else if (Regex.IsMatch(str, "^(?<year>\\d{2,4})年$"))  
        //    //    isDateTime = true;  

        //    // 农历1  
        //    else if (Regex.IsMatch(StrSource, "^(甲|乙|丙|丁|戊|己|庚|辛|壬|癸)(子|丑|寅|卯|辰|巳|午|未|申|酉|戌|亥)年((正|一|二|三|四|五|六|七|八|九|十|十一|十二)月((一|二|三|四|五|六|七|八|九|十){1,3}(日)?)?)?$"))
        //        isDateTime = true;
        //    // 农历2  
        //    else if (Regex.IsMatch(StrSource, "^((甲|乙|丙|丁|戊|己|庚|辛|壬|癸)(子|丑|寅|卯|辰|巳|午|未|申|酉|戌|亥)年)?(正|一|二|三|四|五|六|七|八|九|十|十一|十二)月初(一|二|三|四|五|六|七|八|九|十)$"))
        //        isDateTime = true;

        //    // XX时XX分XX秒  
        //    else if (Regex.IsMatch(StrSource, "^(?<hour>\\d{1,2})(时|点)(?<minute>\\d{1,2})分((?<second>\\d{1,2})秒)?$"))
        //        isDateTime = true;
        //    //xx:xx:xx
        //    else if (Regex.IsMatch(StrSource, "^(?<hour>\\d{1,2}):(?<minute>\\d{1,2}):((?<second>\\d{1,2}))$"))
        //        isDateTime = true;
        //    // XX时XX分XX秒  
        //    else if (Regex.IsMatch(StrSource, "^((零|一|二|三|四|五|六|七|八|九|十){1,3})(时|点)((零|一|二|三|四|五|六|七|八|九|十){1,3})分(((零|一|二|三|四|五|六|七|八|九|十){1,3})秒)?$"))
        //        isDateTime = true;
        //    // XX分XX秒  
        //    else if (Regex.IsMatch(StrSource, "^(?<minute>\\d{1,2})分(?<second>\\d{1,2})秒$"))
        //        isDateTime = true;
        //    // XX分XX秒  
        //    else if (Regex.IsMatch(StrSource, "^((零|一|二|三|四|五|六|七|八|九|十){1,3})分((零|一|二|三|四|五|六|七|八|九|十){1,3})秒$"))
        //        isDateTime = true;

        //    // XX时  
        //    else if (Regex.IsMatch(StrSource, "\\b(?<hour>\\d{1,2})(时|点钟)\\b"))
        //        isDateTime = true;
        //    else
        //        isDateTime = false;
        //    return isDateTime;
        //}
        /// <summary>
        /// 验证日期时间的区间有效性
        /// </summary>
        /// <param name="StrSource">日期时间字符串（2014-12-12(2014/12/12) 12:59:59）</param>
        /// <param name="minDateTime">最小日期时间</param>
        /// <param name="maxDateTime">最大日期时间</param>
        /// <returns></returns>
        public bool BetweenDateTime(string StrSource, string minDateTime, string maxDateTime)
        {
            try
            {
                DateTime date;
                bool isDateTime = true;
                if (!DateTime.TryParse(StrSource, out date) || !DateTime.TryParse(minDateTime, out date) || !DateTime.TryParse(maxDateTime, out date))
                {
                    ErrorMessage = "格式不正确";
                    isDateTime = false;
                }
                string str = StrSource.Replace("-", "");
                str = str.Replace("/", "");
                str = str.Replace(":", "");
                str = str.Replace(" ", "");
                string minDate = minDateTime.Replace("-", "");
                minDate = minDate.Replace("/", "");
                minDate = minDate.Replace(":", "");
                minDate = minDate.Replace(" ", "");
                string maxDate = maxDateTime.Replace(":", "");
                maxDate = maxDate.Replace("/", "");
                maxDate = maxDate.Replace(" ", "");
                maxDate = maxDate.Replace("-", "");
                if (long.Parse(str) >= long.Parse(minDate) && long.Parse(str) <= long.Parse(maxDate))
                {
                    isDateTime = true;
                }
                else
                {
                    isDateTime = false;
                }
                return isDateTime;
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message+ex.StackTrace;
                return false;
            }
        }
        //#endregion
        #region --正则表达式验证--
        /// <summary>
        /// 正则表达式验证
        /// </summary>
        /// <param name="paramStr">要验证的字符串</param>
        /// <param name="paramRegex">相应正则表达式</param>
        /// <returns>返回bool</returns>
        public bool IsRegex(string paramStr, string paramRegex)
        {
            bool isMailBox = false;
            if (Regex.IsMatch(paramStr,paramRegex))
                isMailBox = true;
            return isMailBox;
        }
        #endregion
    }
}
