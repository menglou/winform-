using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 用户菜单Model
    /// </summary>
    public class MDLSM_UserMenuAuthority
    {
        #region 公共属性
        /// <summary>
        /// 用户菜单权限ID
        /// </summary>
        public String UMA_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String UMA_User_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String UMA_Org_ID { get; set; }
        /// <summary>
        /// 菜单明细ID
        /// </summary>
        public String UMA_MenuD_ID { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? UMA_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String UMA_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? UMA_CreatedTime { get; set; }
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
        public String UMA_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UMA_UpdatedTime { get; set; }
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
        public Int64? UMA_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String UMA_TransID { get; set; }
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
        /// 用户菜单权限ID
        /// </summary>
        public String WHERE_UMA_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String WHERE_UMA_User_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_UMA_Org_ID { get; set; }
        /// <summary>
        /// 菜单明细ID
        /// </summary>
        public String WHERE_UMA_MenuD_ID { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_UMA_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_UMA_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_UMA_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_UMA_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_UMA_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_UMA_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_UMA_TransID { get; set; }
        #endregion

    }
}
