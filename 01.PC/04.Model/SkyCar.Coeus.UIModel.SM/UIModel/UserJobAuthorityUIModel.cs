using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SM.UIModel
{
    /// <summary>
    /// 用户作业授权UIModel
    /// </summary>
    public class UserJobAuthorityUIModel : BaseNotificationUIModel
    {
        #region 公共属性
        /// <summary>
        /// 用户作业权限ID
        /// </summary>
        public String UJA_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String UJA_User_ID { get; set; }
        /// <summary>
        /// 组织
        /// </summary>
        public String UJA_Org_ID { get; set; }
        /// <summary>
        /// 作业ID
        /// </summary>
        public String UJA_BJ_ID { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? UJA_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String UJA_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? UJA_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String UJA_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UJA_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? UJA_VersionNo { get; set; }
        #endregion

        /// <summary>
        /// 用户作业权限ID
        /// </summary>
        public String WHERE_UJA_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public String WHERE_UJA_User_ID { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public String BJ_ID { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        public String BJ_Name { get; set; }
    }
}
