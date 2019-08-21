using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM.APModel
{
    /// <summary>
    /// 应收单确认收款UIModel
    /// </summary>
    public class ReceiveableCollectionConfirmUIModel : BaseUIModel
    {
        #region 公共属性
        /// <summary>
        /// 是否来源应付单
        /// </summary>
        public bool IsBusinessSourceAccountPayableBill { get; set; }
        /// <summary>
        /// 单据ID
        /// </summary>
        public String BusinessID { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public String BusinessNo { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String BusinessOrgID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String BusinessOrgName { get; set; }
        /// <summary>
        /// 单据来源类型编码
        /// </summary>
        public String BusinessSourceTypeCode { get; set; }
        /// <summary>
        /// 单据来源类型名称
        /// </summary>
        public String BusinessSourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String BusinessSourceNo { get; set; }
        /// <summary>
        /// 付款对象类型编码
        /// </summary>
        public String PayObjectTypeCode { get; set; }
        /// <summary>
        /// 付款对象类型名称
        /// </summary>
        public String PayObjectTypeName { get; set; }
        /// <summary>
        /// 付款对象ID
        /// </summary>
        public String PayObjectID { get; set; }
        /// <summary>
        /// 付款对象
        /// </summary>
        public String PayObjectName { get; set; }
        /// <summary>
        /// 收款方式编码
        /// </summary>
        public String ReceiveTypeCode { get; set; }
        /// <summary>
        /// 收款方式名称
        /// </summary>
        public String ReceiveTypeName { get; set; }
        /// <summary>
        /// 收款账号
        /// </summary>
        public String ReceiveAccount { get; set; }
        /// <summary>
        /// 收款凭证编号
        /// </summary>
        public String ReceiveCertificateNo { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? ReceivableTotalAmount { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public Decimal? ReceiveTotalAmount { get; set; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public Decimal? UnReceiveTotalAmount { get; set; }
        /// <summary>
        /// 本次收款金额
        /// </summary>
        public Decimal? ThisReceiveAmount { get; set; }
        /// <summary>
        /// 客户组织
        /// </summary>
        public string SOD_StockInOrgName { get; set; }
        /// <summary>
        /// 钱包ID
        /// </summary>
        public string Wal_ID { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public string Wal_CustomerID { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Wal_No { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal? Wal_AvailableBalance { get; set; }
        /// <summary>
        /// 钱包版本号
        /// </summary>
        public Int64? Wal_VersionNo { get; set; }
        #endregion

        #region 应收单属性
        /// <summary>
        /// 应收单ID
        /// </summary>
        public String ARB_ID { get; set; }
        /// <summary>
        /// 应收单号
        /// </summary>
        public String ARB_No { get; set; }
        /// <summary>
        /// 单据方向编码
        /// </summary>
        public String ARB_BillDirectCode { get; set; }
        /// <summary>
        /// 单据方向名称
        /// </summary>
        public String ARB_BillDirectName { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String ARB_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String ARB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单据号
        /// </summary>
        public String ARB_SrcBillNo { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String ARB_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String ARB_Org_Name { get; set; }
        /// <summary>
        /// 付款对象类型编码
        /// </summary>
        public String ARB_PayObjectTypeCode { get; set; }
        /// <summary>
        /// 付款对象类型名称
        /// </summary>
        public String ARB_PayObjectTypeName { get; set; }
        /// <summary>
        /// 付款对象ID
        /// </summary>
        public String ARB_PayObjectID { get; set; }
        /// <summary>
        /// 付款对象名称
        /// </summary>
        public String ARB_PayObjectName { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? ARB_AccountReceivableAmount { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public Decimal? ARB_ReceivedAmount { get; set; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public Decimal? ARB_UnReceiveAmount { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String ARB_BusinessStatusCode { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        public String ARB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String ARB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String ARB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String ARB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? ARB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String ARB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ARB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String ARB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ARB_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? ARB_VersionNo { get; set; }
        #endregion
    }
}
