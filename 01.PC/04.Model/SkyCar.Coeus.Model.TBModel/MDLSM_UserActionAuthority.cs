using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 用户菜单动作Model
    /// </summary>
    public class MDLSM_UserActionAuthority
    {
        #region 公共属性
        /// <summary>
        /// 用户菜单动作权限ID
        /// </summary>
        public String UAA_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String UAA_User_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String UAA_Org_ID { get; set; }
        /// <summary>
        /// 菜单明细ID
        /// </summary>
        public String UAA_MenuD_ID { get; set; }
        /// <summary>
        /// 动作ID
        /// </summary>
        public String UAA_Action_ID { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? UAA_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String UAA_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? UAA_CreatedTime { get; set; }
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
        public String UAA_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UAA_UpdatedTime { get; set; }
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
        public Int64? UAA_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String UAA_TransID { get; set; }
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
        /// 用户菜单动作权限ID
        /// </summary>
        public String WHERE_UAA_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String WHERE_UAA_User_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_UAA_Org_ID { get; set; }
        /// <summary>
        /// 菜单明细ID
        /// </summary>
        public String WHERE_UAA_MenuD_ID { get; set; }
        /// <summary>
        /// 动作ID
        /// </summary>
        public String WHERE_UAA_Action_ID { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_UAA_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_UAA_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_UAA_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_UAA_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_UAA_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_UAA_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_UAA_TransID { get; set; }
        #endregion

    }
}
