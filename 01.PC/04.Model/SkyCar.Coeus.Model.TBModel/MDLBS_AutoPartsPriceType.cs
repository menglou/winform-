using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 配件价格类别Model
    /// </summary>
    public class MDLBS_AutoPartsPriceType
    {
        #region 公共属性
        /// <summary>
        /// 配件价格类别ID
        /// </summary>
        public String APPT_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String APPT_Org_ID { get; set; }
        /// <summary>
        /// 配件价格类别名称
        /// </summary>
        public String APPT_Name { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public String APPT_Barcode { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public Decimal? APPT_Price { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APPT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APPT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APPT_CreatedTime { get; set; }
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
        public String APPT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APPT_UpdatedTime { get; set; }
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
        public Int64? APPT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String APPT_TransID { get; set; }
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
        /// 配件价格类别ID
        /// </summary>
        public String WHERE_APPT_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_APPT_Org_ID { get; set; }
        /// <summary>
        /// 配件价格类别名称
        /// </summary>
        public String WHERE_APPT_Name { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public String WHERE_APPT_Barcode { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public Decimal? WHERE_APPT_Price { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_APPT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_APPT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_APPT_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_APPT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_APPT_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_APPT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_APPT_TransID { get; set; }
        #endregion

    }
}
