using System;

namespace SkyCar.Coeus.UIModel.Common.QCModel
{
    /// <summary>
    /// 汽修商户组织查询QCModel
    /// </summary>
    public class AutoFactoryOrgQueryQCModel : BaseQCModel
    {
        #region 公共属性-条件用
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String WHERE_AFC_Name { get; set; }
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String WHERE_AFC_Code { get; set; }
        /// <summary>
        /// 汽修商户组织名称
        /// </summary>
        public String WHERE_AFC_AROrg_Name { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_OrgID { get; set; }
        #endregion
    }
}
