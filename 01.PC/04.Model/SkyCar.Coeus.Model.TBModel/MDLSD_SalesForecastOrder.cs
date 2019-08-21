using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 销售预测订单Model
    /// </summary>
    public class MDLSD_SalesForecastOrder
    {
        #region 公共属性
        /// <summary>
        /// 销售预测订单ID
        /// </summary>
        public String SFO_ID { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public String SFO_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String SFO_Org_ID { get; set; }
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String SFO_AutoFactoryCode { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String SFO_AutoFactoryName { get; set; }
        /// <summary>
        /// 汽修商客户ID
        /// </summary>
        public String SFO_CustomerID { get; set; }
        /// <summary>
        /// 汽修商客户名称
        /// </summary>
        public String SFO_CustomerName { get; set; }
        /// <summary>
        /// 汽修商组织编码
        /// </summary>
        public String SFO_AutoFactoryOrgCode { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String SFO_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String SFO_SourceTypeName { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String SFO_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String SFO_StatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String SFO_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SFO_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SFO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SFO_CreatedTime { get; set; }
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
        public String SFO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SFO_UpdatedTime { get; set; }
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
        public Int64? SFO_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String SFO_TransID { get; set; }
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
        /// 销售预测订单ID
        /// </summary>
        public String WHERE_SFO_ID { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public String WHERE_SFO_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SFO_Org_ID { get; set; }
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String WHERE_SFO_AutoFactoryCode { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String WHERE_SFO_AutoFactoryName { get; set; }
        /// <summary>
        /// 汽修商客户ID
        /// </summary>
        public String WHERE_SFO_CustomerID { get; set; }
        /// <summary>
        /// 汽修商客户名称
        /// </summary>
        public String WHERE_SFO_CustomerName { get; set; }
        /// <summary>
        /// 汽修商组织编码
        /// </summary>
        public String WHERE_SFO_AutoFactoryOrgCode { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String WHERE_SFO_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_SFO_SourceTypeName { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String WHERE_SFO_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_SFO_StatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_SFO_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SFO_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_SFO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_SFO_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_SFO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_SFO_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SFO_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_SFO_TransID { get; set; }
        #endregion

    }
}
