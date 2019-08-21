using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 仓位查询QCModel
    /// </summary>
    public class WarehouseBinQueryQCModel : BaseQCModel
    {
        #region 公共属性-条件用
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String WHERE_WHB_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHERE_WHB_WH_ID { get; set; }
        /// <summary>
        /// 仓位名称
        /// </summary>
        public String WHERE_WHB_Name { get; set; }
        /// <summary>
        /// 仓位描述
        /// </summary>
        public String WHERE_WHB_Description { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_WHB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_WHB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_WHB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_WHB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_WHB_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_WHB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_WHB_TransID { get; set; }
        #endregion

        #region 其他

        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WHERE_WH_Name { get; set; }
        #endregion
    }
}
