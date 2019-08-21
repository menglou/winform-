using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 共享库存Model
    /// </summary>
    public class MDLPIS_ShareInventory
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String SI_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String SI_Org_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String SI_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String SI_WHB_ID { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String SI_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String SI_OEMNo { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String SI_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String SI_BatchNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String SI_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String SI_Specification { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String SI_SUPP_ID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? SI_Qty { get; set; }
        /// <summary>
        /// 采购单价可见
        /// </summary>
        public Boolean? SI_PurchasePriceIsVisible { get; set; }
        /// <summary>
        /// 采购单价
        /// </summary>
        public Decimal? SI_PurchaseUnitPrice { get; set; }
        /// <summary>
        /// 普通客户销售单价
        /// </summary>
        public Decimal? SI_PriceOfGeneralCustomer { get; set; }
        /// <summary>
        /// 一般汽修商户销售单价
        /// </summary>
        public Decimal? SI_PriceOfCommonAutoFactory { get; set; }
        /// <summary>
        /// 平台内汽修商销售单价
        /// </summary>
        public Decimal? SI_PriceOfPlatformAutoFactory { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SI_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SI_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SI_CreatedTime { get; set; }
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
        public String SI_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SI_UpdatedTime { get; set; }
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
        public Int64? SI_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String SI_TransID { get; set; }
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
        public String WHERE_SI_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SI_Org_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHERE_SI_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String WHERE_SI_WHB_ID { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_SI_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String WHERE_SI_OEMNo { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_SI_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String WHERE_SI_BatchNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_SI_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String WHERE_SI_Specification { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String WHERE_SI_SUPP_ID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? WHERE_SI_Qty { get; set; }
        /// <summary>
        /// 采购单价可见
        /// </summary>
        public Boolean? WHERE_SI_PurchasePriceIsVisible { get; set; }
        /// <summary>
        /// 采购单价
        /// </summary>
        public Decimal? WHERE_SI_PurchaseUnitPrice { get; set; }
        /// <summary>
        /// 普通客户销售单价
        /// </summary>
        public Decimal? WHERE_SI_PriceOfGeneralCustomer { get; set; }
        /// <summary>
        /// 一般汽修商户销售单价
        /// </summary>
        public Decimal? WHERE_SI_PriceOfCommonAutoFactory { get; set; }
        /// <summary>
        /// 平台内汽修商销售单价
        /// </summary>
        public Decimal? WHERE_SI_PriceOfPlatformAutoFactory { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SI_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_SI_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_SI_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_SI_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_SI_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SI_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_SI_TransID { get; set; }
        #endregion

    }
}
