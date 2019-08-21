using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 微信消息Model
    /// </summary>
    public class MDLWC_Message
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String WMsg_ID { get; set; }
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public String WMsg_ToUserName { get; set; }
        /// <summary>
        /// 发送方OpenID
        /// </summary>
        public String WMsg_FromUserName { get; set; }
        /// <summary>
        /// 消息创建时间
        /// </summary>
        public Int64? WMsg_CreateTime { get; set; }
        /// <summary>
        /// 消息体
        /// </summary>
        public String WMsg_MessageBody { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public String WMsg_MsgType { get; set; }
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public String WMsg_Content { get; set; }
        /// <summary>
        /// 消息ID
        /// </summary>
        public String WMsg_MsgId { get; set; }
        /// <summary>
        /// 图片链接
        /// </summary>
        public String WMsg_PicUrl { get; set; }
        /// <summary>
        /// 图片消息媒体id
        /// </summary>
        public String WMsg_MediaId { get; set; }
        /// <summary>
        /// 语音格式
        /// </summary>
        public String WMsg_Format { get; set; }
        /// <summary>
        /// 视频消息缩略图的媒体id
        /// </summary>
        public String WMsg_ThumbMediaId { get; set; }
        /// <summary>
        /// 地理位置维度
        /// </summary>
        public String WMsg_Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public String WMsg_Location_Y { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public String WMsg_Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public String WMsg_Label { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public String WMsg_Title { get; set; }
        /// <summary>
        /// 消息描述
        /// </summary>
        public String WMsg_Description { get; set; }
        /// <summary>
        /// 消息链接
        /// </summary>
        public String WMsg_Url { get; set; }
        /// <summary>
        /// 事件类型
        /// </summary>
        public String WMsg_Event { get; set; }
        /// <summary>
        /// 事件KEY值
        /// </summary>
        public String WMsg_EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket
        /// </summary>
        public String WMsg_Ticket { get; set; }
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public String WMsg_Latitude { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public String WMsg_Longitude { get; set; }
        /// <summary>
        /// 地理位置精度
        /// </summary>
        public String WMsg_Precision { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WMsg_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WMsg_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WMsg_CreatedTime { get; set; }
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
        public String WMsg_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WMsg_UpdatedTime { get; set; }
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
        public Int64? WMsg_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WMsg_TransID { get; set; }
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
        public String WHERE_WMsg_ID { get; set; }
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public String WHERE_WMsg_ToUserName { get; set; }
        /// <summary>
        /// 发送方OpenID
        /// </summary>
        public String WHERE_WMsg_FromUserName { get; set; }
        /// <summary>
        /// 消息创建时间
        /// </summary>
        public Int64? WHERE_WMsg_CreateTime { get; set; }
        /// <summary>
        /// 消息体
        /// </summary>
        public String WHERE_WMsg_MessageBody { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public String WHERE_WMsg_MsgType { get; set; }
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public String WHERE_WMsg_Content { get; set; }
        /// <summary>
        /// 消息ID
        /// </summary>
        public String WHERE_WMsg_MsgId { get; set; }
        /// <summary>
        /// 图片链接
        /// </summary>
        public String WHERE_WMsg_PicUrl { get; set; }
        /// <summary>
        /// 图片消息媒体id
        /// </summary>
        public String WHERE_WMsg_MediaId { get; set; }
        /// <summary>
        /// 语音格式
        /// </summary>
        public String WHERE_WMsg_Format { get; set; }
        /// <summary>
        /// 视频消息缩略图的媒体id
        /// </summary>
        public String WHERE_WMsg_ThumbMediaId { get; set; }
        /// <summary>
        /// 地理位置维度
        /// </summary>
        public String WHERE_WMsg_Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public String WHERE_WMsg_Location_Y { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public String WHERE_WMsg_Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public String WHERE_WMsg_Label { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public String WHERE_WMsg_Title { get; set; }
        /// <summary>
        /// 消息描述
        /// </summary>
        public String WHERE_WMsg_Description { get; set; }
        /// <summary>
        /// 消息链接
        /// </summary>
        public String WHERE_WMsg_Url { get; set; }
        /// <summary>
        /// 事件类型
        /// </summary>
        public String WHERE_WMsg_Event { get; set; }
        /// <summary>
        /// 事件KEY值
        /// </summary>
        public String WHERE_WMsg_EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket
        /// </summary>
        public String WHERE_WMsg_Ticket { get; set; }
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public String WHERE_WMsg_Latitude { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public String WHERE_WMsg_Longitude { get; set; }
        /// <summary>
        /// 地理位置精度
        /// </summary>
        public String WHERE_WMsg_Precision { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_WMsg_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_WMsg_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_WMsg_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_WMsg_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_WMsg_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_WMsg_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_WMsg_TransID { get; set; }
        #endregion

    }
}
