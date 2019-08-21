using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// Venus对应的配件档案表
    /// </summary>
    public class MDLAPM_AutoPartsArchive
    {
        #region 公共属性
        /// <summary>
        /// 配置档案ID
        /// </summary>
        public String APA_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String APA_Org_ID { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public String APA_Barcode { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String APA_OEMNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String APA_ThirdNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String APA_Name { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String APA_Specification { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String APA_UOM { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String APA_Level { get; set; }
        /// <summary>
        /// 汽车品牌
        /// </summary>
        public String APA_VehicleBrand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String APA_VehicleInspire { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String APA_VehicleCapacity { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public String APA_VehicleYearModel { get; set; }
        /// <summary>
        /// 变速类型
        /// </summary>
        public String APA_VehicleGearboxType { get; set; }
        /// <summary>
        /// 默认供应商ID
        /// </summary>
        public String APA_SUPP_ID { get; set; }
        /// <summary>
        /// 默认仓库ID
        /// </summary>
        public String APA_WH_ID { get; set; }
        /// <summary>
        /// 默认仓位ID
        /// </summary>
        public String APA_WHB_ID { get; set; }
        /// <summary>
        /// 安全库存是否预警
        /// </summary>
        public Boolean? APA_IsWarningSafeStock { get; set; }
        /// <summary>
        /// 安全库存
        /// </summary>
        public Int32? APA_SafeStockNum { get; set; }
        /// <summary>
        /// 呆滞件是否预警
        /// </summary>
        public Boolean? APA_IsWarningDeadStock { get; set; }
        /// <summary>
        /// 呆滞天数
        /// </summary>
        public Int32? APA_SlackDays { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public Boolean? APA_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APA_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APA_CreatedTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public String APA_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APA_UpdatedTime { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? APA_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String APA_TransID { get; set; }

        #endregion
    }
}
