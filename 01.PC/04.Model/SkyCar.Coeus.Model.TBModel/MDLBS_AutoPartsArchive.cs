using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 配件档案Model
    /// </summary>
    public class MDLBS_AutoPartsArchive
    {
        #region 公共属性
        /// <summary>
        /// 配置档案ID
        /// </summary>
        public String APA_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String APA_Org_ID { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public String APA_Barcode { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String APA_OEMNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String APA_ThirdNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String APA_Name { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String APA_Specification { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String APA_UOM { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String APA_Level { get; set; }
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
        /// 变速类型编码
        /// </summary>
        public String APA_VehicleGearboxTypeCode { get; set; }
        /// <summary>
        /// 变速类型名称
        /// </summary>
        public String APA_VehicleGearboxTypeName { get; set; }
        /// <summary>
        /// 默认供应商ID
        /// </summary>
        public String APA_SUPP_ID { get; set; }
        /// <summary>
        /// 默认仓库ID
        /// </summary>
        public String APA_WH_ID { get; set; }
        /// <summary>
        /// 默认仓位ID
        /// </summary>
        public String APA_WHB_ID { get; set; }
        /// <summary>
        /// 安全库存是否预警
        /// </summary>
        public Boolean? APA_IsWarningSafeStock { get; set; }
        /// <summary>
        /// 安全库存
        /// </summary>
        public Int32? APA_SafeStockNum { get; set; }
        /// <summary>
        /// 呆滞件是否预警
        /// </summary>
        public Boolean? APA_IsWarningDeadStock { get; set; }
        /// <summary>
        /// 呆滞天数
        /// </summary>
        public Int32? APA_SlackDays { get; set; }
        /// <summary>
        /// 销价系数
        /// </summary>
        public Decimal? APA_SalePriceRate { get; set; }
        /// <summary>
        /// 销价
        /// </summary>
        public Decimal? APA_SalePrice { get; set; }
        /// <summary>
        /// 车型代码
        /// </summary>
        public String APA_VehicleModelCode { get; set; }
        /// <summary>
        /// 互换码
        /// </summary>
        public String APA_ExchangeCode { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APA_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APA_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APA_CreatedTime { get; set; }
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
        public String APA_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APA_UpdatedTime { get; set; }
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
        public Int64? APA_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String APA_TransID { get; set; }
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
        /// 配置档案ID
        /// </summary>
        public String WHERE_APA_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_APA_Org_ID { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public String WHERE_APA_Barcode { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String WHERE_APA_OEMNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_APA_ThirdNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_APA_Name { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String WHERE_APA_Brand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String WHERE_APA_Specification { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String WHERE_APA_UOM { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String WHERE_APA_Level { get; set; }
        /// <summary>
        /// 汽车品牌
        /// </summary>
        public String WHERE_APA_VehicleBrand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String WHERE_APA_VehicleInspire { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String WHERE_APA_VehicleCapacity { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public String WHERE_APA_VehicleYearModel { get; set; }
        /// <summary>
        /// 变速类型编码
        /// </summary>
        public String WHERE_APA_VehicleGearboxTypeCode { get; set; }
        /// <summary>
        /// 变速类型名称
        /// </summary>
        public String WHERE_APA_VehicleGearboxTypeName { get; set; }
        /// <summary>
        /// 默认供应商ID
        /// </summary>
        public String WHERE_APA_SUPP_ID { get; set; }
        /// <summary>
        /// 默认仓库ID
        /// </summary>
        public String WHERE_APA_WH_ID { get; set; }
        /// <summary>
        /// 默认仓位ID
        /// </summary>
        public String WHERE_APA_WHB_ID { get; set; }
        /// <summary>
        /// 安全库存是否预警
        /// </summary>
        public Boolean? WHERE_APA_IsWarningSafeStock { get; set; }
        /// <summary>
        /// 安全库存
        /// </summary>
        public Int32? WHERE_APA_SafeStockNum { get; set; }
        /// <summary>
        /// 呆滞件是否预警
        /// </summary>
        public Boolean? WHERE_APA_IsWarningDeadStock { get; set; }
        /// <summary>
        /// 呆滞天数
        /// </summary>
        public Int32? WHERE_APA_SlackDays { get; set; }
        /// <summary>
        /// 销价系数
        /// </summary>
        public Decimal? WHERE_APA_SalePriceRate { get; set; }
        /// <summary>
        /// 销价
        /// </summary>
        public Decimal? WHERE_APA_SalePrice { get; set; }
        /// <summary>
        /// 车型代码
        /// </summary>
        public String WHERE_APA_VehicleModelCode { get; set; }
        /// <summary>
        /// 互换码
        /// </summary>
        public String WHERE_APA_ExchangeCode { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_APA_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_APA_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_APA_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_APA_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_APA_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_APA_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_APA_TransID { get; set; }
        #endregion

    }
}
