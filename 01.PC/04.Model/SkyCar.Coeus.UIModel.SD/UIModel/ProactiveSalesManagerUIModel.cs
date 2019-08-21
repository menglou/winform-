using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SD
{
    /// <summary>
    /// 主动销售管理UIModel
    /// </summary>
    public class ProactiveSalesManagerUIModel : BaseUIModel
    {
        #region 销售订单属性

        /// <summary>
        /// 单据编号
        /// </summary>
        public String SO_No { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String SO_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String SO_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String SO_SourceNo { get; set; }
        /// <summary>
        /// 客户类型编码
        /// </summary>
        public String SO_CustomerTypeCode { get; set; }
        /// <summary>
        /// 客户类型名称
        /// </summary>
        public String SO_CustomerTypeName { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String SO_CustomerName { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public String SO_CustomerID { get; set; }
        /// <summary>
        /// 是否价格含税
        /// </summary>
        public Boolean? SO_IsPriceIncludeTax { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public Decimal? SO_TaxRate { get; set; }
        /// <summary>
        /// 税额
        /// </summary>
        public Decimal? SO_TotalTax { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal? SO_TotalAmount { get; set; }
        /// <summary>
        /// 未税总金额
        /// </summary>
        public Decimal? SO_TotalNetAmount { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String SO_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String SO_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String SO_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String SO_ApprovalStatusName { get; set; }
        /// <summary>
        /// 配件价格类别
        /// </summary>
        public String SO_AutoPartsPriceType { get; set; }
        /// <summary>
        /// 业务员ID
        /// </summary>
        public String SO_SalesByID { get; set; }
        /// <summary>
        /// 业务员名称
        /// </summary>
        public String SO_SalesByName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String SO_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SO_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SO_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String SO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SO_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SO_VersionNo { get; set; }
        /// <summary>
        /// 销售订单ID
        /// </summary>
        public String SO_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String SO_Org_ID { get; set; }

        #endregion

        #region 其他属性

        private Boolean _isChecked = false;
        /// <summary>
        /// 选择
        /// </summary>
        public Boolean IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    PropertyValueChanged = true;
                }
                _isChecked = value;
            }
        }

        /// <summary>
        /// 组织简称
        /// </summary>
        public String Org_ShortName { get; set; }

        /// <summary>
        /// 是否打印条码
        /// </summary>
        public bool IsPrintBarcode { get; set; }

        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String AutoFactoryCode { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String AutoFactoryName { get; set; }
        /// <summary>
        /// 汽修商组织ID
        /// </summary>
        public String AROrgID { get; set; }
        /// <summary>
        /// 汽修商组织编码
        /// </summary>
        public String AROrgCode { get; set; }
        /// <summary>
        /// 汽修商组织名称
        /// </summary>
        public String AROrgName { get; set; }
        /// <summary>
        /// 汽修商组织联系方式
        /// </summary>
        public String AROrgPhone { get; set; }
        /// <summary>
        /// 汽修商组织地址
        /// </summary>
        public String AROrgAddress { get; set; }
        /// <summary>
        /// 配件价格类别（客户配置）
        /// </summary>
        public String AutoPartsPriceType { get; set; }

        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? AccountReceivableAmount { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public Decimal? ReceivedAmount { get; set; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public Decimal? UnReceiveAmount { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? AccountPayableAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public Decimal? PaidAmount { get; set; }
        /// <summary>
        /// 未付金额
        /// </summary>
        public Decimal? UnPaidAmount { get; set; }

        /// <summary>
        /// 信用额度
        /// </summary>
        public Decimal? CreditAmount { get; set; }
        /// <summary>
        /// 欠款金额
        /// </summary>
        public Decimal? DebtAmount { get; set; }

        /// <summary>
        /// 钱包账号
        /// </summary>
        public String Wal_No { get; set; }
        /// <summary>
        /// 可用余额
        /// </summary>
        public Decimal? Wal_AvailableBalance { get; set; }
        #endregion
    }
}
