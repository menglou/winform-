using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 组织查询UIModel
    /// </summary>
    public class OrgQueryUIModel : BaseUIModel
    {
        /// <summary>
        /// 行标识
        /// </summary>
        public string RowID { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }

        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String Org_ID { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public String Org_MCT_ID { get; set; }
        /// <summary>
        /// 门店编码
        /// </summary>
        public String Org_Code { get; set; }
        /// <summary>
        /// 平台编码
        /// </summary>
        public String Org_PlatformCode { get; set; }
        /// <summary>
        /// 组织全称
        /// </summary>
        public String Org_FullName { get; set; }
        /// <summary>
        /// 组织简称
        /// </summary>
        public String Org_ShortName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public String Org_Contacter { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public String Org_TEL { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        public String Org_PhoneNo { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String Org_Addr { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String Org_Remark { get; set; }
        #endregion
    }
}
