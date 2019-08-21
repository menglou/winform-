using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 用户作业权限Model
    /// </summary>
    public class MDLSM_UserJobAuthority
    {
        #region 公共属性
        /// <summary>
        /// 用户作业权限ID
        /// </summary>
        public String UJA_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String UJA_User_ID { get; set; }
        /// <summary>
        /// 组织
        /// </summary>
        public String UJA_Org_ID { get; set; }
        /// <summary>
        /// 作业ID
        /// </summary>
        public String UJA_BJ_ID { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? UJA_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String UJA_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? UJA_CreatedTime { get; set; }
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
        public String UJA_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UJA_UpdatedTime { get; set; }
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
        public Int64? UJA_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String UJA_TransID { get; set; }
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
        /// 用户作业权限ID
        /// </summary>
        public String WHERE_UJA_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String WHERE_UJA_User_ID { get; set; }
        /// <summary>
        /// 组织
        /// </summary>
        public String WHERE_UJA_Org_ID { get; set; }
        /// <summary>
        /// 作业ID
        /// </summary>
        public String WHERE_UJA_BJ_ID { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_UJA_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_UJA_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_UJA_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_UJA_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_UJA_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_UJA_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_UJA_TransID { get; set; }
        #endregion

    }
}
