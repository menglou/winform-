using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM
{
    /// <summary>
    /// 付款单管理QCModel
    /// </summary>
    public class PayBillManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 付款单号
        /// </summary>
        public String WHERE_PB_No { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String WHERE_PB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public String WHERE_PB_BusinessStatusName { get; set; }
        /// <summary>
        /// 收款对象类型
        /// </summary>
        public String WHERE_PB_RecObjectTypeName { get; set; }
        /// <summary>
        /// 收款对象
        /// </summary>
        public String WHERE_PB_RecObjectName { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public String WHERE_PB_PayTypeName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_PB_IsValid { get; set; }
        /// <summary>
        /// 付款组织ID
        /// </summary>
        public String WHERE_PB_Pay_Org_ID { get; set; }
        /// <summary>
        /// 付款组织名称
        /// </summary>
        public String WHERE_PB_Pay_Org_Name { get; set; }
        /// <summary>
        /// 付款日期-开始（查询条件用）
        /// </summary>
        public DateTime? _DateStart { get; set; }
        /// <summary>
        /// 付款日期-终了（查询条件用）
        /// </summary>
        public DateTime? _DateEnd { get; set; }
        /// <summary>
        /// 付款明细来源单号
        /// </summary>
        public String WHERE_PBD_SrcBillNo { get; set; }
    }
}
