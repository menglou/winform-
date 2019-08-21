using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 车辆信息Model
    /// </summary>
    public class MDLBS_VehicleInfo
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String VC_ID { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public String VC_VIN { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public String VC_PlateNumber { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public String VC_Brand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String VC_Inspire { get; set; }
        /// <summary>
        /// 车型描述
        /// </summary>
        public String VC_BrandDesc { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String VC_Capacity { get; set; }
        /// <summary>
        /// 发动机型号
        /// </summary>
        public String VC_EngineType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String VC_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? VC_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String VC_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? VC_CreatedTime { get; set; }
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
        public String VC_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? VC_UpdatedTime { get; set; }
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
        public Int64? VC_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String VC_TransID { get; set; }
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
        public String WHERE_VC_ID { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public String WHERE_VC_VIN { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public String WHERE_VC_PlateNumber { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public String WHERE_VC_Brand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String WHERE_VC_Inspire { get; set; }
        /// <summary>
        /// 车型描述
        /// </summary>
        public String WHERE_VC_BrandDesc { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String WHERE_VC_Capacity { get; set; }
        /// <summary>
        /// 发动机型号
        /// </summary>
        public String WHERE_VC_EngineType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_VC_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_VC_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_VC_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_VC_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_VC_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_VC_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_VC_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_VC_TransID { get; set; }
        #endregion

    }
}
