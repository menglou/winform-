using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 物流订单异动Model
    /// </summary>
    public class MDLSD_LogisticsBillTrans
    {
        #region 公共属性
        /// <summary>
        /// 物流订单异动ID
        /// </summary>
        public String LBT_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String LBT_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String LBT_Org_Name { get; set; }
        /// <summary>
        /// 物流订单ID
        /// </summary>
        public String LBT_LB_ID { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public String LBT_LB_NO { get; set; }
        /// <summary>
        /// 异动时间
        /// </summary>
        public DateTime? LBT_Time { get; set; }
        /// <summary>
        /// 异动时间-开始（查询条件用）
        /// </summary>
        public DateTime? _TimeStart { get; set; }
        /// <summary>
        /// 异动时间-终了（查询条件用）
        /// </summary>
        public DateTime? _TimeEnd { get; set; }
        /// <summary>
        /// 异动状态
        /// </summary>
        public String LBT_Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String LBT_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? LBT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String LBT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? LBT_CreatedTime { get; set; }
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
        public String LBT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LBT_UpdatedTime { get; set; }
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
        public Int64? LBT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String LBT_TransID { get; set; }
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
        /// 物流订单异动ID
        /// </summary>
        public String WHERE_LBT_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_LBT_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_LBT_Org_Name { get; set; }
        /// <summary>
        /// 物流订单ID
        /// </summary>
        public String WHERE_LBT_LB_ID { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public String WHERE_LBT_LB_NO { get; set; }
        /// <summary>
        /// 异动时间
        /// </summary>
        public DateTime? WHERE_LBT_Time { get; set; }
        /// <summary>
        /// 异动状态
        /// </summary>
        public String WHERE_LBT_Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_LBT_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_LBT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_LBT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_LBT_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_LBT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_LBT_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_LBT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_LBT_TransID { get; set; }
        #endregion

    }
}
