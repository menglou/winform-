using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 下发路径Model
    /// </summary>
    public class MDLSD_DistributePath
    {
        #region 公共属性
        /// <summary>
        /// 下发路径ID
        /// </summary>
        public String DP_ID { get; set; }
        /// <summary>
        /// 来源组织
        /// </summary>
        public String DP_Org_ID_From { get; set; }
        /// <summary>
        /// 目标组织
        /// </summary>
        public String DP_Org_ID_To { get; set; }
        /// <summary>
        /// 下发人
        /// </summary>
        public String DP_SendPerson { get; set; }
        /// <summary>
        /// 下发数据ID
        /// </summary>
        public String DP_SendDataID { get; set; }
        /// <summary>
        /// 下发数据类型编码
        /// </summary>
        public String DP_SendDataTypeCode { get; set; }
        /// <summary>
        /// 下发数据类型名称
        /// </summary>
        public String DP_SendDataTypeName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String DP_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? DP_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String DP_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? DP_CreatedTime { get; set; }
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
        public String DP_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? DP_UpdatedTime { get; set; }
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
        public Int64? DP_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String DP_TransID { get; set; }
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
        /// 下发路径ID
        /// </summary>
        public String WHERE_DP_ID { get; set; }
        /// <summary>
        /// 来源组织
        /// </summary>
        public String WHERE_DP_Org_ID_From { get; set; }
        /// <summary>
        /// 目标组织
        /// </summary>
        public String WHERE_DP_Org_ID_To { get; set; }
        /// <summary>
        /// 下发人
        /// </summary>
        public String WHERE_DP_SendPerson { get; set; }
        /// <summary>
        /// 下发数据ID
        /// </summary>
        public String WHERE_DP_SendDataID { get; set; }
        /// <summary>
        /// 下发数据类型编码
        /// </summary>
        public String WHERE_DP_SendDataTypeCode { get; set; }
        /// <summary>
        /// 下发数据类型名称
        /// </summary>
        public String WHERE_DP_SendDataTypeName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_DP_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_DP_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_DP_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_DP_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_DP_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_DP_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_DP_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_DP_TransID { get; set; }
        #endregion

    }
}
