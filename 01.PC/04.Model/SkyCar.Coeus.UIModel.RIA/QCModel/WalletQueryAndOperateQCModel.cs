using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RIA.QCModel
{
    /// <summary>
    /// 钱包查询及操作QCModel
    /// </summary>
    public class WalletQueryAndOperateQCModel : BaseQCModel
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_Wal_Org_ID { get; set; }
        /// <summary>
        /// 钱包账号
        /// </summary>
        public String WHERE_Wal_No { get; set; }
        /// <summary>
        /// 所有人类别
        /// </summary>
        public String WHERE_Wal_OwnerTypeName { get; set; }
        /// <summary>
        /// 开户人
        /// </summary>
        public String WHERE_Wal_CustomerName { get; set; }
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String WHERE_Wal_AutoFactoryCode { get; set; }
        /// <summary>
        /// 汽修商组织编码
        /// </summary>
        public String WHERE_Wal_AutoFactoryOrgCode { get; set; }
        /// <summary>
        /// 推荐员工
        /// </summary>
        public String WHERE_Wal_RecommendEmployee { get; set; }
        /// <summary>
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeEnd { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String WHERE_AutoFactoryName { get; set; }
        /// <summary>
        /// 开户人手机号
        /// </summary>
        public String WHERE_CustomerPhoneNo { get; set; }
    }
}
