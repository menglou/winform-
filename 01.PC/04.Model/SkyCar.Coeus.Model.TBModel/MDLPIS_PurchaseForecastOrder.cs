using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 采购预测订单Model
    /// </summary>
    public class MDLPIS_PurchaseForecastOrder
    {
        #region 公共属性
        /// <summary>
        /// 采购预测订单ID
        /// </summary>
        public String PFO_ID { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public String PFO_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String PFO_Org_ID { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String PFO_SUPP_ID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String PFO_SUPP_Name { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String PFO_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String PFO_SourceTypeName { get; set; }
        /// <summary>
        /// 订单总额
        /// </summary>
        public Decimal? PFO_TotalAmount { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String PFO_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String PFO_StatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String PFO_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? PFO_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String PFO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? PFO_CreatedTime { get; set; }
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
        public String PFO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? PFO_UpdatedTime { get; set; }
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
        public Int64? PFO_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String PFO_TransID { get; set; }
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
        /// 采购预测订单ID
        /// </summary>
        public String WHERE_PFO_ID { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public String WHERE_PFO_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_PFO_Org_ID { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String WHERE_PFO_SUPP_ID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String WHERE_PFO_SUPP_Name { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String WHERE_PFO_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_PFO_SourceTypeName { get; set; }
        /// <summary>
        /// 订单总额
        /// </summary>
        public Decimal? WHERE_PFO_TotalAmount { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String WHERE_PFO_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_PFO_StatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_PFO_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_PFO_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_PFO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_PFO_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_PFO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_PFO_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_PFO_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_PFO_TransID { get; set; }
        #endregion

    }
}
