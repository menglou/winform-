using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SM
{
    /// <summary>
    /// 已授权汽修商户授权查询QCModel
    /// </summary>
    public class MerchantAuthorityQueryQCModel : BaseQCModel
    {
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String WHERE_ASAH_ARMerchant_Name { get; set; }
        /// <summary>
        /// 汽修组织名称
        /// </summary>
        public String WHERE_ASAH_AROrg_Name { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_ASAH_IsValid { get; set; }
    }
}
