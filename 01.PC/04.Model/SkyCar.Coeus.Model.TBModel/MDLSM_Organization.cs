using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 组织管理Model
    /// </summary>
    public class MDLSM_Organization
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String Org_ID { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public String Org_MCT_ID { get; set; }
        /// <summary>
        /// 门店编码
        /// </summary>
        public String Org_Code { get; set; }
        /// <summary>
        /// 平台编码
        /// </summary>
        public String Org_PlatformCode { get; set; }
        /// <summary>
        /// 组织全称
        /// </summary>
        public String Org_FullName { get; set; }
        /// <summary>
        /// 组织简称
        /// </summary>
        public String Org_ShortName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public String Org_Contacter { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public String Org_TEL { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        public String Org_PhoneNo { get; set; }
        /// <summary>
        /// 省份Code
        /// </summary>
        public String Org_Prov_Code { get; set; }
        /// <summary>
        /// 城市Code
        /// </summary>
        public String Org_City_Code { get; set; }
        /// <summary>
        /// 区域Code
        /// </summary>
        public String Org_Dist_Code { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String Org_Addr { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public String Org_Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public String Org_Latitude { get; set; }
        /// <summary>
        /// 标注点显示标题
        /// </summary>
        public String Org_MarkerTitle { get; set; }
        /// <summary>
        /// 标注点显示内容
        /// </summary>
        public String Org_MarkerContent { get; set; }
        /// <summary>
        /// 主营品牌
        /// </summary>
        public String Org_MainBrands { get; set; }
        /// <summary>
        /// 主营产品
        /// </summary>
        public String Org_MainProducts { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String Org_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? Org_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String Org_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Org_CreatedTime { get; set; }
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
        public String Org_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Org_UpdatedTime { get; set; }
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
        public Int64? Org_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String Org_TransID { get; set; }
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
        public String WHERE_Org_ID { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public String WHERE_Org_MCT_ID { get; set; }
        /// <summary>
        /// 门店编码
        /// </summary>
        public String WHERE_Org_Code { get; set; }
        /// <summary>
        /// 平台编码
        /// </summary>
        public String WHERE_Org_PlatformCode { get; set; }
        /// <summary>
        /// 组织全称
        /// </summary>
        public String WHERE_Org_FullName { get; set; }
        /// <summary>
        /// 组织简称
        /// </summary>
        public String WHERE_Org_ShortName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public String WHERE_Org_Contacter { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public String WHERE_Org_TEL { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        public String WHERE_Org_PhoneNo { get; set; }
        /// <summary>
        /// 省份Code
        /// </summary>
        public String WHERE_Org_Prov_Code { get; set; }
        /// <summary>
        /// 城市Code
        /// </summary>
        public String WHERE_Org_City_Code { get; set; }
        /// <summary>
        /// 区域Code
        /// </summary>
        public String WHERE_Org_Dist_Code { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String WHERE_Org_Addr { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public String WHERE_Org_Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public String WHERE_Org_Latitude { get; set; }
        /// <summary>
        /// 标注点显示标题
        /// </summary>
        public String WHERE_Org_MarkerTitle { get; set; }
        /// <summary>
        /// 标注点显示内容
        /// </summary>
        public String WHERE_Org_MarkerContent { get; set; }
        /// <summary>
        /// 主营品牌
        /// </summary>
        public String WHERE_Org_MainBrands { get; set; }
        /// <summary>
        /// 主营产品
        /// </summary>
        public String WHERE_Org_MainProducts { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_Org_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_Org_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_Org_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_Org_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_Org_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_Org_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_Org_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_Org_TransID { get; set; }
        #endregion

    }
}
