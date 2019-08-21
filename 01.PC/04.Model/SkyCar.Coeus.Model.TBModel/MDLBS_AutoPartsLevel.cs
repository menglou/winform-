using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 配件级别Model
    /// </summary>
    public class MDLBS_AutoPartsLevel
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String APL_ID { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String APL_Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String APL_Name { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public Int32? APL_DispayIndex { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String APL_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APL_CreatedTime { get; set; }
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
        public String APL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APL_UpdatedTime { get; set; }
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
        public Int64? APL_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String APL_TransID { get; set; }
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
        /// ID
        /// </summary>
        public String WHERE_APL_ID { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String WHERE_APL_Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String WHERE_APL_Name { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public Int32? WHERE_APL_DispayIndex { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_APL_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_APL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_APL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_APL_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_APL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_APL_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_APL_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_APL_TransID { get; set; }
        #endregion

    }
}
