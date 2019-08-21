using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 仓库查询QCModel
    /// </summary>
    public class WarehouseQueryQCModel : BaseQCModel
    {
        #region 公共属性-条件用
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHERE_WH_ID { get; set; }
        /// <summary>
        /// 仓库编号
        /// </summary>
        public String WHERE_WH_No { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WHERE_WH_Name { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_WH_Org_ID { get; set; }
        /// <summary>
        /// 仓库地址
        /// </summary>
        public String WHERE_WH_Address { get; set; }
        /// <summary>
        /// 仓库描述
        /// </summary>
        public String WHERE_WH_Description { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_WH_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_WH_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_WH_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_WH_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_WH_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_WH_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_WH_TransID { get; set; }
        #endregion
        
        /// <summary>
        /// 组织简称
        /// </summary>
        public String WHERE_Org_ShortName { get; set; }
    }
}
