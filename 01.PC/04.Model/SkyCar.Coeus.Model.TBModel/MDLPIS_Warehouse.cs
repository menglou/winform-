using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 仓库Model
    /// </summary>
    public class MDLPIS_Warehouse
    {
        #region 公共属性
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WH_ID { get; set; }
        /// <summary>
        /// 仓库编号
        /// </summary>
        public String WH_No { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WH_Name { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WH_Org_ID { get; set; }
        /// <summary>
        /// 仓库地址
        /// </summary>
        public String WH_Address { get; set; }
        /// <summary>
        /// 仓库描述
        /// </summary>
        public String WH_Description { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WH_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WH_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WH_CreatedTime { get; set; }
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
        public String WH_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WH_UpdatedTime { get; set; }
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
        public Int64? WH_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WH_TransID { get; set; }
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
        /// 仓库ID
        /// </summary>
        public String WHERE_WH_ID { get; set; }
        /// <summary>
        /// 仓库编号
        /// </summary>
        public String WHERE_WH_No { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WHERE_WH_Name { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_WH_Org_ID { get; set; }
        /// <summary>
        /// 仓库地址
        /// </summary>
        public String WHERE_WH_Address { get; set; }
        /// <summary>
        /// 仓库描述
        /// </summary>
        public String WHERE_WH_Description { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_WH_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_WH_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_WH_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_WH_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_WH_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_WH_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_WH_TransID { get; set; }
        #endregion

    }
}
