using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 调拨单Model
    /// </summary>
    public class MDLPIS_TransferBill
    {
        #region 公共属性
        /// <summary>
        /// 调拨单ID
        /// </summary>
        public String TB_ID { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public String TB_No { get; set; }
        /// <summary>
        /// 单据类型编码
        /// </summary>
        public String TB_TypeCode { get; set; }
        /// <summary>
        /// 单据类型名称
        /// </summary>
        public String TB_TypeName { get; set; }
        /// <summary>
        /// 调拨类型编码
        /// </summary>
        public String TB_TransferTypeCode { get; set; }
        /// <summary>
        /// 调拨类型名称
        /// </summary>
        public String TB_TransferTypeName { get; set; }
        /// <summary>
        /// 调出组织ID
        /// </summary>
        public String TB_TransferOutOrgId { get; set; }
        /// <summary>
        /// 调出组织名称
        /// </summary>
        public String TB_TransferOutOrgName { get; set; }
        /// <summary>
        /// 调入组织ID
        /// </summary>
        public String TB_TransferInOrgId { get; set; }
        /// <summary>
        /// 调入组织名称
        /// </summary>
        public String TB_TransferInOrgName { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String TB_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String TB_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String TB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String TB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String TB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? TB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String TB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? TB_CreatedTime { get; set; }
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
        public String TB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? TB_UpdatedTime { get; set; }
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
        public Int64? TB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String TB_TransID { get; set; }
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
        /// 调拨单ID
        /// </summary>
        public String WHERE_TB_ID { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public String WHERE_TB_No { get; set; }
        /// <summary>
        /// 单据类型编码
        /// </summary>
        public String WHERE_TB_TypeCode { get; set; }
        /// <summary>
        /// 单据类型名称
        /// </summary>
        public String WHERE_TB_TypeName { get; set; }
        /// <summary>
        /// 调拨类型编码
        /// </summary>
        public String WHERE_TB_TransferTypeCode { get; set; }
        /// <summary>
        /// 调拨类型名称
        /// </summary>
        public String WHERE_TB_TransferTypeName { get; set; }
        /// <summary>
        /// 调出组织ID
        /// </summary>
        public String WHERE_TB_TransferOutOrgId { get; set; }
        /// <summary>
        /// 调出组织名称
        /// </summary>
        public String WHERE_TB_TransferOutOrgName { get; set; }
        /// <summary>
        /// 调入组织ID
        /// </summary>
        public String WHERE_TB_TransferInOrgId { get; set; }
        /// <summary>
        /// 调入组织名称
        /// </summary>
        public String WHERE_TB_TransferInOrgName { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String WHERE_TB_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_TB_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_TB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_TB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_TB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_TB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_TB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_TB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_TB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_TB_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_TB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_TB_TransID { get; set; }
        #endregion

    }
}
