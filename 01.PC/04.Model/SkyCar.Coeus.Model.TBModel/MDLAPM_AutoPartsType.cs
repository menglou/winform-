using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 配件类别Model
    /// </summary>
    public class MDLAPM_AutoPartsType
    {
        #region 公共属性
        /// <summary>
        /// 配件类别ID
        /// </summary>
        public String APT_ID { get; set; }
        /// <summary>
        /// 配件类别名称
        /// </summary>
        public String APT_Name { get; set; }
        /// <summary>
        /// 父级类别ID
        /// </summary>
        public String APT_ParentID { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APT_CreatedTime { get; set; }
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
        public String APT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APT_UpdatedTime { get; set; }
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
        public Int64? APT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String APT_TransID { get; set; }
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
        /// 配件类别ID
        /// </summary>
        public String WHERE_APT_ID { get; set; }
        /// <summary>
        /// 配件类别名称
        /// </summary>
        public String WHERE_APT_Name { get; set; }
        /// <summary>
        /// 父级类别ID
        /// </summary>
        public String WHERE_APT_ParentID { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_APT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_APT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_APT_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_APT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_APT_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_APT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_APT_TransID { get; set; }
        #endregion

    }
}
