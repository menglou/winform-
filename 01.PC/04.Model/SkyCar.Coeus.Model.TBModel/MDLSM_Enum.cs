using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 系统枚举Model
    /// </summary>
    public class MDLSM_Enum
    {
        #region 公共属性
        /// <summary>
        /// 枚举ID
        /// </summary>
        public String Enum_ID { get; set; }
        /// <summary>
        /// 枚举Key
        /// </summary>
        public String Enum_Key { get; set; }
        /// <summary>
        /// 枚举名称
        /// </summary>
        public String Enum_Name { get; set; }
        /// <summary>
        /// 枚举值编码
        /// </summary>
        public String Enum_ValueCode { get; set; }
        /// <summary>
        /// 枚举值
        /// </summary>
        public Int32? Enum_Value { get; set; }
        /// <summary>
        /// 枚举显示名称
        /// </summary>
        public String Enum_DisplayName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String Enum_Info { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? Enum_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String Enum_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Enum_CreatedTime { get; set; }
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
        public String Enum_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Enum_UpdatedTime { get; set; }
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
        public Int64? Enum_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String Enum_TransID { get; set; }
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
        /// 枚举ID
        /// </summary>
        public String WHERE_Enum_ID { get; set; }
        /// <summary>
        /// 枚举Key
        /// </summary>
        public String WHERE_Enum_Key { get; set; }
        /// <summary>
        /// 枚举名称
        /// </summary>
        public String WHERE_Enum_Name { get; set; }
        /// <summary>
        /// 枚举值编码
        /// </summary>
        public String WHERE_Enum_ValueCode { get; set; }
        /// <summary>
        /// 枚举值
        /// </summary>
        public Int32? WHERE_Enum_Value { get; set; }
        /// <summary>
        /// 枚举显示名称
        /// </summary>
        public String WHERE_Enum_DisplayName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_Enum_Info { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_Enum_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_Enum_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_Enum_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_Enum_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_Enum_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_Enum_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_Enum_TransID { get; set; }
        #endregion

    }
}
