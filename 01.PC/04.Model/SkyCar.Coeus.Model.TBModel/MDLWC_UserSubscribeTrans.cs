using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 微信用户关注明细Model
    /// </summary>
    public class MDLWC_UserSubscribeTrans
    {
        #region 公共属性
        /// <summary>
        /// 微信用户关注明细ID
        /// </summary>
        public String WUST_ID { get; set; }
        /// <summary>
        /// 开放平台ID
        /// </summary>
        public String WUST_UnionID { get; set; }
        /// <summary>
        /// OpenID
        /// </summary>
        public String WUST_OpenID { get; set; }
        /// <summary>
        /// 事件
        /// </summary>
        public String WUST_Event { get; set; }
        /// <summary>
        /// 异动时间
        /// </summary>
        public DateTime? WUST_Time { get; set; }
        /// <summary>
        /// 异动时间-开始（查询条件用）
        /// </summary>
        public DateTime? _TimeStart { get; set; }
        /// <summary>
        /// 异动时间-终了（查询条件用）
        /// </summary>
        public DateTime? _TimeEnd { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WUST_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WUST_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WUST_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WUST_CreatedTime { get; set; }
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
        public String WUST_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WUST_UpdatedTime { get; set; }
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
        public Int64? WUST_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WUST_TransID { get; set; }
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
        /// 微信用户关注明细ID
        /// </summary>
        public String WHERE_WUST_ID { get; set; }
        /// <summary>
        /// 开放平台ID
        /// </summary>
        public String WHERE_WUST_UnionID { get; set; }
        /// <summary>
        /// OpenID
        /// </summary>
        public String WHERE_WUST_OpenID { get; set; }
        /// <summary>
        /// 事件
        /// </summary>
        public String WHERE_WUST_Event { get; set; }
        /// <summary>
        /// 异动时间
        /// </summary>
        public DateTime? WHERE_WUST_Time { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_WUST_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_WUST_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_WUST_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_WUST_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_WUST_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_WUST_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_WUST_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_WUST_TransID { get; set; }
        #endregion

    }
}
