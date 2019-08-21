using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RIA.UIModel
{
    /// <summary>
    /// 钱包金额流水UIModel
    /// </summary>
    public class WalletTransLogQueryUIModel : BaseUIModel
    {
        #region 电子钱包异动

        /// <summary>
        /// 受理组织
        /// </summary>
        public String WalT_Org_Name { get; set; }
        /// <summary>
        /// 钱包账号
        /// </summary>
        public String WalT_Wal_No { get; set; }
        /// <summary>
        /// 开户人
        /// </summary>
        public String Wal_CustomerName { get; set; }
        /// <summary>
        /// 异动时间
        /// </summary>
        public DateTime? WalT_Time { get; set; }
        /// <summary>
        /// 异动类型
        /// </summary>
        public String WalT_TypeName { get; set; }
        /// <summary>
        /// 充值方式
        /// </summary>
        public String WalT_RechargeTypeName { get; set; }
        /// <summary>
        /// 异动金额
        /// </summary>
        public Decimal? WalT_Amount { get; set; }
        /// <summary>
        /// 异动单号
        /// </summary>
        public String WalT_BillNo { get; set; }
        /// <summary>
        /// 通道
        /// </summary>
        public String WalT_ChannelName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WalT_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WalT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WalT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WalT_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WalT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WalT_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WalT_VersionNo { get; set; }
        /// <summary>
        /// 通道编码
        /// </summary>
        public String WalT_ChannelCode { get; set; }
        /// <summary>
        /// 充值方式编码
        /// </summary>
        public String WalT_RechargeTypeCode { get; set; }
        /// <summary>
        /// 钱包异动ID
        /// </summary>
        public String WalT_ID { get; set; }
        /// <summary>
        /// 受理组织ID
        /// </summary>
        public String WalT_Org_ID { get; set; }
        /// <summary>
        /// 钱包ID
        /// </summary>
        public String WalT_Wal_ID { get; set; }
        /// <summary>
        /// 异动类型编码
        /// </summary>
        public String WalT_TypeCode { get; set; }
        #endregion

        #region 其他属性

        private bool _isChecked = false;
        /// <summary>
        /// 选择
        /// </summary>
        public bool IsChecked
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
        /// 组织ID
        /// </summary>
        public String Wal_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String Wal_Org_Name { get; set; }
        /// <summary>
        /// 钱包所有人类别编码
        /// </summary>
        public String Wal_OwnerTypeCode { get; set; }
        /// <summary>
        /// 钱包所有人类别名称
        /// </summary>
        public String Wal_OwnerTypeName { get; set; }
        /// <summary>
        /// 开户人ID
        /// </summary>
        public String Wal_CustomerID { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String AutoFactoryName { get; set; }
        /// <summary>
        /// 开户人手机号
        /// </summary>
        public String CustomerPhoneNo { get; set; }
        #endregion
    }
}
