using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 库存Model
    /// </summary>
    public class MDLPIS_Inventory
    {
        #region 公共属性
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
        public String INV_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? INV_UpdatedTime { get; set; }
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
        public Int64? INV_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String INV_TransID { get; set; }
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
        public String WHERE_INV_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_INV_Org_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHERE_INV_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String WHERE_INV_WHB_ID { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_INV_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String WHERE_INV_OEMNo { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_INV_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String WHERE_INV_BatchNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_INV_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String WHERE_INV_Specification { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String WHERE_INV_SUPP_ID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? WHERE_INV_Qty { get; set; }
        /// <summary>
        /// 采购单价
        /// </summary>
        public Decimal? WHERE_INV_PurchaseUnitPrice { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_INV_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_INV_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_INV_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_INV_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_INV_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_INV_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_INV_TransID { get; set; }
        #endregion

    }
}
