using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 采购订单明细Model
    /// </summary>
    public class MDLPIS_PurchaseOrderDetail
    {
        #region 公共属性
        /// <summary>
        /// 采购订单明细ID
        /// </summary>
        public String POD_ID { get; set; }
        /// <summary>
        /// 采购订单ID
        /// </summary>
        public String POD_PO_ID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public String POD_PO_No { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String POD_AutoPartsBarcode { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String POD_ThirdCode { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String POD_OEMCode { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String POD_AutoPartsName { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String POD_AutoPartsBrand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String POD_AutoPartsSpec { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String POD_AutoPartsLevel { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String POD_UOM { get; set; }
        /// <summary>
        /// 汽车品牌
        /// </summary>
        public String POD_VehicleBrand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String POD_VehicleInspire { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String POD_VehicleCapacity { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public String POD_VehicleYearModel { get; set; }
        /// <summary>
        /// 变速类型
        /// </summary>
        public String POD_VehicleGearboxType { get; set; }
        /// <summary>
        /// 进货仓库ID
        /// </summary>
        public String POD_WH_ID { get; set; }
        /// <summary>
        /// 进货仓位ID
        /// </summary>
        public String POD_WHB_ID { get; set; }
        /// <summary>
        /// 订货数量
        /// </summary>
        public Decimal? POD_OrderQty { get; set; }
        /// <summary>
        /// 签收数量
        /// </summary>
        public Decimal? POD_ReceivedQty { get; set; }
        /// <summary>
        /// 订货单价
        /// </summary>
        public Decimal? POD_UnitPrice { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String POD_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String POD_StatusName { get; set; }
        /// <summary>
        /// 到货时间
        /// </summary>
        public DateTime? POD_ReceivedTime { get; set; }
        /// <summary>
        /// 到货时间-开始（查询条件用）
        /// </summary>
        public DateTime? _ReceivedTimeStart { get; set; }
        /// <summary>
        /// 到货时间-终了（查询条件用）
        /// </summary>
        public DateTime? _ReceivedTimeEnd { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? POD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String POD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? POD_CreatedTime { get; set; }
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
        public String POD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? POD_UpdatedTime { get; set; }
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
        public Int64? POD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String POD_TransID { get; set; }
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
        /// 采购订单明细ID
        /// </summary>
        public String WHERE_POD_ID { get; set; }
        /// <summary>
        /// 采购订单ID
        /// </summary>
        public String WHERE_POD_PO_ID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public String WHERE_POD_PO_No { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_POD_AutoPartsBarcode { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_POD_ThirdCode { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String WHERE_POD_OEMCode { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_POD_AutoPartsName { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String WHERE_POD_AutoPartsBrand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String WHERE_POD_AutoPartsSpec { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String WHERE_POD_AutoPartsLevel { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String WHERE_POD_UOM { get; set; }
        /// <summary>
        /// 汽车品牌
        /// </summary>
        public String WHERE_POD_VehicleBrand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String WHERE_POD_VehicleInspire { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String WHERE_POD_VehicleCapacity { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public String WHERE_POD_VehicleYearModel { get; set; }
        /// <summary>
        /// 变速类型
        /// </summary>
        public String WHERE_POD_VehicleGearboxType { get; set; }
        /// <summary>
        /// 进货仓库ID
        /// </summary>
        public String WHERE_POD_WH_ID { get; set; }
        /// <summary>
        /// 进货仓位ID
        /// </summary>
        public String WHERE_POD_WHB_ID { get; set; }
        /// <summary>
        /// 订货数量
        /// </summary>
        public Decimal? WHERE_POD_OrderQty { get; set; }
        /// <summary>
        /// 签收数量
        /// </summary>
        public Decimal? WHERE_POD_ReceivedQty { get; set; }
        /// <summary>
        /// 订货单价
        /// </summary>
        public Decimal? WHERE_POD_UnitPrice { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String WHERE_POD_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_POD_StatusName { get; set; }
        /// <summary>
        /// 到货时间
        /// </summary>
        public DateTime? WHERE_POD_ReceivedTime { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_POD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_POD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_POD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_POD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_POD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_POD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_POD_TransID { get; set; }
        #endregion

    }
}
