using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM
{
    /// <summary>
    /// 付款单明细UIModel
    /// </summary>
    public class PayBillDetailUIModelToPrint : BaseNotificationUIModel
    {
        #region 付款单明细属性

        /// <summary>
        /// 付款单明细ID
        /// </summary>
        public String PBD_ID { get; set; }
        /// <summary>
        /// 付款单ID
        /// </summary>
        public String PBD_PB_ID { get; set; }
        /// <summary>
        /// 付款单号
        /// </summary>
        public String PBD_PB_No { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String PBD_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String PBD_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String PBD_SrcBillNo { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public Decimal? PBD_PayAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String PBD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? PBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String PBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? PBD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String PBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? PBD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? PBD_VersionNo { get; set; }

        #endregion

        /// <summary>
        /// 选择
        /// </summary>
        public Boolean IsChecked { get; set; }
        
    }
}
