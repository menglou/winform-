using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 应收单日志Model
    /// </summary>
    public class MDLFM_AccountReceivableBillLog
    {
        #region 公共属性
        /// <summary>
        /// 应收单日志ID
        /// </summary>
        public String ARBL_ID { get; set; }
        /// <summary>
        /// 应收单ID
        /// </summary>
        public String ARBL_ARB_ID { get; set; }
        /// <summary>
        /// 应收单明细ID
        /// </summary>
        public String ARBL_ARBD_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String ARBL_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String ARBL_Org_Name { get; set; }
        /// <summary>
        /// 操作类型编码
        /// </summary>
        public String ARBL_OperateTypeCode { get; set; }
        /// <summary>
        /// 操作类型名称
        /// </summary>
        public String ARBL_OperateTypeName { get; set; }
        /// <summary>
        /// 应收单明细版本号
        /// </summary>
        public Int64? ARBL_APBD_VersionNo { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? ARBL_ARAmount { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public Decimal? ARBL_ReceivedAmount { get; set; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public Decimal? ARBL_UnReceiveAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String ARBL_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? ARBL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String ARBL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ARBL_CreatedTime { get; set; }
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
        public String ARBL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ARBL_UpdatedTime { get; set; }
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
        public Int64? ARBL_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String ARBL_TransID { get; set; }
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
        /// 应收单日志ID
        /// </summary>
        public String WHERE_ARBL_ID { get; set; }
        /// <summary>
        /// 应收单ID
        /// </summary>
        public String WHERE_ARBL_ARB_ID { get; set; }
        /// <summary>
        /// 应收单明细ID
        /// </summary>
        public String WHERE_ARBL_ARBD_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_ARBL_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_ARBL_Org_Name { get; set; }
        /// <summary>
        /// 操作类型编码
        /// </summary>
        public String WHERE_ARBL_OperateTypeCode { get; set; }
        /// <summary>
        /// 操作类型名称
        /// </summary>
        public String WHERE_ARBL_OperateTypeName { get; set; }
        /// <summary>
        /// 应收单明细版本号
        /// </summary>
        public Int64? WHERE_ARBL_APBD_VersionNo { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? WHERE_ARBL_ARAmount { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public Decimal? WHERE_ARBL_ReceivedAmount { get; set; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public Decimal? WHERE_ARBL_UnReceiveAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_ARBL_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_ARBL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_ARBL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_ARBL_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_ARBL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_ARBL_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_ARBL_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_ARBL_TransID { get; set; }
        #endregion

    }
}
