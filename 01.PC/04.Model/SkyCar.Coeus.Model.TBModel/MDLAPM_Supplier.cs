using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// Venus对应的供应商表
    /// </summary>
    public class MDLAPM_Supplier
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String SUPP_ID { get; set; }
        /// <summary>
        /// 汽配商户编码
        /// </summary>
        public String SUPP_MerchantCode { get; set; }
        /// <summary>
        /// 汽配商户名称
        /// </summary>
        public String SUPP_MerchantName { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String SUPP_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String SUPP_SourceTypeName { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String SUPP_Name { get; set; }
        /// <summary>
        /// 供应商编码
        /// </summary>
        public String SUPP_Code { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public String SUPP_Contacter { get; set; }
        /// <summary>
        /// 固定号码
        /// </summary>
        public String SUPP_Tel { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public String SUPP_Phone { get; set; }
        /// <summary>
        /// QQ号码
        /// </summary>
        public String SUPP_QQ { get; set; }
        /// <summary>
        /// 结款方式
        /// </summary>
        public String SUPP_SettlementMode { get; set; }
        /// <summary>
        /// 结款周期
        /// </summary>
        public Int32? SUPP_SettlementCycle { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public String SUPP_BankName { get; set; }
        /// <summary>
        /// 开户名
        /// </summary>
        public String SUPP_BankAccountName { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public String SUPP_BankAccountNo { get; set; }
        /// <summary>
        /// 主营配件
        /// </summary>
        public String SUPP_MainAutoParts { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public String SUPP_Addreess { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String SUPP_Remark { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public Boolean? SUPP_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SUPP_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SUPP_CreatedTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public String SUPP_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SUPP_UpdatedTime { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SUPP_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String SUPP_TransID { get; set; }

        #endregion
    }
}
