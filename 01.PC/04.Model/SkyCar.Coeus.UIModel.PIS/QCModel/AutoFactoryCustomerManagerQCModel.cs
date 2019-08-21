using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 汽修商管理QCModel
    /// </summary>
    public class AutoFactoryCustomerManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_AFC_Org_ID { get; set; }
        /// <summary>
        /// 汽修商编码
        /// </summary>
        public String WHERE_AFC_Code { get; set; }
        /// <summary>
        /// 汽修商名称
        /// </summary>
        public String WHERE_AFC_Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public String WHERE_AFC_PhoneNo { get; set; }
        /// <summary>
        /// 是否平台商户
        /// </summary>
        public Boolean? WHERE_AFC_IsPlatform { get; set; }
        /// <summary>
        /// 是否终止销售
        /// </summary>
        public Boolean? WHERE_AFC_IsEndSales { get; set; }

        /// <summary>
        /// 汽修组织名称
        /// </summary>
        public String WHERE_AFC_AROrg_Name { get; set; }
        /// <summary>
        /// 汽修组织联系人
        /// </summary>
        public String WHERE_AFC_AROrg_Contacter { get; set; }
        /// <summary>
        /// 汽修组织联系方式
        /// </summary>
        public String WHERE_AFC_AROrg_Phone { get; set; }
    }
}
