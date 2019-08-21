using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 仓位Model
    /// </summary>
    public class MDLPIS_WarehouseBin
    {
        #region 公共属性
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String WHB_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHB_WH_ID { get; set; }
        /// <summary>
        /// 仓位名称
        /// </summary>
        public String WHB_Name { get; set; }
        /// <summary>
        /// 仓位描述
        /// </summary>
        public String WHB_Description { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHB_CreatedTime { get; set; }
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
        public String WHB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHB_UpdatedTime { get; set; }
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
        public Int64? WHB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHB_TransID { get; set; }
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
        /// 仓位ID
        /// </summary>
        public String WHERE_WHB_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHERE_WHB_WH_ID { get; set; }
        /// <summary>
        /// 仓位名称
        /// </summary>
        public String WHERE_WHB_Name { get; set; }
        /// <summary>
        /// 仓位描述
        /// </summary>
        public String WHERE_WHB_Description { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_WHB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_WHB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_WHB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_WHB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_WHB_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_WHB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_WHB_TransID { get; set; }
        #endregion

    }
}
