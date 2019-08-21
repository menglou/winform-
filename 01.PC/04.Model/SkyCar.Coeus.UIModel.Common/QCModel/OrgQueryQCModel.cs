using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 组织查询QCModel
    /// </summary>
    public class OrgQueryQCModel : BaseQCModel
    {
        #region 公共属性-条件用
        /// <summary>
        /// 门店编码
        /// </summary>
        public String WHERE_Org_Code { get; set; }
        /// <summary>
        /// 组织全称
        /// </summary>
        public String WHERE_Org_FullName { get; set; }
        /// <summary>
        /// 组织简称
        /// </summary>
        public String WHERE_Org_ShortName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public String WHERE_Org_Contacter { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public String WHERE_Org_TEL { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        public String WHERE_Org_PhoneNo { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String WHERE_User_ID { get; set; }
        #endregion
    }
}
