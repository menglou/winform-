using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 系统消息Model
    /// </summary>
    public class MDLSM_Message
    {
        #region 公共属性
        /// <summary>
        /// 消息ID
        /// </summary>
        public String Msg_ID { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String Msg_Code { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public String Msg_Content { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? Msg_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String Msg_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Msg_CreatedTime { get; set; }
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
        public String Msg_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Msg_UpdatedTime { get; set; }
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
        public Int64? Msg_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String Msg_TransID { get; set; }
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
        /// 消息ID
        /// </summary>
        public String WHERE_Msg_ID { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public String WHERE_Msg_Code { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public String WHERE_Msg_Content { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_Msg_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_Msg_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_Msg_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_Msg_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_Msg_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_Msg_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_Msg_TransID { get; set; }
        #endregion

    }
}
