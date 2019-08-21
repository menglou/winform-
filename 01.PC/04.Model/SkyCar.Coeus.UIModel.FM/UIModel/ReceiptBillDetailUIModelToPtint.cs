using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM
{
    /// <summary>
    /// 收款单明细管理UIModel
    /// </summary>
    public class ReceiptBillDetailUIModelToPtint : BaseNotificationUIModel
    {
        #region 收款单明细属性

        /// <summary>
        /// 收款单明细ID
        /// </summary>
        public String RBD_ID { get; set; }
        /// <summary>
        /// 收款单ID
        /// </summary>
        public String RBD_RB_ID { get; set; }
        /// <summary>
        /// 收款单号
        /// </summary>
        public String RBD_RB_No { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String RBD_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String RBD_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String RBD_SrcBillNo { get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public Decimal? RBD_ReceiveAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String RBD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? RBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String RBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? RBD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String RBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? RBD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? RBD_VersionNo { get; set; }

        #endregion

        /// <summary>
        /// 选择
        /// </summary>
        public Boolean IsChecked { get; set; }
    }
}
