using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 销售订单明细Model
    /// </summary>
    public class MDLSD_SalesOrderDetail
    {
        #region 公共属性
        /// <summary>
        /// 销售订单明细ID
        /// </summary>
        public String SOD_ID { get; set; }
        /// <summary>
        /// 销售订单ID
        /// </summary>
        public String SOD_SO_ID { get; set; }
        /// <summary>
        /// 计价基准
        /// </summary>
        public Decimal? SOD_SalePriceRate { get; set; }
        /// <summary>
        /// 计价基准可改
        /// </summary>
        public Boolean? SOD_SalePriceRateIsChangeable { get; set; }
        /// <summary>
        /// 价格是否含税
        /// </summary>
        public Boolean? SOD_PriceIsIncludeTax { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public Decimal? SOD_TaxRate { get; set; }
        /// <summary>
        /// 税额
        /// </summary>
        public Decimal? SOD_TotalTax { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public Decimal? SOD_Qty { get; set; }
        /// <summary>
        /// 签收数量
        /// </summary>
        public Decimal? SOD_SignQty { get; set; }
        /// <summary>
        /// 拒收数量
        /// </summary>
        public Decimal? SOD_RejectQty { get; set; }
        /// <summary>
        /// 丢失数量
        /// </summary>
        public Decimal? SOD_LoseQty { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public Decimal? SOD_UnitPrice { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal? SOD_TotalAmount { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String SOD_Barcode { get; set; }
        /// <summary>
        /// 配件批次号（原库存批次）
        /// </summary>
        public String SOD_BatchNo { get; set; }
        /// <summary>
        /// 配件批次号（汽修厂用）
        /// </summary>
        public String SOD_BatchNoNew { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String SOD_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String SOD_Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String SOD_UOM { get; set; }
        /// <summary>
        /// 入库组织ID
        /// </summary>
        public String SOD_StockInOrgID { get; set; }
        /// <summary>
        /// 入库组织编码
        /// </summary>
        public String SOD_StockInOrgCode { get; set; }
        /// <summary>
        /// 入库组织名称
        /// </summary>
        public String SOD_StockInOrgName { get; set; }
        /// <summary>
        /// 入库仓库ID
        /// </summary>
        public String SOD_StockInWarehouseID { get; set; }
        /// <summary>
        /// 入库仓库名称
        /// </summary>
        public String SOD_StockInWarehouseName { get; set; }
        /// <summary>
        /// 入库仓位ID
        /// </summary>
        public String SOD_StockInBinID { get; set; }
        /// <summary>
        /// 入库仓位名称
        /// </summary>
        public String SOD_StockInBinName { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String SOD_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String SOD_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String SOD_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String SOD_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String SOD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SOD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SOD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SOD_CreatedTime { get; set; }
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
        public String SOD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SOD_UpdatedTime { get; set; }
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
        public Int64? SOD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String SOD_TransID { get; set; }
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
        /// 销售订单明细ID
        /// </summary>
        public String WHERE_SOD_ID { get; set; }
        /// <summary>
        /// 销售订单ID
        /// </summary>
        public String WHERE_SOD_SO_ID { get; set; }
        /// <summary>
        /// 计价基准
        /// </summary>
        public Decimal? WHERE_SOD_SalePriceRate { get; set; }
        /// <summary>
        /// 计价基准可改
        /// </summary>
        public Boolean? WHERE_SOD_SalePriceRateIsChangeable { get; set; }
        /// <summary>
        /// 价格是否含税
        /// </summary>
        public Boolean? WHERE_SOD_PriceIsIncludeTax { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public Decimal? WHERE_SOD_TaxRate { get; set; }
        /// <summary>
        /// 税额
        /// </summary>
        public Decimal? WHERE_SOD_TotalTax { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public Decimal? WHERE_SOD_Qty { get; set; }
        /// <summary>
        /// 签收数量
        /// </summary>
        public Decimal? WHERE_SOD_SignQty { get; set; }
        /// <summary>
        /// 拒收数量
        /// </summary>
        public Decimal? WHERE_SOD_RejectQty { get; set; }
        /// <summary>
        /// 丢失数量
        /// </summary>
        public Decimal? WHERE_SOD_LoseQty { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public Decimal? WHERE_SOD_UnitPrice { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal? WHERE_SOD_TotalAmount { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_SOD_Barcode { get; set; }
        /// <summary>
        /// 配件批次号（原库存批次）
        /// </summary>
        public String WHERE_SOD_BatchNo { get; set; }
        /// <summary>
        /// 配件批次号（汽修厂用）
        /// </summary>
        public String WHERE_SOD_BatchNoNew { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_SOD_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String WHERE_SOD_Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String WHERE_SOD_UOM { get; set; }
        /// <summary>
        /// 入库组织ID
        /// </summary>
        public String WHERE_SOD_StockInOrgID { get; set; }
        /// <summary>
        /// 入库组织编码
        /// </summary>
        public String WHERE_SOD_StockInOrgCode { get; set; }
        /// <summary>
        /// 入库组织名称
        /// </summary>
        public String WHERE_SOD_StockInOrgName { get; set; }
        /// <summary>
        /// 入库仓库ID
        /// </summary>
        public String WHERE_SOD_StockInWarehouseID { get; set; }
        /// <summary>
        /// 入库仓库名称
        /// </summary>
        public String WHERE_SOD_StockInWarehouseName { get; set; }
        /// <summary>
        /// 入库仓位ID
        /// </summary>
        public String WHERE_SOD_StockInBinID { get; set; }
        /// <summary>
        /// 入库仓位名称
        /// </summary>
        public String WHERE_SOD_StockInBinName { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String WHERE_SOD_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_SOD_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_SOD_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_SOD_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_SOD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SOD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_SOD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_SOD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_SOD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_SOD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SOD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_SOD_TransID { get; set; }
        #endregion

    }
}
