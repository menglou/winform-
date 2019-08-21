using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 付款单明细Model
    /// </summary>
    public class MDLFM_PayBillDetail
    {
        #region 公共属性
        /// <summary>
        /// 付款单明细ID
        /// </summary>
        public String PBD_ID { get; set; }
        /// <summary>
        /// 付款单ID
        /// </summary>
        public String PBD_PB_ID { get; set; }
        /// <summary>
        /// 付款单号
        /// </summary>
        public String PBD_PB_No { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String PBD_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String PBD_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String PBD_SrcBillNo { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public Decimal? PBD_PayAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String PBD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? PBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String PBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? PBD_CreatedTime { get; set; }
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
        public String PBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? PBD_UpdatedTime { get; set; }
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
        public Int64? PBD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String PBD_TransID { get; set; }
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
        /// 付款单明细ID
        /// </summary>
        public String WHERE_PBD_ID { get; set; }
        /// <summary>
        /// 付款单ID
        /// </summary>
        public String WHERE_PBD_PB_ID { get; set; }
        /// <summary>
        /// 付款单号
        /// </summary>
        public String WHERE_PBD_PB_No { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String WHERE_PBD_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_PBD_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String WHERE_PBD_SrcBillNo { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public Decimal? WHERE_PBD_PayAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_PBD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_PBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_PBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_PBD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_PBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_PBD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_PBD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_PBD_TransID { get; set; }
        #endregion

    }
}
