using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 菜单明细Model
    /// </summary>
    public class MDLSM_MenuDetail
    {
        #region 公共属性
        /// <summary>
        /// 菜单明细ID
        /// </summary>
        public String MenuD_ID { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public String MenuD_Menu_ID { get; set; }
        /// <summary>
        /// 菜单分组ID
        /// </summary>
        public String MenuD_MenuG_ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String MenuD_Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String MenuD_Remark { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String MenuD_Code { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public String MenuD_Picture { get; set; }
        /// <summary>
        /// 图标Key
        /// </summary>
        public String MenuD_ImgListKey { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? MenuD_Index { get; set; }
        /// <summary>
        /// 类全名
        /// </summary>
        public String MenuD_ClassFullName { get; set; }
        /// <summary>
        /// URI
        /// </summary>
        public String MenuD_URI { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public Boolean? MenuD_IsVisible { get; set; }
        /// <summary>
        /// Grid页面大小
        /// </summary>
        public Int32? MenuD_GridPageSize { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? MenuD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String MenuD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? MenuD_CreatedTime { get; set; }
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
        public String MenuD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? MenuD_UpdatedTime { get; set; }
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
        public Int64? MenuD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String MenuD_TransID { get; set; }
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
        /// 菜单明细ID
        /// </summary>
        public String WHERE_MenuD_ID { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public String WHERE_MenuD_Menu_ID { get; set; }
        /// <summary>
        /// 菜单分组ID
        /// </summary>
        public String WHERE_MenuD_MenuG_ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String WHERE_MenuD_Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_MenuD_Remark { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String WHERE_MenuD_Code { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public String WHERE_MenuD_Picture { get; set; }
        /// <summary>
        /// 图标Key
        /// </summary>
        public String WHERE_MenuD_ImgListKey { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? WHERE_MenuD_Index { get; set; }
        /// <summary>
        /// 类全名
        /// </summary>
        public String WHERE_MenuD_ClassFullName { get; set; }
        /// <summary>
        /// URI
        /// </summary>
        public String WHERE_MenuD_URI { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public Boolean? WHERE_MenuD_IsVisible { get; set; }
        /// <summary>
        /// Grid页面大小
        /// </summary>
        public Int32? WHERE_MenuD_GridPageSize { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_MenuD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_MenuD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_MenuD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_MenuD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_MenuD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_MenuD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_MenuD_TransID { get; set; }
        #endregion

    }
}
