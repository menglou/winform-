using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 菜单Model
    /// </summary>
    public class MDLSM_Menu
    {
        #region 公共属性
        /// <summary>
        /// 菜单ID
        /// </summary>
        public String Menu_ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String Menu_Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String Menu_Remark { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String Menu_Code { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? Menu_Index { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public Boolean? Menu_IsVisible { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? Menu_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String Menu_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Menu_CreatedTime { get; set; }
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
        public String Menu_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Menu_UpdatedTime { get; set; }
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
        public Int64? Menu_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String Menu_TransID { get; set; }
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
        public String WHERE_Menu_ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String WHERE_Menu_Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_Menu_Remark { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String WHERE_Menu_Code { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? WHERE_Menu_Index { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        public Boolean? WHERE_Menu_IsVisible { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_Menu_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_Menu_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_Menu_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_Menu_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_Menu_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_Menu_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_Menu_TransID { get; set; }
        #endregion

    }
}
