using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 消息推送Model
    /// </summary>
    public class MDLSM_PushMesage
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String PM_ID { get; set; }
        /// <summary>
        /// 推送商户ID
        /// </summary>
        public String PM_MCT_ID { get; set; }
        /// <summary>
        /// 推送产品ID
        /// </summary>
        public String PM_SP_ID { get; set; }
        /// <summary>
        /// 推送内容
        /// </summary>
        public String PM_Content { get; set; }
        /// <summary>
        /// 发送人
        /// </summary>
        public String PM_Sender { get; set; }
        /// <summary>
        /// 接受人
        /// </summary>
        public String PM_Receiver { get; set; }
        /// <summary>
        /// 推送方式
        /// </summary>
        public String PM_PushType { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime? PM_SendTime { get; set; }
        /// <summary>
        /// 发送时间-开始（查询条件用）
        /// </summary>
        public DateTime? _SendTimeStart { get; set; }
        /// <summary>
        /// 发送时间-终了（查询条件用）
        /// </summary>
        public DateTime? _SendTimeEnd { get; set; }
        /// <summary>
        /// 查看时间
        /// </summary>
        public DateTime? PM_ReadTime { get; set; }
        /// <summary>
        /// 查看时间-开始（查询条件用）
        /// </summary>
        public DateTime? _ReadTimeStart { get; set; }
        /// <summary>
        /// 查看时间-终了（查询条件用）
        /// </summary>
        public DateTime? _ReadTimeEnd { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String PM_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? PM_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String PM_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? PM_CreatedTime { get; set; }
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
        public String PM_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? PM_UpdatedTime { get; set; }
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
        public Int64? PM_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String PM_TransID { get; set; }
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
        public String WHERE_PM_ID { get; set; }
        /// <summary>
        /// 推送商户ID
        /// </summary>
        public String WHERE_PM_MCT_ID { get; set; }
        /// <summary>
        /// 推送产品ID
        /// </summary>
        public String WHERE_PM_SP_ID { get; set; }
        /// <summary>
        /// 推送内容
        /// </summary>
        public String WHERE_PM_Content { get; set; }
        /// <summary>
        /// 发送人
        /// </summary>
        public String WHERE_PM_Sender { get; set; }
        /// <summary>
        /// 接受人
        /// </summary>
        public String WHERE_PM_Receiver { get; set; }
        /// <summary>
        /// 推送方式
        /// </summary>
        public String WHERE_PM_PushType { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime? WHERE_PM_SendTime { get; set; }
        /// <summary>
        /// 查看时间
        /// </summary>
        public DateTime? WHERE_PM_ReadTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_PM_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_PM_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_PM_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_PM_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_PM_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_PM_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_PM_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_PM_TransID { get; set; }
        #endregion

    }
}
