using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 车辆品牌车系Model
    /// </summary>
    public class MDLBS_VehicleBrandInspireSumma
    {
        #region 公共属性
        /// <summary>
        /// 品牌车系ID
        /// </summary>
        public String VBIS_ID { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public String VBIS_Brand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String VBIS_Inspire { get; set; }
        /// <summary>
        /// 车型描述
        /// </summary>
        public String VBIS_ModelDesc { get; set; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        public String VBIS_Model { get; set; }
        /// <summary>
        /// 品牌拼音首字母
        /// </summary>
        public String VBIS_BrandSpellCode { get; set; }
        /// <summary>
        /// 车系拼音首字母
        /// </summary>
        public String VBIS_InspireSpellCode { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? VBIS_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String VBIS_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? VBIS_CreatedTime { get; set; }
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
        public String VBIS_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? VBIS_UpdatedTime { get; set; }
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
        public Int64? VBIS_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String VBIS_TransID { get; set; }
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
        /// 品牌车系ID
        /// </summary>
        public String WHERE_VBIS_ID { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public String WHERE_VBIS_Brand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String WHERE_VBIS_Inspire { get; set; }
        /// <summary>
        /// 车型描述
        /// </summary>
        public String WHERE_VBIS_ModelDesc { get; set; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        public String WHERE_VBIS_Model { get; set; }
        /// <summary>
        /// 品牌拼音首字母
        /// </summary>
        public String WHERE_VBIS_BrandSpellCode { get; set; }
        /// <summary>
        /// 车系拼音首字母
        /// </summary>
        public String WHERE_VBIS_InspireSpellCode { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_VBIS_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_VBIS_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_VBIS_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_VBIS_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_VBIS_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_VBIS_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_VBIS_TransID { get; set; }
        #endregion

    }
}
