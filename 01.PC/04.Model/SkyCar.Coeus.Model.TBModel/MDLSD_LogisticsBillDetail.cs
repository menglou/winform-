using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 物流订单明细Model
    /// </summary>
    public class MDLSD_LogisticsBillDetail
    {
        #region 公共属性
        /// <summary>
        /// 物流订单明细ID
        /// </summary>
        public String LBD_ID { get; set; }
        /// <summary>
        /// 物流订单ID
        /// </summary>
        public String LBD_LB_ID { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public String LBD_LB_No { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public String LBD_Barcode { get; set; }
        /// <summary>
        /// 配件批次号（原库存批次）
        /// </summary>
        public String LBD_BatchNo { get; set; }
        /// <summary>
        /// 配件批次号（汽修厂用）
        /// </summary>
        public String LBD_BatchNoNew { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String LBD_Name { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String LBD_Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String LBD_UOM { get; set; }
        /// <summary>
        /// 配送数量
        /// </summary>
        public Decimal? LBD_DeliveryQty { get; set; }
        /// <summary>
        /// 签收数量
        /// </summary>
        public Decimal? LBD_SignQty { get; set; }
        /// <summary>
        /// 拒收数量
        /// </summary>
        public Decimal? LBD_RejectQty { get; set; }
        /// <summary>
        /// 丢失数量
        /// </summary>
        public Decimal? LBD_LoseQty { get; set; }
        /// <summary>
        /// 赔偿金
        /// </summary>
        public Decimal? LBD_Indemnification { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? LBD_AccountReceivableAmount { get; set; }
        /// <summary>
        /// 状态编码
        /// </summary>
        public String LBD_StatusCode { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public String LBD_StatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String LBD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? LBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String LBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? LBD_CreatedTime { get; set; }
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
        public String LBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LBD_UpdatedTime { get; set; }
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
        public Int64? LBD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String LBD_TransID { get; set; }
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
        /// 物流订单明细ID
        /// </summary>
        public String WHERE_LBD_ID { get; set; }
        /// <summary>
        /// 物流订单ID
        /// </summary>
        public String WHERE_LBD_LB_ID { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public String WHERE_LBD_LB_No { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public String WHERE_LBD_Barcode { get; set; }
        /// <summary>
        /// 配件批次号（原库存批次）
        /// </summary>
        public String WHERE_LBD_BatchNo { get; set; }
        /// <summary>
        /// 配件批次号（汽修厂用）
        /// </summary>
        public String WHERE_LBD_BatchNoNew { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String WHERE_LBD_Name { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String WHERE_LBD_Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String WHERE_LBD_UOM { get; set; }
        /// <summary>
        /// 配送数量
        /// </summary>
        public Decimal? WHERE_LBD_DeliveryQty { get; set; }
        /// <summary>
        /// 签收数量
        /// </summary>
        public Decimal? WHERE_LBD_SignQty { get; set; }
        /// <summary>
        /// 拒收数量
        /// </summary>
        public Decimal? WHERE_LBD_RejectQty { get; set; }
        /// <summary>
        /// 丢失数量
        /// </summary>
        public Decimal? WHERE_LBD_LoseQty { get; set; }
        /// <summary>
        /// 赔偿金
        /// </summary>
        public Decimal? WHERE_LBD_Indemnification { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? WHERE_LBD_AccountReceivableAmount { get; set; }
        /// <summary>
        /// 状态编码
        /// </summary>
        public String WHERE_LBD_StatusCode { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public String WHERE_LBD_StatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_LBD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_LBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_LBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_LBD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_LBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_LBD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_LBD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_LBD_TransID { get; set; }
        #endregion

    }
}
