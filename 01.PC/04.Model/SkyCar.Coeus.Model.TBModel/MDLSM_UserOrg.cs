using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 用户组织Model
    /// </summary>
    public class MDLSM_UserOrg
    {
        #region 公共属性
        /// <summary>
        /// 用户组织ID
        /// </summary>
        public String UO_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String UO_User_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String UO_Org_ID { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? UO_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String UO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? UO_CreatedTime { get; set; }
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
        public String UO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UO_UpdatedTime { get; set; }
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
        public Int64? UO_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String UO_TransID { get; set; }
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
        /// 用户组织ID
        /// </summary>
        public String WHERE_UO_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String WHERE_UO_User_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_UO_Org_ID { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_UO_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_UO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_UO_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_UO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_UO_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_UO_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_UO_TransID { get; set; }
        #endregion

    }
}
