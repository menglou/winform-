using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SM
{
    /// <summary>
    /// 已授权汽修商户授权查询UIModel
    /// </summary>
    public class MerchantAuthorityQueryUIModel : BaseUIModel
    {
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String ASAH_ARMerchant_Code { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String ASAH_ARMerchant_Name { get; set; }
        /// <summary>
        /// 汽修组织编码
        /// </summary>
        public String ASAH_AROrg_Code { get; set; }
        /// <summary>
        /// 汽修组织名称
        /// </summary>
        public String ASAH_AROrg_Name { get; set; }
        /// <summary>
        /// 汽修组织联系人
        /// </summary>
        public String ASAH_AROrg_Contacter { get; set; }
        /// <summary>
        /// 汽修组织联系方式
        /// </summary>
        public String ASAH_AROrg_Phone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String ASAH_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? ASAH_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String ASAH_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ASAH_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String ASAH_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ASAH_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? ASAH_VersionNo { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public String ASAH_ID { get; set; }
    }
}
