using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 销售模板Model
    /// </summary>
    public class MDLSD_SalesTemplate
    {
        #region 公共属性
        /// <summary>
        /// 销售模板ID
        /// </summary>
        public String SasT_ID { get; set; }
        /// <summary>
        /// 销售模板名称
        /// </summary>
        public String SasT_Name { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String SasT_Org_ID { get; set; }
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String SasT_AutoFactoryCode { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String SasT_AutoFactoryName { get; set; }
        /// <summary>
        /// 汽修商客户ID
        /// </summary>
        public String SasT_CustomerID { get; set; }
        /// <summary>
        /// 汽修商客户名称
        /// </summary>
        public String SasT_CustomerName { get; set; }
        /// <summary>
        /// 汽修商组织编码
        /// </summary>
        public String SasT_AutoFactoryOrgCode { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String SasT_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String SasT_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String SasT_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SasT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SasT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SasT_CreatedTime { get; set; }
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
        public String SasT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SasT_UpdatedTime { get; set; }
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
        public Int64? SasT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String SasT_TransID { get; set; }
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
        /// 销售模板ID
        /// </summary>
        public String WHERE_SasT_ID { get; set; }
        /// <summary>
        /// 销售模板名称
        /// </summary>
        public String WHERE_SasT_Name { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SasT_Org_ID { get; set; }
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String WHERE_SasT_AutoFactoryCode { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String WHERE_SasT_AutoFactoryName { get; set; }
        /// <summary>
        /// 汽修商客户ID
        /// </summary>
        public String WHERE_SasT_CustomerID { get; set; }
        /// <summary>
        /// 汽修商客户名称
        /// </summary>
        public String WHERE_SasT_CustomerName { get; set; }
        /// <summary>
        /// 汽修商组织编码
        /// </summary>
        public String WHERE_SasT_AutoFactoryOrgCode { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_SasT_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_SasT_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_SasT_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SasT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_SasT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_SasT_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_SasT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_SasT_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SasT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_SasT_TransID { get; set; }
        #endregion

    }
}
