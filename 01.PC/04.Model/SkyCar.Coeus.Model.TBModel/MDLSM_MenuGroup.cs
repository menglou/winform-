using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 菜单分组Model
    /// </summary>
    public class MDLSM_MenuGroup
    {
        #region 公共属性
        /// <summary>
        /// 菜单分组ID
        /// </summary>
        public String MenuG_ID { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public String MenuG_Menu_ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String MenuG_Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String MenuG_Remark { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String MenuG_Code { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? MenuG_Index { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public Boolean? MenuG_IsVisible { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? MenuG_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String MenuG_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? MenuG_CreatedTime { get; set; }
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
        public String MenuG_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? MenuG_UpdatedTime { get; set; }
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
        public Int64? MenuG_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String MenuG_TransID { get; set; }
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
        /// 菜单分组ID
        /// </summary>
        public String WHERE_MenuG_ID { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public String WHERE_MenuG_Menu_ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String WHERE_MenuG_Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_MenuG_Remark { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String WHERE_MenuG_Code { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? WHERE_MenuG_Index { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public Boolean? WHERE_MenuG_IsVisible { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_MenuG_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_MenuG_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_MenuG_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_MenuG_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_MenuG_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_MenuG_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_MenuG_TransID { get; set; }
        #endregion

    }
}
