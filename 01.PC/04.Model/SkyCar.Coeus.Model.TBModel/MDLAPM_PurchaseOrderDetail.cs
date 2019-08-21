using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// Venus配件采购订单明细Model
    /// </summary>
    public class MDLAPM_PurchaseOrderDetail
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String POD_ID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public String POD_PO_No { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String POD_AutoPartsBarcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String POD_AutoPartsBatchNo { get; set; }
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
        /// 汽配商户编码
        /// </summary>
        public String POD_SUPP_MerchantCode { get; set; }
        /// <summary>
        /// 汽配商户名称
        /// </summary>
        public String POD_SUPP_MerchantName { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String POD_SUPP_ID { get; set; }
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
        public Decimal? POD_OrderQuantity { get; set; }
        /// <summary>
        /// 签收数量
        /// </summary>
        public Decimal? POD_ReceivedQuantity { get; set; }
        /// <summary>
        /// 订货单价
        /// </summary>
        public Decimal? POD_UnitPrice { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public String POD_Status { get; set; }
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
        #endregion
    }
}
