using System;

namespace SkyCar.Coeus.UIModel.Common.QCModel
{
    /// <summary>
    /// 客户查询QCModel
    /// </summary>
    public class CustomerQueryQCModel : BaseQCModel
    {
        #region 公共属性-条件用
        /// <summary>
        /// 是否平台商户
        /// </summary>
        public Boolean? WHERE_AFC_IsPlatform { get; set; }
        /// <summary>
        /// 汽修商编码
        /// </summary>
        public String WHERE_AFC_Code { get; set; }
        /// <summary>
        /// 汽修商名称
        /// </summary>
        public String WHERE_AFC_Name { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        public String WHERE_CustomerType { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public String WHERE_CustomerID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String WHERE_CustomerName { get; set; }
        /// <summary>
        /// 汽修商编码
        /// </summary>
        public String WHERE_AutoFactoryCode { get; set; }
        /// <summary>
        /// 汽修商名称
        /// </summary>
        public String WHERE_AutoFactoryName { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_OrgID { get; set; }
        #endregion
    }
}
