using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RIA.QCModel
{
    /// <summary>
    /// 钱包金额流水QCModel
    /// </summary>
    public class WalletTransLogQueryQCModel : BaseQCModel
    {
        #region 电子钱包异动

        /// <summary>
        /// 受理组织ID
        /// </summary>
        public String WHERE_WalT_Org_ID { get; set; }
        /// <summary>
        /// 钱包账号
        /// </summary>
        public String WHERE_WalT_Wal_No { get; set; }
        /// <summary>
        /// 异动类型
        /// </summary>
        public String WHERE_WalT_TypeName { get; set; }
        /// <summary>
        /// 充值方式
        /// </summary>
        public String WHERE_WalT_RechargeTypeName { get; set; }
        /// <summary>
        /// 异动单号
        /// </summary>
        public String WHERE_WalT_BillNo { get; set; }
        /// <summary>
        /// 异动时间-开始（查询条件用）
        /// </summary>
        public DateTime? _TimeStart { get; set; }
        /// <summary>
        /// 异动时间-终了（查询条件用）
        /// </summary>
        public DateTime? _TimeEnd { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_WalT_Remark { get; set; }

        #endregion

        #region 其他属性
        /// <summary>
        /// 所有人类别
        /// </summary>
        public String WHERE_Wal_OwnerTypeName { get; set; }
        /// <summary>
        /// 开户人
        /// </summary>
        public String WHERE_Wal_CustomerName { get; set; }
        /// <summary>
        /// 汽修商名称
        /// </summary>
        public String WHERE_AutoFactoryName { get; set; }
        /// <summary>
        /// 开户人手机号
        /// </summary>
        public String WHERE_CustomerPhoneNo { get; set; }
        #endregion
    }
}
