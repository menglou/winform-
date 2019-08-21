using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 销售预测订单查询QCModel
    /// </summary>
    public class SalesForecastOrderQCModel : BaseQCModel
    {
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
        /// 客户ID
        /// </summary>
        public String WHERE_SFO_CustomerID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String WHERE_SFO_CustomerName { get; set; }
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
