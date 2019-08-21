using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 微信用户Model
    /// </summary>
    public class MDLWC_User
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String WU_ID { get; set; }
        /// <summary>
        /// 用户的标识
        /// </summary>
        public String WU_OpenID { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public String WU_NickName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public String WU_Sex { get; set; }
        /// <summary>
        /// 用户所在城市
        /// </summary>
        public String WU_City { get; set; }
        /// <summary>
        /// 用户所在国家
        /// </summary>
        public String WU_Country { get; set; }
        /// <summary>
        /// 用户所在省份
        /// </summary>
        public String WU_Province { get; set; }
        /// <summary>
        /// 用户的语言
        /// </summary>
        public String WU_Language { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public String WU_HeadImgURL { get; set; }
        /// <summary>
        /// 微信用户关注公众号的标识
        /// </summary>
        public String WU_Subscribe { get; set; }
        /// <summary>
        /// 用户关注时间戳
        /// </summary>
        public String WU_SubscribeTime { get; set; }
        /// <summary>
        /// 开放平台ID
        /// </summary>
        public String WU_UnionID { get; set; }
        /// <summary>
        /// 公众号运营者对粉丝的备注
        /// </summary>
        public String WU_Remark { get; set; }
        /// <summary>
        /// 用户所在的分组ID
        /// </summary>
        public String WU_GroupID { get; set; }
        /// <summary>
        /// 用户特权信息
        /// </summary>
        public String WU_Privilege { get; set; }
        /// <summary>
        /// 最近一次关注时间
        /// </summary>
        public DateTime? WU_SubscribeTimeNew { get; set; }
        /// <summary>
        /// 最近一次关注时间-开始（查询条件用）
        /// </summary>
        public DateTime? _SubscribeTimeNewStart { get; set; }
        /// <summary>
        /// 最近一次关注时间-终了（查询条件用）
        /// </summary>
        public DateTime? _SubscribeTimeNewEnd { get; set; }
        /// <summary>
        /// 最近一次取消关注时间
        /// </summary>
        public DateTime? WU_UnSubscribeTimeNew { get; set; }
        /// <summary>
        /// 最近一次取消关注时间-开始（查询条件用）
        /// </summary>
        public DateTime? _UnSubscribeTimeNewStart { get; set; }
        /// <summary>
        /// 最近一次取消关注时间-终了（查询条件用）
        /// </summary>
        public DateTime? _UnSubscribeTimeNewEnd { get; set; }
        /// <summary>
        /// 最近一次关注的来源组织
        /// </summary>
        public String WU_Org_ID { get; set; }
        /// <summary>
        /// 最近一次访问时间
        /// </summary>
        public DateTime? WU_LastVisitTime { get; set; }
        /// <summary>
        /// 最近一次访问时间-开始（查询条件用）
        /// </summary>
        public DateTime? _LastVisitTimeStart { get; set; }
        /// <summary>
        /// 最近一次访问时间-终了（查询条件用）
        /// </summary>
        public DateTime? _LastVisitTimeEnd { get; set; }
        /// <summary>
        /// 最近一次经度
        /// </summary>
        public String WU_Latitude { get; set; }
        /// <summary>
        /// 最近一次经度
        /// </summary>
        public String WU_Longitude { get; set; }
        /// <summary>
        /// 最近一次坐标更新时间
        /// </summary>
        public DateTime? WU_LocationUpdTime { get; set; }
        /// <summary>
        /// 最近一次坐标更新时间-开始（查询条件用）
        /// </summary>
        public DateTime? _LocationUpdTimeStart { get; set; }
        /// <summary>
        /// 最近一次坐标更新时间-终了（查询条件用）
        /// </summary>
        public DateTime? _LocationUpdTimeEnd { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WU_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WU_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WU_CreatedTime { get; set; }
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
        public String WU_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WU_UpdatedTime { get; set; }
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
        public Int64? WU_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WU_TransID { get; set; }
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
        public String WHERE_WU_ID { get; set; }
        /// <summary>
        /// 用户的标识
        /// </summary>
        public String WHERE_WU_OpenID { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public String WHERE_WU_NickName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public String WHERE_WU_Sex { get; set; }
        /// <summary>
        /// 用户所在城市
        /// </summary>
        public String WHERE_WU_City { get; set; }
        /// <summary>
        /// 用户所在国家
        /// </summary>
        public String WHERE_WU_Country { get; set; }
        /// <summary>
        /// 用户所在省份
        /// </summary>
        public String WHERE_WU_Province { get; set; }
        /// <summary>
        /// 用户的语言
        /// </summary>
        public String WHERE_WU_Language { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public String WHERE_WU_HeadImgURL { get; set; }
        /// <summary>
        /// 微信用户关注公众号的标识
        /// </summary>
        public String WHERE_WU_Subscribe { get; set; }
        /// <summary>
        /// 用户关注时间戳
        /// </summary>
        public String WHERE_WU_SubscribeTime { get; set; }
        /// <summary>
        /// 开放平台ID
        /// </summary>
        public String WHERE_WU_UnionID { get; set; }
        /// <summary>
        /// 公众号运营者对粉丝的备注
        /// </summary>
        public String WHERE_WU_Remark { get; set; }
        /// <summary>
        /// 用户所在的分组ID
        /// </summary>
        public String WHERE_WU_GroupID { get; set; }
        /// <summary>
        /// 用户特权信息
        /// </summary>
        public String WHERE_WU_Privilege { get; set; }
        /// <summary>
        /// 最近一次关注时间
        /// </summary>
        public DateTime? WHERE_WU_SubscribeTimeNew { get; set; }
        /// <summary>
        /// 最近一次取消关注时间
        /// </summary>
        public DateTime? WHERE_WU_UnSubscribeTimeNew { get; set; }
        /// <summary>
        /// 最近一次关注的来源组织
        /// </summary>
        public String WHERE_WU_Org_ID { get; set; }
        /// <summary>
        /// 最近一次访问时间
        /// </summary>
        public DateTime? WHERE_WU_LastVisitTime { get; set; }
        /// <summary>
        /// 最近一次经度
        /// </summary>
        public String WHERE_WU_Latitude { get; set; }
        /// <summary>
        /// 最近一次经度
        /// </summary>
        public String WHERE_WU_Longitude { get; set; }
        /// <summary>
        /// 最近一次坐标更新时间
        /// </summary>
        public DateTime? WHERE_WU_LocationUpdTime { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_WU_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_WU_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_WU_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_WU_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_WU_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_WU_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_WU_TransID { get; set; }
        #endregion

    }
}
