using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 车辆品牌件信息Model
    /// </summary>
    public class MDLBS_VehicleThirdPartsInfo
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String VTPI_ID { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public String VTPI_VC_VIN { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String VTPI_ThirdNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String VTPI_AutoPartsName { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String VTPI_AutoPartsBrand { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String VTPI_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? VTPI_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String VTPI_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? VTPI_CreatedTime { get; set; }
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
        public String VTPI_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? VTPI_UpdatedTime { get; set; }
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
        public Int64? VTPI_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String VTPI_TransID { get; set; }
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
        public String WHERE_VTPI_ID { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public String WHERE_VTPI_VC_VIN { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_VTPI_ThirdNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_VTPI_AutoPartsName { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String WHERE_VTPI_AutoPartsBrand { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_VTPI_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_VTPI_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_VTPI_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_VTPI_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_VTPI_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_VTPI_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_VTPI_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_VTPI_TransID { get; set; }
        #endregion

    }
}
