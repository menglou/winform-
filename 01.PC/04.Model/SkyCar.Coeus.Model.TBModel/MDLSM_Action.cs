using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 系统动作Model
    /// </summary>
    public class MDLSM_Action
    {
        #region 公共属性
        /// <summary>
        /// 动作ID
        /// </summary>
        public String Act_ID { get; set; }
        /// <summary>
        /// Key
        /// </summary>
        public String Act_Key { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String Act_Name { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? Act_Index { get; set; }
        /// <summary>
        /// 是否显示到界面
        /// </summary>
        public Boolean? Act_IsDisplayInUI { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? Act_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String Act_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Act_CreatedTime { get; set; }
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
        public String Act_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Act_UpdatedTime { get; set; }
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
        public Int64? Act_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String Act_TransID { get; set; }
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
        /// 动作ID
        /// </summary>
        public String WHERE_Act_ID { get; set; }
        /// <summary>
        /// Key
        /// </summary>
        public String WHERE_Act_Key { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String WHERE_Act_Name { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public Int32? WHERE_Act_Index { get; set; }
        /// <summary>
        /// 是否显示到界面
        /// </summary>
        public Boolean? WHERE_Act_IsDisplayInUI { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_Act_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_Act_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_Act_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_Act_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_Act_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_Act_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_Act_TransID { get; set; }
        #endregion

    }
}
