using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 库存查询明细UIModel
    /// </summary>
    public class InventoryQueryDetailUIModel : BaseNotificationUIModel
    {
        #region 库存
        /// <summary>
        /// ID
        /// </summary>
        public String INV_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String INV_Org_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String INV_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String INV_WHB_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String Org_ShortName { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public String WH_Name { get; set; }
        /// <summary>
        /// 仓位
        /// </summary>
        public String WHB_Name { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String INV_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String INV_OEMNo { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String INV_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String INV_BatchNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String INV_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String INV_Specification { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String INV_SUPP_ID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String SUPP_Name { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? INV_Qty { get; set; }
        /// <summary>
        /// 采购单价
        /// </summary>
        public Decimal? INV_PurchaseUnitPrice { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? INV_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String INV_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? INV_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String INV_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? INV_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? INV_VersionNo { get; set; }

        #endregion

        #region 配件档案字段
        /// <summary>
        /// 计量单位
        /// </summary>
        public String APA_UOM { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String APA_Level { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
        /// <summary>
        /// 汽车品牌
        /// </summary>
        public String APA_VehicleBrand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String APA_VehicleInspire { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String APA_VehicleCapacity { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public String APA_VehicleYearModel { get; set; }
        /// <summary>
        /// 变速类型名称
        /// </summary>
        public String APA_VehicleGearboxTypeName { get; set; }
        /// <summary>
        /// 安全库存是否预警
        /// </summary>
        public Boolean APA_IsWarningSafeStock { get; set; }
        /// <summary>
        /// 安全库存
        /// </summary>
        public int APA_SafeStockNum { get; set; }
        /// <summary>
        /// 呆滞件是否预警
        /// </summary>
        public Boolean APA_IsWarningDeadStock { get; set; }
        /// <summary>
        /// 呆滞天数
        /// </summary>
        public int APA_SlackDays { get; set; }
        /// <summary>
        /// 销价系数
        /// </summary>
        public Decimal APA_SalePriceRate { get; set; }
        /// <summary>
        /// 销价
        /// </summary>
        public Decimal APA_SalePrice { get; set; }
        /// <summary>
        /// 车型代码
        /// </summary>
        public String APA_VehicleModelCode { get; set; }
        /// <summary>
        /// 互换码
        /// </summary>
        public String APA_ExchangeCode { get; set; }
        #endregion

        #region 其他属性

        private Boolean _ischecked;
        /// <summary>
        /// 选择
        /// </summary>
        public Boolean IsChecked
        {
            get { return _ischecked; }
            set
            {
                _ischecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }
        private int _printcount;
        /// <summary>
        /// 打印次数
        /// </summary>
        public int PrintCount
        {
            get { return _printcount; }
            set
            {
                _printcount = value;
                RaisePropertyChanged(() => PrintCount);
            }
        }
        #endregion
    }
}
