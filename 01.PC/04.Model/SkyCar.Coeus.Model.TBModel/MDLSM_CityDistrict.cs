using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 城市区域Model
    /// </summary>
    public class MDLSM_CityDistrict
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String Dist_ID { get; set; }
        /// <summary>
        /// 区域编码
        /// </summary>
        public String Dist_Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String Dist_Name { get; set; }
        /// <summary>
        /// 城市编码
        /// </summary>
        public String Dist_City_Code { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? Dist_Index { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? Dist_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String Dist_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Dist_CreatedTime { get; set; }
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
        public String Dist_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Dist_UpdatedTime { get; set; }
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
        public Int64? Dist_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String Dist_TransID { get; set; }
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
        /// ID
        /// </summary>
        public String WHERE_Dist_ID { get; set; }
        /// <summary>
        /// 区域编码
        /// </summary>
        public String WHERE_Dist_Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String WHERE_Dist_Name { get; set; }
        /// <summary>
        /// 城市编码
        /// </summary>
        public String WHERE_Dist_City_Code { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? WHERE_Dist_Index { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_Dist_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_Dist_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_Dist_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_Dist_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_Dist_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_Dist_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_Dist_TransID { get; set; }
        #endregion

    }
}
