using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 普通客户管理QCModel
    /// </summary>
    public class GeneralCustomerManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_GC_Org_ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public String WHERE_GC_Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public String WHERE_GC_PhoneNo { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_GC_IsValid { get; set; }
    }
}
