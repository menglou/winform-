using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM.APModel
{
    /// <summary>
    /// 收付款UIModel
    /// </summary>
    public class ReceiptAndPayUIModel : BaseUIModel
    {
        /// <summary>
        /// 单据ID
        /// </summary>
        public String BillID { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public String BillNo { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String BillOrgID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String BillOrgName { get; set; }
        /// <summary>
        /// 单据类型（收款单/付款单）
        /// </summary>
        public String BillType { get; set; }
        /// <summary>
        /// 单据来源类型编码
        /// </summary>
        public String BillSourceTypeCode { get; set; }
        /// <summary>
        /// 单据来源类型名称
        /// </summary>
        public String BillSourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String BillSourceNo { get; set; }
        /// <summary>
        /// 交易方式编码
        /// </summary>
        public String TradeTypeCode { get; set; }
        /// <summary>
        /// 交易方式名称
        /// </summary>
        public String TradeTypeName { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public Decimal? TradeTotalAmount { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public String BillCreatedBy { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime? BillCreatedTime { get; set; }
    }
}
