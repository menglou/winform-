using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 汽修商客户配件价格类别UIModel
    /// </summary>
    public class ARCustomerAutoPartsPriceTypeUIModel : BaseUIModel
    {
        /// <summary>
        /// 汽配商户编码
        /// </summary>
        public String SAAPPT_SupMerchantCode { get; set; }
        /// <summary>
        /// 汽配组织编码
        /// </summary>
        public String SAAPPT_SupOrgCode { get; set; }
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String SAAPPT_ARMerchantCode { get; set; }
        /// <summary>
        /// 汽修组织编码
        /// </summary>
        public String SAAPPT_AROrgCode { get; set; }
        /// <summary>
        /// 配件价格类别
        /// </summary>
        public String SAAPPT_AutoPartsPriceType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String SAAPPT_Remark { get; set; }

        /// <summary>
        /// 操作类别
        /// </summary>
        public String SAAPPT_OperateType { get; set; }
    }
}
