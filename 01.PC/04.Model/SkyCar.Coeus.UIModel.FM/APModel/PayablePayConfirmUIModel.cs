using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM.APModel
{
    /// <summary>
    /// 应付单确认付款UIModel
    /// </summary>
    public class PayablePayConfirmUIModel : BaseUIModel
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
        /// 收款对象类型编码
        /// </summary>
        public String ReceiveObjectTypeCode { get; set; }
        /// <summary>
        /// 收款对象类型名称
        /// </summary>
        public String ReceiveObjectTypeName { get; set; }
        /// <summary>
        /// 收款对象ID
        /// </summary>
        public String ReceiveObjectID { get; set; }
        /// <summary>
        /// 收款对象
        /// </summary>
        public String ReceiveObjectName { get; set; }


        /// <summary>
        /// 付款方式编码
        /// </summary>
        public String PayTypeCode { get; set; }
        /// <summary>
        /// 付款方式名称
        /// </summary>
        public String PayTypeName { get; set; }
        /// <summary>
        /// 付款账号
        /// </summary>
        public String PayAccount { get; set; }
        /// <summary>
        /// 付款凭证编号
        /// </summary>
        public String PayCertificateNo { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? PayableTotalAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public Decimal? PayTotalAmount { get; set; }
        /// <summary>
        /// 未付金额
        /// </summary>
        public Decimal? UnPayTotalAmount { get; set; }
        /// <summary>
        /// 本次付款金额
        /// </summary>
        public Decimal? ThisPayAmount { get; set; }

        ///// <summary>
        ///// 客户组织
        ///// </summary>
        //public string SOD_StockInOrgName { get; set; }
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

        #region 应付单属性
        /// <summary>
        /// 应付单号
        /// </summary>
        public String APB_No { get; set; }
        /// <summary>
        /// 单据方向
        /// </summary>
        public String APB_BillDirectName { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String APB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String APB_SourceBillNo { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String APB_Org_Name { get; set; }
        /// <summary>
        /// 收款对象类型编码
        /// </summary>
        public String APB_ReceiveObjectTypeCode { get; set; }
        /// <summary>
        /// 收款对象类型名称
        /// </summary>
        public String APB_ReceiveObjectTypeName { get; set; }
        /// <summary>
        /// 收款对象ID
        /// </summary>
        public String APB_ReceiveObjectID { get; set; }
        /// <summary>
        /// 收款对象名称
        /// </summary>
        public String APB_ReceiveObjectName { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? APB_AccountPayableAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public Decimal? APB_PaidAmount { get; set; }
        /// <summary>
        /// 未付金额
        /// </summary>
        public Decimal? APB_UnpaidAmount { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public String APB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String APB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String APB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String APB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APB_UpdatedTime { get; set; }
        /// <summary>
        /// 应付单ID
        /// </summary>
        public String APB_ID { get; set; }
        /// <summary>
        /// 单据方向编码
        /// </summary>
        public String APB_BillDirectCode { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String APB_SourceTypeCode { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String APB_Org_ID { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String APB_BusinessStatusCode { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String APB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? APB_VersionNo { get; set; }

        #endregion
    }
}
