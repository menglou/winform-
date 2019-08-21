using System;

namespace SkyCar.Coeus.UIModel.Common.UIModel
{
    /// <summary>
    /// 用户查询UIModel
    /// </summary>
    public class UserQueryUIModel : BaseUIModel
    {
        /// <summary>
        /// 行标识
        /// </summary>
        public string RowID { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }

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
        /// 性别
        /// </summary>
        public String User_Sex { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String User_Address { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public String User_PhoneNo { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        public String Org_ID { get; set; }
        /// <summary>
        /// 组织简称
        /// </summary>
        public String Org_ShortName { get; set; }
    }
}
