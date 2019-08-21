using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.UIModel
{
    public class LoginUIModel
    {
        #region 属性

        /// <summary>
        /// 用户ID
        /// </summary>
        public String User_ID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public String User_Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String User_Password { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public String User_EMPNO { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        public String UO_Org_ID { get; set; }

        /// <summary>
        /// 组织简称
        /// </summary>
        public String Org_ShortName { get; set; }

        public String Soft_VersionNo { get; set; }

        /// <summary>
        /// 名字首拼
        /// </summary>
        public String UserName_ShortSpellCode { get; set; }

        /// <summary>
        /// 名字全拼
        /// </summary>
        public String UserName_FullSpellCode { get; set; }

        #endregion
    }
}
