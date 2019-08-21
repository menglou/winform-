using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// Venus配件采购订单Model
    /// </summary>
    public class MDLAPM_PurchaseOrder
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String PO_ID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public String PO_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String PO_Org_ID { get; set; }
        /// <summary>
        /// 接车单号
        /// </summary>
        public String PO_ARVPB_No { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public String PO_PlateNumber { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public String PO_VIN { get; set; }
        /// <summary>
        /// 发动机号
        /// </summary>
        public String PO_EngineNo { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public Decimal? PO_Amount { get; set; }
        /// <summary>
        /// 物流费
        /// </summary>
        public Decimal? PO_LogisticFee { get; set; }
        /// <summary>
        /// 采购订单状态
        /// </summary>
        public String PO_Status { get; set; }
        /// <summary>
        /// 到货时间
        /// </summary>
        public DateTime? PO_ReceivedTime { get; set; }
        /// <summary>
        /// 到货时间-开始（查询条件用）
        /// </summary>
        public DateTime? _ReceivedTimeStart { get; set; }
        /// <summary>
        /// 到货时间-终了（查询条件用）
        /// </summary>
        public DateTime? _ReceivedTimeEnd { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String PO_SourceType { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? PO_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String PO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? PO_CreatedTime { get; set; }
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
        public String PO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? PO_UpdatedTime { get; set; }
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
        public Int64? PO_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String PO_TransID { get; set; }
        #endregion
    }
}
