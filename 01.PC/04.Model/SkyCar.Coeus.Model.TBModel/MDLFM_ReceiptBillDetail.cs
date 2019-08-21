using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 收款单明细Model
    /// </summary>
    public class MDLFM_ReceiptBillDetail
    {
        #region 公共属性
        /// <summary>
        /// 收款单明细ID
        /// </summary>
        public String RBD_ID { get; set; }
        /// <summary>
        /// 收款单ID
        /// </summary>
        public String RBD_RB_ID { get; set; }
        /// <summary>
        /// 收款单号
        /// </summary>
        public String RBD_RB_No { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String RBD_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String RBD_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String RBD_SrcBillNo { get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public Decimal? RBD_ReceiveAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String RBD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? RBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String RBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? RBD_CreatedTime { get; set; }
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
        public String RBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? RBD_UpdatedTime { get; set; }
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
        public Int64? RBD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String RBD_TransID { get; set; }
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
        /// 收款单明细ID
        /// </summary>
        public String WHERE_RBD_ID { get; set; }
        /// <summary>
        /// 收款单ID
        /// </summary>
        public String WHERE_RBD_RB_ID { get; set; }
        /// <summary>
        /// 收款单号
        /// </summary>
        public String WHERE_RBD_RB_No { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String WHERE_RBD_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_RBD_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String WHERE_RBD_SrcBillNo { get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public Decimal? WHERE_RBD_ReceiveAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_RBD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_RBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_RBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_RBD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_RBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_RBD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_RBD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_RBD_TransID { get; set; }
        #endregion

    }
}
