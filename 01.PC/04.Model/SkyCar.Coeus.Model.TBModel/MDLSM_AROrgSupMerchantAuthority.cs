using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 汽配汽修商户授权Model
    /// </summary>
    public class MDLSM_AROrgSupMerchantAuthority
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String ASAH_ID { get; set; }
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
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeEnd { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String ASAH_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ASAH_UpdatedTime { get; set; }
        /// <summary>
        /// 修改时间-开始（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeStart { get; set; }
        /// <summary>
        /// 修改时间-终了（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeEnd { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? ASAH_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String ASAH_TransID { get; set; }
        /// <summary>
        /// 总记录条数（翻页用）
        /// </summary>
        public Int32? RecordCount { get; set; }
        /// <summary>
        /// 当前页面索引/要跳转的页码（翻页用）
        /// </summary>
        public Int32? PageIndex { get; set; }
        /// <summary>
        /// 页面大小（翻页用）
        /// </summary>
        public Int32? PageSize { get; set; }
        #endregion

        #region 公共属性-条件用
        /// <summary>
        /// ID
        /// </summary>
        public String WHERE_ASAH_ID { get; set; }
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String WHERE_ASAH_ARMerchant_Code { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String WHERE_ASAH_ARMerchant_Name { get; set; }
        /// <summary>
        /// 汽修组织编码
        /// </summary>
        public String WHERE_ASAH_AROrg_Code { get; set; }
        /// <summary>
        /// 汽修组织名称
        /// </summary>
        public String WHERE_ASAH_AROrg_Name { get; set; }
        /// <summary>
        /// 汽修组织联系人
        /// </summary>
        public String WHERE_ASAH_AROrg_Contacter { get; set; }
        /// <summary>
        /// 汽修组织联系方式
        /// </summary>
        public String WHERE_ASAH_AROrg_Phone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_ASAH_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_ASAH_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_ASAH_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_ASAH_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_ASAH_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_ASAH_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_ASAH_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_ASAH_TransID { get; set; }
        #endregion

    }
}
