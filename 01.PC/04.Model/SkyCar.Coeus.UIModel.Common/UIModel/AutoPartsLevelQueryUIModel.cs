using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 配件级别查询UIModel
    /// </summary>
    public class AutoPartsLevelQueryUIModel : BaseUIModel
    {
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
        /// 修改人
        /// </summary>
        public String APL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APL_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? APL_VersionNo { get; set; }
    }
}
