using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 采购预测订单明细Model
    /// </summary>
    public class MDLPIS_PurchaseForecastOrderDetail
    {
        #region 公共属性
        /// <summary>
        /// 采购预测订单明细ID
        /// </summary>
        public String PFOD_ID { get; set; }
        /// <summary>
        /// 采购预测订单ID
        /// </summary>
        public String PFOD_PFO_ID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public String PFOD_PFO_No { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String PFOD_AutoPartsBarcode { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String PFOD_ThirdCode { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String PFOD_OEMCode { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String PFOD_AutoPartsName { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String PFOD_AutoPartsBrand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String PFOD_AutoPartsSpec { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String PFOD_AutoPartsLevel { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String PFOD_UOM { get; set; }
        /// <summary>
        /// 汽车品牌
        /// </summary>
        public String PFOD_VehicleBrand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String PFOD_VehicleInspire { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String PFOD_VehicleCapacity { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public String PFOD_VehicleYearModel { get; set; }
        /// <summary>
        /// 变速类型
        /// </summary>
        public String PFOD_VehicleGearboxType { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? PFOD_Qty { get; set; }
        /// <summary>
        /// 最后一次采购单价
        /// </summary>
        public Decimal? PFOD_LastUnitPrice { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? PFOD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String PFOD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? PFOD_CreatedTime { get; set; }
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
        public String PFOD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? PFOD_UpdatedTime { get; set; }
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
        public Int64? PFOD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String PFOD_TransID { get; set; }
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
        /// 采购预测订单明细ID
        /// </summary>
        public String WHERE_PFOD_ID { get; set; }
        /// <summary>
        /// 采购预测订单ID
        /// </summary>
        public String WHERE_PFOD_PFO_ID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public String WHERE_PFOD_PFO_No { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_PFOD_AutoPartsBarcode { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_PFOD_ThirdCode { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String WHERE_PFOD_OEMCode { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_PFOD_AutoPartsName { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String WHERE_PFOD_AutoPartsBrand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String WHERE_PFOD_AutoPartsSpec { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String WHERE_PFOD_AutoPartsLevel { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String WHERE_PFOD_UOM { get; set; }
        /// <summary>
        /// 汽车品牌
        /// </summary>
        public String WHERE_PFOD_VehicleBrand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String WHERE_PFOD_VehicleInspire { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String WHERE_PFOD_VehicleCapacity { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public String WHERE_PFOD_VehicleYearModel { get; set; }
        /// <summary>
        /// 变速类型
        /// </summary>
        public String WHERE_PFOD_VehicleGearboxType { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? WHERE_PFOD_Qty { get; set; }
        /// <summary>
        /// 最后一次采购单价
        /// </summary>
        public Decimal? WHERE_PFOD_LastUnitPrice { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_PFOD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_PFOD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_PFOD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_PFOD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_PFOD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_PFOD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_PFOD_TransID { get; set; }
        #endregion

    }
}
