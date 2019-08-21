using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 应付单日志Model
    /// </summary>
    public class MDLFM_AccountPayableBillLog
    {
        #region 公共属性
        /// <summary>
        /// 应付单日志ID
        /// </summary>
        public String APBL_ID { get; set; }
        /// <summary>
        /// 应付单ID
        /// </summary>
        public String APBL_APB_ID { get; set; }
        /// <summary>
        /// 应付单明细ID
        /// </summary>
        public String APBL_APBD_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String APBL_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String APBL_Org_Name { get; set; }
        /// <summary>
        /// 操作类型编码
        /// </summary>
        public String APBL_OperateTypeCode { get; set; }
        /// <summary>
        /// 操作类型名称
        /// </summary>
        public String APBL_OperateTypeName { get; set; }
        /// <summary>
        /// 应付单明细版本号
        /// </summary>
        public Int64? APBL_APBD_VersionNo { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? APBL_APAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public Decimal? APBL_PaidAmount { get; set; }
        /// <summary>
        /// 未付金额
        /// </summary>
        public Decimal? APBL_UnpaidAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String APBL_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APBL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APBL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APBL_CreatedTime { get; set; }
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
        public String APBL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APBL_UpdatedTime { get; set; }
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
        public Int64? APBL_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String APBL_TransID { get; set; }
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
        /// 应付单日志ID
        /// </summary>
        public String WHERE_APBL_ID { get; set; }
        /// <summary>
        /// 应付单ID
        /// </summary>
        public String WHERE_APBL_APB_ID { get; set; }
        /// <summary>
        /// 应付单明细ID
        /// </summary>
        public String WHERE_APBL_APBD_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_APBL_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_APBL_Org_Name { get; set; }
        /// <summary>
        /// 操作类型编码
        /// </summary>
        public String WHERE_APBL_OperateTypeCode { get; set; }
        /// <summary>
        /// 操作类型名称
        /// </summary>
        public String WHERE_APBL_OperateTypeName { get; set; }
        /// <summary>
        /// 应付单明细版本号
        /// </summary>
        public Int64? WHERE_APBL_APBD_VersionNo { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? WHERE_APBL_APAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public Decimal? WHERE_APBL_PaidAmount { get; set; }
        /// <summary>
        /// 未付金额
        /// </summary>
        public Decimal? WHERE_APBL_UnpaidAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_APBL_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_APBL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_APBL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_APBL_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_APBL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_APBL_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_APBL_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_APBL_TransID { get; set; }
        #endregion

    }
}
