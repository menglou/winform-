using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 微信菜单Model
    /// </summary>
    public class MDLWC_Menu
    {
        #region 公共属性
        /// <summary>
        /// 菜单ID
        /// </summary>
        public String WM_ID { get; set; }
        /// <summary>
        /// 平台编码
        /// </summary>
        public String WM_PlatformCode { get; set; }
        /// <summary>
        /// 父菜单
        /// </summary>
        public String WM_ParentID { get; set; }
        /// <summary>
        /// 是否叶节点
        /// </summary>
        public Boolean? WM_Isleff { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public String WM_Name { get; set; }
        /// <summary>
        /// 菜单类型
        /// </summary>
        public String WM_Type { get; set; }
        /// <summary>
        /// 菜单Key
        /// </summary>
        public String WM_Key { get; set; }
        /// <summary>
        /// 菜单URL
        /// </summary>
        public String WM_URL { get; set; }
        /// <summary>
        /// MediaID
        /// </summary>
        public String WM_MediaID { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public Int32? WM_Index { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WM_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WM_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WM_CreatedTime { get; set; }
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
        public String WM_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WM_UpdatedTime { get; set; }
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
        public Int64? WM_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WM_TransID { get; set; }
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
        /// 菜单ID
        /// </summary>
        public String WHERE_WM_ID { get; set; }
        /// <summary>
        /// 平台编码
        /// </summary>
        public String WHERE_WM_PlatformCode { get; set; }
        /// <summary>
        /// 父菜单
        /// </summary>
        public String WHERE_WM_ParentID { get; set; }
        /// <summary>
        /// 是否叶节点
        /// </summary>
        public Boolean? WHERE_WM_Isleff { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public String WHERE_WM_Name { get; set; }
        /// <summary>
        /// 菜单类型
        /// </summary>
        public String WHERE_WM_Type { get; set; }
        /// <summary>
        /// 菜单Key
        /// </summary>
        public String WHERE_WM_Key { get; set; }
        /// <summary>
        /// 菜单URL
        /// </summary>
        public String WHERE_WM_URL { get; set; }
        /// <summary>
        /// MediaID
        /// </summary>
        public String WHERE_WM_MediaID { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public Int32? WHERE_WM_Index { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_WM_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_WM_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_WM_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_WM_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_WM_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_WM_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_WM_TransID { get; set; }
        #endregion

    }
}
