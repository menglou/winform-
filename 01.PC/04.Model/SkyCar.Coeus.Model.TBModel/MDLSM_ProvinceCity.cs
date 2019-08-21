using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 省份城市Model
    /// </summary>
    public class MDLSM_ProvinceCity
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String City_ID { get; set; }
        /// <summary>
        /// 大区ID
        /// </summary>
        public String City_Reg_ID { get; set; }
        /// <summary>
        /// 城市编码
        /// </summary>
        public String City_Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String City_Name { get; set; }
        /// <summary>
        /// 车牌编码
        /// </summary>
        public String City_PlateCode { get; set; }
        /// <summary>
        /// 省份编码
        /// </summary>
        public String City_Prov_Code { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? City_Index { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? City_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String City_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? City_CreatedTime { get; set; }
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
        public String City_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? City_UpdatedTime { get; set; }
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
        public Int64? City_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String City_TransID { get; set; }
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
        public String WHERE_City_ID { get; set; }
        /// <summary>
        /// 大区ID
        /// </summary>
        public String WHERE_City_Reg_ID { get; set; }
        /// <summary>
        /// 城市编码
        /// </summary>
        public String WHERE_City_Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String WHERE_City_Name { get; set; }
        /// <summary>
        /// 车牌编码
        /// </summary>
        public String WHERE_City_PlateCode { get; set; }
        /// <summary>
        /// 省份编码
        /// </summary>
        public String WHERE_City_Prov_Code { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? WHERE_City_Index { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_City_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_City_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_City_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_City_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_City_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_City_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_City_TransID { get; set; }
        #endregion

    }
}
