using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 中国省份Model
    /// </summary>
    public class MDLSM_ChineseProvince
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String Prov_ID { get; set; }
        /// <summary>
        /// 大区ID
        /// </summary>
        public String Prov_Reg_ID { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String Prov_Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String Prov_Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public String Prov_ShortName { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? Prov_Index { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? Prov_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String Prov_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Prov_CreatedTime { get; set; }
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
        public String Prov_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Prov_UpdatedTime { get; set; }
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
        public Int64? Prov_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String Prov_TransID { get; set; }
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
        public String WHERE_Prov_ID { get; set; }
        /// <summary>
        /// 大区ID
        /// </summary>
        public String WHERE_Prov_Reg_ID { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String WHERE_Prov_Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String WHERE_Prov_Name { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public String WHERE_Prov_ShortName { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? WHERE_Prov_Index { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_Prov_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_Prov_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_Prov_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_Prov_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_Prov_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_Prov_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_Prov_TransID { get; set; }
        #endregion

    }
}
