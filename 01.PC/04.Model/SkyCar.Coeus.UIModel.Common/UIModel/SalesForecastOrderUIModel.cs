using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 销售预测订单查询UIModel
    /// </summary>
    public class SalesForecastOrderUIModel : BaseUIModel
    {
        /// <summary>
        /// 行标识
        /// </summary>
        public string RowID { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }

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
        /// 修改人
        /// </summary>
        public String SFO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SFO_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SFO_VersionNo { get; set; }
        #endregion
    }
}
