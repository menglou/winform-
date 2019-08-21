using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.Common.Const
{
    public class RegularFormula
    {
        /// <summary>
        /// 手机号正则表达式
        /// </summary>
        public const string REGULAR_PHONE = "^(13[0-9]|15[0-9]|17[0-9]|18[0-9]|14[0-9])[0-9]{8}$";

        /// <summary>
        /// 电话号码正则表达式   
        /// </summary>
        public const string REGULAR_TEL =  @"^\d{3,4}-\d{6,8}$";


        /// <summary>
        /// 正浮点数正则表达式
        /// </summary>
        public const string REGULAR_AMOUNT = @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$";

        /// <summary>
        /// 身份证号码正则表达式
        /// </summary>
        public const string REGULAR_IDENTITYNO = @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$";

        /// <summary>
        /// 金额
        /// </summary>
        public const string REGULAR_AMOUNT1 = @"^[0-9]+([.]\d{1,2})?$";

        /// <summary>
        /// 正整数
        /// </summary>
        public const string POSITIVE_INTEGER = @"^[0]|[1-9]+[0-9]?$";
    }
}
