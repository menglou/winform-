using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 消息推送接收日志Model
    /// </summary>
    public class MDLCSM_PushMesageLog
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String PML_ID { get; set; }
        /// <summary>
        /// 业务提醒ID
        /// </summary>
        public String PML_BRL_ID { get; set; }
        /// <summary>
        /// 推送方式
        /// </summary>
        public String PML_PushMode { get; set; }
        /// <summary>
        /// 推送内容
        /// </summary>
        public String PML_Content { get; set; }
        /// <summary>
        /// 推送者类别
        /// </summary>
        public String PML_SenderType { get; set; }
        /// <summary>
        /// 推送者
        /// </summary>
        public String PML_Sender { get; set; }
        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime? PML_SendTime { get; set; }
        /// <summary>
        /// 推送时间-开始（查询条件用）
        /// </summary>
        public DateTime? _SendTimeStart { get; set; }
        /// <summary>
        /// 推送时间-终了（查询条件用）
        /// </summary>
        public DateTime? _SendTimeEnd { get; set; }
        /// <summary>
        /// 推送状态
        /// </summary>
        public String PML_SendStatus { get; set; }
        /// <summary>
        /// 接收者类别
        /// </summary>
        public String PML_ReceiverType { get; set; }
        /// <summary>
        /// 接收者
        /// </summary>
        public String PML_Receiver { get; set; }
        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? PML_ReceiveTime { get; set; }
        /// <summary>
        /// 接收时间-开始（查询条件用）
        /// </summary>
        public DateTime? _ReceiveTimeStart { get; set; }
        /// <summary>
        /// 接收时间-终了（查询条件用）
        /// </summary>
        public DateTime? _ReceiveTimeEnd { get; set; }
        /// <summary>
        /// 接收状态
        /// </summary>
        public String PML_ReceiveStatus { get; set; }
        /// <summary>
        /// 作业ID
        /// </summary>
        public String PML_BJ_ID { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        public String PML_BJ_Name { get; set; }
        /// <summary>
        /// 业务消息类别
        /// </summary>
        public String PML_BusMsgType { get; set; }
        /// <summary>
        /// 业务单号
        /// </summary>
        public String PML_BussinessCode { get; set; }
        /// <summary>
        /// JSON格式内容
        /// </summary>
        public String PML_JsonFormatContent { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String PML_Remark { get; set; }
        /// <summary>
        /// 是否需要跟踪
        /// </summary>
        public Boolean? PML_IsNeedTrack { get; set; }
        /// <summary>
        /// 跟踪人
        /// </summary>
        public String PML_TrackedBy { get; set; }
        /// <summary>
        /// 跟踪状态
        /// </summary>
        public String PML_TrackStatus { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? PML_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String PML_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? PML_CreatedTime { get; set; }
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
        public String PML_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? PML_UpdatedTime { get; set; }
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
        public Int64? PML_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String PML_TransID { get; set; }
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
        public String WHERE_PML_ID { get; set; }
        /// <summary>
        /// 业务提醒ID
        /// </summary>
        public String WHERE_PML_BRL_ID { get; set; }
        /// <summary>
        /// 推送方式
        /// </summary>
        public String WHERE_PML_PushMode { get; set; }
        /// <summary>
        /// 推送内容
        /// </summary>
        public String WHERE_PML_Content { get; set; }
        /// <summary>
        /// 推送者类别
        /// </summary>
        public String WHERE_PML_SenderType { get; set; }
        /// <summary>
        /// 推送者
        /// </summary>
        public String WHERE_PML_Sender { get; set; }
        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime? WHERE_PML_SendTime { get; set; }
        /// <summary>
        /// 推送状态
        /// </summary>
        public String WHERE_PML_SendStatus { get; set; }
        /// <summary>
        /// 接收者类别
        /// </summary>
        public String WHERE_PML_ReceiverType { get; set; }
        /// <summary>
        /// 接收者
        /// </summary>
        public String WHERE_PML_Receiver { get; set; }
        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? WHERE_PML_ReceiveTime { get; set; }
        /// <summary>
        /// 接收状态
        /// </summary>
        public String WHERE_PML_ReceiveStatus { get; set; }
        /// <summary>
        /// 作业ID
        /// </summary>
        public String WHERE_PML_BJ_ID { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        public String WHERE_PML_BJ_Name { get; set; }
        /// <summary>
        /// 业务消息类别
        /// </summary>
        public String WHERE_PML_BusMsgType { get; set; }
        /// <summary>
        /// 业务单号
        /// </summary>
        public String WHERE_PML_BussinessCode { get; set; }
        /// <summary>
        /// JSON格式内容
        /// </summary>
        public String WHERE_PML_JsonFormatContent { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_PML_Remark { get; set; }
        /// <summary>
        /// 是否需要跟踪
        /// </summary>
        public Boolean? WHERE_PML_IsNeedTrack { get; set; }
        /// <summary>
        /// 跟踪人
        /// </summary>
        public String WHERE_PML_TrackedBy { get; set; }
        /// <summary>
        /// 跟踪状态
        /// </summary>
        public String WHERE_PML_TrackStatus { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_PML_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_PML_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_PML_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_PML_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_PML_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_PML_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_PML_TransID { get; set; }
        #endregion

    }
}
