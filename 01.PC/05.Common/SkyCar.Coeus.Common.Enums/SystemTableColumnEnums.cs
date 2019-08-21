/************************************************************************
* 文件名: XXX.SystemTableColumnEnums.cs
* ----------------------------------------------------------------------
* 文件概要:系统枚举
*
*     本文件由工具自动生成，请勿手动修改此文件！
*
* 详细:
*  [系统名]     : SkyCar
*  [子系统名]   : 无
*  [功能概要]   : 系统枚举定义
*
* 履历:
* No.   日期  　     姓名  　　内容
* 1.       Tool      自动生成，请勿修改；
* 
************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.Common.Enums
{
    /// <summary>
    /// 系统表字段枚举
    /// </summary>
    public class SystemTableColumnEnums
    {
        #region 系统字段

        public static Dictionary<string, string> Columns = new Dictionary<string, string>
        {
            {"APA_ID", "配置档案ID"},
            {"APA_Org_ID", "组织ID"},
            {"APA_Barcode", "条形码"},
            {"APA_OEMNo", "原厂编码"},
            {"APA_ThirdNo", "第三方编码"},
            {"APA_Name", "配件名称"},
            {"APA_Brand", "配件品牌"},
            {"APA_Specification", "规格型号"},
            {"APA_UOM", "计量单位"},
            {"APA_Level", "配件级别"},
            {"APA_VehicleBrand", "汽车品牌"},
            {"APA_VehicleInspire", "车系"},
            {"APA_VehicleCapacity", "排量"},
            {"APA_VehicleYearModel", "年款"},
            {"APA_VehicleGearboxTypeCode", "变速类型编码"},
            {"APA_VehicleGearboxTypeName", "变速类型名称"},
            {"APA_SUPP_ID", "默认供应商ID"},
            {"APA_WH_ID", "默认仓库ID"},
            {"APA_WHB_ID", "默认仓位ID"},
            {"APA_IsWarningSafeStock", "安全库存是否预警"},
            {"APA_SafeStockNum", "安全库存"},
            {"APA_IsWarningDeadStock", "呆滞件是否预警"},
            {"APA_SlackDays", "呆滞天数"},
            {"APA_SalePriceRate", "销价系数"},
            {"APA_SalePrice", "销价"},
            {"APA_VehicleModelCode", "车型代码"},
            {"APA_ExchangeCode", "互换码"},
            {"APA_IsValid", "有效"},
            {"APA_CreatedBy", "创建人"},
            {"APA_CreatedTime", "创建时间"},
            {"APA_UpdatedBy", "修改人"},
            {"APA_UpdatedTime", "修改时间"},
            {"APA_VersionNo", "版本号"},
            {"APA_TransID", "事务编号"},
            {"APN_ID", "配置名称ID"},
            {"APN_Name", "配件名称"},
            {"APN_Alias", "配件别名"},
            {"APN_NameSpellCode", "名称拼音简写"},
            {"APN_AliasSpellCode", "别名拼音简写"},
            {"APN_APT_ID", "配件类别ID"},
            {"APN_SlackDays", "呆滞天数"},
            {"APN_UOM", "计量单位"},
            {"APN_FixUOM", "固定计量单位"},
            {"APN_IsValid", "有效"},
            {"APN_CreatedBy", "创建人"},
            {"APN_CreatedTime", "创建时间"},
            {"APN_UpdatedBy", "修改人"},
            {"APN_UpdatedTime", "修改时间"},
            {"APN_VersionNo", "版本号"},
            {"APN_TransID", "事务编号"},
            {"VBIS_ID", "品牌车系ID"},
            {"VBIS_Brand", "品牌"},
            {"VBIS_Inspire", "车系"},
            {"VBIS_ModelDesc", "车型描述"},
            {"VBIS_Model", "车辆类型"},
            {"VBIS_BrandSpellCode", "品牌拼音首字母"},
            {"VBIS_InspireSpellCode", "车系拼音首字母"},
            {"VBIS_IsValid", "有效"},
            {"VBIS_CreatedBy", "创建人"},
            {"VBIS_CreatedTime", "创建时间"},
            {"VBIS_UpdatedBy", "修改人"},
            {"VBIS_UpdatedTime", "修改时间"},
            {"VBIS_VersionNo", "版本号"},
            {"VBIS_TransID", "事务编号"},
            {"APL_ID", "ID"},
            {"APL_Code", "编码"},
            {"APL_Name", "名称"},
            {"APL_DispayIndex", "显示顺序"},
            {"APL_Remark", "备注"},
            {"APL_IsValid", "有效"},
            {"APL_CreatedBy", "创建人"},
            {"APL_CreatedTime", "创建时间"},
            {"APL_UpdatedBy", "修改人"},
            {"APL_UpdatedTime", "修改时间"},
            {"APL_VersionNo", "版本号"},
            {"APL_TransID", "事务编号"},
            {"APT_ID", "配件类别ID"},
            {"APT_Name", "配件类别名称"},
            {"APT_ParentID", "父级类别ID"},
            {"APT_Index", "顺序"},
            {"APT_IsValid", "有效"},
            {"APT_CreatedBy", "创建人"},
            {"APT_CreatedTime", "创建时间"},
            {"APT_UpdatedBy", "修改人"},
            {"APT_UpdatedTime", "修改时间"},
            {"APT_VersionNo", "版本号"},
            {"APT_TransID", "事务编号"},
            {"APPT_ID", "配件价格类别ID"},
            {"APPT_Org_ID", "组织ID"},
            {"APPT_Name", "配件价格类别名称"},
            {"APPT_Barcode", "条形码"},
            {"APPT_Price", "价格"},
            {"APPT_IsValid", "有效"},
            {"APPT_CreatedBy", "创建人"},
            {"APPT_CreatedTime", "创建时间"},
            {"APPT_UpdatedBy", "修改人"},
            {"APPT_UpdatedTime", "修改时间"},
            {"APPT_VersionNo", "版本号"},
            {"APPT_TransID", "事务编号"},
            {"VC_ID", "ID"},
            {"VC_VIN", "车架号"},
            {"VC_PlateNumber", "车牌号"},
            {"VC_Brand", "品牌"},
            {"VC_Inspire", "车系"},
            {"VC_BrandDesc", "车型描述"},
            {"VC_Capacity", "排量"},
            {"VC_EngineType", "发动机型号"},
            {"VC_Remark", "备注"},
            {"VC_IsValid", "有效"},
            {"VC_CreatedBy", "创建人"},
            {"VC_CreatedTime", "创建时间"},
            {"VC_UpdatedBy", "修改人"},
            {"VC_UpdatedTime", "修改时间"},
            {"VC_VersionNo", "版本号"},
            {"VC_TransID", "事务编号"},
            {"VOPI_ID", "ID"},
            {"VOPI_VC_VIN", "车架号"},
            {"VOPI_OEMNo", "原厂编码"},
            {"VOPI_AutoPartsName", "配件名称"},
            {"VOPI_Remark", "备注"},
            {"VOPI_IsValid", "有效"},
            {"VOPI_CreatedBy", "创建人"},
            {"VOPI_CreatedTime", "创建时间"},
            {"VOPI_UpdatedBy", "修改人"},
            {"VOPI_UpdatedTime", "修改时间"},
            {"VOPI_VersionNo", "版本号"},
            {"VOPI_TransID", "事务编号"},
            {"VTPI_ID", "ID"},
            {"VTPI_VC_VIN", "车架号"},
            {"VTPI_ThirdNo", "第三方编码"},
            {"VTPI_AutoPartsName", "配件名称"},
            {"VTPI_AutoPartsBrand", "配件品牌"},
            {"VTPI_Remark", "备注"},
            {"VTPI_IsValid", "有效"},
            {"VTPI_CreatedBy", "创建人"},
            {"VTPI_CreatedTime", "创建时间"},
            {"VTPI_UpdatedBy", "修改人"},
            {"VTPI_UpdatedTime", "修改时间"},
            {"VTPI_VersionNo", "版本号"},
            {"VTPI_TransID", "事务编号"},
            {"VBPI_ID", "ID"},
            {"VBPI_VC_VIN", "车架号"},
            {"VBPI_VOPI_OEMNo", "原厂编码"},
            {"VBPI_ThirdNo", "第三方编码"},
            {"VBPI_AutoPartsBrand", "配件品牌"},
            {"VBPI_Remark", "备注"},
            {"VBPI_IsValid", "有效"},
            {"VBPI_CreatedBy", "创建人"},
            {"VBPI_CreatedTime", "创建时间"},
            {"VBPI_UpdatedBy", "修改人"},
            {"VBPI_UpdatedTime", "修改时间"},
            {"VBPI_VersionNo", "版本号"},
            {"VBPI_TransID", "事务编号"},
            {"CS_ID", "编码段ID"},
            {"CS_ER_ID", "编码规则ID"},
            {"CS_TypeCode", "编码段类型编码"},
            {"CS_TypeName", "编码段类型名称"},
            {"CS_Length", "长度"},
            {"CS_PanddingChar", "填充字符"},
            {"CS_PanddingStyleCode", "填充方式编码"},
            {"CS_PanddingStyleName", "填充方式名称"},
            {"CS_Value", "编码段值"},
            {"CS_Index", "顺序"},
            {"CS_IsValid", "有效"},
            {"CS_CreatedBy", "创建人"},
            {"CS_CreatedTime", "创建时间"},
            {"CS_UpdatedBy", "修改人"},
            {"CS_UpdatedTime", "修改时间"},
            {"CS_VersionNo", "版本号"},
            {"CS_TransID", "事务编号"},
            {"ER_ID", "规则ID"},
            {"ER_MasterCode", "主编码"},
            {"ER_Code", "规则编码"},
            {"ER_Name", "规则名称"},
            {"ER_Module", "模块"},
            {"ER_Remark", "说明"},
            {"ER_IsValid", "有效"},
            {"ER_CreatedBy", "创建人"},
            {"ER_CreatedTime", "创建时间"},
            {"ER_UpdatedBy", "修改人"},
            {"ER_UpdatedTime", "修改时间"},
            {"ER_VersionNo", "版本号"},
            {"ER_TransID", "事务编号"},
            {"SN_ID", "系统编号ID"},
            {"SN_ER_ID", "规则ID"},
            {"SN_Value", "编码值"},
            {"SN_Status", "状态"},
            {"SN_IsValid", "有效"},
            {"SN_CreatedBy", "创建人"},
            {"SN_CreatedTime", "创建时间"},
            {"SN_UpdatedBy", "修改人"},
            {"SN_UpdatedTime", "修改时间"},
            {"SN_VersionNo", "版本号"},
            {"SN_TransID", "事务编号"},
            {"Org_ID", "ID"},
            {"Org_MCT_ID", "商户ID"},
            {"Org_Code", "门店编码"},
            {"Org_PlatformCode", "平台编码"},
            {"Org_FullName", "组织全称"},
            {"Org_ShortName", "组织简称"},
            {"Org_Contacter", "联系人"},
            {"Org_TEL", "固定电话"},
            {"Org_PhoneNo", "移动电话"},
            {"Org_Prov_Code", "省份Code"},
            {"Org_City_Code", "城市Code"},
            {"Org_Dist_Code", "区域Code"},
            {"Org_Addr", "地址"},
            {"Org_Longitude", "经度"},
            {"Org_Latitude", "纬度"},
            {"Org_MarkerTitle", "标注点显示标题"},
            {"Org_MarkerContent", "标注点显示内容"},
            {"Org_MainBrands", "主营品牌"},
            {"Org_MainProducts", "主营产品"},
            {"Org_Remark", "备注"},
            {"Org_IsValid", "有效"},
            {"Org_CreatedBy", "创建人"},
            {"Org_CreatedTime", "创建时间"},
            {"Org_UpdatedBy", "修改人"},
            {"Org_UpdatedTime", "修改时间"},
            {"Org_VersionNo", "版本号"},
            {"Org_TransID", "事务编号"},
            {"Menu_ID", "菜单ID"},
            {"Menu_Name", "名称"},
            {"Menu_Remark", "备注"},
            {"Menu_Code", "编码"},
            {"Menu_Index", "顺序"},
            {"Menu_IsVisible", "是否可见"},
            {"Menu_IsValid", "有效"},
            {"Menu_CreatedBy", "创建人"},
            {"Menu_CreatedTime", "创建时间"},
            {"Menu_UpdatedBy", "修改人"},
            {"Menu_UpdatedTime", "修改时间"},
            {"Menu_VersionNo", "版本号"},
            {"Menu_TransID", "事务编号"},
            {"MenuG_ID", "菜单分组ID"},
            {"MenuG_Menu_ID", "菜单ID"},
            {"MenuG_Name", "名称"},
            {"MenuG_Remark", "备注"},
            {"MenuG_Code", "编码"},
            {"MenuG_Index", "顺序"},
            {"MenuG_IsVisible", "是否可见"},
            {"MenuG_IsValid", "有效"},
            {"MenuG_CreatedBy", "创建人"},
            {"MenuG_CreatedTime", "创建时间"},
            {"MenuG_UpdatedBy", "修改人"},
            {"MenuG_UpdatedTime", "修改时间"},
            {"MenuG_VersionNo", "版本号"},
            {"MenuG_TransID", "事务编号"},
            {"MenuD_ID", "菜单明细ID"},
            {"MenuD_Menu_ID", "菜单ID"},
            {"MenuD_MenuG_ID", "菜单分组ID"},
            {"MenuD_Name", "名称"},
            {"MenuD_Remark", "备注"},
            {"MenuD_Code", "编码"},
            {"MenuD_Picture", "图标"},
            {"MenuD_ImgListKey", "图标Key"},
            {"MenuD_Index", "顺序"},
            {"MenuD_ClassFullName", "类全名"},
            {"MenuD_URI", "URI"},
            {"MenuD_IsVisible", "是否可见"},
            {"MenuD_GridPageSize", "Grid页面大小"},
            {"MenuD_IsValid", "有效"},
            {"MenuD_CreatedBy", "创建人"},
            {"MenuD_CreatedTime", "创建时间"},
            {"MenuD_UpdatedBy", "修改人"},
            {"MenuD_UpdatedTime", "修改时间"},
            {"MenuD_VersionNo", "版本号"},
            {"MenuD_TransID", "事务编号"},
            {"Act_ID", "动作ID"},
            {"Act_Key", "Key"},
            {"Act_Name", "名称"},
            {"Act_Index", "顺序"},
            {"Act_IsDisplayInUI", "是否显示到界面"},
            {"Act_IsValid", "有效"},
            {"Act_CreatedBy", "创建人"},
            {"Act_CreatedTime", "创建时间"},
            {"Act_UpdatedBy", "修改人"},
            {"Act_UpdatedTime", "修改时间"},
            {"Act_VersionNo", "版本号"},
            {"Act_TransID", "事务编号"},
            {"MDA_ID", "菜单明细动作ID"},
            {"MDA_MenuD_ID", "菜单明细ID"},
            {"MDA_Action_ID", "动作ID"},
            {"MDA_IsValid", "有效"},
            {"MDA_CreatedBy", "创建人"},
            {"MDA_CreatedTime", "创建时间"},
            {"MDA_UpdatedBy", "修改人"},
            {"MDA_UpdatedTime", "修改时间"},
            {"MDA_VersionNo", "版本号"},
            {"MDA_TransID", "事务编号"},
            {"UMA_ID", "用户菜单权限ID"},
            {"UMA_User_ID", "用户ID"},
            {"UMA_Org_ID", "组织ID"},
            {"UMA_MenuD_ID", "菜单明细ID"},
            {"UMA_IsValid", "有效"},
            {"UMA_CreatedBy", "创建人"},
            {"UMA_CreatedTime", "创建时间"},
            {"UMA_UpdatedBy", "修改人"},
            {"UMA_UpdatedTime", "修改时间"},
            {"UMA_VersionNo", "版本号"},
            {"UMA_TransID", "事务编号"},
            {"UAA_ID", "用户菜单动作权限ID"},
            {"UAA_User_ID", "用户ID"},
            {"UAA_Org_ID", "组织ID"},
            {"UAA_MenuD_ID", "菜单明细ID"},
            {"UAA_Action_ID", "动作ID"},
            {"UAA_IsValid", "有效"},
            {"UAA_CreatedBy", "创建人"},
            {"UAA_CreatedTime", "创建时间"},
            {"UAA_UpdatedBy", "修改人"},
            {"UAA_UpdatedTime", "修改时间"},
            {"UAA_VersionNo", "版本号"},
            {"UAA_TransID", "事务编号"},
            {"User_ID", "用户ID"},
            {"User_Name", "姓名"},
            {"User_Password", "密码"},
            {"User_EMPNO", "工号"},
            {"User_IDNo", "身份证号码"},
            {"User_Sex", "性别"},
            {"User_Address", "地址"},
            {"User_PhoneNo", "电话号码"},
            {"User_IsAllowWechatCertificate", "是否允许微信认证"},
            {"User_IsWechatCertified", "是否已微信认证"},
            {"User_PrintTitlePrefix", "打印标题前缀"},
            {"User_IsValid", "有效"},
            {"User_CreatedBy", "创建人"},
            {"User_CreatedTime", "创建时间"},
            {"User_UpdatedBy", "修改人"},
            {"User_UpdatedTime", "修改时间"},
            {"User_VersionNo", "版本号"},
            {"User_TransID", "事务编号"},
            {"ULL_ID", "ID"},
            {"ULL_User_ID", "用户ID"},
            {"ULL_User_Name", "用户名"},
            {"ULL_IPAdress", "IP地址"},
            {"ULL_MACAdress", "MAC地址"},
            {"ULL_HostName", "主机名称"},
            {"ULL_OrgID", "组织ID"},
            {"ULL_OrgName", "组织名称"},
            {"ULL_LogType", "日志类型"},
            {"ULL_TerminalType", "终端类型"},
            {"ULL_LogTime", "发生时间"},
            {"ULL_IsValid", "有效"},
            {"ULL_CreatedBy", "创建人"},
            {"ULL_CreatedTime", "创建时间"},
            {"ULL_UpdatedBy", "修改人"},
            {"ULL_UpdatedTime", "修改时间"},
            {"ULL_VersionNo", "版本号"},
            {"ULL_TransID", "事务编号"},
            {"UO_ID", "用户组织ID"},
            {"UO_User_ID", "用户ID"},
            {"UO_Org_ID", "组织ID"},
            {"UO_IsValid", "有效"},
            {"UO_CreatedBy", "创建人"},
            {"UO_CreatedTime", "创建时间"},
            {"UO_UpdatedBy", "修改人"},
            {"UO_UpdatedTime", "修改时间"},
            {"UO_VersionNo", "版本号"},
            {"UO_TransID", "事务编号"},
            {"UBR_ID", "ID"},
            {"UBR_User_ID", "用户ID"},
            {"UBR_Org_ID", "组织ID"},
            {"UBR_Org_Name", "组织名称"},
            {"UBR_GroupName", "班组名称"},
            {"UBR_BusinessRole", "业务角色"},
            {"UBR_Jobs", "业务工种"},
            {"UBR_CertType", "行业证件类型"},
            {"UBR_CertNo", "证件号码"},
            {"UBR_TecLevel", "能力级别"},
            {"UBR_PerformanceRatio", "绩效系数"},
            {"UBR_IsSuitableForApp", "是否适用于APP"},
            {"UBR_IsValid", "有效"},
            {"UBR_CreatedBy", "创建人"},
            {"UBR_CreatedTime", "创建时间"},
            {"UBR_UpdatedBy", "修改人"},
            {"UBR_UpdatedTime", "修改时间"},
            {"UBR_VersionNo", "版本号"},
            {"UBR_TransID", "事务编号"},
            {"UJA_ID", "用户作业权限ID"},
            {"UJA_User_ID", "用户ID"},
            {"UJA_Org_ID", "组织"},
            {"UJA_BJ_ID", "作业ID"},
            {"UJA_IsValid", "有效"},
            {"UJA_CreatedBy", "创建人"},
            {"UJA_CreatedTime", "创建时间"},
            {"UJA_UpdatedBy", "修改人"},
            {"UJA_UpdatedTime", "修改时间"},
            {"UJA_VersionNo", "版本号"},
            {"UJA_TransID", "事务编号"},
            {"ASAH_ID", "ID"},
            {"ASAH_ARMerchant_Code", "汽修商户编码"},
            {"ASAH_ARMerchant_Name", "汽修商户名称"},
            {"ASAH_AROrg_Code", "汽修组织编码"},
            {"ASAH_AROrg_Name", "汽修组织名称"},
            {"ASAH_AROrg_Contacter", "汽修组织联系人"},
            {"ASAH_AROrg_Phone", "汽修组织联系方式"},
            {"ASAH_Remark", "备注"},
            {"ASAH_IsValid", "有效"},
            {"ASAH_CreatedBy", "创建人"},
            {"ASAH_CreatedTime", "创建时间"},
            {"ASAH_UpdatedBy", "修改人"},
            {"ASAH_UpdatedTime", "修改时间"},
            {"ASAH_VersionNo", "版本号"},
            {"ASAH_TransID", "事务编号"},
            {"ASOAH_ID", "ID"},
            {"ASOAH_SupOrg_ID", "汽配组织ID"},
            {"ASOAH_AFC_ID", "汽修商客户ID"},
            {"ASOAH_ARMerchant_Code", "汽修商户编码"},
            {"ASOAH_ARMerchant_Name", "汽修商户名称"},
            {"ASOAH_AROrg_Code", "汽修组织编码"},
            {"ASOAH_AROrg_Name", "汽修组织名称"},
            {"ASOAH_Remark", "备注"},
            {"ASOAH_IsValid", "有效"},
            {"ASOAH_CreatedBy", "创建人"},
            {"ASOAH_CreatedTime", "创建时间"},
            {"ASOAH_UpdatedBy", "修改人"},
            {"ASOAH_UpdatedTime", "修改时间"},
            {"ASOAH_VersionNo", "版本号"},
            {"ASOAH_TransID", "事务编号"},
            {"CUL_ID", "许可证ID"},
            {"CUL_No", "许可证号"},
            {"CUL_Org_ID", "组织ID"},
            {"CUL_Org_Name", "组织名称"},
            {"CUL_Name", "用户姓名"},
            {"CUL_ApplyReason", "申请原因"},
            {"CUL_ContactNo", "联系方式"},
            {"CUL_NetworkCardType1", "网卡类型1"},
            {"CUL_MACAdress1", "网卡地址1"},
            {"CUL_NetworkCardType2", "网卡类型2"},
            {"CUL_MACAdress2", "网卡地址2"},
            {"CUL_NetworkCardType3", "网卡类型3"},
            {"CUL_MACAdress3", "网卡地址3"},
            {"CUL_ApproveStatus", "审核状态"},
            {"CUL_InvalidDate", "失效日期"},
            {"CUL_Remark", "备注"},
            {"CUL_IsValid", "有效"},
            {"CUL_CreatedBy", "创建人"},
            {"CUL_CreatedTime", "创建时间"},
            {"CUL_UpdatedBy", "修改人"},
            {"CUL_UpdatedTime", "修改时间"},
            {"CUL_VersionNo", "版本号"},
            {"CUL_TransID", "事务编号"},
            {"Para_ID", "参数ID"},
            {"Para_Code1", "参数编码1"},
            {"Para_Name1", "参数描述1"},
            {"Para_Value1", "参数值1"},
            {"Para_Code2", "参数编码2"},
            {"Para_Name2", "参数描述2"},
            {"Para_Value2", "参数值2"},
            {"Para_Code3", "参数编码3"},
            {"Para_Name3", "参数描述3"},
            {"Para_Value3", "参数值3"},
            {"Para_Code4", "参数编码4"},
            {"Para_Name4", "参数描述4"},
            {"Para_Value4", "参数值4"},
            {"Para_Code5", "参数编码5"},
            {"Para_Name5", "参数描述5"},
            {"Para_Value5", "参数值5"},
            {"Para_IsValid", "有效"},
            {"Para_CreatedBy", "创建人"},
            {"Para_CreatedTime", "创建时间"},
            {"Para_UpdatedBy", "修改人"},
            {"Para_UpdatedTime", "修改时间"},
            {"Para_VersionNo", "版本号"},
            {"Para_TransID", "事务编号"},
            {"CT_ID", "ID"},
            {"CT_Type", "类型"},
            {"CT_Name", "参数名称"},
            {"CT_Value", "参数值"},
            {"CT_Desc", "参数描述"},
            {"CT_IsValid", "有效"},
            {"CT_CreatedBy", "创建人"},
            {"CT_CreatedTime", "创建时间"},
            {"CT_UpdatedBy", "修改人"},
            {"CT_UpdatedTime", "修改时间"},
            {"CT_VersionNo", "版本号"},
            {"CT_TransID", "事务编号"},
            {"Enum_ID", "枚举ID"},
            {"Enum_Key", "枚举Key"},
            {"Enum_Name", "枚举名称"},
            {"Enum_ValueCode", "枚举值编码"},
            {"Enum_Value", "枚举值"},
            {"Enum_DisplayName", "枚举显示名称"},
            {"Enum_Info", "备注"},
            {"Enum_IsValid", "有效"},
            {"Enum_CreatedBy", "创建人"},
            {"Enum_CreatedTime", "创建时间"},
            {"Enum_UpdatedBy", "修改人"},
            {"Enum_UpdatedTime", "修改时间"},
            {"Enum_VersionNo", "版本号"},
            {"Enum_TransID", "事务编号"},
            {"Msg_ID", "消息ID"},
            {"Msg_Code", "编码"},
            {"Msg_Content", "内容"},
            {"Msg_IsValid", "有效"},
            {"Msg_CreatedBy", "创建人"},
            {"Msg_CreatedTime", "创建时间"},
            {"Msg_UpdatedBy", "修改人"},
            {"Msg_UpdatedTime", "修改时间"},
            {"Msg_VersionNo", "版本号"},
            {"Msg_TransID", "事务编号"},
            {"PM_ID", "ID"},
            {"PM_MCT_ID", "推送商户ID"},
            {"PM_SP_ID", "推送产品ID"},
            {"PM_Content", "推送内容"},
            {"PM_Sender", "发送人"},
            {"PM_Receiver", "接受人"},
            {"PM_PushType", "推送方式"},
            {"PM_SendTime", "发送时间"},
            {"PM_ReadTime", "查看时间"},
            {"PM_Remark", "备注"},
            {"PM_IsValid", "有效"},
            {"PM_CreatedBy", "创建人"},
            {"PM_CreatedTime", "创建时间"},
            {"PM_UpdatedBy", "修改人"},
            {"PM_UpdatedTime", "修改时间"},
            {"PM_VersionNo", "版本号"},
            {"PM_TransID", "事务编号"},
            {"Reg_ID", "ID"},
            {"Reg_Code", "编码"},
            {"Reg_Name", "名称"},
            {"Reg_ShortName", "简称"},
            {"Reg_Index", "顺序"},
            {"Reg_IsValid", "有效"},
            {"Reg_CreatedBy", "创建人"},
            {"Reg_CreatedTime", "创建时间"},
            {"Reg_UpdatedBy", "修改人"},
            {"Reg_UpdatedTime", "修改时间"},
            {"Reg_VersionNo", "版本号"},
            {"Reg_TransID", "事务编号"},
            {"Prov_ID", "ID"},
            {"Prov_Reg_ID", "大区ID"},
            {"Prov_Code", "编码"},
            {"Prov_Name", "名称"},
            {"Prov_ShortName", "简称"},
            {"Prov_Index", "顺序"},
            {"Prov_IsValid", "有效"},
            {"Prov_CreatedBy", "创建人"},
            {"Prov_CreatedTime", "创建时间"},
            {"Prov_UpdatedBy", "修改人"},
            {"Prov_UpdatedTime", "修改时间"},
            {"Prov_VersionNo", "版本号"},
            {"Prov_TransID", "事务编号"},
            {"City_ID", "ID"},
            {"City_Reg_ID", "大区ID"},
            {"City_Code", "城市编码"},
            {"City_Name", "名称"},
            {"City_PlateCode", "车牌编码"},
            {"City_Prov_Code", "省份编码"},
            {"City_Index", "顺序"},
            {"City_IsValid", "有效"},
            {"City_CreatedBy", "创建人"},
            {"City_CreatedTime", "创建时间"},
            {"City_UpdatedBy", "修改人"},
            {"City_UpdatedTime", "修改时间"},
            {"City_VersionNo", "版本号"},
            {"City_TransID", "事务编号"},
            {"Dist_ID", "ID"},
            {"Dist_Code", "区域编码"},
            {"Dist_Name", "名称"},
            {"Dist_City_Code", "城市编码"},
            {"Dist_Index", "顺序"},
            {"Dist_IsValid", "有效"},
            {"Dist_CreatedBy", "创建人"},
            {"Dist_CreatedTime", "创建时间"},
            {"Dist_UpdatedBy", "修改人"},
            {"Dist_UpdatedTime", "修改时间"},
            {"Dist_VersionNo", "版本号"},
            {"Dist_TransID", "事务编号"},
            {"PB_ID", "付款ID"},
            {"PB_No", "付款单号"},
            {"PB_Pay_Org_ID", "付款组织ID"},
            {"PB_Pay_Org_Name", "付款组织名称"},
            {"PB_Date", "付款日期"},
            {"PB_RecObjectTypeCode", "收款对象类型编码"},
            {"PB_RecObjectTypeName", "收款对象类型名称"},
            {"PB_RecObjectID", "收款对象ID"},
            {"PB_RecObjectName", "收款对象"},
            {"PB_PayableTotalAmount", "应付合计金额"},
            {"PB_RealPayableTotalAmount", "实付合计金额"},
            {"PB_PayAccount", "付款方账号"},
            {"PB_RecAccount", "收款方账号"},
            {"PB_PayTypeCode", "付款方式编码"},
            {"PB_PayTypeName", "付款方式名称"},
            {"PB_CertificateNo", "付款凭证编号"},
            {"PB_CertificatePic", "付款凭证图片"},
            {"PB_BusinessStatusCode", "业务状态编码"},
            {"PB_BusinessStatusName", "业务状态名称"},
            {"PB_ApprovalStatusCode", "审核状态编码"},
            {"PB_ApprovalStatusName", "审核状态名称"},
            {"PB_Remark", "备注"},
            {"PB_IsValid", "有效"},
            {"PB_CreatedBy", "创建人"},
            {"PB_CreatedTime", "创建时间"},
            {"PB_UpdatedBy", "修改人"},
            {"PB_UpdatedTime", "修改时间"},
            {"PB_VersionNo", "版本号"},
            {"PB_TransID", "事务编号"},
            {"PBD_ID", "付款单明细ID"},
            {"PBD_PB_ID", "付款单ID"},
            {"PBD_PB_No", "付款单号"},
            {"PBD_SourceTypeCode", "来源类型编码"},
            {"PBD_SourceTypeName", "来源类型名称"},
            {"PBD_SrcBillNo", "来源单号"},
            {"PBD_PayAmount", "付款金额"},
            {"PBD_Remark", "备注"},
            {"PBD_IsValid", "有效"},
            {"PBD_CreatedBy", "创建人"},
            {"PBD_CreatedTime", "创建时间"},
            {"PBD_UpdatedBy", "修改人"},
            {"PBD_UpdatedTime", "修改时间"},
            {"PBD_VersionNo", "版本号"},
            {"PBD_TransID", "事务编号"},
            {"RB_ID", "收款ID"},
            {"RB_No", "收款单号"},
            {"RB_Rec_Org_ID", "收款组织ID"},
            {"RB_Rec_Org_Name", "收款组织名称"},
            {"RB_Date", "收款日期"},
            {"RB_PayObjectTypeCode", "付款对象类型编码"},
            {"RB_PayObjectTypeName", "付款对象类型名称"},
            {"RB_PayObjectID", "付款对象ID"},
            {"RB_PayObjectName", "付款对象"},
            {"RB_ReceiveTypeCode", "收款通道编码"},
            {"RB_ReceiveTypeName", "收款通道名称"},
            {"RB_ReceiveAccount", "收款账号"},
            {"RB_CertificateNo", "收款凭证编号"},
            {"RB_CertificatePic", "收款凭证图片"},
            {"RB_ReceiveTotalAmount", "合计金额"},
            {"RB_BusinessStatusCode", "业务状态编码"},
            {"RB_BusinessStatusName", "业务状态名称"},
            {"RB_ApprovalStatusCode", "审核状态编码"},
            {"RB_ApprovalStatusName", "审核状态名称"},
            {"RB_Remark", "备注"},
            {"RB_IsValid", "有效"},
            {"RB_CreatedBy", "创建人"},
            {"RB_CreatedTime", "创建时间"},
            {"RB_UpdatedBy", "修改人"},
            {"RB_UpdatedTime", "修改时间"},
            {"RB_VersionNo", "版本号"},
            {"RB_TransID", "事务编号"},
            {"RBD_ID", "收款单明细ID"},
            {"RBD_RB_ID", "收款单ID"},
            {"RBD_RB_No", "收款单号"},
            {"RBD_SourceTypeCode", "来源类型编码"},
            {"RBD_SourceTypeName", "来源类型名称"},
            {"RBD_SrcBillNo", "来源单号"},
            {"RBD_ReceiveAmount", "收款金额"},
            {"RBD_Remark", "备注"},
            {"RBD_IsValid", "有效"},
            {"RBD_CreatedBy", "创建人"},
            {"RBD_CreatedTime", "创建时间"},
            {"RBD_UpdatedBy", "修改人"},
            {"RBD_UpdatedTime", "修改时间"},
            {"RBD_VersionNo", "版本号"},
            {"RBD_TransID", "事务编号"},
            {"APB_ID", "应付单ID"},
            {"APB_No", "应付单号"},
            {"APB_BillDirectCode", "单据方向编码"},
            {"APB_BillDirectName", "单据方向名称"},
            {"APB_SourceTypeCode", "来源类型"},
            {"APB_SourceTypeName", "来源类型"},
            {"APB_SourceBillNo", "来源单据号"},
            {"APB_Org_ID", "组织ID"},
            {"APB_Org_Name", "组织名称"},
            {"APB_ReceiveObjectTypeCode", "收款对象类型编码"},
            {"APB_ReceiveObjectTypeName", "收款对象类型名称"},
            {"APB_ReceiveObjectID", "收款对象ID"},
            {"APB_ReceiveObjectName", "收款对象名称"},
            {"APB_AccountPayableAmount", "应付金额"},
            {"APB_PaidAmount", "已付金额"},
            {"APB_UnpaidAmount", "未付金额"},
            {"APB_BusinessStatusCode", "业务状态编码"},
            {"APB_BusinessStatusName", "业务状态名称"},
            {"APB_ApprovalStatusCode", "审核状态编码"},
            {"APB_ApprovalStatusName", "审核状态名称"},
            {"APB_ReconciliationTime", "对账时间"},
            {"APB_Remark", "备注"},
            {"APB_IsValid", "有效"},
            {"APB_CreatedBy", "创建人"},
            {"APB_CreatedTime", "创建时间"},
            {"APB_UpdatedBy", "修改人"},
            {"APB_UpdatedTime", "修改时间"},
            {"APB_VersionNo", "版本号"},
            {"APB_TransID", "事务编号"},
            {"APBD_ID", "应付单明细ID"},
            {"APBD_APB_ID", "应付单ID"},
            {"APBD_IsMinusDetail", "是否负向明细"},
            {"APBD_SourceBillNo", "来源单据号"},
            {"APBD_SourceBillDetailID", "来源单据明细ID"},
            {"APBD_Org_ID", "组织ID"},
            {"APBD_Org_Name", "组织名称"},
            {"APBD_AccountPayableAmount", "应付金额"},
            {"APBD_PaidAmount", "已付金额"},
            {"APBD_UnpaidAmount", "未付金额"},
            {"APBD_BusinessStatusCode", "业务状态编码"},
            {"APBD_BusinessStatusName", "业务状态名称"},
            {"APBD_ApprovalStatusCode", "审核状态编码"},
            {"APBD_ApprovalStatusName", "审核状态名称"},
            {"APBD_Remark", "备注"},
            {"APBD_IsValid", "有效"},
            {"APBD_CreatedBy", "创建人"},
            {"APBD_CreatedTime", "创建时间"},
            {"APBD_UpdatedBy", "修改人"},
            {"APBD_UpdatedTime", "修改时间"},
            {"APBD_VersionNo", "版本号"},
            {"APBD_TransID", "事务编号"},
            {"APBL_ID", "应付单日志ID"},
            {"APBL_APB_ID", "应付单ID"},
            {"APBL_APBD_ID", "应付单明细ID"},
            {"APBL_Org_ID", "组织ID"},
            {"APBL_Org_Name", "组织名称"},
            {"APBL_OperateTypeCode", "操作类型编码"},
            {"APBL_OperateTypeName", "操作类型名称"},
            {"APBL_APBD_VersionNo", "应付单明细版本号"},
            {"APBL_APAmount", "应付金额"},
            {"APBL_PaidAmount", "已付金额"},
            {"APBL_UnpaidAmount", "未付金额"},
            {"APBL_Remark", "备注"},
            {"APBL_IsValid", "有效"},
            {"APBL_CreatedBy", "创建人"},
            {"APBL_CreatedTime", "创建时间"},
            {"APBL_UpdatedBy", "修改人"},
            {"APBL_UpdatedTime", "修改时间"},
            {"APBL_VersionNo", "版本号"},
            {"APBL_TransID", "事务编号"},
            {"ARB_ID", "应收单ID"},
            {"ARB_No", "应收单号"},
            {"ARB_BillDirectCode", "单据方向编码"},
            {"ARB_BillDirectName", "单据方向名称"},
            {"ARB_SourceTypeCode", "来源类型"},
            {"ARB_SourceTypeName", "来源类型"},
            {"ARB_SrcBillNo", "来源单据号"},
            {"ARB_Org_ID", "组织ID"},
            {"ARB_Org_Name", "组织名称"},
            {"ARB_PayObjectTypeCode", "付款对象类型编码"},
            {"ARB_PayObjectTypeName", "付款对象类型名称"},
            {"ARB_PayObjectID", "付款对象ID"},
            {"ARB_PayObjectName", "付款对象名称"},
            {"ARB_AccountReceivableAmount", "应收金额"},
            {"ARB_ReceivedAmount", "已收金额"},
            {"ARB_UnReceiveAmount", "未收金额"},
            {"ARB_BusinessStatusCode", "业务状态编码"},
            {"ARB_BusinessStatusName", "业务状态名称"},
            {"ARB_ApprovalStatusCode", "审核状态编码"},
            {"ARB_ApprovalStatusName", "审核状态名称"},
            {"ARB_ReconciliationTime", "对账时间"},
            {"ARB_Remark", "备注"},
            {"ARB_IsValid", "有效"},
            {"ARB_CreatedBy", "创建人"},
            {"ARB_CreatedTime", "创建时间"},
            {"ARB_UpdatedBy", "修改人"},
            {"ARB_UpdatedTime", "修改时间"},
            {"ARB_VersionNo", "版本号"},
            {"ARB_TransID", "事务编号"},
            {"ARBD_ID", "应收单明细ID"},
            {"ARBD_ARB_ID", "应收单ID"},
            {"ARBD_IsMinusDetail", "是否负向明细"},
            {"ARBD_SrcBillNo", "来源单据号"},
            {"ARBD_SrcBillDetailID", "来源单据明细ID"},
            {"ARBD_Org_ID", "组织ID"},
            {"ARBD_Org_Name", "组织名称"},
            {"ARBD_AccountReceivableAmount", "应收金额"},
            {"ARBD_ReceivedAmount", "已收金额"},
            {"ARBD_UnReceiveAmount", "未收金额"},
            {"ARBD_BusinessStatusCode", "业务状态编码"},
            {"ARBD_BusinessStatusName", "业务状态名称"},
            {"ARBD_ApprovalStatusCode", "审核状态编码"},
            {"ARBD_ApprovalStatusName", "审核状态名称"},
            {"ARBD_Remark", "备注"},
            {"ARBD_IsValid", "有效"},
            {"ARBD_CreatedBy", "创建人"},
            {"ARBD_CreatedTime", "创建时间"},
            {"ARBD_UpdatedBy", "修改人"},
            {"ARBD_UpdatedTime", "修改时间"},
            {"ARBD_VersionNo", "版本号"},
            {"ARBD_TransID", "事务编号"},
            {"ARBL_ID", "应收单日志ID"},
            {"ARBL_ARB_ID", "应收单ID"},
            {"ARBL_ARBD_ID", "应收单明细ID"},
            {"ARBL_Org_ID", "组织ID"},
            {"ARBL_Org_Name", "组织名称"},
            {"ARBL_OperateTypeCode", "操作类型编码"},
            {"ARBL_OperateTypeName", "操作类型名称"},
            {"ARBL_APBD_VersionNo", "应收单明细版本号"},
            {"ARBL_ARAmount", "应收金额"},
            {"ARBL_ReceivedAmount", "已收金额"},
            {"ARBL_UnReceiveAmount", "未收金额"},
            {"ARBL_Remark", "备注"},
            {"ARBL_IsValid", "有效"},
            {"ARBL_CreatedBy", "创建人"},
            {"ARBL_CreatedTime", "创建时间"},
            {"ARBL_UpdatedBy", "修改人"},
            {"ARBL_UpdatedTime", "修改时间"},
            {"ARBL_VersionNo", "版本号"},
            {"ARBL_TransID", "事务编号"},
            {"PO_ID", "采购订单ID"},
            {"PO_No", "订单号"},
            {"PO_Org_ID", "组织ID"},
            {"PO_SUPP_ID", "供应商ID"},
            {"PO_SUPP_Name", "供应商名称"},
            {"PO_SourceTypeCode", "来源类型编码"},
            {"PO_SourceTypeName", "来源类型名称"},
            {"PO_SourceNo", "来源单号"},
            {"PO_TotalAmount", "订单总额"},
            {"PO_LogisticFee", "物流费"},
            {"PO_StatusCode", "单据状态编码"},
            {"PO_StatusName", "单据状态名称"},
            {"PO_ApprovalStatusCode", "审核状态编码"},
            {"PO_ApprovalStatusName", "审核状态名称"},
            {"PO_ReceivedTime", "到货时间"},
            {"PO_IsValid", "有效"},
            {"PO_CreatedBy", "创建人"},
            {"PO_CreatedTime", "创建时间"},
            {"PO_UpdatedBy", "修改人"},
            {"PO_UpdatedTime", "修改时间"},
            {"PO_VersionNo", "版本号"},
            {"PO_TransID", "事务编号"},
            {"POD_ID", "采购订单明细ID"},
            {"POD_PO_ID", "采购订单ID"},
            {"POD_PO_No", "订单号"},
            {"POD_AutoPartsBarcode", "配件条码"},
            {"POD_ThirdCode", "第三方编码"},
            {"POD_OEMCode", "原厂编码"},
            {"POD_AutoPartsName", "配件名称"},
            {"POD_AutoPartsBrand", "配件品牌"},
            {"POD_AutoPartsSpec", "规格型号"},
            {"POD_AutoPartsLevel", "配件级别"},
            {"POD_UOM", "计量单位"},
            {"POD_VehicleBrand", "汽车品牌"},
            {"POD_VehicleInspire", "车系"},
            {"POD_VehicleCapacity", "排量"},
            {"POD_VehicleYearModel", "年款"},
            {"POD_VehicleGearboxType", "变速类型"},
            {"POD_WH_ID", "进货仓库ID"},
            {"POD_WHB_ID", "进货仓位ID"},
            {"POD_OrderQty", "订货数量"},
            {"POD_ReceivedQty", "签收数量"},
            {"POD_UnitPrice", "订货单价"},
            {"POD_StatusCode", "单据状态编码"},
            {"POD_StatusName", "单据状态名称"},
            {"POD_ReceivedTime", "到货时间"},
            {"POD_IsValid", "有效"},
            {"POD_CreatedBy", "创建人"},
            {"POD_CreatedTime", "创建时间"},
            {"POD_UpdatedBy", "修改人"},
            {"POD_UpdatedTime", "修改时间"},
            {"POD_VersionNo", "版本号"},
            {"POD_TransID", "事务编号"},
            {"PFO_ID", "采购预测订单ID"},
            {"PFO_No", "单号"},
            {"PFO_Org_ID", "组织ID"},
            {"PFO_SUPP_ID", "供应商ID"},
            {"PFO_SUPP_Name", "供应商名称"},
            {"PFO_SourceTypeCode", "来源类型编码"},
            {"PFO_SourceTypeName", "来源类型名称"},
            {"PFO_TotalAmount", "订单总额"},
            {"PFO_StatusCode", "单据状态编码"},
            {"PFO_StatusName", "单据状态名称"},
            {"PFO_Remark", "备注"},
            {"PFO_IsValid", "有效"},
            {"PFO_CreatedBy", "创建人"},
            {"PFO_CreatedTime", "创建时间"},
            {"PFO_UpdatedBy", "修改人"},
            {"PFO_UpdatedTime", "修改时间"},
            {"PFO_VersionNo", "版本号"},
            {"PFO_TransID", "事务编号"},
            {"PFOD_ID", "采购预测订单明细ID"},
            {"PFOD_PFO_ID", "采购预测订单ID"},
            {"PFOD_PFO_No", "订单号"},
            {"PFOD_AutoPartsBarcode", "配件条码"},
            {"PFOD_ThirdCode", "第三方编码"},
            {"PFOD_OEMCode", "原厂编码"},
            {"PFOD_AutoPartsName", "配件名称"},
            {"PFOD_AutoPartsBrand", "配件品牌"},
            {"PFOD_AutoPartsSpec", "规格型号"},
            {"PFOD_AutoPartsLevel", "配件级别"},
            {"PFOD_UOM", "计量单位"},
            {"PFOD_VehicleBrand", "汽车品牌"},
            {"PFOD_VehicleInspire", "车系"},
            {"PFOD_VehicleCapacity", "排量"},
            {"PFOD_VehicleYearModel", "年款"},
            {"PFOD_VehicleGearboxType", "变速类型"},
            {"PFOD_Qty", "数量"},
            {"PFOD_LastUnitPrice", "最后一次采购单价"},
            {"PFOD_IsValid", "有效"},
            {"PFOD_CreatedBy", "创建人"},
            {"PFOD_CreatedTime", "创建时间"},
            {"PFOD_UpdatedBy", "修改人"},
            {"PFOD_UpdatedTime", "修改时间"},
            {"PFOD_VersionNo", "版本号"},
            {"PFOD_TransID", "事务编号"},
            {"WH_ID", "仓库ID"},
            {"WH_No", "仓库编号"},
            {"WH_Name", "仓库名称"},
            {"WH_Org_ID", "组织ID"},
            {"WH_Address", "仓库地址"},
            {"WH_Description", "仓库描述"},
            {"WH_IsValid", "有效"},
            {"WH_CreatedBy", "创建人"},
            {"WH_CreatedTime", "创建时间"},
            {"WH_UpdatedBy", "修改人"},
            {"WH_UpdatedTime", "修改时间"},
            {"WH_VersionNo", "版本号"},
            {"WH_TransID", "事务编号"},
            {"WHB_ID", "仓位ID"},
            {"WHB_WH_ID", "仓库ID"},
            {"WHB_Name", "仓位名称"},
            {"WHB_Description", "仓位描述"},
            {"WHB_IsValid", "有效"},
            {"WHB_CreatedBy", "创建人"},
            {"WHB_CreatedTime", "创建时间"},
            {"WHB_UpdatedBy", "修改人"},
            {"WHB_UpdatedTime", "修改时间"},
            {"WHB_VersionNo", "版本号"},
            {"WHB_TransID", "事务编号"},
            {"SOB_ID", "出库单ID"},
            {"SOB_Org_ID", "组织ID"},
            {"SOB_No", "单号"},
            {"SOB_SourceTypeCode", "来源类型编码"},
            {"SOB_SourceTypeName", "来源类型名称"},
            {"SOB_SourceNo", "来源单号"},
            {"SOB_SUPP_ID", "供应商ID"},
            {"SOB_SUPP_Name", "供应商名称"},
            {"SOB_StatusCode", "单据状态编码"},
            {"SOB_StatusName", "单据状态名称"},
            {"SOB_ApprovalStatusCode", "审核状态编码"},
            {"SOB_ApprovalStatusName", "审核状态名称"},
            {"SOB_Remark", "备注"},
            {"SOB_IsValid", "有效"},
            {"SOB_CreatedBy", "创建人"},
            {"SOB_CreatedTime", "创建时间"},
            {"SOB_UpdatedBy", "修改人"},
            {"SOB_UpdatedTime", "修改时间"},
            {"SOB_VersionNo", "版本号"},
            {"SOB_TransID", "事务编号"},
            {"SOBD_ID", "出库单明细ID"},
            {"SOBD_SOB_ID", "出库单ID"},
            {"SOBD_SOB_No", "出库单号"},
            {"SOBD_SourceDetailID", "来源单明细ID"},
            {"SOBD_Barcode", "配件条码"},
            {"SOBD_BatchNo", "配件批次号"},
            {"SOBD_ThirdNo", "第三方编码"},
            {"SOBD_OEMNo", "原厂编码"},
            {"SOBD_Name", "配件名称"},
            {"SOBD_Specification", "配件规格型号"},
            {"SOBD_WH_ID", "仓库ID"},
            {"SOBD_WHB_ID", "仓位ID"},
            {"SOBD_UnitCostPrice", "进货单价"},
            {"SOBD_Qty", "出库数量"},
            {"SOBD_UOM", "单位"},
            {"SOBD_UnitSalePrice", "销售单价"},
            {"SOBD_Amount", "金额"},
            {"SOBD_IsValid", "有效"},
            {"SOBD_CreatedBy", "创建人"},
            {"SOBD_CreatedTime", "创建时间"},
            {"SOBD_UpdatedBy", "修改人"},
            {"SOBD_UpdatedTime", "修改时间"},
            {"SOBD_VersionNo", "版本号"},
            {"SOBD_TransID", "事务编号"},
            {"SUPP_ID", "ID"},
            {"SUPP_Code", "编码"},
            {"SUPP_Name", "名称"},
            {"SUPP_ShortName", "简称"},
            {"SUPP_Contacter", "联系人"},
            {"SUPP_Tel", "固定号码"},
            {"SUPP_Phone", "电话号码"},
            {"SUPP_QQ", "QQ号码"},
            {"SUPP_Territory", "地区"},
            {"SUPP_Prov_Code", "省"},
            {"SUPP_City_Code", "市"},
            {"SUPP_Dist_Code", "区"},
            {"SUPP_Address", "地址"},
            {"SUPP_EvaluateLevel", "评估等级"},
            {"SUPP_LastEvaluateDate", "最近评估日"},
            {"SUPP_BankName", "开户行"},
            {"SUPP_BankAccountName", "开户名"},
            {"SUPP_BankAccountNo", "账号"},
            {"SUPP_MainAutoParts", "主营配件"},
            {"SUPP_Remark", "备注"},
            {"SUPP_IsValid", "有效"},
            {"SUPP_CreatedBy", "创建人"},
            {"SUPP_CreatedTime", "创建时间"},
            {"SUPP_UpdatedBy", "修改人"},
            {"SUPP_UpdatedTime", "修改时间"},
            {"SUPP_VersionNo", "版本号"},
            {"SUPP_TransID", "事务编号"},
            {"INV_ID", "ID"},
            {"INV_Org_ID", "组织ID"},
            {"INV_WH_ID", "仓库ID"},
            {"INV_WHB_ID", "仓位ID"},
            {"INV_ThirdNo", "第三方编码"},
            {"INV_OEMNo", "原厂编码"},
            {"INV_Barcode", "配件条码"},
            {"INV_BatchNo", "配件批次号"},
            {"INV_Name", "配件名称"},
            {"INV_Specification", "配件规格型号"},
            {"INV_SUPP_ID", "供应商ID"},
            {"INV_Qty", "数量"},
            {"INV_PurchaseUnitPrice", "采购单价"},
            {"INV_IsValid", "有效"},
            {"INV_CreatedBy", "创建人"},
            {"INV_CreatedTime", "创建时间"},
            {"INV_UpdatedBy", "修改人"},
            {"INV_UpdatedTime", "修改时间"},
            {"INV_VersionNo", "版本号"},
            {"INV_TransID", "事务编号"},
            {"INVP_ID", "ID"},
            {"INVP_SourceTypeCode", "来源类型编码"},
            {"INVP_SourceTypeName", "来源类型名称"},
            {"INVP_AutoPartsID", "配件ID"},
            {"INVP_Barcode", "条码"},
            {"INVP_PictureName", "图片名称"},
            {"INVP_IsValid", "有效"},
            {"INVP_CreatedBy", "创建人"},
            {"INVP_CreatedTime", "创建时间"},
            {"INVP_UpdatedBy", "修改人"},
            {"INVP_UpdatedTime", "修改时间"},
            {"INVP_VersionNo", "版本号"},
            {"INVP_TransID", "事务编号"},
            {"ITL_ID", "库存异动日志ID"},
            {"ITL_TransType", "异动类型"},
            {"ITL_Org_ID", "组织ID"},
            {"ITL_WH_ID", "仓库ID"},
            {"ITL_WHB_ID", "仓位ID"},
            {"ITL_BusinessNo", "业务单号"},
            {"ITL_Barcode", "配件条码"},
            {"ITL_BatchNo", "配件批次号"},
            {"ITL_Name", "配件名称"},
            {"ITL_Specification", "配件规格型号"},
            {"ITL_UnitCostPrice", "单位成本"},
            {"ITL_UnitSalePrice", "单位销价"},
            {"ITL_Qty", "异动数量"},
            {"ITL_AfterTransQty", "异动后库存数量"},
            {"ITL_Source", "出发地"},
            {"ITL_Destination", "目的地"},
            {"ITL_IsValid", "有效"},
            {"ITL_CreatedBy", "创建人"},
            {"ITL_CreatedTime", "创建时间"},
            {"ITL_UpdatedBy", "修改人"},
            {"ITL_UpdatedTime", "修改时间"},
            {"ITL_VersionNo", "版本号"},
            {"ITL_TransID", "事务编号"},
            {"SI_ID", "ID"},
            {"SI_Org_ID", "组织ID"},
            {"SI_WH_ID", "仓库ID"},
            {"SI_WHB_ID", "仓位ID"},
            {"SI_ThirdNo", "第三方编码"},
            {"SI_OEMNo", "原厂编码"},
            {"SI_Barcode", "配件条码"},
            {"SI_BatchNo", "配件批次号"},
            {"SI_Name", "配件名称"},
            {"SI_Specification", "配件规格型号"},
            {"SI_SUPP_ID", "供应商ID"},
            {"SI_Qty", "数量"},
            {"SI_PurchasePriceIsVisible", "采购单价可见"},
            {"SI_PurchaseUnitPrice", "采购单价"},
            {"SI_PriceOfGeneralCustomer", "普通客户销售单价"},
            {"SI_PriceOfCommonAutoFactory", "一般汽修商户销售单价"},
            {"SI_PriceOfPlatformAutoFactory", "平台内汽修商销售单价"},
            {"SI_IsValid", "有效"},
            {"SI_CreatedBy", "创建人"},
            {"SI_CreatedTime", "创建时间"},
            {"SI_UpdatedBy", "修改人"},
            {"SI_UpdatedTime", "修改时间"},
            {"SI_VersionNo", "版本号"},
            {"SI_TransID", "事务编号"},
            {"ST_ID", "盘点任务ID"},
            {"ST_No", "盘点单号"},
            {"ST_CheckAmount", "盘点次数"},
            {"ST_Org_ID", "组织ID"},
            {"ST_WH_ID", "仓库ID"},
            {"ST_WHB_ID", "仓位ID"},
            {"ST_StartTime", "开始时间"},
            {"ST_EndTime", "结束时间"},
            {"ST_IsShowCost", "显示成本"},
            {"ST_DueQty", "应有库存量"},
            {"ST_ActualQty", "实际库存量"},
            {"ST_QtyLossRatio", "数量损失率"},
            {"ST_DueAmount", "应有库存金额"},
            {"ST_ActualAmount", "实际库存金额"},
            {"ST_AmountLossRatio", "金额损失率"},
            {"ST_CheckResultCode", "盘点结果编码"},
            {"ST_CheckResultName", "盘点结果名称"},
            {"ST_StatusCode", "盘点单状态编码"},
            {"ST_StatusName", "盘点单状态名称"},
            {"ST_ApprovalStatusCode", "审核状态编码"},
            {"ST_ApprovalStatusName", "审核状态名称"},
            {"ST_Remark", "备注"},
            {"ST_IsValid", "有效"},
            {"ST_CreatedBy", "创建人"},
            {"ST_CreatedTime", "创建时间"},
            {"ST_UpdatedBy", "修改人"},
            {"ST_UpdatedTime", "修改时间"},
            {"ST_VersionNo", "版本号"},
            {"ST_TransID", "事务编号"},
            {"STD_ID", "盘点任务明细ID"},
            {"STD_TB_ID", "盘点任务ID"},
            {"STD_TB_No", "盘点单号"},
            {"STD_WH_ID", "仓库ID"},
            {"STD_WHB_ID", "仓位ID"},
            {"STD_DueQty", "应有量"},
            {"STD_ActualQty", "实际量"},
            {"STD_AdjustQty", "差异数量"},
            {"STD_ApprDiffQty", "允差数量"},
            {"STD_ApprDiffQtyRate", "数量允差比"},
            {"STD_SnapshotQty", "调整数量"},
            {"STD_DueAmount", "应有金额"},
            {"STD_ActualAmount", "实际金额"},
            {"STD_AmountLossRatio", "金额损失率"},
            {"STD_Barcode", "配件条码"},
            {"STD_BatchNo", "配件批次号"},
            {"STD_ThirdNo", "第三方编码"},
            {"STD_OEMNo", "原厂编码"},
            {"STD_Name", "配件名称"},
            {"STD_Specification", "配件规格型号"},
            {"STD_UOM", "单位"},
            {"STD_IsValid", "有效"},
            {"STD_CreatedBy", "创建人"},
            {"STD_CreatedTime", "创建时间"},
            {"STD_UpdatedBy", "修改人"},
            {"STD_UpdatedTime", "修改时间"},
            {"STD_VersionNo", "版本号"},
            {"STD_TransID", "事务编号"},
            {"GC_ID", "普通客户ID"},
            {"GC_Org_ID", "组织ID"},
            {"GC_Name", "姓名"},
            {"GC_PhoneNo", "手机号码"},
            {"GC_Address", "地址"},
            {"GC_CreditAmount", "信用额度"},
            {"GC_PaymentTypeCode", "默认支付类型编码"},
            {"GC_PaymentTypeName", "默认支付类型名称"},
            {"GC_BillingTypeCode", "默认开票类型编码"},
            {"GC_BillingTypeName", "默认开票类型名称"},
            {"GC_DeliveryTypeCode", "默认物流人员类型编码"},
            {"GC_DeliveryTypeName", "默认物流人员类型名称"},
            {"GC_DeliveryByID", "默认物流人员ID"},
            {"GC_DeliveryByName", "默认物流人员名称"},
            {"GC_DeliveryByPhoneNo", "默认物流人员手机号"},
            {"GC_IsEndSales", "终止销售"},
            {"GC_AutoPartsPriceType", "配件价格类别"},
            {"GC_Remark", "备注"},
            {"GC_IsValid", "有效"},
            {"GC_CreatedBy", "创建人"},
            {"GC_CreatedTime", "创建时间"},
            {"GC_UpdatedBy", "修改人"},
            {"GC_UpdatedTime", "修改时间"},
            {"GC_VersionNo", "版本号"},
            {"GC_TransID", "事务编号"},
            {"AFC_ID", "汽修商客户ID"},
            {"AFC_Org_ID", "组织ID"},
            {"AFC_IsPlatform", "是否平台商户"},
            {"AFC_Code", "汽修商编码"},
            {"AFC_Name", "汽修商名称"},
            {"AFC_Contacter", "汽修商联系人"},
            {"AFC_PhoneNo", "汽修商联系方式"},
            {"AFC_Address", "汽修商地址"},
            {"AFC_AROrg_Code", "汽修组织编码"},
            {"AFC_AROrg_Name", "汽修组织名称"},
            {"AFC_AROrg_Contacter", "汽修组织联系人"},
            {"AFC_AROrg_Phone", "汽修组织联系方式"},
            {"AFC_AROrg_Address", "汽修组织地址"},
            {"AFC_CreditAmount", "信用额度"},
            {"AFC_PaymentTypeCode", "默认支付类型编码"},
            {"AFC_PaymentTypeName", "默认支付类型名称"},
            {"AFC_BillingTypeCode", "默认开票类型编码"},
            {"AFC_BillingTypeName", "默认开票类型名称"},
            {"AFC_DeliveryTypeCode", "默认物流人员类型编码"},
            {"AFC_DeliveryTypeName", "默认物流人员类型名称"},
            {"AFC_DeliveryByID", "默认物流人员ID"},
            {"AFC_DeliveryByName", "默认物流人员名称"},
            {"AFC_DeliveryByPhoneNo", "默认物流人员手机号"},
            {"AFC_IsEndSales", "终止销售"},
            {"AFC_AutoPartsPriceType", "配件价格类别"},
            {"AFC_Remark", "备注"},
            {"AFC_IsValid", "有效"},
            {"AFC_CreatedBy", "创建人"},
            {"AFC_CreatedTime", "创建时间"},
            {"AFC_UpdatedBy", "修改人"},
            {"AFC_UpdatedTime", "修改时间"},
            {"AFC_VersionNo", "版本号"},
            {"AFC_TransID", "事务编号"},
            {"TB_ID", "调拨单ID"},
            {"TB_No", "单号"},
            {"TB_TypeCode", "单据类型编码"},
            {"TB_TypeName", "单据类型名称"},
            {"TB_TransferTypeCode", "调拨类型编码"},
            {"TB_TransferTypeName", "调拨类型名称"},
            {"TB_TransferOutOrgId", "调出组织ID"},
            {"TB_TransferOutOrgName", "调出组织名称"},
            {"TB_TransferInOrgId", "调入组织ID"},
            {"TB_TransferInOrgName", "调入组织名称"},
            {"TB_StatusCode", "单据状态编码"},
            {"TB_StatusName", "单据状态名称"},
            {"TB_ApprovalStatusCode", "审核状态编码"},
            {"TB_ApprovalStatusName", "审核状态名称"},
            {"TB_Remark", "备注"},
            {"TB_IsValid", "有效"},
            {"TB_CreatedBy", "创建人"},
            {"TB_CreatedTime", "创建时间"},
            {"TB_UpdatedBy", "修改人"},
            {"TB_UpdatedTime", "修改时间"},
            {"TB_VersionNo", "版本号"},
            {"TB_TransID", "事务编号"},
            {"TBD_ID", "调拨单明细ID"},
            {"TBD_TB_ID", "调拨单ID"},
            {"TBD_TB_No", "调拨单号"},
            {"TBD_TransOutWhId", "调出仓库ID"},
            {"TBD_TransOutBinId", "调出仓位ID"},
            {"TBD_TransInWhId", "调入仓库ID"},
            {"TBD_TransInBinId", "调入仓位ID"},
            {"TBD_Barcode", "配件条码"},
            {"TBD_TransOutBatchNo", "调出配件批次号"},
            {"TBD_TransInBatchNo", "调入配件批次号"},
            {"TBD_ThirdNo", "第三方编码"},
            {"TBD_OEMNo", "原厂编码"},
            {"TBD_Name", "配件名称"},
            {"TBD_Specification", "配件规格型号"},
            {"TBD_SUPP_ID", "供应商ID"},
            {"TBD_Qty", "数量"},
            {"TBD_UOM", "单位"},
            {"TBD_SourUnitPrice", "源库存单价"},
            {"TBD_DestUnitPrice", "入库单价"},
            {"TBD_IsSettled", "已结算标志"},
            {"TBD_IsValid", "有效"},
            {"TBD_CreatedBy", "创建人"},
            {"TBD_CreatedTime", "创建时间"},
            {"TBD_UpdatedBy", "修改人"},
            {"TBD_UpdatedTime", "修改时间"},
            {"TBD_VersionNo", "版本号"},
            {"TBD_TransID", "事务编号"},
            {"SIB_ID", "入库单ID"},
            {"SIB_Org_ID", "组织ID"},
            {"SIB_No", "单号"},
            {"SIB_SourceTypeCode", "来源类型编码"},
            {"SIB_SourceTypeName", "来源类型名称"},
            {"SIB_SourceNo", "来源单号"},
            {"SIB_StatusCode", "单据状态编码"},
            {"SIB_StatusName", "单据状态名称"},
            {"SIB_ApprovalStatusCode", "审核状态编码"},
            {"SIB_ApprovalStatusName", "审核状态名称"},
            {"SIB_Remark", "备注"},
            {"SIB_IsValid", "有效"},
            {"SIB_CreatedBy", "创建人"},
            {"SIB_CreatedTime", "创建时间"},
            {"SIB_UpdatedBy", "修改人"},
            {"SIB_UpdatedTime", "修改时间"},
            {"SIB_VersionNo", "版本号"},
            {"SIB_TransID", "事务编号"},
            {"SID_ID", "入库单明细ID"},
            {"SID_SIB_ID", "入库单ID"},
            {"SID_SIB_No", "入库单号"},
            {"SID_SourceDetailID", "来源单明细ID"},
            {"SID_Barcode", "配件条码"},
            {"SID_BatchNo", "配件批次号"},
            {"SID_ThirdNo", "第三方编码"},
            {"SID_OEMNo", "原厂编码"},
            {"SID_Name", "配件名称"},
            {"SID_Specification", "配件规格型号"},
            {"SID_SUPP_ID", "供应商ID"},
            {"SID_WH_ID", "仓库ID"},
            {"SID_WHB_ID", "仓位ID"},
            {"SID_Qty", "入库数量"},
            {"SID_UOM", "单位"},
            {"SID_UnitCostPrice", "入库单价"},
            {"SID_Amount", "入库金额"},
            {"SID_IsSettled", "已结算标志"},
            {"SID_IsValid", "有效"},
            {"SID_CreatedBy", "创建人"},
            {"SID_CreatedTime", "创建时间"},
            {"SID_UpdatedBy", "修改人"},
            {"SID_UpdatedTime", "修改时间"},
            {"SID_VersionNo", "版本号"},
            {"SID_TransID", "事务编号"},
            {"WM_ID", "菜单ID"},
            {"WM_PlatformCode", "平台编码"},
            {"WM_ParentID", "父菜单"},
            {"WM_Isleff", "是否叶节点"},
            {"WM_Name", "菜单名称"},
            {"WM_Type", "菜单类型"},
            {"WM_Key", "菜单Key"},
            {"WM_URL", "菜单URL"},
            {"WM_MediaID", "MediaID"},
            {"WM_Index", "显示顺序"},
            {"WM_IsValid", "有效"},
            {"WM_CreatedBy", "创建人"},
            {"WM_CreatedTime", "创建时间"},
            {"WM_UpdatedBy", "修改人"},
            {"WM_UpdatedTime", "修改时间"},
            {"WM_VersionNo", "版本号"},
            {"WM_TransID", "事务编号"},
            {"WMsg_ID", "ID"},
            {"WMsg_ToUserName", "开发者微信号"},
            {"WMsg_FromUserName", "发送方OpenID"},
            {"WMsg_CreateTime", "消息创建时间"},
            {"WMsg_MessageBody", "消息体"},
            {"WMsg_MsgType", "消息类型"},
            {"WMsg_Content", "文本消息内容"},
            {"WMsg_MsgId", "消息ID"},
            {"WMsg_PicUrl", "图片链接"},
            {"WMsg_MediaId", "图片消息媒体id"},
            {"WMsg_Format", "语音格式"},
            {"WMsg_ThumbMediaId", "视频消息缩略图的媒体id"},
            {"WMsg_Location_X", "地理位置维度"},
            {"WMsg_Location_Y", "地理位置经度"},
            {"WMsg_Scale", "地图缩放大小"},
            {"WMsg_Label", "地理位置信息"},
            {"WMsg_Title", "消息标题"},
            {"WMsg_Description", "消息描述"},
            {"WMsg_Url", "消息链接"},
            {"WMsg_Event", "事件类型"},
            {"WMsg_EventKey", "事件KEY值"},
            {"WMsg_Ticket", "二维码的ticket"},
            {"WMsg_Latitude", "地理位置纬度"},
            {"WMsg_Longitude", "地理位置经度"},
            {"WMsg_Precision", "地理位置精度"},
            {"WMsg_IsValid", "有效"},
            {"WMsg_CreatedBy", "创建人"},
            {"WMsg_CreatedTime", "创建时间"},
            {"WMsg_UpdatedBy", "修改人"},
            {"WMsg_UpdatedTime", "修改时间"},
            {"WMsg_VersionNo", "版本号"},
            {"WMsg_TransID", "事务编号"},
            {"WU_ID", "ID"},
            {"WU_OpenID", "用户的标识"},
            {"WU_NickName", "昵称"},
            {"WU_Sex", "性别"},
            {"WU_City", "用户所在城市"},
            {"WU_Country", "用户所在国家"},
            {"WU_Province", "用户所在省份"},
            {"WU_Language", "用户的语言"},
            {"WU_HeadImgURL", "用户头像"},
            {"WU_Subscribe", "微信用户关注公众号的标识"},
            {"WU_SubscribeTime", "用户关注时间戳"},
            {"WU_UnionID", "开放平台ID"},
            {"WU_Remark", "公众号运营者对粉丝的备注"},
            {"WU_GroupID", "用户所在的分组ID"},
            {"WU_Privilege", "用户特权信息"},
            {"WU_SubscribeTimeNew", "最近一次关注时间"},
            {"WU_UnSubscribeTimeNew", "最近一次取消关注时间"},
            {"WU_Org_ID", "最近一次关注的来源组织"},
            {"WU_LastVisitTime", "最近一次访问时间"},
            {"WU_Latitude", "最近一次经度"},
            {"WU_Longitude", "最近一次经度"},
            {"WU_LocationUpdTime", "最近一次坐标更新时间"},
            {"WU_IsValid", "有效"},
            {"WU_CreatedBy", "创建人"},
            {"WU_CreatedTime", "创建时间"},
            {"WU_UpdatedBy", "修改人"},
            {"WU_UpdatedTime", "修改时间"},
            {"WU_VersionNo", "版本号"},
            {"WU_TransID", "事务编号"},
            {"WUD_ID", "微信用户明细ID"},
            {"WUD_UnionID", "开放平台ID"},
            {"WUD_OpenID", "OpenID"},
            {"WUD_Type", "用户类型"},
            {"WUD_CertificationTime", "认证时间"},
            {"WUD_Mark", "认证标识"},
            {"WUD_Name", "用户名称"},
            {"WUD_UserID", "用户ID"},
            {"WUD_IsManager", "是否商户管理者"},
            {"WUD_AllowMultipleCertificate", "是否允许当前商户被多次认证"},
            {"WUD_IsValid", "有效"},
            {"WUD_CreatedBy", "创建人"},
            {"WUD_CreatedTime", "创建时间"},
            {"WUD_UpdatedBy", "修改人"},
            {"WUD_UpdatedTime", "修改时间"},
            {"WUD_VersionNo", "版本号"},
            {"WUD_TransID", "事务编号"},
            {"WUDT_ID", "明细异动ID"},
            {"WUDT_UnionID", "开放平台ID"},
            {"WUDT_OpenID", "OpenID"},
            {"WUDT_Mark", "认证标识"},
            {"WUDT_Name", "用户名称"},
            {"WUDT_UserID", "用户ID"},
            {"WUDT_Type", "异动类型"},
            {"WUDT_CertificationType", "认证类型"},
            {"WUDT_Time", "异动时间"},
            {"WUDT_Remark", "备注"},
            {"WUDT_IsValid", "有效"},
            {"WUDT_CreatedBy", "创建人"},
            {"WUDT_CreatedTime", "创建时间"},
            {"WUDT_UpdatedBy", "修改人"},
            {"WUDT_UpdatedTime", "修改时间"},
            {"WUDT_VersionNo", "版本号"},
            {"WUDT_TransID", "事务编号"},
            {"WUST_ID", "微信用户关注明细ID"},
            {"WUST_UnionID", "开放平台ID"},
            {"WUST_OpenID", "OpenID"},
            {"WUST_Event", "事件"},
            {"WUST_Time", "异动时间"},
            {"WUST_Remark", "备注"},
            {"WUST_IsValid", "有效"},
            {"WUST_CreatedBy", "创建人"},
            {"WUST_CreatedTime", "创建时间"},
            {"WUST_UpdatedBy", "修改人"},
            {"WUST_UpdatedTime", "修改时间"},
            {"WUST_VersionNo", "版本号"},
            {"WUST_TransID", "事务编号"},
            {"LB_ID", "物流订单ID"},
            {"LB_No", "物流单号"},
            {"LB_Org_ID", "组织ID"},
            {"LB_Org_Name", "组织名称"},
            {"LB_SourceTypeCode", "来源类型编码"},
            {"LB_SourceTypeName", "来源类型名称"},
            {"LB_SourceNo", "物流单来源单号"},
            {"LB_SourceCode", "物流人员类型编码"},
            {"LB_SourceName", "物流人员类型名称"},
            {"LB_DeliveryByID", "物流人员ID"},
            {"LB_DeliveryBy", "物流人员名称"},
            {"LB_PhoneNo", "物流人员手机号"},
            {"LB_AcceptTime", "物流人员接单时间"},
            {"LB_AcceptPicPath1", "物流人员接单图片路径1"},
            {"LB_AcceptPicPath2", "物流人员接单图片路径2"},
            {"LB_Receiver", "收件人姓名"},
            {"LB_ReceiverAddress", "收件人地址"},
            {"LB_ReceiverPostcode", "收件人邮编"},
            {"LB_ReceiverPhoneNo", "收件人手机号"},
            {"LB_ReceivedBy", "签收人"},
            {"LB_ReceivedTime", "签收时间"},
            {"LB_ReceivedPicPath1", "签收拍照图片路径1"},
            {"LB_ReceivedPicPath2", "签收拍照图片路径2"},
            {"LB_Fee", "物流费"},
            {"LB_Indemnification", "赔偿金"},
            {"LB_AccountReceivableAmount", "应收金额"},
            {"LB_PayStautsCode", "物流费付款状态编码"},
            {"LB_PayStautsName", "物流费付款状态名称"},
            {"LB_StatusCode", "单据状态编码"},
            {"LB_StatusName", "单据状态名称"},
            {"LB_ApprovalStatusCode", "审核状态编码"},
            {"LB_ApprovalStatusName", "审核状态名称"},
            {"LB_Remark", "备注"},
            {"LB_IsValid", "有效"},
            {"LB_CreatedBy", "创建人"},
            {"LB_CreatedTime", "创建时间"},
            {"LB_UpdatedBy", "修改人"},
            {"LB_UpdatedTime", "修改时间"},
            {"LB_VersionNo", "版本号"},
            {"LB_TransID", "事务编号"},
            {"LBD_ID", "物流订单明细ID"},
            {"LBD_LB_ID", "物流订单ID"},
            {"LBD_LB_No", "物流单号"},
            {"LBD_Barcode", "条码"},
            {"LBD_BatchNo", "配件批次号（原库存批次）"},
            {"LBD_BatchNoNew", "配件批次号（汽修厂用）"},
            {"LBD_Name", "名称"},
            {"LBD_Specification", "规格型号"},
            {"LBD_UOM", "单位"},
            {"LBD_DeliveryQty", "配送数量"},
            {"LBD_SignQty", "签收数量"},
            {"LBD_RejectQty", "拒收数量"},
            {"LBD_LoseQty", "丢失数量"},
            {"LBD_Indemnification", "赔偿金"},
            {"LBD_AccountReceivableAmount", "应收金额"},
            {"LBD_StatusCode", "状态编码"},
            {"LBD_StatusName", "状态名称"},
            {"LBD_Remark", "备注"},
            {"LBD_IsValid", "有效"},
            {"LBD_CreatedBy", "创建人"},
            {"LBD_CreatedTime", "创建时间"},
            {"LBD_UpdatedBy", "修改人"},
            {"LBD_UpdatedTime", "修改时间"},
            {"LBD_VersionNo", "版本号"},
            {"LBD_TransID", "事务编号"},
            {"LBT_ID", "物流订单异动ID"},
            {"LBT_Org_ID", "组织ID"},
            {"LBT_Org_Name", "组织名称"},
            {"LBT_LB_ID", "物流订单ID"},
            {"LBT_LB_NO", "单号"},
            {"LBT_Time", "异动时间"},
            {"LBT_Status", "异动状态"},
            {"LBT_Remark", "备注"},
            {"LBT_IsValid", "有效"},
            {"LBT_CreatedBy", "创建人"},
            {"LBT_CreatedTime", "创建时间"},
            {"LBT_UpdatedBy", "修改人"},
            {"LBT_UpdatedTime", "修改时间"},
            {"LBT_VersionNo", "版本号"},
            {"LBT_TransID", "事务编号"},
            {"SO_ID", "销售订单ID"},
            {"SO_No", "单据编号"},
            {"SO_Org_ID", "组织ID"},
            {"SO_SourceTypeCode", "来源类型编码"},
            {"SO_SourceTypeName", "来源类型名称"},
            {"SO_SourceNo", "来源单号"},
            {"SO_CustomerTypeCode", "客户类型编码"},
            {"SO_CustomerTypeName", "客户类型名称"},
            {"SO_CustomerID", "客户ID"},
            {"SO_CustomerName", "客户名称"},
            {"SO_IsPriceIncludeTax", "是否价格含税"},
            {"SO_TaxRate", "税率"},
            {"SO_TotalTax", "税额"},
            {"SO_TotalAmount", "总金额"},
            {"SO_TotalNetAmount", "未税总金额"},
            {"SO_StatusCode", "单据状态编码"},
            {"SO_StatusName", "单据状态名称"},
            {"SO_ApprovalStatusCode", "审核状态编码"},
            {"SO_ApprovalStatusName", "审核状态名称"},
            {"SO_AutoPartsPriceType", "配件价格类别"},
            {"SO_SalesByID", "业务员ID"},
            {"SO_SalesByName", "业务员名称"},
            {"SO_Remark", "备注"},
            {"SO_IsValid", "有效"},
            {"SO_CreatedBy", "创建人"},
            {"SO_CreatedTime", "创建时间"},
            {"SO_UpdatedBy", "修改人"},
            {"SO_UpdatedTime", "修改时间"},
            {"SO_VersionNo", "版本号"},
            {"SO_TransID", "事务编号"},
            {"SOD_ID", "销售订单明细ID"},
            {"SOD_SO_ID", "销售订单ID"},
            {"SOD_SalePriceRate", "计价基准"},
            {"SOD_SalePriceRateIsChangeable", "计价基准可改"},
            {"SOD_PriceIsIncludeTax", "价格是否含税"},
            {"SOD_TaxRate", "税率"},
            {"SOD_TotalTax", "税额"},
            {"SOD_Qty", "销售数量"},
            {"SOD_SignQty", "签收数量"},
            {"SOD_RejectQty", "拒收数量"},
            {"SOD_LoseQty", "丢失数量"},
            {"SOD_UnitPrice", "单价"},
            {"SOD_TotalAmount", "总金额"},
            {"SOD_Barcode", "配件条码"},
            {"SOD_BatchNo", "配件批次号（原库存批次）"},
            {"SOD_BatchNoNew", "配件批次号（汽修厂用）"},
            {"SOD_Name", "配件名称"},
            {"SOD_Specification", "配件规格型号"},
            {"SOD_UOM", "单位"},
            {"SOD_StockInOrgID", "入库组织ID"},
            {"SOD_StockInOrgCode", "入库组织编码"},
            {"SOD_StockInOrgName", "入库组织名称"},
            {"SOD_StockInWarehouseID", "入库仓库ID"},
            {"SOD_StockInWarehouseName", "入库仓库名称"},
            {"SOD_StockInBinID", "入库仓位ID"},
            {"SOD_StockInBinName", "入库仓位名称"},
            {"SOD_StatusCode", "单据状态编码"},
            {"SOD_StatusName", "单据状态名称"},
            {"SOD_ApprovalStatusCode", "审核状态编码"},
            {"SOD_ApprovalStatusName", "审核状态名称"},
            {"SOD_Remark", "备注"},
            {"SOD_IsValid", "有效"},
            {"SOD_CreatedBy", "创建人"},
            {"SOD_CreatedTime", "创建时间"},
            {"SOD_UpdatedBy", "修改人"},
            {"SOD_UpdatedTime", "修改时间"},
            {"SOD_VersionNo", "版本号"},
            {"SOD_TransID", "事务编号"},
            {"DP_ID", "下发路径ID"},
            {"DP_Org_ID_From", "来源组织"},
            {"DP_Org_ID_To", "目标组织"},
            {"DP_SendPerson", "下发人"},
            {"DP_SendDataID", "下发数据ID"},
            {"DP_SendDataTypeCode", "下发数据类型编码"},
            {"DP_SendDataTypeName", "下发数据类型名称"},
            {"DP_Remark", "备注"},
            {"DP_IsValid", "有效"},
            {"DP_CreatedBy", "创建人"},
            {"DP_CreatedTime", "创建时间"},
            {"DP_UpdatedBy", "修改人"},
            {"DP_UpdatedTime", "修改时间"},
            {"DP_VersionNo", "版本号"},
            {"DP_TransID", "事务编号"},
            {"SasT_ID", "销售模板ID"},
            {"SasT_Name", "销售模板名称"},
            {"SasT_Org_ID", "组织ID"},
            {"SasT_AutoFactoryCode", "汽修商户编码"},
            {"SasT_AutoFactoryName", "汽修商户名称"},
            {"SasT_CustomerID", "汽修商客户ID"},
            {"SasT_CustomerName", "汽修商客户名称"},
            {"SasT_AutoFactoryOrgCode", "汽修商组织编码"},
            {"SasT_ApprovalStatusCode", "审核状态编码"},
            {"SasT_ApprovalStatusName", "审核状态名称"},
            {"SasT_Remark", "备注"},
            {"SasT_IsValid", "有效"},
            {"SasT_CreatedBy", "创建人"},
            {"SasT_CreatedTime", "创建时间"},
            {"SasT_UpdatedBy", "修改人"},
            {"SasT_UpdatedTime", "修改时间"},
            {"SasT_VersionNo", "版本号"},
            {"SasT_TransID", "事务编号"},
            {"SasTD_ID", "销售模板明细ID"},
            {"SasTD_SasT_ID", "销售模板ID"},
            {"SasTD_PriceIsIncludeTax", "价格是否含税"},
            {"SasTD_TaxRate", "税率"},
            {"SasTD_TotalTax", "税额"},
            {"SasTD_Qty", "数量"},
            {"SasTD_UnitPrice", "单价"},
            {"SasTD_TotalAmount", "总金额"},
            {"SasTD_Barcode", "配件条码"},
            {"SasTD_Name", "配件名称"},
            {"SasTD_Specification", "配件规格型号"},
            {"SasTD_UOM", "单位"},
            {"SasTD_AutoFactoryOrgID", "汽修商组织ID"},
            {"SasTD_AutoFactoryOrgName", "汽修商组织名称"},
            {"SasTD_Remark", "备注"},
            {"SasTD_IsValid", "有效"},
            {"SasTD_CreatedBy", "创建人"},
            {"SasTD_CreatedTime", "创建时间"},
            {"SasTD_UpdatedBy", "修改人"},
            {"SasTD_UpdatedTime", "修改时间"},
            {"SasTD_VersionNo", "版本号"},
            {"SasTD_TransID", "事务编号"},
            {"SFO_ID", "销售预测订单ID"},
            {"SFO_No", "单据编号"},
            {"SFO_Org_ID", "组织ID"},
            {"SFO_AutoFactoryCode", "汽修商户编码"},
            {"SFO_AutoFactoryName", "汽修商户名称"},
            {"SFO_CustomerID", "汽修商客户ID"},
            {"SFO_CustomerName", "汽修商客户名称"},
            {"SFO_AutoFactoryOrgCode", "汽修商组织编码"},
            {"SFO_SourceTypeCode", "来源类型编码"},
            {"SFO_SourceTypeName", "来源类型名称"},
            {"SFO_StatusCode", "单据状态编码"},
            {"SFO_StatusName", "单据状态名称"},
            {"SFO_Remark", "备注"},
            {"SFO_IsValid", "有效"},
            {"SFO_CreatedBy", "创建人"},
            {"SFO_CreatedTime", "创建时间"},
            {"SFO_UpdatedBy", "修改人"},
            {"SFO_UpdatedTime", "修改时间"},
            {"SFO_VersionNo", "版本号"},
            {"SFO_TransID", "事务编号"},
            {"SFOD_ID", "销售预测订单明细ID"},
            {"SFOD_ST_ID", "销售预测订单ID"},
            {"SFOD_PriceIsIncludeTax", "价格是否含税"},
            {"SFOD_TaxRate", "税率"},
            {"SFOD_TotalTax", "税额"},
            {"SFOD_Qty", "数量"},
            {"SFOD_UnitPrice", "单价"},
            {"SFOD_TotalAmount", "总金额"},
            {"SFOD_Barcode", "配件条码"},
            {"SFOD_BatchNoNew", "配件批次号（汽修厂用）"},
            {"SFOD_Name", "配件名称"},
            {"SFOD_Specification", "配件规格型号"},
            {"SFOD_UOM", "单位"},
            {"SFOD_AutoFactoryOrgID", "汽修商组织ID"},
            {"SFOD_AutoFactoryOrgName", "汽修商组织名称"},
            {"SFOD_Remark", "备注"},
            {"SFOD_IsValid", "有效"},
            {"SFOD_CreatedBy", "创建人"},
            {"SFOD_CreatedTime", "创建时间"},
            {"SFOD_UpdatedBy", "修改人"},
            {"SFOD_UpdatedTime", "修改时间"},
            {"SFOD_VersionNo", "版本号"},
            {"SFOD_TransID", "事务编号"},
            {"BJ_ID", "ID"},
            {"BJ_Org_ID", "组织ID"},
            {"BJ_Org_Name", "组织名称"},
            {"BJ_Code", "作业编码"},
            {"BJ_Name", "作业名称"},
            {"BJ_GroupName", "作业分组"},
            {"BJ_Pattern", "作业方式（消息推送，调度执行）"},
            {"BJ_PushMode", "消息类别（PC端，APP端，微信）"},
            {"BJ_BusinessType", "业务类别（验车，交强险到期，商业险到期，换驾驶证，生日祝福）"},
            {"BJ_FullClassName", "类全名"},
            {"BJ_ExecutionType", "执行类型"},
            {"BJ_ExecuteTime", "执行一次的时间"},
            {"BJ_ExecutionInterval", "执行间隔"},
            {"BJ_ExecutionIntervalValue", "执行间隔值"},
            {"BJ_DayExecutionType", "日执行类型"},
            {"BJ_DayExecutionTime", "日一次执行时间"},
            {"BJ_DayExecutionFrequency", "日执行间隔"},
            {"BJ_DayExecutionIntervalValue", "日执行间隔值"},
            {"BJ_DayExecutionStartTime", "日执行间隔的开始时间"},
            {"BJ_DayExecutionEndTime", "日执行间隔的结束时间"},
            {"BJ_PlanStartDate", "计划生效时间"},
            {"BJ_PlanEndDate", "计划失效时间"},
            {"BJ_CronExpression", "Cron表达式"},
            {"BJ_Remark", "计划说明"},
            {"BJ_IsValid", "有效"},
            {"BJ_CreatedBy", "创建人"},
            {"BJ_CreatedTime", "创建时间"},
            {"BJ_UpdatedBy", "修改人"},
            {"BJ_UpdatedTime", "修改时间"},
            {"BJ_VersionNo", "版本号"},
            {"BJ_TransID", "事务编号"},
            {"BJL_ID", "ID"},
            {"BJL_Org_ID", "组织ID"},
            {"BJL_Org_Name", "组织名称"},
            {"BJL_Code", "作业编码"},
            {"BJL_Name", "作业名称"},
            {"BJL_Pattern", "作业方式（消息推送，调度执行）"},
            {"BJL_PushMode", "推送方式（PC端，APP端，微信）"},
            {"BJL_ExectueStartDate", "执行开始时间"},
            {"BJL_ExectueEndDate", "执行结束时间"},
            {"BJL_StartMode", "启动方式（自动，手动）"},
            {"BJL_Details", "日志明细"},
            {"BJL_IsValid", "有效"},
            {"BJL_CreatedBy", "创建人"},
            {"BJL_CreatedTime", "创建时间"},
            {"BJL_UpdatedBy", "修改人"},
            {"BJL_UpdatedTime", "修改时间"},
            {"BJL_VersionNo", "版本号"},
            {"BJL_TransID", "事务编号"},
            {"BRL_ID", "ID"},
            {"BRL_Org_ID", "组织ID"},
            {"BRL_Org_Name", "组织"},
            {"BRL_BJ_BusinessType", "业务类别"},
            {"BRL_RemindObjectType", "被提醒对象类别"},
            {"BRL_RemindObject", "被提醒对象"},
            {"BRL_ComInsuranceCompany", "交强险保险公司"},
            {"BRL_BusInsuranceCompany", "商业险保险公司"},
            {"BRL_RelateDate", "相关日期"},
            {"BRL_Remark", "备注"},
            {"BRL_IsValid", "有效"},
            {"BRL_CreatedBy", "创建人"},
            {"BRL_CreatedTime", "创建时间"},
            {"BRL_UpdatedBy", "修改人"},
            {"BRL_UpdatedTime", "修改时间"},
            {"BRL_VersionNo", "版本号"},
            {"BRL_TransID", "事务编号"},
            {"PML_ID", "ID"},
            {"PML_BRL_ID", "业务提醒ID"},
            {"PML_PushMode", "推送方式"},
            {"PML_Content", "推送内容"},
            {"PML_SenderType", "推送者类别"},
            {"PML_Sender", "推送者"},
            {"PML_SendTime", "推送时间"},
            {"PML_SendStatus", "推送状态"},
            {"PML_ReceiverType", "接收者类别"},
            {"PML_Receiver", "接收者"},
            {"PML_ReceiveTime", "接收时间"},
            {"PML_ReceiveStatus", "接收状态"},
            {"PML_BJ_ID", "作业ID"},
            {"PML_BJ_Name", "作业名称"},
            {"PML_BusMsgType", "业务消息类别"},
            {"PML_BussinessCode", "业务单号"},
            {"PML_JsonFormatContent", "JSON格式内容"},
            {"PML_Remark", "备注"},
            {"PML_IsNeedTrack", "是否需要跟踪"},
            {"PML_TrackedBy", "跟踪人"},
            {"PML_TrackStatus", "跟踪状态"},
            {"PML_IsValid", "有效"},
            {"PML_CreatedBy", "创建人"},
            {"PML_CreatedTime", "创建时间"},
            {"PML_UpdatedBy", "修改人"},
            {"PML_UpdatedTime", "修改时间"},
            {"PML_VersionNo", "版本号"},
            {"PML_TransID", "事务编号"},
            {"Wal_ID", "钱包ID"},
            {"Wal_Org_ID", "组织ID"},
            {"Wal_Org_Name", "组织名称"},
            {"Wal_No", "钱包账号"},
            {"Wal_SourceTypeCode", "钱包来源类型编码"},
            {"Wal_SourceTypeName", "钱包来源类型名称"},
            {"Wal_SourceNo", "来源账号"},
            {"Wal_OwnerTypeCode", "钱包所有人类别编码"},
            {"Wal_OwnerTypeName", "钱包所有人类别名称"},
            {"Wal_CustomerID", "开户人ID"},
            {"Wal_CustomerName", "开户人姓名"},
            {"Wal_AutoFactoryCode", "汽修商户编码"},
            {"Wal_AutoFactoryOrgCode", "汽修商户组织编码"},
            {"Wal_TradingPassword", "交易密码"},
            {"Wal_AvailableBalance", "可用余额"},
            {"Wal_FreezingBalance", "冻结余额"},
            {"Wal_DepositBaseAmount", "充值基数"},
            {"Wal_RecommendEmployeeID", "推荐员工ID"},
            {"Wal_RecommendEmployee", "推荐员工"},
            {"Wal_CreatedByOrgID", "开户组织ID"},
            {"Wal_CreatedByOrgName", "开户组织名称"},
            {"Wal_EffectiveTime", "生效时间"},
            {"Wal_IneffectiveTime", "失效时间"},
            {"Wal_StatusCode", "钱包状态编码"},
            {"Wal_StatusName", "钱包状态名称"},
            {"Wal_Remark", "备注"},
            {"Wal_IsValid", "有效"},
            {"Wal_CreatedBy", "创建人"},
            {"Wal_CreatedTime", "创建时间"},
            {"Wal_UpdatedBy", "修改人"},
            {"Wal_UpdatedTime", "修改时间"},
            {"Wal_VersionNo", "版本号"},
            {"Wal_TransID", "事务编号"},
            {"WalT_ID", "钱包异动ID"},
            {"WalT_Org_ID", "受理组织ID"},
            {"WalT_Org_Name", "受理组织名称"},
            {"WalT_Wal_ID", "钱包ID"},
            {"WalT_Wal_No", "钱包账号"},
            {"WalT_Time", "异动时间"},
            {"WalT_TypeCode", "异动类型编码"},
            {"WalT_TypeName", "异动类型名称"},
            {"WalT_RechargeTypeCode", "充值方式编码"},
            {"WalT_RechargeTypeName", "充值方式名称"},
            {"WalT_ChannelCode", "通道编码"},
            {"WalT_ChannelName", "通道名称"},
            {"WalT_Amount", "异动金额"},
            {"WalT_BillNo", "单据编号"},
            {"WalT_Remark", "备注"},
            {"WalT_IsValid", "有效"},
            {"WalT_CreatedBy", "创建人"},
            {"WalT_CreatedTime", "创建时间"},
            {"WalT_UpdatedBy", "修改人"},
            {"WalT_UpdatedTime", "修改时间"},
            {"WalT_VersionNo", "版本号"},
            {"WalT_TransID", "事务编号"},
        };

        #endregion

        /// <summary>
        /// 配件档案
        /// </summary>
        public class BS_AutoPartsArchive
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 配置档案ID
                /// </summary>
                public const string APA_ID = "配置档案ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string APA_Org_ID = "组织ID";

                /// <summary>
                /// 条形码
                /// </summary>
                public const string APA_Barcode = "条形码";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string APA_OEMNo = "原厂编码";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string APA_ThirdNo = "第三方编码";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string APA_Name = "配件名称";

                /// <summary>
                /// 配件品牌
                /// </summary>
                public const string APA_Brand = "配件品牌";

                /// <summary>
                /// 规格型号
                /// </summary>
                public const string APA_Specification = "规格型号";

                /// <summary>
                /// 计量单位
                /// </summary>
                public const string APA_UOM = "计量单位";

                /// <summary>
                /// 配件级别
                /// </summary>
                public const string APA_Level = "配件级别";

                /// <summary>
                /// 汽车品牌
                /// </summary>
                public const string APA_VehicleBrand = "汽车品牌";

                /// <summary>
                /// 车系
                /// </summary>
                public const string APA_VehicleInspire = "车系";

                /// <summary>
                /// 排量
                /// </summary>
                public const string APA_VehicleCapacity = "排量";

                /// <summary>
                /// 年款
                /// </summary>
                public const string APA_VehicleYearModel = "年款";

                /// <summary>
                /// 变速类型编码
                /// </summary>
                public const string APA_VehicleGearboxTypeCode = "变速类型编码";

                /// <summary>
                /// 变速类型名称
                /// </summary>
                public const string APA_VehicleGearboxTypeName = "变速类型名称";

                /// <summary>
                /// 默认供应商ID
                /// </summary>
                public const string APA_SUPP_ID = "默认供应商ID";

                /// <summary>
                /// 默认仓库ID
                /// </summary>
                public const string APA_WH_ID = "默认仓库ID";

                /// <summary>
                /// 默认仓位ID
                /// </summary>
                public const string APA_WHB_ID = "默认仓位ID";

                /// <summary>
                /// 安全库存是否预警
                /// </summary>
                public const string APA_IsWarningSafeStock = "安全库存是否预警";

                /// <summary>
                /// 安全库存
                /// </summary>
                public const string APA_SafeStockNum = "安全库存";

                /// <summary>
                /// 呆滞件是否预警
                /// </summary>
                public const string APA_IsWarningDeadStock = "呆滞件是否预警";

                /// <summary>
                /// 呆滞天数
                /// </summary>
                public const string APA_SlackDays = "呆滞天数";

                /// <summary>
                /// 销价系数
                /// </summary>
                public const string APA_SalePriceRate = "销价系数";

                /// <summary>
                /// 销价
                /// </summary>
                public const string APA_SalePrice = "销价";

                /// <summary>
                /// 车型代码
                /// </summary>
                public const string APA_VehicleModelCode = "车型代码";

                /// <summary>
                /// 互换码
                /// </summary>
                public const string APA_ExchangeCode = "互换码";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APA_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APA_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APA_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APA_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APA_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APA_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APA_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 配置档案ID
                /// </summary>
                public const string APA_ID = "APA_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string APA_Org_ID = "APA_Org_ID";

                /// <summary>
                /// 条形码
                /// </summary>
                public const string APA_Barcode = "APA_Barcode";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string APA_OEMNo = "APA_OEMNo";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string APA_ThirdNo = "APA_ThirdNo";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string APA_Name = "APA_Name";

                /// <summary>
                /// 配件品牌
                /// </summary>
                public const string APA_Brand = "APA_Brand";

                /// <summary>
                /// 规格型号
                /// </summary>
                public const string APA_Specification = "APA_Specification";

                /// <summary>
                /// 计量单位
                /// </summary>
                public const string APA_UOM = "APA_UOM";

                /// <summary>
                /// 配件级别
                /// </summary>
                public const string APA_Level = "APA_Level";

                /// <summary>
                /// 汽车品牌
                /// </summary>
                public const string APA_VehicleBrand = "APA_VehicleBrand";

                /// <summary>
                /// 车系
                /// </summary>
                public const string APA_VehicleInspire = "APA_VehicleInspire";

                /// <summary>
                /// 排量
                /// </summary>
                public const string APA_VehicleCapacity = "APA_VehicleCapacity";

                /// <summary>
                /// 年款
                /// </summary>
                public const string APA_VehicleYearModel = "APA_VehicleYearModel";

                /// <summary>
                /// 变速类型编码
                /// </summary>
                public const string APA_VehicleGearboxTypeCode = "APA_VehicleGearboxTypeCode";

                /// <summary>
                /// 变速类型名称
                /// </summary>
                public const string APA_VehicleGearboxTypeName = "APA_VehicleGearboxTypeName";

                /// <summary>
                /// 默认供应商ID
                /// </summary>
                public const string APA_SUPP_ID = "APA_SUPP_ID";

                /// <summary>
                /// 默认仓库ID
                /// </summary>
                public const string APA_WH_ID = "APA_WH_ID";

                /// <summary>
                /// 默认仓位ID
                /// </summary>
                public const string APA_WHB_ID = "APA_WHB_ID";

                /// <summary>
                /// 安全库存是否预警
                /// </summary>
                public const string APA_IsWarningSafeStock = "APA_IsWarningSafeStock";

                /// <summary>
                /// 安全库存
                /// </summary>
                public const string APA_SafeStockNum = "APA_SafeStockNum";

                /// <summary>
                /// 呆滞件是否预警
                /// </summary>
                public const string APA_IsWarningDeadStock = "APA_IsWarningDeadStock";

                /// <summary>
                /// 呆滞天数
                /// </summary>
                public const string APA_SlackDays = "APA_SlackDays";

                /// <summary>
                /// 销价系数
                /// </summary>
                public const string APA_SalePriceRate = "APA_SalePriceRate";

                /// <summary>
                /// 销价
                /// </summary>
                public const string APA_SalePrice = "APA_SalePrice";

                /// <summary>
                /// 车型代码
                /// </summary>
                public const string APA_VehicleModelCode = "APA_VehicleModelCode";

                /// <summary>
                /// 互换码
                /// </summary>
                public const string APA_ExchangeCode = "APA_ExchangeCode";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APA_IsValid = "APA_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APA_CreatedBy = "APA_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APA_CreatedTime = "APA_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APA_UpdatedBy = "APA_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APA_UpdatedTime = "APA_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APA_VersionNo = "APA_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APA_TransID = "APA_TransID";

            }

        }
        /// <summary>
        /// 配件名称
        /// </summary>
        public class BS_AutoPartsName
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 配置名称ID
                /// </summary>
                public const string APN_ID = "配置名称ID";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string APN_Name = "配件名称";

                /// <summary>
                /// 配件别名
                /// </summary>
                public const string APN_Alias = "配件别名";

                /// <summary>
                /// 名称拼音简写
                /// </summary>
                public const string APN_NameSpellCode = "名称拼音简写";

                /// <summary>
                /// 别名拼音简写
                /// </summary>
                public const string APN_AliasSpellCode = "别名拼音简写";

                /// <summary>
                /// 配件类别ID
                /// </summary>
                public const string APN_APT_ID = "配件类别ID";

                /// <summary>
                /// 呆滞天数
                /// </summary>
                public const string APN_SlackDays = "呆滞天数";

                /// <summary>
                /// 计量单位
                /// </summary>
                public const string APN_UOM = "计量单位";

                /// <summary>
                /// 固定计量单位
                /// </summary>
                public const string APN_FixUOM = "固定计量单位";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APN_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APN_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APN_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APN_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APN_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APN_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APN_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 配置名称ID
                /// </summary>
                public const string APN_ID = "APN_ID";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string APN_Name = "APN_Name";

                /// <summary>
                /// 配件别名
                /// </summary>
                public const string APN_Alias = "APN_Alias";

                /// <summary>
                /// 名称拼音简写
                /// </summary>
                public const string APN_NameSpellCode = "APN_NameSpellCode";

                /// <summary>
                /// 别名拼音简写
                /// </summary>
                public const string APN_AliasSpellCode = "APN_AliasSpellCode";

                /// <summary>
                /// 配件类别ID
                /// </summary>
                public const string APN_APT_ID = "APN_APT_ID";

                /// <summary>
                /// 呆滞天数
                /// </summary>
                public const string APN_SlackDays = "APN_SlackDays";

                /// <summary>
                /// 计量单位
                /// </summary>
                public const string APN_UOM = "APN_UOM";

                /// <summary>
                /// 固定计量单位
                /// </summary>
                public const string APN_FixUOM = "APN_FixUOM";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APN_IsValid = "APN_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APN_CreatedBy = "APN_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APN_CreatedTime = "APN_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APN_UpdatedBy = "APN_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APN_UpdatedTime = "APN_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APN_VersionNo = "APN_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APN_TransID = "APN_TransID";

            }

        }
        /// <summary>
        /// 车辆品牌车系
        /// </summary>
        public class BS_VehicleBrandInspireSumma
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 品牌车系ID
                /// </summary>
                public const string VBIS_ID = "品牌车系ID";

                /// <summary>
                /// 品牌
                /// </summary>
                public const string VBIS_Brand = "品牌";

                /// <summary>
                /// 车系
                /// </summary>
                public const string VBIS_Inspire = "车系";

                /// <summary>
                /// 车型描述
                /// </summary>
                public const string VBIS_ModelDesc = "车型描述";

                /// <summary>
                /// 车辆类型
                /// </summary>
                public const string VBIS_Model = "车辆类型";

                /// <summary>
                /// 品牌拼音首字母
                /// </summary>
                public const string VBIS_BrandSpellCode = "品牌拼音首字母";

                /// <summary>
                /// 车系拼音首字母
                /// </summary>
                public const string VBIS_InspireSpellCode = "车系拼音首字母";

                /// <summary>
                /// 有效
                /// </summary>
                public const string VBIS_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string VBIS_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string VBIS_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string VBIS_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string VBIS_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string VBIS_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string VBIS_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 品牌车系ID
                /// </summary>
                public const string VBIS_ID = "VBIS_ID";

                /// <summary>
                /// 品牌
                /// </summary>
                public const string VBIS_Brand = "VBIS_Brand";

                /// <summary>
                /// 车系
                /// </summary>
                public const string VBIS_Inspire = "VBIS_Inspire";

                /// <summary>
                /// 车型描述
                /// </summary>
                public const string VBIS_ModelDesc = "VBIS_ModelDesc";

                /// <summary>
                /// 车辆类型
                /// </summary>
                public const string VBIS_Model = "VBIS_Model";

                /// <summary>
                /// 品牌拼音首字母
                /// </summary>
                public const string VBIS_BrandSpellCode = "VBIS_BrandSpellCode";

                /// <summary>
                /// 车系拼音首字母
                /// </summary>
                public const string VBIS_InspireSpellCode = "VBIS_InspireSpellCode";

                /// <summary>
                /// 有效
                /// </summary>
                public const string VBIS_IsValid = "VBIS_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string VBIS_CreatedBy = "VBIS_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string VBIS_CreatedTime = "VBIS_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string VBIS_UpdatedBy = "VBIS_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string VBIS_UpdatedTime = "VBIS_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string VBIS_VersionNo = "VBIS_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string VBIS_TransID = "VBIS_TransID";

            }

        }
        /// <summary>
        /// 配件级别
        /// </summary>
        public class BS_AutoPartsLevel
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string APL_ID = "ID";

                /// <summary>
                /// 编码
                /// </summary>
                public const string APL_Code = "编码";

                /// <summary>
                /// 名称
                /// </summary>
                public const string APL_Name = "名称";

                /// <summary>
                /// 显示顺序
                /// </summary>
                public const string APL_DispayIndex = "显示顺序";

                /// <summary>
                /// 备注
                /// </summary>
                public const string APL_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APL_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APL_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APL_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APL_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APL_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APL_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APL_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string APL_ID = "APL_ID";

                /// <summary>
                /// 编码
                /// </summary>
                public const string APL_Code = "APL_Code";

                /// <summary>
                /// 名称
                /// </summary>
                public const string APL_Name = "APL_Name";

                /// <summary>
                /// 显示顺序
                /// </summary>
                public const string APL_DispayIndex = "APL_DispayIndex";

                /// <summary>
                /// 备注
                /// </summary>
                public const string APL_Remark = "APL_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APL_IsValid = "APL_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APL_CreatedBy = "APL_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APL_CreatedTime = "APL_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APL_UpdatedBy = "APL_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APL_UpdatedTime = "APL_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APL_VersionNo = "APL_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APL_TransID = "APL_TransID";

            }

        }
        /// <summary>
        /// 配件类别
        /// </summary>
        public class BS_AutoPartsType
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 配件类别ID
                /// </summary>
                public const string APT_ID = "配件类别ID";

                /// <summary>
                /// 配件类别名称
                /// </summary>
                public const string APT_Name = "配件类别名称";

                /// <summary>
                /// 父级类别ID
                /// </summary>
                public const string APT_ParentID = "父级类别ID";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string APT_Index = "顺序";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APT_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APT_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APT_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APT_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APT_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APT_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APT_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 配件类别ID
                /// </summary>
                public const string APT_ID = "APT_ID";

                /// <summary>
                /// 配件类别名称
                /// </summary>
                public const string APT_Name = "APT_Name";

                /// <summary>
                /// 父级类别ID
                /// </summary>
                public const string APT_ParentID = "APT_ParentID";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string APT_Index = "APT_Index";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APT_IsValid = "APT_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APT_CreatedBy = "APT_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APT_CreatedTime = "APT_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APT_UpdatedBy = "APT_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APT_UpdatedTime = "APT_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APT_VersionNo = "APT_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APT_TransID = "APT_TransID";

            }

        }
        /// <summary>
        /// 配件价格类别
        /// </summary>
        public class BS_AutoPartsPriceType
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 配件价格类别ID
                /// </summary>
                public const string APPT_ID = "配件价格类别ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string APPT_Org_ID = "组织ID";

                /// <summary>
                /// 配件价格类别名称
                /// </summary>
                public const string APPT_Name = "配件价格类别名称";

                /// <summary>
                /// 条形码
                /// </summary>
                public const string APPT_Barcode = "条形码";

                /// <summary>
                /// 价格
                /// </summary>
                public const string APPT_Price = "价格";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APPT_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APPT_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APPT_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APPT_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APPT_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APPT_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APPT_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 配件价格类别ID
                /// </summary>
                public const string APPT_ID = "APPT_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string APPT_Org_ID = "APPT_Org_ID";

                /// <summary>
                /// 配件价格类别名称
                /// </summary>
                public const string APPT_Name = "APPT_Name";

                /// <summary>
                /// 条形码
                /// </summary>
                public const string APPT_Barcode = "APPT_Barcode";

                /// <summary>
                /// 价格
                /// </summary>
                public const string APPT_Price = "APPT_Price";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APPT_IsValid = "APPT_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APPT_CreatedBy = "APPT_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APPT_CreatedTime = "APPT_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APPT_UpdatedBy = "APPT_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APPT_UpdatedTime = "APPT_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APPT_VersionNo = "APPT_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APPT_TransID = "APPT_TransID";

            }

        }
        /// <summary>
        /// 车辆信息
        /// </summary>
        public class BS_VehicleInfo
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string VC_ID = "ID";

                /// <summary>
                /// 车架号
                /// </summary>
                public const string VC_VIN = "车架号";

                /// <summary>
                /// 车牌号
                /// </summary>
                public const string VC_PlateNumber = "车牌号";

                /// <summary>
                /// 品牌
                /// </summary>
                public const string VC_Brand = "品牌";

                /// <summary>
                /// 车系
                /// </summary>
                public const string VC_Inspire = "车系";

                /// <summary>
                /// 车型描述
                /// </summary>
                public const string VC_BrandDesc = "车型描述";

                /// <summary>
                /// 排量
                /// </summary>
                public const string VC_Capacity = "排量";

                /// <summary>
                /// 发动机型号
                /// </summary>
                public const string VC_EngineType = "发动机型号";

                /// <summary>
                /// 备注
                /// </summary>
                public const string VC_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string VC_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string VC_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string VC_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string VC_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string VC_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string VC_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string VC_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string VC_ID = "VC_ID";

                /// <summary>
                /// 车架号
                /// </summary>
                public const string VC_VIN = "VC_VIN";

                /// <summary>
                /// 车牌号
                /// </summary>
                public const string VC_PlateNumber = "VC_PlateNumber";

                /// <summary>
                /// 品牌
                /// </summary>
                public const string VC_Brand = "VC_Brand";

                /// <summary>
                /// 车系
                /// </summary>
                public const string VC_Inspire = "VC_Inspire";

                /// <summary>
                /// 车型描述
                /// </summary>
                public const string VC_BrandDesc = "VC_BrandDesc";

                /// <summary>
                /// 排量
                /// </summary>
                public const string VC_Capacity = "VC_Capacity";

                /// <summary>
                /// 发动机型号
                /// </summary>
                public const string VC_EngineType = "VC_EngineType";

                /// <summary>
                /// 备注
                /// </summary>
                public const string VC_Remark = "VC_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string VC_IsValid = "VC_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string VC_CreatedBy = "VC_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string VC_CreatedTime = "VC_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string VC_UpdatedBy = "VC_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string VC_UpdatedTime = "VC_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string VC_VersionNo = "VC_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string VC_TransID = "VC_TransID";

            }

        }
        /// <summary>
        /// 车辆原厂件信息
        /// </summary>
        public class BS_VehicleOemPartsInfo
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string VOPI_ID = "ID";

                /// <summary>
                /// 车架号
                /// </summary>
                public const string VOPI_VC_VIN = "车架号";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string VOPI_OEMNo = "原厂编码";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string VOPI_AutoPartsName = "配件名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string VOPI_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string VOPI_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string VOPI_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string VOPI_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string VOPI_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string VOPI_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string VOPI_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string VOPI_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string VOPI_ID = "VOPI_ID";

                /// <summary>
                /// 车架号
                /// </summary>
                public const string VOPI_VC_VIN = "VOPI_VC_VIN";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string VOPI_OEMNo = "VOPI_OEMNo";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string VOPI_AutoPartsName = "VOPI_AutoPartsName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string VOPI_Remark = "VOPI_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string VOPI_IsValid = "VOPI_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string VOPI_CreatedBy = "VOPI_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string VOPI_CreatedTime = "VOPI_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string VOPI_UpdatedBy = "VOPI_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string VOPI_UpdatedTime = "VOPI_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string VOPI_VersionNo = "VOPI_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string VOPI_TransID = "VOPI_TransID";

            }

        }
        /// <summary>
        /// 车辆品牌件信息
        /// </summary>
        public class BS_VehicleThirdPartsInfo
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string VTPI_ID = "ID";

                /// <summary>
                /// 车架号
                /// </summary>
                public const string VTPI_VC_VIN = "车架号";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string VTPI_ThirdNo = "第三方编码";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string VTPI_AutoPartsName = "配件名称";

                /// <summary>
                /// 配件品牌
                /// </summary>
                public const string VTPI_AutoPartsBrand = "配件品牌";

                /// <summary>
                /// 备注
                /// </summary>
                public const string VTPI_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string VTPI_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string VTPI_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string VTPI_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string VTPI_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string VTPI_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string VTPI_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string VTPI_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string VTPI_ID = "VTPI_ID";

                /// <summary>
                /// 车架号
                /// </summary>
                public const string VTPI_VC_VIN = "VTPI_VC_VIN";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string VTPI_ThirdNo = "VTPI_ThirdNo";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string VTPI_AutoPartsName = "VTPI_AutoPartsName";

                /// <summary>
                /// 配件品牌
                /// </summary>
                public const string VTPI_AutoPartsBrand = "VTPI_AutoPartsBrand";

                /// <summary>
                /// 备注
                /// </summary>
                public const string VTPI_Remark = "VTPI_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string VTPI_IsValid = "VTPI_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string VTPI_CreatedBy = "VTPI_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string VTPI_CreatedTime = "VTPI_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string VTPI_UpdatedBy = "VTPI_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string VTPI_UpdatedTime = "VTPI_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string VTPI_VersionNo = "VTPI_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string VTPI_TransID = "VTPI_TransID";

            }

        }
        /// <summary>
        /// 车辆原厂品牌关联信息
        /// </summary>
        public class BS_VehicleBrandPartsInfo
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string VBPI_ID = "ID";

                /// <summary>
                /// 车架号
                /// </summary>
                public const string VBPI_VC_VIN = "车架号";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string VBPI_VOPI_OEMNo = "原厂编码";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string VBPI_ThirdNo = "第三方编码";

                /// <summary>
                /// 配件品牌
                /// </summary>
                public const string VBPI_AutoPartsBrand = "配件品牌";

                /// <summary>
                /// 备注
                /// </summary>
                public const string VBPI_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string VBPI_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string VBPI_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string VBPI_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string VBPI_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string VBPI_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string VBPI_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string VBPI_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string VBPI_ID = "VBPI_ID";

                /// <summary>
                /// 车架号
                /// </summary>
                public const string VBPI_VC_VIN = "VBPI_VC_VIN";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string VBPI_VOPI_OEMNo = "VBPI_VOPI_OEMNo";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string VBPI_ThirdNo = "VBPI_ThirdNo";

                /// <summary>
                /// 配件品牌
                /// </summary>
                public const string VBPI_AutoPartsBrand = "VBPI_AutoPartsBrand";

                /// <summary>
                /// 备注
                /// </summary>
                public const string VBPI_Remark = "VBPI_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string VBPI_IsValid = "VBPI_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string VBPI_CreatedBy = "VBPI_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string VBPI_CreatedTime = "VBPI_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string VBPI_UpdatedBy = "VBPI_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string VBPI_UpdatedTime = "VBPI_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string VBPI_VersionNo = "VBPI_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string VBPI_TransID = "VBPI_TransID";

            }

        }
        /// <summary>
        /// 编码段
        /// </summary>
        public class SM_CodingSegment
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 编码段ID
                /// </summary>
                public const string CS_ID = "编码段ID";

                /// <summary>
                /// 编码规则ID
                /// </summary>
                public const string CS_ER_ID = "编码规则ID";

                /// <summary>
                /// 编码段类型编码
                /// </summary>
                public const string CS_TypeCode = "编码段类型编码";

                /// <summary>
                /// 编码段类型名称
                /// </summary>
                public const string CS_TypeName = "编码段类型名称";

                /// <summary>
                /// 长度
                /// </summary>
                public const string CS_Length = "长度";

                /// <summary>
                /// 填充字符
                /// </summary>
                public const string CS_PanddingChar = "填充字符";

                /// <summary>
                /// 填充方式编码
                /// </summary>
                public const string CS_PanddingStyleCode = "填充方式编码";

                /// <summary>
                /// 填充方式名称
                /// </summary>
                public const string CS_PanddingStyleName = "填充方式名称";

                /// <summary>
                /// 编码段值
                /// </summary>
                public const string CS_Value = "编码段值";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string CS_Index = "顺序";

                /// <summary>
                /// 有效
                /// </summary>
                public const string CS_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string CS_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string CS_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string CS_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string CS_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string CS_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string CS_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 编码段ID
                /// </summary>
                public const string CS_ID = "CS_ID";

                /// <summary>
                /// 编码规则ID
                /// </summary>
                public const string CS_ER_ID = "CS_ER_ID";

                /// <summary>
                /// 编码段类型编码
                /// </summary>
                public const string CS_TypeCode = "CS_TypeCode";

                /// <summary>
                /// 编码段类型名称
                /// </summary>
                public const string CS_TypeName = "CS_TypeName";

                /// <summary>
                /// 长度
                /// </summary>
                public const string CS_Length = "CS_Length";

                /// <summary>
                /// 填充字符
                /// </summary>
                public const string CS_PanddingChar = "CS_PanddingChar";

                /// <summary>
                /// 填充方式编码
                /// </summary>
                public const string CS_PanddingStyleCode = "CS_PanddingStyleCode";

                /// <summary>
                /// 填充方式名称
                /// </summary>
                public const string CS_PanddingStyleName = "CS_PanddingStyleName";

                /// <summary>
                /// 编码段值
                /// </summary>
                public const string CS_Value = "CS_Value";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string CS_Index = "CS_Index";

                /// <summary>
                /// 有效
                /// </summary>
                public const string CS_IsValid = "CS_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string CS_CreatedBy = "CS_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string CS_CreatedTime = "CS_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string CS_UpdatedBy = "CS_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string CS_UpdatedTime = "CS_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string CS_VersionNo = "CS_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string CS_TransID = "CS_TransID";

            }

        }
        /// <summary>
        /// 编码规则
        /// </summary>
        public class SM_EncodingRule
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 规则ID
                /// </summary>
                public const string ER_ID = "规则ID";

                /// <summary>
                /// 主编码
                /// </summary>
                public const string ER_MasterCode = "主编码";

                /// <summary>
                /// 规则编码
                /// </summary>
                public const string ER_Code = "规则编码";

                /// <summary>
                /// 规则名称
                /// </summary>
                public const string ER_Name = "规则名称";

                /// <summary>
                /// 模块
                /// </summary>
                public const string ER_Module = "模块";

                /// <summary>
                /// 说明
                /// </summary>
                public const string ER_Remark = "说明";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ER_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ER_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ER_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ER_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ER_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ER_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ER_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 规则ID
                /// </summary>
                public const string ER_ID = "ER_ID";

                /// <summary>
                /// 主编码
                /// </summary>
                public const string ER_MasterCode = "ER_MasterCode";

                /// <summary>
                /// 规则编码
                /// </summary>
                public const string ER_Code = "ER_Code";

                /// <summary>
                /// 规则名称
                /// </summary>
                public const string ER_Name = "ER_Name";

                /// <summary>
                /// 模块
                /// </summary>
                public const string ER_Module = "ER_Module";

                /// <summary>
                /// 说明
                /// </summary>
                public const string ER_Remark = "ER_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ER_IsValid = "ER_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ER_CreatedBy = "ER_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ER_CreatedTime = "ER_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ER_UpdatedBy = "ER_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ER_UpdatedTime = "ER_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ER_VersionNo = "ER_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ER_TransID = "ER_TransID";

            }

        }
        /// <summary>
        /// 系统编号
        /// </summary>
        public class SM_SystemNo
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 系统编号ID
                /// </summary>
                public const string SN_ID = "系统编号ID";

                /// <summary>
                /// 规则ID
                /// </summary>
                public const string SN_ER_ID = "规则ID";

                /// <summary>
                /// 编码值
                /// </summary>
                public const string SN_Value = "编码值";

                /// <summary>
                /// 状态
                /// </summary>
                public const string SN_Status = "状态";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SN_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SN_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SN_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SN_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SN_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SN_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SN_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 系统编号ID
                /// </summary>
                public const string SN_ID = "SN_ID";

                /// <summary>
                /// 规则ID
                /// </summary>
                public const string SN_ER_ID = "SN_ER_ID";

                /// <summary>
                /// 编码值
                /// </summary>
                public const string SN_Value = "SN_Value";

                /// <summary>
                /// 状态
                /// </summary>
                public const string SN_Status = "SN_Status";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SN_IsValid = "SN_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SN_CreatedBy = "SN_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SN_CreatedTime = "SN_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SN_UpdatedBy = "SN_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SN_UpdatedTime = "SN_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SN_VersionNo = "SN_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SN_TransID = "SN_TransID";

            }

        }
        /// <summary>
        /// 组织管理
        /// </summary>
        public class SM_Organization
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string Org_ID = "ID";

                /// <summary>
                /// 商户ID
                /// </summary>
                public const string Org_MCT_ID = "商户ID";

                /// <summary>
                /// 门店编码
                /// </summary>
                public const string Org_Code = "门店编码";

                /// <summary>
                /// 平台编码
                /// </summary>
                public const string Org_PlatformCode = "平台编码";

                /// <summary>
                /// 组织全称
                /// </summary>
                public const string Org_FullName = "组织全称";

                /// <summary>
                /// 组织简称
                /// </summary>
                public const string Org_ShortName = "组织简称";

                /// <summary>
                /// 联系人
                /// </summary>
                public const string Org_Contacter = "联系人";

                /// <summary>
                /// 固定电话
                /// </summary>
                public const string Org_TEL = "固定电话";

                /// <summary>
                /// 移动电话
                /// </summary>
                public const string Org_PhoneNo = "移动电话";

                /// <summary>
                /// 省份Code
                /// </summary>
                public const string Org_Prov_Code = "省份Code";

                /// <summary>
                /// 城市Code
                /// </summary>
                public const string Org_City_Code = "城市Code";

                /// <summary>
                /// 区域Code
                /// </summary>
                public const string Org_Dist_Code = "区域Code";

                /// <summary>
                /// 地址
                /// </summary>
                public const string Org_Addr = "地址";

                /// <summary>
                /// 经度
                /// </summary>
                public const string Org_Longitude = "经度";

                /// <summary>
                /// 纬度
                /// </summary>
                public const string Org_Latitude = "纬度";

                /// <summary>
                /// 标注点显示标题
                /// </summary>
                public const string Org_MarkerTitle = "标注点显示标题";

                /// <summary>
                /// 标注点显示内容
                /// </summary>
                public const string Org_MarkerContent = "标注点显示内容";

                /// <summary>
                /// 主营品牌
                /// </summary>
                public const string Org_MainBrands = "主营品牌";

                /// <summary>
                /// 主营产品
                /// </summary>
                public const string Org_MainProducts = "主营产品";

                /// <summary>
                /// 备注
                /// </summary>
                public const string Org_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Org_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Org_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Org_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Org_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Org_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Org_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Org_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string Org_ID = "Org_ID";

                /// <summary>
                /// 商户ID
                /// </summary>
                public const string Org_MCT_ID = "Org_MCT_ID";

                /// <summary>
                /// 门店编码
                /// </summary>
                public const string Org_Code = "Org_Code";

                /// <summary>
                /// 平台编码
                /// </summary>
                public const string Org_PlatformCode = "Org_PlatformCode";

                /// <summary>
                /// 组织全称
                /// </summary>
                public const string Org_FullName = "Org_FullName";

                /// <summary>
                /// 组织简称
                /// </summary>
                public const string Org_ShortName = "Org_ShortName";

                /// <summary>
                /// 联系人
                /// </summary>
                public const string Org_Contacter = "Org_Contacter";

                /// <summary>
                /// 固定电话
                /// </summary>
                public const string Org_TEL = "Org_TEL";

                /// <summary>
                /// 移动电话
                /// </summary>
                public const string Org_PhoneNo = "Org_PhoneNo";

                /// <summary>
                /// 省份Code
                /// </summary>
                public const string Org_Prov_Code = "Org_Prov_Code";

                /// <summary>
                /// 城市Code
                /// </summary>
                public const string Org_City_Code = "Org_City_Code";

                /// <summary>
                /// 区域Code
                /// </summary>
                public const string Org_Dist_Code = "Org_Dist_Code";

                /// <summary>
                /// 地址
                /// </summary>
                public const string Org_Addr = "Org_Addr";

                /// <summary>
                /// 经度
                /// </summary>
                public const string Org_Longitude = "Org_Longitude";

                /// <summary>
                /// 纬度
                /// </summary>
                public const string Org_Latitude = "Org_Latitude";

                /// <summary>
                /// 标注点显示标题
                /// </summary>
                public const string Org_MarkerTitle = "Org_MarkerTitle";

                /// <summary>
                /// 标注点显示内容
                /// </summary>
                public const string Org_MarkerContent = "Org_MarkerContent";

                /// <summary>
                /// 主营品牌
                /// </summary>
                public const string Org_MainBrands = "Org_MainBrands";

                /// <summary>
                /// 主营产品
                /// </summary>
                public const string Org_MainProducts = "Org_MainProducts";

                /// <summary>
                /// 备注
                /// </summary>
                public const string Org_Remark = "Org_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Org_IsValid = "Org_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Org_CreatedBy = "Org_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Org_CreatedTime = "Org_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Org_UpdatedBy = "Org_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Org_UpdatedTime = "Org_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Org_VersionNo = "Org_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Org_TransID = "Org_TransID";

            }

        }
        /// <summary>
        /// 菜单
        /// </summary>
        public class SM_Menu
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 菜单ID
                /// </summary>
                public const string Menu_ID = "菜单ID";

                /// <summary>
                /// 名称
                /// </summary>
                public const string Menu_Name = "名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string Menu_Remark = "备注";

                /// <summary>
                /// 编码
                /// </summary>
                public const string Menu_Code = "编码";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string Menu_Index = "顺序";

                /// <summary>
                /// 是否可见
                /// </summary>
                public const string Menu_IsVisible = "是否可见";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Menu_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Menu_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Menu_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Menu_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Menu_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Menu_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Menu_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 菜单ID
                /// </summary>
                public const string Menu_ID = "Menu_ID";

                /// <summary>
                /// 名称
                /// </summary>
                public const string Menu_Name = "Menu_Name";

                /// <summary>
                /// 备注
                /// </summary>
                public const string Menu_Remark = "Menu_Remark";

                /// <summary>
                /// 编码
                /// </summary>
                public const string Menu_Code = "Menu_Code";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string Menu_Index = "Menu_Index";

                /// <summary>
                /// 是否可见
                /// </summary>
                public const string Menu_IsVisible = "Menu_IsVisible";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Menu_IsValid = "Menu_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Menu_CreatedBy = "Menu_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Menu_CreatedTime = "Menu_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Menu_UpdatedBy = "Menu_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Menu_UpdatedTime = "Menu_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Menu_VersionNo = "Menu_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Menu_TransID = "Menu_TransID";

            }

        }
        /// <summary>
        /// 菜单分组
        /// </summary>
        public class SM_MenuGroup
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 菜单分组ID
                /// </summary>
                public const string MenuG_ID = "菜单分组ID";

                /// <summary>
                /// 菜单ID
                /// </summary>
                public const string MenuG_Menu_ID = "菜单ID";

                /// <summary>
                /// 名称
                /// </summary>
                public const string MenuG_Name = "名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string MenuG_Remark = "备注";

                /// <summary>
                /// 编码
                /// </summary>
                public const string MenuG_Code = "编码";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string MenuG_Index = "顺序";

                /// <summary>
                /// 是否可见
                /// </summary>
                public const string MenuG_IsVisible = "是否可见";

                /// <summary>
                /// 有效
                /// </summary>
                public const string MenuG_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string MenuG_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string MenuG_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string MenuG_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string MenuG_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string MenuG_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string MenuG_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 菜单分组ID
                /// </summary>
                public const string MenuG_ID = "MenuG_ID";

                /// <summary>
                /// 菜单ID
                /// </summary>
                public const string MenuG_Menu_ID = "MenuG_Menu_ID";

                /// <summary>
                /// 名称
                /// </summary>
                public const string MenuG_Name = "MenuG_Name";

                /// <summary>
                /// 备注
                /// </summary>
                public const string MenuG_Remark = "MenuG_Remark";

                /// <summary>
                /// 编码
                /// </summary>
                public const string MenuG_Code = "MenuG_Code";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string MenuG_Index = "MenuG_Index";

                /// <summary>
                /// 是否可见
                /// </summary>
                public const string MenuG_IsVisible = "MenuG_IsVisible";

                /// <summary>
                /// 有效
                /// </summary>
                public const string MenuG_IsValid = "MenuG_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string MenuG_CreatedBy = "MenuG_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string MenuG_CreatedTime = "MenuG_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string MenuG_UpdatedBy = "MenuG_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string MenuG_UpdatedTime = "MenuG_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string MenuG_VersionNo = "MenuG_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string MenuG_TransID = "MenuG_TransID";

            }

        }
        /// <summary>
        /// 菜单明细
        /// </summary>
        public class SM_MenuDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 菜单明细ID
                /// </summary>
                public const string MenuD_ID = "菜单明细ID";

                /// <summary>
                /// 菜单ID
                /// </summary>
                public const string MenuD_Menu_ID = "菜单ID";

                /// <summary>
                /// 菜单分组ID
                /// </summary>
                public const string MenuD_MenuG_ID = "菜单分组ID";

                /// <summary>
                /// 名称
                /// </summary>
                public const string MenuD_Name = "名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string MenuD_Remark = "备注";

                /// <summary>
                /// 编码
                /// </summary>
                public const string MenuD_Code = "编码";

                /// <summary>
                /// 图标
                /// </summary>
                public const string MenuD_Picture = "图标";

                /// <summary>
                /// 图标Key
                /// </summary>
                public const string MenuD_ImgListKey = "图标Key";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string MenuD_Index = "顺序";

                /// <summary>
                /// 类全名
                /// </summary>
                public const string MenuD_ClassFullName = "类全名";

                /// <summary>
                /// URI
                /// </summary>
                public const string MenuD_URI = "URI";

                /// <summary>
                /// 是否可见
                /// </summary>
                public const string MenuD_IsVisible = "是否可见";

                /// <summary>
                /// Grid页面大小
                /// </summary>
                public const string MenuD_GridPageSize = "Grid页面大小";

                /// <summary>
                /// 有效
                /// </summary>
                public const string MenuD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string MenuD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string MenuD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string MenuD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string MenuD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string MenuD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string MenuD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 菜单明细ID
                /// </summary>
                public const string MenuD_ID = "MenuD_ID";

                /// <summary>
                /// 菜单ID
                /// </summary>
                public const string MenuD_Menu_ID = "MenuD_Menu_ID";

                /// <summary>
                /// 菜单分组ID
                /// </summary>
                public const string MenuD_MenuG_ID = "MenuD_MenuG_ID";

                /// <summary>
                /// 名称
                /// </summary>
                public const string MenuD_Name = "MenuD_Name";

                /// <summary>
                /// 备注
                /// </summary>
                public const string MenuD_Remark = "MenuD_Remark";

                /// <summary>
                /// 编码
                /// </summary>
                public const string MenuD_Code = "MenuD_Code";

                /// <summary>
                /// 图标
                /// </summary>
                public const string MenuD_Picture = "MenuD_Picture";

                /// <summary>
                /// 图标Key
                /// </summary>
                public const string MenuD_ImgListKey = "MenuD_ImgListKey";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string MenuD_Index = "MenuD_Index";

                /// <summary>
                /// 类全名
                /// </summary>
                public const string MenuD_ClassFullName = "MenuD_ClassFullName";

                /// <summary>
                /// URI
                /// </summary>
                public const string MenuD_URI = "MenuD_URI";

                /// <summary>
                /// 是否可见
                /// </summary>
                public const string MenuD_IsVisible = "MenuD_IsVisible";

                /// <summary>
                /// Grid页面大小
                /// </summary>
                public const string MenuD_GridPageSize = "MenuD_GridPageSize";

                /// <summary>
                /// 有效
                /// </summary>
                public const string MenuD_IsValid = "MenuD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string MenuD_CreatedBy = "MenuD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string MenuD_CreatedTime = "MenuD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string MenuD_UpdatedBy = "MenuD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string MenuD_UpdatedTime = "MenuD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string MenuD_VersionNo = "MenuD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string MenuD_TransID = "MenuD_TransID";

            }

        }
        /// <summary>
        /// 系统动作
        /// </summary>
        public class SM_Action
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 动作ID
                /// </summary>
                public const string Act_ID = "动作ID";

                /// <summary>
                /// Key
                /// </summary>
                public const string Act_Key = "Key";

                /// <summary>
                /// 名称
                /// </summary>
                public const string Act_Name = "名称";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string Act_Index = "顺序";

                /// <summary>
                /// 是否显示到界面
                /// </summary>
                public const string Act_IsDisplayInUI = "是否显示到界面";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Act_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Act_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Act_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Act_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Act_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Act_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Act_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 动作ID
                /// </summary>
                public const string Act_ID = "Act_ID";

                /// <summary>
                /// Key
                /// </summary>
                public const string Act_Key = "Act_Key";

                /// <summary>
                /// 名称
                /// </summary>
                public const string Act_Name = "Act_Name";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string Act_Index = "Act_Index";

                /// <summary>
                /// 是否显示到界面
                /// </summary>
                public const string Act_IsDisplayInUI = "Act_IsDisplayInUI";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Act_IsValid = "Act_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Act_CreatedBy = "Act_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Act_CreatedTime = "Act_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Act_UpdatedBy = "Act_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Act_UpdatedTime = "Act_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Act_VersionNo = "Act_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Act_TransID = "Act_TransID";

            }

        }
        /// <summary>
        /// 菜单明细动作
        /// </summary>
        public class SM_MenuDetailAction
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 菜单明细动作ID
                /// </summary>
                public const string MDA_ID = "菜单明细动作ID";

                /// <summary>
                /// 菜单明细ID
                /// </summary>
                public const string MDA_MenuD_ID = "菜单明细ID";

                /// <summary>
                /// 动作ID
                /// </summary>
                public const string MDA_Action_ID = "动作ID";

                /// <summary>
                /// 有效
                /// </summary>
                public const string MDA_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string MDA_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string MDA_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string MDA_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string MDA_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string MDA_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string MDA_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 菜单明细动作ID
                /// </summary>
                public const string MDA_ID = "MDA_ID";

                /// <summary>
                /// 菜单明细ID
                /// </summary>
                public const string MDA_MenuD_ID = "MDA_MenuD_ID";

                /// <summary>
                /// 动作ID
                /// </summary>
                public const string MDA_Action_ID = "MDA_Action_ID";

                /// <summary>
                /// 有效
                /// </summary>
                public const string MDA_IsValid = "MDA_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string MDA_CreatedBy = "MDA_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string MDA_CreatedTime = "MDA_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string MDA_UpdatedBy = "MDA_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string MDA_UpdatedTime = "MDA_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string MDA_VersionNo = "MDA_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string MDA_TransID = "MDA_TransID";

            }

        }
        /// <summary>
        /// 用户菜单
        /// </summary>
        public class SM_UserMenuAuthority
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 用户菜单权限ID
                /// </summary>
                public const string UMA_ID = "用户菜单权限ID";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string UMA_User_ID = "用户ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string UMA_Org_ID = "组织ID";

                /// <summary>
                /// 菜单明细ID
                /// </summary>
                public const string UMA_MenuD_ID = "菜单明细ID";

                /// <summary>
                /// 有效
                /// </summary>
                public const string UMA_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string UMA_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string UMA_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string UMA_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string UMA_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string UMA_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string UMA_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 用户菜单权限ID
                /// </summary>
                public const string UMA_ID = "UMA_ID";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string UMA_User_ID = "UMA_User_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string UMA_Org_ID = "UMA_Org_ID";

                /// <summary>
                /// 菜单明细ID
                /// </summary>
                public const string UMA_MenuD_ID = "UMA_MenuD_ID";

                /// <summary>
                /// 有效
                /// </summary>
                public const string UMA_IsValid = "UMA_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string UMA_CreatedBy = "UMA_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string UMA_CreatedTime = "UMA_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string UMA_UpdatedBy = "UMA_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string UMA_UpdatedTime = "UMA_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string UMA_VersionNo = "UMA_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string UMA_TransID = "UMA_TransID";

            }

        }
        /// <summary>
        /// 用户菜单动作
        /// </summary>
        public class SM_UserActionAuthority
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 用户菜单动作权限ID
                /// </summary>
                public const string UAA_ID = "用户菜单动作权限ID";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string UAA_User_ID = "用户ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string UAA_Org_ID = "组织ID";

                /// <summary>
                /// 菜单明细ID
                /// </summary>
                public const string UAA_MenuD_ID = "菜单明细ID";

                /// <summary>
                /// 动作ID
                /// </summary>
                public const string UAA_Action_ID = "动作ID";

                /// <summary>
                /// 有效
                /// </summary>
                public const string UAA_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string UAA_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string UAA_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string UAA_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string UAA_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string UAA_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string UAA_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 用户菜单动作权限ID
                /// </summary>
                public const string UAA_ID = "UAA_ID";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string UAA_User_ID = "UAA_User_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string UAA_Org_ID = "UAA_Org_ID";

                /// <summary>
                /// 菜单明细ID
                /// </summary>
                public const string UAA_MenuD_ID = "UAA_MenuD_ID";

                /// <summary>
                /// 动作ID
                /// </summary>
                public const string UAA_Action_ID = "UAA_Action_ID";

                /// <summary>
                /// 有效
                /// </summary>
                public const string UAA_IsValid = "UAA_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string UAA_CreatedBy = "UAA_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string UAA_CreatedTime = "UAA_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string UAA_UpdatedBy = "UAA_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string UAA_UpdatedTime = "UAA_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string UAA_VersionNo = "UAA_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string UAA_TransID = "UAA_TransID";

            }

        }
        /// <summary>
        /// 用户
        /// </summary>
        public class SM_User
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 用户ID
                /// </summary>
                public const string User_ID = "用户ID";

                /// <summary>
                /// 姓名
                /// </summary>
                public const string User_Name = "姓名";

                /// <summary>
                /// 密码
                /// </summary>
                public const string User_Password = "密码";

                /// <summary>
                /// 工号
                /// </summary>
                public const string User_EMPNO = "工号";

                /// <summary>
                /// 身份证号码
                /// </summary>
                public const string User_IDNo = "身份证号码";

                /// <summary>
                /// 性别
                /// </summary>
                public const string User_Sex = "性别";

                /// <summary>
                /// 地址
                /// </summary>
                public const string User_Address = "地址";

                /// <summary>
                /// 电话号码
                /// </summary>
                public const string User_PhoneNo = "电话号码";

                /// <summary>
                /// 是否允许微信认证
                /// </summary>
                public const string User_IsAllowWechatCertificate = "是否允许微信认证";

                /// <summary>
                /// 是否已微信认证
                /// </summary>
                public const string User_IsWechatCertified = "是否已微信认证";

                /// <summary>
                /// 打印标题前缀
                /// </summary>
                public const string User_PrintTitlePrefix = "打印标题前缀";

                /// <summary>
                /// 有效
                /// </summary>
                public const string User_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string User_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string User_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string User_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string User_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string User_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string User_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 用户ID
                /// </summary>
                public const string User_ID = "User_ID";

                /// <summary>
                /// 姓名
                /// </summary>
                public const string User_Name = "User_Name";

                /// <summary>
                /// 密码
                /// </summary>
                public const string User_Password = "User_Password";

                /// <summary>
                /// 工号
                /// </summary>
                public const string User_EMPNO = "User_EMPNO";

                /// <summary>
                /// 身份证号码
                /// </summary>
                public const string User_IDNo = "User_IDNo";

                /// <summary>
                /// 性别
                /// </summary>
                public const string User_Sex = "User_Sex";

                /// <summary>
                /// 地址
                /// </summary>
                public const string User_Address = "User_Address";

                /// <summary>
                /// 电话号码
                /// </summary>
                public const string User_PhoneNo = "User_PhoneNo";

                /// <summary>
                /// 是否允许微信认证
                /// </summary>
                public const string User_IsAllowWechatCertificate = "User_IsAllowWechatCertificate";

                /// <summary>
                /// 是否已微信认证
                /// </summary>
                public const string User_IsWechatCertified = "User_IsWechatCertified";

                /// <summary>
                /// 打印标题前缀
                /// </summary>
                public const string User_PrintTitlePrefix = "User_PrintTitlePrefix";

                /// <summary>
                /// 有效
                /// </summary>
                public const string User_IsValid = "User_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string User_CreatedBy = "User_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string User_CreatedTime = "User_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string User_UpdatedBy = "User_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string User_UpdatedTime = "User_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string User_VersionNo = "User_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string User_TransID = "User_TransID";

            }

        }
        /// <summary>
        /// 用户登录日志
        /// </summary>
        public class SM_UserLoginLog
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string ULL_ID = "ID";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string ULL_User_ID = "用户ID";

                /// <summary>
                /// 用户名
                /// </summary>
                public const string ULL_User_Name = "用户名";

                /// <summary>
                /// IP地址
                /// </summary>
                public const string ULL_IPAdress = "IP地址";

                /// <summary>
                /// MAC地址
                /// </summary>
                public const string ULL_MACAdress = "MAC地址";

                /// <summary>
                /// 主机名称
                /// </summary>
                public const string ULL_HostName = "主机名称";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string ULL_OrgID = "组织ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string ULL_OrgName = "组织名称";

                /// <summary>
                /// 日志类型
                /// </summary>
                public const string ULL_LogType = "日志类型";

                /// <summary>
                /// 终端类型
                /// </summary>
                public const string ULL_TerminalType = "终端类型";

                /// <summary>
                /// 发生时间
                /// </summary>
                public const string ULL_LogTime = "发生时间";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ULL_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ULL_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ULL_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ULL_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ULL_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ULL_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ULL_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string ULL_ID = "ULL_ID";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string ULL_User_ID = "ULL_User_ID";

                /// <summary>
                /// 用户名
                /// </summary>
                public const string ULL_User_Name = "ULL_User_Name";

                /// <summary>
                /// IP地址
                /// </summary>
                public const string ULL_IPAdress = "ULL_IPAdress";

                /// <summary>
                /// MAC地址
                /// </summary>
                public const string ULL_MACAdress = "ULL_MACAdress";

                /// <summary>
                /// 主机名称
                /// </summary>
                public const string ULL_HostName = "ULL_HostName";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string ULL_OrgID = "ULL_OrgID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string ULL_OrgName = "ULL_OrgName";

                /// <summary>
                /// 日志类型
                /// </summary>
                public const string ULL_LogType = "ULL_LogType";

                /// <summary>
                /// 终端类型
                /// </summary>
                public const string ULL_TerminalType = "ULL_TerminalType";

                /// <summary>
                /// 发生时间
                /// </summary>
                public const string ULL_LogTime = "ULL_LogTime";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ULL_IsValid = "ULL_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ULL_CreatedBy = "ULL_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ULL_CreatedTime = "ULL_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ULL_UpdatedBy = "ULL_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ULL_UpdatedTime = "ULL_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ULL_VersionNo = "ULL_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ULL_TransID = "ULL_TransID";

            }

        }
        /// <summary>
        /// 用户组织
        /// </summary>
        public class SM_UserOrg
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 用户组织ID
                /// </summary>
                public const string UO_ID = "用户组织ID";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string UO_User_ID = "用户ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string UO_Org_ID = "组织ID";

                /// <summary>
                /// 有效
                /// </summary>
                public const string UO_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string UO_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string UO_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string UO_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string UO_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string UO_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string UO_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 用户组织ID
                /// </summary>
                public const string UO_ID = "UO_ID";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string UO_User_ID = "UO_User_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string UO_Org_ID = "UO_Org_ID";

                /// <summary>
                /// 有效
                /// </summary>
                public const string UO_IsValid = "UO_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string UO_CreatedBy = "UO_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string UO_CreatedTime = "UO_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string UO_UpdatedBy = "UO_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string UO_UpdatedTime = "UO_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string UO_VersionNo = "UO_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string UO_TransID = "UO_TransID";

            }

        }
        /// <summary>
        /// 用户业务角色
        /// </summary>
        public class SM_UserBusinessRole
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string UBR_ID = "ID";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string UBR_User_ID = "用户ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string UBR_Org_ID = "组织ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string UBR_Org_Name = "组织名称";

                /// <summary>
                /// 班组名称
                /// </summary>
                public const string UBR_GroupName = "班组名称";

                /// <summary>
                /// 业务角色
                /// </summary>
                public const string UBR_BusinessRole = "业务角色";

                /// <summary>
                /// 业务工种
                /// </summary>
                public const string UBR_Jobs = "业务工种";

                /// <summary>
                /// 行业证件类型
                /// </summary>
                public const string UBR_CertType = "行业证件类型";

                /// <summary>
                /// 证件号码
                /// </summary>
                public const string UBR_CertNo = "证件号码";

                /// <summary>
                /// 能力级别
                /// </summary>
                public const string UBR_TecLevel = "能力级别";

                /// <summary>
                /// 绩效系数
                /// </summary>
                public const string UBR_PerformanceRatio = "绩效系数";

                /// <summary>
                /// 是否适用于APP
                /// </summary>
                public const string UBR_IsSuitableForApp = "是否适用于APP";

                /// <summary>
                /// 有效
                /// </summary>
                public const string UBR_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string UBR_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string UBR_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string UBR_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string UBR_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string UBR_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string UBR_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string UBR_ID = "UBR_ID";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string UBR_User_ID = "UBR_User_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string UBR_Org_ID = "UBR_Org_ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string UBR_Org_Name = "UBR_Org_Name";

                /// <summary>
                /// 班组名称
                /// </summary>
                public const string UBR_GroupName = "UBR_GroupName";

                /// <summary>
                /// 业务角色
                /// </summary>
                public const string UBR_BusinessRole = "UBR_BusinessRole";

                /// <summary>
                /// 业务工种
                /// </summary>
                public const string UBR_Jobs = "UBR_Jobs";

                /// <summary>
                /// 行业证件类型
                /// </summary>
                public const string UBR_CertType = "UBR_CertType";

                /// <summary>
                /// 证件号码
                /// </summary>
                public const string UBR_CertNo = "UBR_CertNo";

                /// <summary>
                /// 能力级别
                /// </summary>
                public const string UBR_TecLevel = "UBR_TecLevel";

                /// <summary>
                /// 绩效系数
                /// </summary>
                public const string UBR_PerformanceRatio = "UBR_PerformanceRatio";

                /// <summary>
                /// 是否适用于APP
                /// </summary>
                public const string UBR_IsSuitableForApp = "UBR_IsSuitableForApp";

                /// <summary>
                /// 有效
                /// </summary>
                public const string UBR_IsValid = "UBR_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string UBR_CreatedBy = "UBR_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string UBR_CreatedTime = "UBR_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string UBR_UpdatedBy = "UBR_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string UBR_UpdatedTime = "UBR_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string UBR_VersionNo = "UBR_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string UBR_TransID = "UBR_TransID";

            }

        }
        /// <summary>
        /// 用户作业权限
        /// </summary>
        public class SM_UserJobAuthority
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 用户作业权限ID
                /// </summary>
                public const string UJA_ID = "用户作业权限ID";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string UJA_User_ID = "用户ID";

                /// <summary>
                /// 组织
                /// </summary>
                public const string UJA_Org_ID = "组织";

                /// <summary>
                /// 作业ID
                /// </summary>
                public const string UJA_BJ_ID = "作业ID";

                /// <summary>
                /// 有效
                /// </summary>
                public const string UJA_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string UJA_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string UJA_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string UJA_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string UJA_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string UJA_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string UJA_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 用户作业权限ID
                /// </summary>
                public const string UJA_ID = "UJA_ID";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string UJA_User_ID = "UJA_User_ID";

                /// <summary>
                /// 组织
                /// </summary>
                public const string UJA_Org_ID = "UJA_Org_ID";

                /// <summary>
                /// 作业ID
                /// </summary>
                public const string UJA_BJ_ID = "UJA_BJ_ID";

                /// <summary>
                /// 有效
                /// </summary>
                public const string UJA_IsValid = "UJA_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string UJA_CreatedBy = "UJA_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string UJA_CreatedTime = "UJA_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string UJA_UpdatedBy = "UJA_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string UJA_UpdatedTime = "UJA_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string UJA_VersionNo = "UJA_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string UJA_TransID = "UJA_TransID";

            }

        }
        /// <summary>
        /// 汽配汽修商户授权
        /// </summary>
        public class SM_AROrgSupMerchantAuthority
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string ASAH_ID = "ID";

                /// <summary>
                /// 汽修商户编码
                /// </summary>
                public const string ASAH_ARMerchant_Code = "汽修商户编码";

                /// <summary>
                /// 汽修商户名称
                /// </summary>
                public const string ASAH_ARMerchant_Name = "汽修商户名称";

                /// <summary>
                /// 汽修组织编码
                /// </summary>
                public const string ASAH_AROrg_Code = "汽修组织编码";

                /// <summary>
                /// 汽修组织名称
                /// </summary>
                public const string ASAH_AROrg_Name = "汽修组织名称";

                /// <summary>
                /// 汽修组织联系人
                /// </summary>
                public const string ASAH_AROrg_Contacter = "汽修组织联系人";

                /// <summary>
                /// 汽修组织联系方式
                /// </summary>
                public const string ASAH_AROrg_Phone = "汽修组织联系方式";

                /// <summary>
                /// 备注
                /// </summary>
                public const string ASAH_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ASAH_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ASAH_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ASAH_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ASAH_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ASAH_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ASAH_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ASAH_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string ASAH_ID = "ASAH_ID";

                /// <summary>
                /// 汽修商户编码
                /// </summary>
                public const string ASAH_ARMerchant_Code = "ASAH_ARMerchant_Code";

                /// <summary>
                /// 汽修商户名称
                /// </summary>
                public const string ASAH_ARMerchant_Name = "ASAH_ARMerchant_Name";

                /// <summary>
                /// 汽修组织编码
                /// </summary>
                public const string ASAH_AROrg_Code = "ASAH_AROrg_Code";

                /// <summary>
                /// 汽修组织名称
                /// </summary>
                public const string ASAH_AROrg_Name = "ASAH_AROrg_Name";

                /// <summary>
                /// 汽修组织联系人
                /// </summary>
                public const string ASAH_AROrg_Contacter = "ASAH_AROrg_Contacter";

                /// <summary>
                /// 汽修组织联系方式
                /// </summary>
                public const string ASAH_AROrg_Phone = "ASAH_AROrg_Phone";

                /// <summary>
                /// 备注
                /// </summary>
                public const string ASAH_Remark = "ASAH_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ASAH_IsValid = "ASAH_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ASAH_CreatedBy = "ASAH_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ASAH_CreatedTime = "ASAH_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ASAH_UpdatedBy = "ASAH_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ASAH_UpdatedTime = "ASAH_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ASAH_VersionNo = "ASAH_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ASAH_TransID = "ASAH_TransID";

            }

        }
        /// <summary>
        /// 汽配汽修组织授权
        /// </summary>
        public class SM_AROrgSupOrgAuthority
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string ASOAH_ID = "ID";

                /// <summary>
                /// 汽配组织ID
                /// </summary>
                public const string ASOAH_SupOrg_ID = "汽配组织ID";

                /// <summary>
                /// 汽修商客户ID
                /// </summary>
                public const string ASOAH_AFC_ID = "汽修商客户ID";

                /// <summary>
                /// 汽修商户编码
                /// </summary>
                public const string ASOAH_ARMerchant_Code = "汽修商户编码";

                /// <summary>
                /// 汽修商户名称
                /// </summary>
                public const string ASOAH_ARMerchant_Name = "汽修商户名称";

                /// <summary>
                /// 汽修组织编码
                /// </summary>
                public const string ASOAH_AROrg_Code = "汽修组织编码";

                /// <summary>
                /// 汽修组织名称
                /// </summary>
                public const string ASOAH_AROrg_Name = "汽修组织名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string ASOAH_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ASOAH_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ASOAH_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ASOAH_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ASOAH_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ASOAH_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ASOAH_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ASOAH_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string ASOAH_ID = "ASOAH_ID";

                /// <summary>
                /// 汽配组织ID
                /// </summary>
                public const string ASOAH_SupOrg_ID = "ASOAH_SupOrg_ID";

                /// <summary>
                /// 汽修商客户ID
                /// </summary>
                public const string ASOAH_AFC_ID = "ASOAH_AFC_ID";

                /// <summary>
                /// 汽修商户编码
                /// </summary>
                public const string ASOAH_ARMerchant_Code = "ASOAH_ARMerchant_Code";

                /// <summary>
                /// 汽修商户名称
                /// </summary>
                public const string ASOAH_ARMerchant_Name = "ASOAH_ARMerchant_Name";

                /// <summary>
                /// 汽修组织编码
                /// </summary>
                public const string ASOAH_AROrg_Code = "ASOAH_AROrg_Code";

                /// <summary>
                /// 汽修组织名称
                /// </summary>
                public const string ASOAH_AROrg_Name = "ASOAH_AROrg_Name";

                /// <summary>
                /// 备注
                /// </summary>
                public const string ASOAH_Remark = "ASOAH_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ASOAH_IsValid = "ASOAH_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ASOAH_CreatedBy = "ASOAH_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ASOAH_CreatedTime = "ASOAH_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ASOAH_UpdatedBy = "ASOAH_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ASOAH_UpdatedTime = "ASOAH_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ASOAH_VersionNo = "ASOAH_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ASOAH_TransID = "ASOAH_TransID";

            }

        }
        /// <summary>
        /// 使用许可证
        /// </summary>
        public class SM_ClientUseLicense
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 许可证ID
                /// </summary>
                public const string CUL_ID = "许可证ID";

                /// <summary>
                /// 许可证号
                /// </summary>
                public const string CUL_No = "许可证号";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string CUL_Org_ID = "组织ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string CUL_Org_Name = "组织名称";

                /// <summary>
                /// 用户姓名
                /// </summary>
                public const string CUL_Name = "用户姓名";

                /// <summary>
                /// 申请原因
                /// </summary>
                public const string CUL_ApplyReason = "申请原因";

                /// <summary>
                /// 联系方式
                /// </summary>
                public const string CUL_ContactNo = "联系方式";

                /// <summary>
                /// 网卡类型1
                /// </summary>
                public const string CUL_NetworkCardType1 = "网卡类型1";

                /// <summary>
                /// 网卡地址1
                /// </summary>
                public const string CUL_MACAdress1 = "网卡地址1";

                /// <summary>
                /// 网卡类型2
                /// </summary>
                public const string CUL_NetworkCardType2 = "网卡类型2";

                /// <summary>
                /// 网卡地址2
                /// </summary>
                public const string CUL_MACAdress2 = "网卡地址2";

                /// <summary>
                /// 网卡类型3
                /// </summary>
                public const string CUL_NetworkCardType3 = "网卡类型3";

                /// <summary>
                /// 网卡地址3
                /// </summary>
                public const string CUL_MACAdress3 = "网卡地址3";

                /// <summary>
                /// 审核状态
                /// </summary>
                public const string CUL_ApproveStatus = "审核状态";

                /// <summary>
                /// 失效日期
                /// </summary>
                public const string CUL_InvalidDate = "失效日期";

                /// <summary>
                /// 备注
                /// </summary>
                public const string CUL_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string CUL_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string CUL_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string CUL_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string CUL_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string CUL_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string CUL_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string CUL_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 许可证ID
                /// </summary>
                public const string CUL_ID = "CUL_ID";

                /// <summary>
                /// 许可证号
                /// </summary>
                public const string CUL_No = "CUL_No";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string CUL_Org_ID = "CUL_Org_ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string CUL_Org_Name = "CUL_Org_Name";

                /// <summary>
                /// 用户姓名
                /// </summary>
                public const string CUL_Name = "CUL_Name";

                /// <summary>
                /// 申请原因
                /// </summary>
                public const string CUL_ApplyReason = "CUL_ApplyReason";

                /// <summary>
                /// 联系方式
                /// </summary>
                public const string CUL_ContactNo = "CUL_ContactNo";

                /// <summary>
                /// 网卡类型1
                /// </summary>
                public const string CUL_NetworkCardType1 = "CUL_NetworkCardType1";

                /// <summary>
                /// 网卡地址1
                /// </summary>
                public const string CUL_MACAdress1 = "CUL_MACAdress1";

                /// <summary>
                /// 网卡类型2
                /// </summary>
                public const string CUL_NetworkCardType2 = "CUL_NetworkCardType2";

                /// <summary>
                /// 网卡地址2
                /// </summary>
                public const string CUL_MACAdress2 = "CUL_MACAdress2";

                /// <summary>
                /// 网卡类型3
                /// </summary>
                public const string CUL_NetworkCardType3 = "CUL_NetworkCardType3";

                /// <summary>
                /// 网卡地址3
                /// </summary>
                public const string CUL_MACAdress3 = "CUL_MACAdress3";

                /// <summary>
                /// 审核状态
                /// </summary>
                public const string CUL_ApproveStatus = "CUL_ApproveStatus";

                /// <summary>
                /// 失效日期
                /// </summary>
                public const string CUL_InvalidDate = "CUL_InvalidDate";

                /// <summary>
                /// 备注
                /// </summary>
                public const string CUL_Remark = "CUL_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string CUL_IsValid = "CUL_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string CUL_CreatedBy = "CUL_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string CUL_CreatedTime = "CUL_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string CUL_UpdatedBy = "CUL_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string CUL_UpdatedTime = "CUL_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string CUL_VersionNo = "CUL_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string CUL_TransID = "CUL_TransID";

            }

        }
        /// <summary>
        /// 系统参数
        /// </summary>
        public class SM_Parameter
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 参数ID
                /// </summary>
                public const string Para_ID = "参数ID";

                /// <summary>
                /// 参数编码1
                /// </summary>
                public const string Para_Code1 = "参数编码1";

                /// <summary>
                /// 参数描述1
                /// </summary>
                public const string Para_Name1 = "参数描述1";

                /// <summary>
                /// 参数值1
                /// </summary>
                public const string Para_Value1 = "参数值1";

                /// <summary>
                /// 参数编码2
                /// </summary>
                public const string Para_Code2 = "参数编码2";

                /// <summary>
                /// 参数描述2
                /// </summary>
                public const string Para_Name2 = "参数描述2";

                /// <summary>
                /// 参数值2
                /// </summary>
                public const string Para_Value2 = "参数值2";

                /// <summary>
                /// 参数编码3
                /// </summary>
                public const string Para_Code3 = "参数编码3";

                /// <summary>
                /// 参数描述3
                /// </summary>
                public const string Para_Name3 = "参数描述3";

                /// <summary>
                /// 参数值3
                /// </summary>
                public const string Para_Value3 = "参数值3";

                /// <summary>
                /// 参数编码4
                /// </summary>
                public const string Para_Code4 = "参数编码4";

                /// <summary>
                /// 参数描述4
                /// </summary>
                public const string Para_Name4 = "参数描述4";

                /// <summary>
                /// 参数值4
                /// </summary>
                public const string Para_Value4 = "参数值4";

                /// <summary>
                /// 参数编码5
                /// </summary>
                public const string Para_Code5 = "参数编码5";

                /// <summary>
                /// 参数描述5
                /// </summary>
                public const string Para_Name5 = "参数描述5";

                /// <summary>
                /// 参数值5
                /// </summary>
                public const string Para_Value5 = "参数值5";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Para_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Para_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Para_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Para_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Para_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Para_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Para_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 参数ID
                /// </summary>
                public const string Para_ID = "Para_ID";

                /// <summary>
                /// 参数编码1
                /// </summary>
                public const string Para_Code1 = "Para_Code1";

                /// <summary>
                /// 参数描述1
                /// </summary>
                public const string Para_Name1 = "Para_Name1";

                /// <summary>
                /// 参数值1
                /// </summary>
                public const string Para_Value1 = "Para_Value1";

                /// <summary>
                /// 参数编码2
                /// </summary>
                public const string Para_Code2 = "Para_Code2";

                /// <summary>
                /// 参数描述2
                /// </summary>
                public const string Para_Name2 = "Para_Name2";

                /// <summary>
                /// 参数值2
                /// </summary>
                public const string Para_Value2 = "Para_Value2";

                /// <summary>
                /// 参数编码3
                /// </summary>
                public const string Para_Code3 = "Para_Code3";

                /// <summary>
                /// 参数描述3
                /// </summary>
                public const string Para_Name3 = "Para_Name3";

                /// <summary>
                /// 参数值3
                /// </summary>
                public const string Para_Value3 = "Para_Value3";

                /// <summary>
                /// 参数编码4
                /// </summary>
                public const string Para_Code4 = "Para_Code4";

                /// <summary>
                /// 参数描述4
                /// </summary>
                public const string Para_Name4 = "Para_Name4";

                /// <summary>
                /// 参数值4
                /// </summary>
                public const string Para_Value4 = "Para_Value4";

                /// <summary>
                /// 参数编码5
                /// </summary>
                public const string Para_Code5 = "Para_Code5";

                /// <summary>
                /// 参数描述5
                /// </summary>
                public const string Para_Name5 = "Para_Name5";

                /// <summary>
                /// 参数值5
                /// </summary>
                public const string Para_Value5 = "Para_Value5";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Para_IsValid = "Para_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Para_CreatedBy = "Para_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Para_CreatedTime = "Para_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Para_UpdatedBy = "Para_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Para_UpdatedTime = "Para_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Para_VersionNo = "Para_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Para_TransID = "Para_TransID";

            }

        }
        /// <summary>
        /// 码表
        /// </summary>
        public class SM_CodeTable
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string CT_ID = "ID";

                /// <summary>
                /// 类型
                /// </summary>
                public const string CT_Type = "类型";

                /// <summary>
                /// 参数名称
                /// </summary>
                public const string CT_Name = "参数名称";

                /// <summary>
                /// 参数值
                /// </summary>
                public const string CT_Value = "参数值";

                /// <summary>
                /// 参数描述
                /// </summary>
                public const string CT_Desc = "参数描述";

                /// <summary>
                /// 有效
                /// </summary>
                public const string CT_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string CT_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string CT_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string CT_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string CT_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string CT_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string CT_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string CT_ID = "CT_ID";

                /// <summary>
                /// 类型
                /// </summary>
                public const string CT_Type = "CT_Type";

                /// <summary>
                /// 参数名称
                /// </summary>
                public const string CT_Name = "CT_Name";

                /// <summary>
                /// 参数值
                /// </summary>
                public const string CT_Value = "CT_Value";

                /// <summary>
                /// 参数描述
                /// </summary>
                public const string CT_Desc = "CT_Desc";

                /// <summary>
                /// 有效
                /// </summary>
                public const string CT_IsValid = "CT_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string CT_CreatedBy = "CT_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string CT_CreatedTime = "CT_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string CT_UpdatedBy = "CT_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string CT_UpdatedTime = "CT_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string CT_VersionNo = "CT_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string CT_TransID = "CT_TransID";

            }

        }
        /// <summary>
        /// 系统枚举
        /// </summary>
        public class SM_Enum
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 枚举ID
                /// </summary>
                public const string Enum_ID = "枚举ID";

                /// <summary>
                /// 枚举Key
                /// </summary>
                public const string Enum_Key = "枚举Key";

                /// <summary>
                /// 枚举名称
                /// </summary>
                public const string Enum_Name = "枚举名称";

                /// <summary>
                /// 枚举值编码
                /// </summary>
                public const string Enum_ValueCode = "枚举值编码";

                /// <summary>
                /// 枚举值
                /// </summary>
                public const string Enum_Value = "枚举值";

                /// <summary>
                /// 枚举显示名称
                /// </summary>
                public const string Enum_DisplayName = "枚举显示名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string Enum_Info = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Enum_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Enum_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Enum_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Enum_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Enum_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Enum_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Enum_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 枚举ID
                /// </summary>
                public const string Enum_ID = "Enum_ID";

                /// <summary>
                /// 枚举Key
                /// </summary>
                public const string Enum_Key = "Enum_Key";

                /// <summary>
                /// 枚举名称
                /// </summary>
                public const string Enum_Name = "Enum_Name";

                /// <summary>
                /// 枚举值编码
                /// </summary>
                public const string Enum_ValueCode = "Enum_ValueCode";

                /// <summary>
                /// 枚举值
                /// </summary>
                public const string Enum_Value = "Enum_Value";

                /// <summary>
                /// 枚举显示名称
                /// </summary>
                public const string Enum_DisplayName = "Enum_DisplayName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string Enum_Info = "Enum_Info";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Enum_IsValid = "Enum_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Enum_CreatedBy = "Enum_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Enum_CreatedTime = "Enum_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Enum_UpdatedBy = "Enum_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Enum_UpdatedTime = "Enum_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Enum_VersionNo = "Enum_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Enum_TransID = "Enum_TransID";

            }

        }
        /// <summary>
        /// 系统消息
        /// </summary>
        public class SM_Message
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 消息ID
                /// </summary>
                public const string Msg_ID = "消息ID";

                /// <summary>
                /// 编码
                /// </summary>
                public const string Msg_Code = "编码";

                /// <summary>
                /// 内容
                /// </summary>
                public const string Msg_Content = "内容";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Msg_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Msg_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Msg_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Msg_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Msg_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Msg_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Msg_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 消息ID
                /// </summary>
                public const string Msg_ID = "Msg_ID";

                /// <summary>
                /// 编码
                /// </summary>
                public const string Msg_Code = "Msg_Code";

                /// <summary>
                /// 内容
                /// </summary>
                public const string Msg_Content = "Msg_Content";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Msg_IsValid = "Msg_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Msg_CreatedBy = "Msg_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Msg_CreatedTime = "Msg_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Msg_UpdatedBy = "Msg_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Msg_UpdatedTime = "Msg_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Msg_VersionNo = "Msg_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Msg_TransID = "Msg_TransID";

            }

        }
        /// <summary>
        /// 消息推送
        /// </summary>
        public class SM_PushMesage
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string PM_ID = "ID";

                /// <summary>
                /// 推送商户ID
                /// </summary>
                public const string PM_MCT_ID = "推送商户ID";

                /// <summary>
                /// 推送产品ID
                /// </summary>
                public const string PM_SP_ID = "推送产品ID";

                /// <summary>
                /// 推送内容
                /// </summary>
                public const string PM_Content = "推送内容";

                /// <summary>
                /// 发送人
                /// </summary>
                public const string PM_Sender = "发送人";

                /// <summary>
                /// 接受人
                /// </summary>
                public const string PM_Receiver = "接受人";

                /// <summary>
                /// 推送方式
                /// </summary>
                public const string PM_PushType = "推送方式";

                /// <summary>
                /// 发送时间
                /// </summary>
                public const string PM_SendTime = "发送时间";

                /// <summary>
                /// 查看时间
                /// </summary>
                public const string PM_ReadTime = "查看时间";

                /// <summary>
                /// 备注
                /// </summary>
                public const string PM_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string PM_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string PM_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string PM_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string PM_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string PM_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string PM_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string PM_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string PM_ID = "PM_ID";

                /// <summary>
                /// 推送商户ID
                /// </summary>
                public const string PM_MCT_ID = "PM_MCT_ID";

                /// <summary>
                /// 推送产品ID
                /// </summary>
                public const string PM_SP_ID = "PM_SP_ID";

                /// <summary>
                /// 推送内容
                /// </summary>
                public const string PM_Content = "PM_Content";

                /// <summary>
                /// 发送人
                /// </summary>
                public const string PM_Sender = "PM_Sender";

                /// <summary>
                /// 接受人
                /// </summary>
                public const string PM_Receiver = "PM_Receiver";

                /// <summary>
                /// 推送方式
                /// </summary>
                public const string PM_PushType = "PM_PushType";

                /// <summary>
                /// 发送时间
                /// </summary>
                public const string PM_SendTime = "PM_SendTime";

                /// <summary>
                /// 查看时间
                /// </summary>
                public const string PM_ReadTime = "PM_ReadTime";

                /// <summary>
                /// 备注
                /// </summary>
                public const string PM_Remark = "PM_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string PM_IsValid = "PM_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string PM_CreatedBy = "PM_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string PM_CreatedTime = "PM_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string PM_UpdatedBy = "PM_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string PM_UpdatedTime = "PM_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string PM_VersionNo = "PM_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string PM_TransID = "PM_TransID";

            }

        }
        /// <summary>
        /// 中国大区
        /// </summary>
        public class SM_ChineseRegion
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string Reg_ID = "ID";

                /// <summary>
                /// 编码
                /// </summary>
                public const string Reg_Code = "编码";

                /// <summary>
                /// 名称
                /// </summary>
                public const string Reg_Name = "名称";

                /// <summary>
                /// 简称
                /// </summary>
                public const string Reg_ShortName = "简称";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string Reg_Index = "顺序";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Reg_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Reg_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Reg_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Reg_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Reg_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Reg_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Reg_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string Reg_ID = "Reg_ID";

                /// <summary>
                /// 编码
                /// </summary>
                public const string Reg_Code = "Reg_Code";

                /// <summary>
                /// 名称
                /// </summary>
                public const string Reg_Name = "Reg_Name";

                /// <summary>
                /// 简称
                /// </summary>
                public const string Reg_ShortName = "Reg_ShortName";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string Reg_Index = "Reg_Index";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Reg_IsValid = "Reg_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Reg_CreatedBy = "Reg_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Reg_CreatedTime = "Reg_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Reg_UpdatedBy = "Reg_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Reg_UpdatedTime = "Reg_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Reg_VersionNo = "Reg_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Reg_TransID = "Reg_TransID";

            }

        }
        /// <summary>
        /// 中国省份
        /// </summary>
        public class SM_ChineseProvince
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string Prov_ID = "ID";

                /// <summary>
                /// 大区ID
                /// </summary>
                public const string Prov_Reg_ID = "大区ID";

                /// <summary>
                /// 编码
                /// </summary>
                public const string Prov_Code = "编码";

                /// <summary>
                /// 名称
                /// </summary>
                public const string Prov_Name = "名称";

                /// <summary>
                /// 简称
                /// </summary>
                public const string Prov_ShortName = "简称";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string Prov_Index = "顺序";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Prov_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Prov_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Prov_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Prov_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Prov_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Prov_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Prov_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string Prov_ID = "Prov_ID";

                /// <summary>
                /// 大区ID
                /// </summary>
                public const string Prov_Reg_ID = "Prov_Reg_ID";

                /// <summary>
                /// 编码
                /// </summary>
                public const string Prov_Code = "Prov_Code";

                /// <summary>
                /// 名称
                /// </summary>
                public const string Prov_Name = "Prov_Name";

                /// <summary>
                /// 简称
                /// </summary>
                public const string Prov_ShortName = "Prov_ShortName";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string Prov_Index = "Prov_Index";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Prov_IsValid = "Prov_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Prov_CreatedBy = "Prov_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Prov_CreatedTime = "Prov_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Prov_UpdatedBy = "Prov_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Prov_UpdatedTime = "Prov_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Prov_VersionNo = "Prov_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Prov_TransID = "Prov_TransID";

            }

        }
        /// <summary>
        /// 省份城市
        /// </summary>
        public class SM_ProvinceCity
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string City_ID = "ID";

                /// <summary>
                /// 大区ID
                /// </summary>
                public const string City_Reg_ID = "大区ID";

                /// <summary>
                /// 城市编码
                /// </summary>
                public const string City_Code = "城市编码";

                /// <summary>
                /// 名称
                /// </summary>
                public const string City_Name = "名称";

                /// <summary>
                /// 车牌编码
                /// </summary>
                public const string City_PlateCode = "车牌编码";

                /// <summary>
                /// 省份编码
                /// </summary>
                public const string City_Prov_Code = "省份编码";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string City_Index = "顺序";

                /// <summary>
                /// 有效
                /// </summary>
                public const string City_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string City_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string City_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string City_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string City_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string City_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string City_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string City_ID = "City_ID";

                /// <summary>
                /// 大区ID
                /// </summary>
                public const string City_Reg_ID = "City_Reg_ID";

                /// <summary>
                /// 城市编码
                /// </summary>
                public const string City_Code = "City_Code";

                /// <summary>
                /// 名称
                /// </summary>
                public const string City_Name = "City_Name";

                /// <summary>
                /// 车牌编码
                /// </summary>
                public const string City_PlateCode = "City_PlateCode";

                /// <summary>
                /// 省份编码
                /// </summary>
                public const string City_Prov_Code = "City_Prov_Code";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string City_Index = "City_Index";

                /// <summary>
                /// 有效
                /// </summary>
                public const string City_IsValid = "City_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string City_CreatedBy = "City_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string City_CreatedTime = "City_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string City_UpdatedBy = "City_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string City_UpdatedTime = "City_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string City_VersionNo = "City_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string City_TransID = "City_TransID";

            }

        }
        /// <summary>
        /// 城市区域
        /// </summary>
        public class SM_CityDistrict
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string Dist_ID = "ID";

                /// <summary>
                /// 区域编码
                /// </summary>
                public const string Dist_Code = "区域编码";

                /// <summary>
                /// 名称
                /// </summary>
                public const string Dist_Name = "名称";

                /// <summary>
                /// 城市编码
                /// </summary>
                public const string Dist_City_Code = "城市编码";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string Dist_Index = "顺序";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Dist_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Dist_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Dist_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Dist_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Dist_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Dist_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Dist_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string Dist_ID = "Dist_ID";

                /// <summary>
                /// 区域编码
                /// </summary>
                public const string Dist_Code = "Dist_Code";

                /// <summary>
                /// 名称
                /// </summary>
                public const string Dist_Name = "Dist_Name";

                /// <summary>
                /// 城市编码
                /// </summary>
                public const string Dist_City_Code = "Dist_City_Code";

                /// <summary>
                /// 顺序
                /// </summary>
                public const string Dist_Index = "Dist_Index";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Dist_IsValid = "Dist_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Dist_CreatedBy = "Dist_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Dist_CreatedTime = "Dist_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Dist_UpdatedBy = "Dist_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Dist_UpdatedTime = "Dist_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Dist_VersionNo = "Dist_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Dist_TransID = "Dist_TransID";

            }

        }
        /// <summary>
        /// 付款单
        /// </summary>
        public class FM_PayBill
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 付款ID
                /// </summary>
                public const string PB_ID = "付款ID";

                /// <summary>
                /// 付款单号
                /// </summary>
                public const string PB_No = "付款单号";

                /// <summary>
                /// 付款组织ID
                /// </summary>
                public const string PB_Pay_Org_ID = "付款组织ID";

                /// <summary>
                /// 付款组织名称
                /// </summary>
                public const string PB_Pay_Org_Name = "付款组织名称";

                /// <summary>
                /// 付款日期
                /// </summary>
                public const string PB_Date = "付款日期";

                /// <summary>
                /// 收款对象类型编码
                /// </summary>
                public const string PB_RecObjectTypeCode = "收款对象类型编码";

                /// <summary>
                /// 收款对象类型名称
                /// </summary>
                public const string PB_RecObjectTypeName = "收款对象类型名称";

                /// <summary>
                /// 收款对象ID
                /// </summary>
                public const string PB_RecObjectID = "收款对象ID";

                /// <summary>
                /// 收款对象
                /// </summary>
                public const string PB_RecObjectName = "收款对象";

                /// <summary>
                /// 应付合计金额
                /// </summary>
                public const string PB_PayableTotalAmount = "应付合计金额";

                /// <summary>
                /// 实付合计金额
                /// </summary>
                public const string PB_RealPayableTotalAmount = "实付合计金额";

                /// <summary>
                /// 付款方账号
                /// </summary>
                public const string PB_PayAccount = "付款方账号";

                /// <summary>
                /// 收款方账号
                /// </summary>
                public const string PB_RecAccount = "收款方账号";

                /// <summary>
                /// 付款方式编码
                /// </summary>
                public const string PB_PayTypeCode = "付款方式编码";

                /// <summary>
                /// 付款方式名称
                /// </summary>
                public const string PB_PayTypeName = "付款方式名称";

                /// <summary>
                /// 付款凭证编号
                /// </summary>
                public const string PB_CertificateNo = "付款凭证编号";

                /// <summary>
                /// 付款凭证图片
                /// </summary>
                public const string PB_CertificatePic = "付款凭证图片";

                /// <summary>
                /// 业务状态编码
                /// </summary>
                public const string PB_BusinessStatusCode = "业务状态编码";

                /// <summary>
                /// 业务状态名称
                /// </summary>
                public const string PB_BusinessStatusName = "业务状态名称";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string PB_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string PB_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string PB_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string PB_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string PB_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string PB_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string PB_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string PB_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string PB_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string PB_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 付款ID
                /// </summary>
                public const string PB_ID = "PB_ID";

                /// <summary>
                /// 付款单号
                /// </summary>
                public const string PB_No = "PB_No";

                /// <summary>
                /// 付款组织ID
                /// </summary>
                public const string PB_Pay_Org_ID = "PB_Pay_Org_ID";

                /// <summary>
                /// 付款组织名称
                /// </summary>
                public const string PB_Pay_Org_Name = "PB_Pay_Org_Name";

                /// <summary>
                /// 付款日期
                /// </summary>
                public const string PB_Date = "PB_Date";

                /// <summary>
                /// 收款对象类型编码
                /// </summary>
                public const string PB_RecObjectTypeCode = "PB_RecObjectTypeCode";

                /// <summary>
                /// 收款对象类型名称
                /// </summary>
                public const string PB_RecObjectTypeName = "PB_RecObjectTypeName";

                /// <summary>
                /// 收款对象ID
                /// </summary>
                public const string PB_RecObjectID = "PB_RecObjectID";

                /// <summary>
                /// 收款对象
                /// </summary>
                public const string PB_RecObjectName = "PB_RecObjectName";

                /// <summary>
                /// 应付合计金额
                /// </summary>
                public const string PB_PayableTotalAmount = "PB_PayableTotalAmount";

                /// <summary>
                /// 实付合计金额
                /// </summary>
                public const string PB_RealPayableTotalAmount = "PB_RealPayableTotalAmount";

                /// <summary>
                /// 付款方账号
                /// </summary>
                public const string PB_PayAccount = "PB_PayAccount";

                /// <summary>
                /// 收款方账号
                /// </summary>
                public const string PB_RecAccount = "PB_RecAccount";

                /// <summary>
                /// 付款方式编码
                /// </summary>
                public const string PB_PayTypeCode = "PB_PayTypeCode";

                /// <summary>
                /// 付款方式名称
                /// </summary>
                public const string PB_PayTypeName = "PB_PayTypeName";

                /// <summary>
                /// 付款凭证编号
                /// </summary>
                public const string PB_CertificateNo = "PB_CertificateNo";

                /// <summary>
                /// 付款凭证图片
                /// </summary>
                public const string PB_CertificatePic = "PB_CertificatePic";

                /// <summary>
                /// 业务状态编码
                /// </summary>
                public const string PB_BusinessStatusCode = "PB_BusinessStatusCode";

                /// <summary>
                /// 业务状态名称
                /// </summary>
                public const string PB_BusinessStatusName = "PB_BusinessStatusName";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string PB_ApprovalStatusCode = "PB_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string PB_ApprovalStatusName = "PB_ApprovalStatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string PB_Remark = "PB_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string PB_IsValid = "PB_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string PB_CreatedBy = "PB_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string PB_CreatedTime = "PB_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string PB_UpdatedBy = "PB_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string PB_UpdatedTime = "PB_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string PB_VersionNo = "PB_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string PB_TransID = "PB_TransID";

            }

        }
        /// <summary>
        /// 付款单明细
        /// </summary>
        public class FM_PayBillDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 付款单明细ID
                /// </summary>
                public const string PBD_ID = "付款单明细ID";

                /// <summary>
                /// 付款单ID
                /// </summary>
                public const string PBD_PB_ID = "付款单ID";

                /// <summary>
                /// 付款单号
                /// </summary>
                public const string PBD_PB_No = "付款单号";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string PBD_SourceTypeCode = "来源类型编码";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string PBD_SourceTypeName = "来源类型名称";

                /// <summary>
                /// 来源单号
                /// </summary>
                public const string PBD_SrcBillNo = "来源单号";

                /// <summary>
                /// 付款金额
                /// </summary>
                public const string PBD_PayAmount = "付款金额";

                /// <summary>
                /// 备注
                /// </summary>
                public const string PBD_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string PBD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string PBD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string PBD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string PBD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string PBD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string PBD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string PBD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 付款单明细ID
                /// </summary>
                public const string PBD_ID = "PBD_ID";

                /// <summary>
                /// 付款单ID
                /// </summary>
                public const string PBD_PB_ID = "PBD_PB_ID";

                /// <summary>
                /// 付款单号
                /// </summary>
                public const string PBD_PB_No = "PBD_PB_No";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string PBD_SourceTypeCode = "PBD_SourceTypeCode";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string PBD_SourceTypeName = "PBD_SourceTypeName";

                /// <summary>
                /// 来源单号
                /// </summary>
                public const string PBD_SrcBillNo = "PBD_SrcBillNo";

                /// <summary>
                /// 付款金额
                /// </summary>
                public const string PBD_PayAmount = "PBD_PayAmount";

                /// <summary>
                /// 备注
                /// </summary>
                public const string PBD_Remark = "PBD_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string PBD_IsValid = "PBD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string PBD_CreatedBy = "PBD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string PBD_CreatedTime = "PBD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string PBD_UpdatedBy = "PBD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string PBD_UpdatedTime = "PBD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string PBD_VersionNo = "PBD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string PBD_TransID = "PBD_TransID";

            }

        }
        /// <summary>
        /// 收款单
        /// </summary>
        public class FM_ReceiptBill
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 收款ID
                /// </summary>
                public const string RB_ID = "收款ID";

                /// <summary>
                /// 收款单号
                /// </summary>
                public const string RB_No = "收款单号";

                /// <summary>
                /// 收款组织ID
                /// </summary>
                public const string RB_Rec_Org_ID = "收款组织ID";

                /// <summary>
                /// 收款组织名称
                /// </summary>
                public const string RB_Rec_Org_Name = "收款组织名称";

                /// <summary>
                /// 收款日期
                /// </summary>
                public const string RB_Date = "收款日期";

                /// <summary>
                /// 付款对象类型编码
                /// </summary>
                public const string RB_PayObjectTypeCode = "付款对象类型编码";

                /// <summary>
                /// 付款对象类型名称
                /// </summary>
                public const string RB_PayObjectTypeName = "付款对象类型名称";

                /// <summary>
                /// 付款对象ID
                /// </summary>
                public const string RB_PayObjectID = "付款对象ID";

                /// <summary>
                /// 付款对象
                /// </summary>
                public const string RB_PayObjectName = "付款对象";

                /// <summary>
                /// 收款通道编码
                /// </summary>
                public const string RB_ReceiveTypeCode = "收款通道编码";

                /// <summary>
                /// 收款通道名称
                /// </summary>
                public const string RB_ReceiveTypeName = "收款通道名称";

                /// <summary>
                /// 收款账号
                /// </summary>
                public const string RB_ReceiveAccount = "收款账号";

                /// <summary>
                /// 收款凭证编号
                /// </summary>
                public const string RB_CertificateNo = "收款凭证编号";

                /// <summary>
                /// 收款凭证图片
                /// </summary>
                public const string RB_CertificatePic = "收款凭证图片";

                /// <summary>
                /// 合计金额
                /// </summary>
                public const string RB_ReceiveTotalAmount = "合计金额";

                /// <summary>
                /// 业务状态编码
                /// </summary>
                public const string RB_BusinessStatusCode = "业务状态编码";

                /// <summary>
                /// 业务状态名称
                /// </summary>
                public const string RB_BusinessStatusName = "业务状态名称";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string RB_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string RB_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string RB_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string RB_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string RB_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string RB_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string RB_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string RB_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string RB_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string RB_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 收款ID
                /// </summary>
                public const string RB_ID = "RB_ID";

                /// <summary>
                /// 收款单号
                /// </summary>
                public const string RB_No = "RB_No";

                /// <summary>
                /// 收款组织ID
                /// </summary>
                public const string RB_Rec_Org_ID = "RB_Rec_Org_ID";

                /// <summary>
                /// 收款组织名称
                /// </summary>
                public const string RB_Rec_Org_Name = "RB_Rec_Org_Name";

                /// <summary>
                /// 收款日期
                /// </summary>
                public const string RB_Date = "RB_Date";

                /// <summary>
                /// 付款对象类型编码
                /// </summary>
                public const string RB_PayObjectTypeCode = "RB_PayObjectTypeCode";

                /// <summary>
                /// 付款对象类型名称
                /// </summary>
                public const string RB_PayObjectTypeName = "RB_PayObjectTypeName";

                /// <summary>
                /// 付款对象ID
                /// </summary>
                public const string RB_PayObjectID = "RB_PayObjectID";

                /// <summary>
                /// 付款对象
                /// </summary>
                public const string RB_PayObjectName = "RB_PayObjectName";

                /// <summary>
                /// 收款通道编码
                /// </summary>
                public const string RB_ReceiveTypeCode = "RB_ReceiveTypeCode";

                /// <summary>
                /// 收款通道名称
                /// </summary>
                public const string RB_ReceiveTypeName = "RB_ReceiveTypeName";

                /// <summary>
                /// 收款账号
                /// </summary>
                public const string RB_ReceiveAccount = "RB_ReceiveAccount";

                /// <summary>
                /// 收款凭证编号
                /// </summary>
                public const string RB_CertificateNo = "RB_CertificateNo";

                /// <summary>
                /// 收款凭证图片
                /// </summary>
                public const string RB_CertificatePic = "RB_CertificatePic";

                /// <summary>
                /// 合计金额
                /// </summary>
                public const string RB_ReceiveTotalAmount = "RB_ReceiveTotalAmount";

                /// <summary>
                /// 业务状态编码
                /// </summary>
                public const string RB_BusinessStatusCode = "RB_BusinessStatusCode";

                /// <summary>
                /// 业务状态名称
                /// </summary>
                public const string RB_BusinessStatusName = "RB_BusinessStatusName";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string RB_ApprovalStatusCode = "RB_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string RB_ApprovalStatusName = "RB_ApprovalStatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string RB_Remark = "RB_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string RB_IsValid = "RB_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string RB_CreatedBy = "RB_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string RB_CreatedTime = "RB_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string RB_UpdatedBy = "RB_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string RB_UpdatedTime = "RB_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string RB_VersionNo = "RB_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string RB_TransID = "RB_TransID";

            }

        }
        /// <summary>
        /// 收款单明细
        /// </summary>
        public class FM_ReceiptBillDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 收款单明细ID
                /// </summary>
                public const string RBD_ID = "收款单明细ID";

                /// <summary>
                /// 收款单ID
                /// </summary>
                public const string RBD_RB_ID = "收款单ID";

                /// <summary>
                /// 收款单号
                /// </summary>
                public const string RBD_RB_No = "收款单号";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string RBD_SourceTypeCode = "来源类型编码";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string RBD_SourceTypeName = "来源类型名称";

                /// <summary>
                /// 来源单号
                /// </summary>
                public const string RBD_SrcBillNo = "来源单号";

                /// <summary>
                /// 收款金额
                /// </summary>
                public const string RBD_ReceiveAmount = "收款金额";

                /// <summary>
                /// 备注
                /// </summary>
                public const string RBD_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string RBD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string RBD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string RBD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string RBD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string RBD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string RBD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string RBD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 收款单明细ID
                /// </summary>
                public const string RBD_ID = "RBD_ID";

                /// <summary>
                /// 收款单ID
                /// </summary>
                public const string RBD_RB_ID = "RBD_RB_ID";

                /// <summary>
                /// 收款单号
                /// </summary>
                public const string RBD_RB_No = "RBD_RB_No";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string RBD_SourceTypeCode = "RBD_SourceTypeCode";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string RBD_SourceTypeName = "RBD_SourceTypeName";

                /// <summary>
                /// 来源单号
                /// </summary>
                public const string RBD_SrcBillNo = "RBD_SrcBillNo";

                /// <summary>
                /// 收款金额
                /// </summary>
                public const string RBD_ReceiveAmount = "RBD_ReceiveAmount";

                /// <summary>
                /// 备注
                /// </summary>
                public const string RBD_Remark = "RBD_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string RBD_IsValid = "RBD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string RBD_CreatedBy = "RBD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string RBD_CreatedTime = "RBD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string RBD_UpdatedBy = "RBD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string RBD_UpdatedTime = "RBD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string RBD_VersionNo = "RBD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string RBD_TransID = "RBD_TransID";

            }

        }
        /// <summary>
        /// 应付单
        /// </summary>
        public class FM_AccountPayableBill
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 应付单ID
                /// </summary>
                public const string APB_ID = "应付单ID";

                /// <summary>
                /// 应付单号
                /// </summary>
                public const string APB_No = "应付单号";

                /// <summary>
                /// 单据方向编码
                /// </summary>
                public const string APB_BillDirectCode = "单据方向编码";

                /// <summary>
                /// 单据方向名称
                /// </summary>
                public const string APB_BillDirectName = "单据方向名称";

                /// <summary>
                /// 来源类型
                /// </summary>
                public const string APB_SourceTypeCode = "来源类型";

                /// <summary>
                /// 来源类型
                /// </summary>
                public const string APB_SourceTypeName = "来源类型";

                /// <summary>
                /// 来源单据号
                /// </summary>
                public const string APB_SourceBillNo = "来源单据号";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string APB_Org_ID = "组织ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string APB_Org_Name = "组织名称";

                /// <summary>
                /// 收款对象类型编码
                /// </summary>
                public const string APB_ReceiveObjectTypeCode = "收款对象类型编码";

                /// <summary>
                /// 收款对象类型名称
                /// </summary>
                public const string APB_ReceiveObjectTypeName = "收款对象类型名称";

                /// <summary>
                /// 收款对象ID
                /// </summary>
                public const string APB_ReceiveObjectID = "收款对象ID";

                /// <summary>
                /// 收款对象名称
                /// </summary>
                public const string APB_ReceiveObjectName = "收款对象名称";

                /// <summary>
                /// 应付金额
                /// </summary>
                public const string APB_AccountPayableAmount = "应付金额";

                /// <summary>
                /// 已付金额
                /// </summary>
                public const string APB_PaidAmount = "已付金额";

                /// <summary>
                /// 未付金额
                /// </summary>
                public const string APB_UnpaidAmount = "未付金额";

                /// <summary>
                /// 业务状态编码
                /// </summary>
                public const string APB_BusinessStatusCode = "业务状态编码";

                /// <summary>
                /// 业务状态名称
                /// </summary>
                public const string APB_BusinessStatusName = "业务状态名称";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string APB_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string APB_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 对账时间
                /// </summary>
                public const string APB_ReconciliationTime = "对账时间";

                /// <summary>
                /// 备注
                /// </summary>
                public const string APB_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APB_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APB_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APB_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APB_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APB_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APB_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APB_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 应付单ID
                /// </summary>
                public const string APB_ID = "APB_ID";

                /// <summary>
                /// 应付单号
                /// </summary>
                public const string APB_No = "APB_No";

                /// <summary>
                /// 单据方向编码
                /// </summary>
                public const string APB_BillDirectCode = "APB_BillDirectCode";

                /// <summary>
                /// 单据方向名称
                /// </summary>
                public const string APB_BillDirectName = "APB_BillDirectName";

                /// <summary>
                /// 来源类型
                /// </summary>
                public const string APB_SourceTypeCode = "APB_SourceTypeCode";

                /// <summary>
                /// 来源类型
                /// </summary>
                public const string APB_SourceTypeName = "APB_SourceTypeName";

                /// <summary>
                /// 来源单据号
                /// </summary>
                public const string APB_SourceBillNo = "APB_SourceBillNo";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string APB_Org_ID = "APB_Org_ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string APB_Org_Name = "APB_Org_Name";

                /// <summary>
                /// 收款对象类型编码
                /// </summary>
                public const string APB_ReceiveObjectTypeCode = "APB_ReceiveObjectTypeCode";

                /// <summary>
                /// 收款对象类型名称
                /// </summary>
                public const string APB_ReceiveObjectTypeName = "APB_ReceiveObjectTypeName";

                /// <summary>
                /// 收款对象ID
                /// </summary>
                public const string APB_ReceiveObjectID = "APB_ReceiveObjectID";

                /// <summary>
                /// 收款对象名称
                /// </summary>
                public const string APB_ReceiveObjectName = "APB_ReceiveObjectName";

                /// <summary>
                /// 应付金额
                /// </summary>
                public const string APB_AccountPayableAmount = "APB_AccountPayableAmount";

                /// <summary>
                /// 已付金额
                /// </summary>
                public const string APB_PaidAmount = "APB_PaidAmount";

                /// <summary>
                /// 未付金额
                /// </summary>
                public const string APB_UnpaidAmount = "APB_UnpaidAmount";

                /// <summary>
                /// 业务状态编码
                /// </summary>
                public const string APB_BusinessStatusCode = "APB_BusinessStatusCode";

                /// <summary>
                /// 业务状态名称
                /// </summary>
                public const string APB_BusinessStatusName = "APB_BusinessStatusName";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string APB_ApprovalStatusCode = "APB_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string APB_ApprovalStatusName = "APB_ApprovalStatusName";

                /// <summary>
                /// 对账时间
                /// </summary>
                public const string APB_ReconciliationTime = "APB_ReconciliationTime";

                /// <summary>
                /// 备注
                /// </summary>
                public const string APB_Remark = "APB_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APB_IsValid = "APB_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APB_CreatedBy = "APB_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APB_CreatedTime = "APB_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APB_UpdatedBy = "APB_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APB_UpdatedTime = "APB_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APB_VersionNo = "APB_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APB_TransID = "APB_TransID";

            }

        }
        /// <summary>
        /// 应付单明细
        /// </summary>
        public class FM_AccountPayableBillDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 应付单明细ID
                /// </summary>
                public const string APBD_ID = "应付单明细ID";

                /// <summary>
                /// 应付单ID
                /// </summary>
                public const string APBD_APB_ID = "应付单ID";

                /// <summary>
                /// 是否负向明细
                /// </summary>
                public const string APBD_IsMinusDetail = "是否负向明细";

                /// <summary>
                /// 来源单据号
                /// </summary>
                public const string APBD_SourceBillNo = "来源单据号";

                /// <summary>
                /// 来源单据明细ID
                /// </summary>
                public const string APBD_SourceBillDetailID = "来源单据明细ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string APBD_Org_ID = "组织ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string APBD_Org_Name = "组织名称";

                /// <summary>
                /// 应付金额
                /// </summary>
                public const string APBD_AccountPayableAmount = "应付金额";

                /// <summary>
                /// 已付金额
                /// </summary>
                public const string APBD_PaidAmount = "已付金额";

                /// <summary>
                /// 未付金额
                /// </summary>
                public const string APBD_UnpaidAmount = "未付金额";

                /// <summary>
                /// 业务状态编码
                /// </summary>
                public const string APBD_BusinessStatusCode = "业务状态编码";

                /// <summary>
                /// 业务状态名称
                /// </summary>
                public const string APBD_BusinessStatusName = "业务状态名称";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string APBD_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string APBD_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string APBD_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APBD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APBD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APBD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APBD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APBD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APBD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APBD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 应付单明细ID
                /// </summary>
                public const string APBD_ID = "APBD_ID";

                /// <summary>
                /// 应付单ID
                /// </summary>
                public const string APBD_APB_ID = "APBD_APB_ID";

                /// <summary>
                /// 是否负向明细
                /// </summary>
                public const string APBD_IsMinusDetail = "APBD_IsMinusDetail";

                /// <summary>
                /// 来源单据号
                /// </summary>
                public const string APBD_SourceBillNo = "APBD_SourceBillNo";

                /// <summary>
                /// 来源单据明细ID
                /// </summary>
                public const string APBD_SourceBillDetailID = "APBD_SourceBillDetailID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string APBD_Org_ID = "APBD_Org_ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string APBD_Org_Name = "APBD_Org_Name";

                /// <summary>
                /// 应付金额
                /// </summary>
                public const string APBD_AccountPayableAmount = "APBD_AccountPayableAmount";

                /// <summary>
                /// 已付金额
                /// </summary>
                public const string APBD_PaidAmount = "APBD_PaidAmount";

                /// <summary>
                /// 未付金额
                /// </summary>
                public const string APBD_UnpaidAmount = "APBD_UnpaidAmount";

                /// <summary>
                /// 业务状态编码
                /// </summary>
                public const string APBD_BusinessStatusCode = "APBD_BusinessStatusCode";

                /// <summary>
                /// 业务状态名称
                /// </summary>
                public const string APBD_BusinessStatusName = "APBD_BusinessStatusName";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string APBD_ApprovalStatusCode = "APBD_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string APBD_ApprovalStatusName = "APBD_ApprovalStatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string APBD_Remark = "APBD_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APBD_IsValid = "APBD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APBD_CreatedBy = "APBD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APBD_CreatedTime = "APBD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APBD_UpdatedBy = "APBD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APBD_UpdatedTime = "APBD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APBD_VersionNo = "APBD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APBD_TransID = "APBD_TransID";

            }

        }
        /// <summary>
        /// 应付单日志
        /// </summary>
        public class FM_AccountPayableBillLog
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 应付单日志ID
                /// </summary>
                public const string APBL_ID = "应付单日志ID";

                /// <summary>
                /// 应付单ID
                /// </summary>
                public const string APBL_APB_ID = "应付单ID";

                /// <summary>
                /// 应付单明细ID
                /// </summary>
                public const string APBL_APBD_ID = "应付单明细ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string APBL_Org_ID = "组织ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string APBL_Org_Name = "组织名称";

                /// <summary>
                /// 操作类型编码
                /// </summary>
                public const string APBL_OperateTypeCode = "操作类型编码";

                /// <summary>
                /// 操作类型名称
                /// </summary>
                public const string APBL_OperateTypeName = "操作类型名称";

                /// <summary>
                /// 应付单明细版本号
                /// </summary>
                public const string APBL_APBD_VersionNo = "应付单明细版本号";

                /// <summary>
                /// 应付金额
                /// </summary>
                public const string APBL_APAmount = "应付金额";

                /// <summary>
                /// 已付金额
                /// </summary>
                public const string APBL_PaidAmount = "已付金额";

                /// <summary>
                /// 未付金额
                /// </summary>
                public const string APBL_UnpaidAmount = "未付金额";

                /// <summary>
                /// 备注
                /// </summary>
                public const string APBL_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APBL_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APBL_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APBL_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APBL_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APBL_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APBL_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APBL_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 应付单日志ID
                /// </summary>
                public const string APBL_ID = "APBL_ID";

                /// <summary>
                /// 应付单ID
                /// </summary>
                public const string APBL_APB_ID = "APBL_APB_ID";

                /// <summary>
                /// 应付单明细ID
                /// </summary>
                public const string APBL_APBD_ID = "APBL_APBD_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string APBL_Org_ID = "APBL_Org_ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string APBL_Org_Name = "APBL_Org_Name";

                /// <summary>
                /// 操作类型编码
                /// </summary>
                public const string APBL_OperateTypeCode = "APBL_OperateTypeCode";

                /// <summary>
                /// 操作类型名称
                /// </summary>
                public const string APBL_OperateTypeName = "APBL_OperateTypeName";

                /// <summary>
                /// 应付单明细版本号
                /// </summary>
                public const string APBL_APBD_VersionNo = "APBL_APBD_VersionNo";

                /// <summary>
                /// 应付金额
                /// </summary>
                public const string APBL_APAmount = "APBL_APAmount";

                /// <summary>
                /// 已付金额
                /// </summary>
                public const string APBL_PaidAmount = "APBL_PaidAmount";

                /// <summary>
                /// 未付金额
                /// </summary>
                public const string APBL_UnpaidAmount = "APBL_UnpaidAmount";

                /// <summary>
                /// 备注
                /// </summary>
                public const string APBL_Remark = "APBL_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string APBL_IsValid = "APBL_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string APBL_CreatedBy = "APBL_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string APBL_CreatedTime = "APBL_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string APBL_UpdatedBy = "APBL_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string APBL_UpdatedTime = "APBL_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string APBL_VersionNo = "APBL_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string APBL_TransID = "APBL_TransID";

            }

        }
        /// <summary>
        /// 应收单
        /// </summary>
        public class FM_AccountReceivableBill
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 应收单ID
                /// </summary>
                public const string ARB_ID = "应收单ID";

                /// <summary>
                /// 应收单号
                /// </summary>
                public const string ARB_No = "应收单号";

                /// <summary>
                /// 单据方向编码
                /// </summary>
                public const string ARB_BillDirectCode = "单据方向编码";

                /// <summary>
                /// 单据方向名称
                /// </summary>
                public const string ARB_BillDirectName = "单据方向名称";

                /// <summary>
                /// 来源类型
                /// </summary>
                public const string ARB_SourceTypeCode = "来源类型";

                /// <summary>
                /// 来源类型
                /// </summary>
                public const string ARB_SourceTypeName = "来源类型";

                /// <summary>
                /// 来源单据号
                /// </summary>
                public const string ARB_SrcBillNo = "来源单据号";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string ARB_Org_ID = "组织ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string ARB_Org_Name = "组织名称";

                /// <summary>
                /// 付款对象类型编码
                /// </summary>
                public const string ARB_PayObjectTypeCode = "付款对象类型编码";

                /// <summary>
                /// 付款对象类型名称
                /// </summary>
                public const string ARB_PayObjectTypeName = "付款对象类型名称";

                /// <summary>
                /// 付款对象ID
                /// </summary>
                public const string ARB_PayObjectID = "付款对象ID";

                /// <summary>
                /// 付款对象名称
                /// </summary>
                public const string ARB_PayObjectName = "付款对象名称";

                /// <summary>
                /// 应收金额
                /// </summary>
                public const string ARB_AccountReceivableAmount = "应收金额";

                /// <summary>
                /// 已收金额
                /// </summary>
                public const string ARB_ReceivedAmount = "已收金额";

                /// <summary>
                /// 未收金额
                /// </summary>
                public const string ARB_UnReceiveAmount = "未收金额";

                /// <summary>
                /// 业务状态编码
                /// </summary>
                public const string ARB_BusinessStatusCode = "业务状态编码";

                /// <summary>
                /// 业务状态名称
                /// </summary>
                public const string ARB_BusinessStatusName = "业务状态名称";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string ARB_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string ARB_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 对账时间
                /// </summary>
                public const string ARB_ReconciliationTime = "对账时间";

                /// <summary>
                /// 备注
                /// </summary>
                public const string ARB_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ARB_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ARB_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ARB_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ARB_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ARB_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ARB_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ARB_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 应收单ID
                /// </summary>
                public const string ARB_ID = "ARB_ID";

                /// <summary>
                /// 应收单号
                /// </summary>
                public const string ARB_No = "ARB_No";

                /// <summary>
                /// 单据方向编码
                /// </summary>
                public const string ARB_BillDirectCode = "ARB_BillDirectCode";

                /// <summary>
                /// 单据方向名称
                /// </summary>
                public const string ARB_BillDirectName = "ARB_BillDirectName";

                /// <summary>
                /// 来源类型
                /// </summary>
                public const string ARB_SourceTypeCode = "ARB_SourceTypeCode";

                /// <summary>
                /// 来源类型
                /// </summary>
                public const string ARB_SourceTypeName = "ARB_SourceTypeName";

                /// <summary>
                /// 来源单据号
                /// </summary>
                public const string ARB_SrcBillNo = "ARB_SrcBillNo";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string ARB_Org_ID = "ARB_Org_ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string ARB_Org_Name = "ARB_Org_Name";

                /// <summary>
                /// 付款对象类型编码
                /// </summary>
                public const string ARB_PayObjectTypeCode = "ARB_PayObjectTypeCode";

                /// <summary>
                /// 付款对象类型名称
                /// </summary>
                public const string ARB_PayObjectTypeName = "ARB_PayObjectTypeName";

                /// <summary>
                /// 付款对象ID
                /// </summary>
                public const string ARB_PayObjectID = "ARB_PayObjectID";

                /// <summary>
                /// 付款对象名称
                /// </summary>
                public const string ARB_PayObjectName = "ARB_PayObjectName";

                /// <summary>
                /// 应收金额
                /// </summary>
                public const string ARB_AccountReceivableAmount = "ARB_AccountReceivableAmount";

                /// <summary>
                /// 已收金额
                /// </summary>
                public const string ARB_ReceivedAmount = "ARB_ReceivedAmount";

                /// <summary>
                /// 未收金额
                /// </summary>
                public const string ARB_UnReceiveAmount = "ARB_UnReceiveAmount";

                /// <summary>
                /// 业务状态编码
                /// </summary>
                public const string ARB_BusinessStatusCode = "ARB_BusinessStatusCode";

                /// <summary>
                /// 业务状态名称
                /// </summary>
                public const string ARB_BusinessStatusName = "ARB_BusinessStatusName";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string ARB_ApprovalStatusCode = "ARB_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string ARB_ApprovalStatusName = "ARB_ApprovalStatusName";

                /// <summary>
                /// 对账时间
                /// </summary>
                public const string ARB_ReconciliationTime = "ARB_ReconciliationTime";

                /// <summary>
                /// 备注
                /// </summary>
                public const string ARB_Remark = "ARB_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ARB_IsValid = "ARB_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ARB_CreatedBy = "ARB_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ARB_CreatedTime = "ARB_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ARB_UpdatedBy = "ARB_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ARB_UpdatedTime = "ARB_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ARB_VersionNo = "ARB_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ARB_TransID = "ARB_TransID";

            }

        }
        /// <summary>
        /// 应收单明细
        /// </summary>
        public class FM_AccountReceivableBillDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 应收单明细ID
                /// </summary>
                public const string ARBD_ID = "应收单明细ID";

                /// <summary>
                /// 应收单ID
                /// </summary>
                public const string ARBD_ARB_ID = "应收单ID";

                /// <summary>
                /// 是否负向明细
                /// </summary>
                public const string ARBD_IsMinusDetail = "是否负向明细";

                /// <summary>
                /// 来源单据号
                /// </summary>
                public const string ARBD_SrcBillNo = "来源单据号";

                /// <summary>
                /// 来源单据明细ID
                /// </summary>
                public const string ARBD_SrcBillDetailID = "来源单据明细ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string ARBD_Org_ID = "组织ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string ARBD_Org_Name = "组织名称";

                /// <summary>
                /// 应收金额
                /// </summary>
                public const string ARBD_AccountReceivableAmount = "应收金额";

                /// <summary>
                /// 已收金额
                /// </summary>
                public const string ARBD_ReceivedAmount = "已收金额";

                /// <summary>
                /// 未收金额
                /// </summary>
                public const string ARBD_UnReceiveAmount = "未收金额";

                /// <summary>
                /// 业务状态编码
                /// </summary>
                public const string ARBD_BusinessStatusCode = "业务状态编码";

                /// <summary>
                /// 业务状态名称
                /// </summary>
                public const string ARBD_BusinessStatusName = "业务状态名称";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string ARBD_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string ARBD_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string ARBD_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ARBD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ARBD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ARBD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ARBD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ARBD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ARBD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ARBD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 应收单明细ID
                /// </summary>
                public const string ARBD_ID = "ARBD_ID";

                /// <summary>
                /// 应收单ID
                /// </summary>
                public const string ARBD_ARB_ID = "ARBD_ARB_ID";

                /// <summary>
                /// 是否负向明细
                /// </summary>
                public const string ARBD_IsMinusDetail = "ARBD_IsMinusDetail";

                /// <summary>
                /// 来源单据号
                /// </summary>
                public const string ARBD_SrcBillNo = "ARBD_SrcBillNo";

                /// <summary>
                /// 来源单据明细ID
                /// </summary>
                public const string ARBD_SrcBillDetailID = "ARBD_SrcBillDetailID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string ARBD_Org_ID = "ARBD_Org_ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string ARBD_Org_Name = "ARBD_Org_Name";

                /// <summary>
                /// 应收金额
                /// </summary>
                public const string ARBD_AccountReceivableAmount = "ARBD_AccountReceivableAmount";

                /// <summary>
                /// 已收金额
                /// </summary>
                public const string ARBD_ReceivedAmount = "ARBD_ReceivedAmount";

                /// <summary>
                /// 未收金额
                /// </summary>
                public const string ARBD_UnReceiveAmount = "ARBD_UnReceiveAmount";

                /// <summary>
                /// 业务状态编码
                /// </summary>
                public const string ARBD_BusinessStatusCode = "ARBD_BusinessStatusCode";

                /// <summary>
                /// 业务状态名称
                /// </summary>
                public const string ARBD_BusinessStatusName = "ARBD_BusinessStatusName";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string ARBD_ApprovalStatusCode = "ARBD_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string ARBD_ApprovalStatusName = "ARBD_ApprovalStatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string ARBD_Remark = "ARBD_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ARBD_IsValid = "ARBD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ARBD_CreatedBy = "ARBD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ARBD_CreatedTime = "ARBD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ARBD_UpdatedBy = "ARBD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ARBD_UpdatedTime = "ARBD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ARBD_VersionNo = "ARBD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ARBD_TransID = "ARBD_TransID";

            }

        }
        /// <summary>
        /// 应收单日志
        /// </summary>
        public class FM_AccountReceivableBillLog
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 应收单日志ID
                /// </summary>
                public const string ARBL_ID = "应收单日志ID";

                /// <summary>
                /// 应收单ID
                /// </summary>
                public const string ARBL_ARB_ID = "应收单ID";

                /// <summary>
                /// 应收单明细ID
                /// </summary>
                public const string ARBL_ARBD_ID = "应收单明细ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string ARBL_Org_ID = "组织ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string ARBL_Org_Name = "组织名称";

                /// <summary>
                /// 操作类型编码
                /// </summary>
                public const string ARBL_OperateTypeCode = "操作类型编码";

                /// <summary>
                /// 操作类型名称
                /// </summary>
                public const string ARBL_OperateTypeName = "操作类型名称";

                /// <summary>
                /// 应收单明细版本号
                /// </summary>
                public const string ARBL_APBD_VersionNo = "应收单明细版本号";

                /// <summary>
                /// 应收金额
                /// </summary>
                public const string ARBL_ARAmount = "应收金额";

                /// <summary>
                /// 已收金额
                /// </summary>
                public const string ARBL_ReceivedAmount = "已收金额";

                /// <summary>
                /// 未收金额
                /// </summary>
                public const string ARBL_UnReceiveAmount = "未收金额";

                /// <summary>
                /// 备注
                /// </summary>
                public const string ARBL_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ARBL_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ARBL_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ARBL_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ARBL_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ARBL_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ARBL_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ARBL_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 应收单日志ID
                /// </summary>
                public const string ARBL_ID = "ARBL_ID";

                /// <summary>
                /// 应收单ID
                /// </summary>
                public const string ARBL_ARB_ID = "ARBL_ARB_ID";

                /// <summary>
                /// 应收单明细ID
                /// </summary>
                public const string ARBL_ARBD_ID = "ARBL_ARBD_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string ARBL_Org_ID = "ARBL_Org_ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string ARBL_Org_Name = "ARBL_Org_Name";

                /// <summary>
                /// 操作类型编码
                /// </summary>
                public const string ARBL_OperateTypeCode = "ARBL_OperateTypeCode";

                /// <summary>
                /// 操作类型名称
                /// </summary>
                public const string ARBL_OperateTypeName = "ARBL_OperateTypeName";

                /// <summary>
                /// 应收单明细版本号
                /// </summary>
                public const string ARBL_APBD_VersionNo = "ARBL_APBD_VersionNo";

                /// <summary>
                /// 应收金额
                /// </summary>
                public const string ARBL_ARAmount = "ARBL_ARAmount";

                /// <summary>
                /// 已收金额
                /// </summary>
                public const string ARBL_ReceivedAmount = "ARBL_ReceivedAmount";

                /// <summary>
                /// 未收金额
                /// </summary>
                public const string ARBL_UnReceiveAmount = "ARBL_UnReceiveAmount";

                /// <summary>
                /// 备注
                /// </summary>
                public const string ARBL_Remark = "ARBL_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ARBL_IsValid = "ARBL_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ARBL_CreatedBy = "ARBL_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ARBL_CreatedTime = "ARBL_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ARBL_UpdatedBy = "ARBL_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ARBL_UpdatedTime = "ARBL_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ARBL_VersionNo = "ARBL_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ARBL_TransID = "ARBL_TransID";

            }

        }
        /// <summary>
        /// 采购订单
        /// </summary>
        public class PIS_PurchaseOrder
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 采购订单ID
                /// </summary>
                public const string PO_ID = "采购订单ID";

                /// <summary>
                /// 订单号
                /// </summary>
                public const string PO_No = "订单号";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string PO_Org_ID = "组织ID";

                /// <summary>
                /// 供应商ID
                /// </summary>
                public const string PO_SUPP_ID = "供应商ID";

                /// <summary>
                /// 供应商名称
                /// </summary>
                public const string PO_SUPP_Name = "供应商名称";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string PO_SourceTypeCode = "来源类型编码";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string PO_SourceTypeName = "来源类型名称";

                /// <summary>
                /// 来源单号
                /// </summary>
                public const string PO_SourceNo = "来源单号";

                /// <summary>
                /// 订单总额
                /// </summary>
                public const string PO_TotalAmount = "订单总额";

                /// <summary>
                /// 物流费
                /// </summary>
                public const string PO_LogisticFee = "物流费";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string PO_StatusCode = "单据状态编码";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string PO_StatusName = "单据状态名称";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string PO_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string PO_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 到货时间
                /// </summary>
                public const string PO_ReceivedTime = "到货时间";

                /// <summary>
                /// 有效
                /// </summary>
                public const string PO_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string PO_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string PO_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string PO_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string PO_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string PO_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string PO_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 采购订单ID
                /// </summary>
                public const string PO_ID = "PO_ID";

                /// <summary>
                /// 订单号
                /// </summary>
                public const string PO_No = "PO_No";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string PO_Org_ID = "PO_Org_ID";

                /// <summary>
                /// 供应商ID
                /// </summary>
                public const string PO_SUPP_ID = "PO_SUPP_ID";

                /// <summary>
                /// 供应商名称
                /// </summary>
                public const string PO_SUPP_Name = "PO_SUPP_Name";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string PO_SourceTypeCode = "PO_SourceTypeCode";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string PO_SourceTypeName = "PO_SourceTypeName";

                /// <summary>
                /// 来源单号
                /// </summary>
                public const string PO_SourceNo = "PO_SourceNo";

                /// <summary>
                /// 订单总额
                /// </summary>
                public const string PO_TotalAmount = "PO_TotalAmount";

                /// <summary>
                /// 物流费
                /// </summary>
                public const string PO_LogisticFee = "PO_LogisticFee";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string PO_StatusCode = "PO_StatusCode";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string PO_StatusName = "PO_StatusName";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string PO_ApprovalStatusCode = "PO_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string PO_ApprovalStatusName = "PO_ApprovalStatusName";

                /// <summary>
                /// 到货时间
                /// </summary>
                public const string PO_ReceivedTime = "PO_ReceivedTime";

                /// <summary>
                /// 有效
                /// </summary>
                public const string PO_IsValid = "PO_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string PO_CreatedBy = "PO_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string PO_CreatedTime = "PO_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string PO_UpdatedBy = "PO_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string PO_UpdatedTime = "PO_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string PO_VersionNo = "PO_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string PO_TransID = "PO_TransID";

            }

        }
        /// <summary>
        /// 采购订单明细
        /// </summary>
        public class PIS_PurchaseOrderDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 采购订单明细ID
                /// </summary>
                public const string POD_ID = "采购订单明细ID";

                /// <summary>
                /// 采购订单ID
                /// </summary>
                public const string POD_PO_ID = "采购订单ID";

                /// <summary>
                /// 订单号
                /// </summary>
                public const string POD_PO_No = "订单号";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string POD_AutoPartsBarcode = "配件条码";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string POD_ThirdCode = "第三方编码";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string POD_OEMCode = "原厂编码";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string POD_AutoPartsName = "配件名称";

                /// <summary>
                /// 配件品牌
                /// </summary>
                public const string POD_AutoPartsBrand = "配件品牌";

                /// <summary>
                /// 规格型号
                /// </summary>
                public const string POD_AutoPartsSpec = "规格型号";

                /// <summary>
                /// 配件级别
                /// </summary>
                public const string POD_AutoPartsLevel = "配件级别";

                /// <summary>
                /// 计量单位
                /// </summary>
                public const string POD_UOM = "计量单位";

                /// <summary>
                /// 汽车品牌
                /// </summary>
                public const string POD_VehicleBrand = "汽车品牌";

                /// <summary>
                /// 车系
                /// </summary>
                public const string POD_VehicleInspire = "车系";

                /// <summary>
                /// 排量
                /// </summary>
                public const string POD_VehicleCapacity = "排量";

                /// <summary>
                /// 年款
                /// </summary>
                public const string POD_VehicleYearModel = "年款";

                /// <summary>
                /// 变速类型
                /// </summary>
                public const string POD_VehicleGearboxType = "变速类型";

                /// <summary>
                /// 进货仓库ID
                /// </summary>
                public const string POD_WH_ID = "进货仓库ID";

                /// <summary>
                /// 进货仓位ID
                /// </summary>
                public const string POD_WHB_ID = "进货仓位ID";

                /// <summary>
                /// 订货数量
                /// </summary>
                public const string POD_OrderQty = "订货数量";

                /// <summary>
                /// 签收数量
                /// </summary>
                public const string POD_ReceivedQty = "签收数量";

                /// <summary>
                /// 订货单价
                /// </summary>
                public const string POD_UnitPrice = "订货单价";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string POD_StatusCode = "单据状态编码";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string POD_StatusName = "单据状态名称";

                /// <summary>
                /// 到货时间
                /// </summary>
                public const string POD_ReceivedTime = "到货时间";

                /// <summary>
                /// 有效
                /// </summary>
                public const string POD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string POD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string POD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string POD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string POD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string POD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string POD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 采购订单明细ID
                /// </summary>
                public const string POD_ID = "POD_ID";

                /// <summary>
                /// 采购订单ID
                /// </summary>
                public const string POD_PO_ID = "POD_PO_ID";

                /// <summary>
                /// 订单号
                /// </summary>
                public const string POD_PO_No = "POD_PO_No";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string POD_AutoPartsBarcode = "POD_AutoPartsBarcode";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string POD_ThirdCode = "POD_ThirdCode";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string POD_OEMCode = "POD_OEMCode";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string POD_AutoPartsName = "POD_AutoPartsName";

                /// <summary>
                /// 配件品牌
                /// </summary>
                public const string POD_AutoPartsBrand = "POD_AutoPartsBrand";

                /// <summary>
                /// 规格型号
                /// </summary>
                public const string POD_AutoPartsSpec = "POD_AutoPartsSpec";

                /// <summary>
                /// 配件级别
                /// </summary>
                public const string POD_AutoPartsLevel = "POD_AutoPartsLevel";

                /// <summary>
                /// 计量单位
                /// </summary>
                public const string POD_UOM = "POD_UOM";

                /// <summary>
                /// 汽车品牌
                /// </summary>
                public const string POD_VehicleBrand = "POD_VehicleBrand";

                /// <summary>
                /// 车系
                /// </summary>
                public const string POD_VehicleInspire = "POD_VehicleInspire";

                /// <summary>
                /// 排量
                /// </summary>
                public const string POD_VehicleCapacity = "POD_VehicleCapacity";

                /// <summary>
                /// 年款
                /// </summary>
                public const string POD_VehicleYearModel = "POD_VehicleYearModel";

                /// <summary>
                /// 变速类型
                /// </summary>
                public const string POD_VehicleGearboxType = "POD_VehicleGearboxType";

                /// <summary>
                /// 进货仓库ID
                /// </summary>
                public const string POD_WH_ID = "POD_WH_ID";

                /// <summary>
                /// 进货仓位ID
                /// </summary>
                public const string POD_WHB_ID = "POD_WHB_ID";

                /// <summary>
                /// 订货数量
                /// </summary>
                public const string POD_OrderQty = "POD_OrderQty";

                /// <summary>
                /// 签收数量
                /// </summary>
                public const string POD_ReceivedQty = "POD_ReceivedQty";

                /// <summary>
                /// 订货单价
                /// </summary>
                public const string POD_UnitPrice = "POD_UnitPrice";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string POD_StatusCode = "POD_StatusCode";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string POD_StatusName = "POD_StatusName";

                /// <summary>
                /// 到货时间
                /// </summary>
                public const string POD_ReceivedTime = "POD_ReceivedTime";

                /// <summary>
                /// 有效
                /// </summary>
                public const string POD_IsValid = "POD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string POD_CreatedBy = "POD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string POD_CreatedTime = "POD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string POD_UpdatedBy = "POD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string POD_UpdatedTime = "POD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string POD_VersionNo = "POD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string POD_TransID = "POD_TransID";

            }

        }
        /// <summary>
        /// 采购预测订单
        /// </summary>
        public class PIS_PurchaseForecastOrder
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 采购预测订单ID
                /// </summary>
                public const string PFO_ID = "采购预测订单ID";

                /// <summary>
                /// 单号
                /// </summary>
                public const string PFO_No = "单号";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string PFO_Org_ID = "组织ID";

                /// <summary>
                /// 供应商ID
                /// </summary>
                public const string PFO_SUPP_ID = "供应商ID";

                /// <summary>
                /// 供应商名称
                /// </summary>
                public const string PFO_SUPP_Name = "供应商名称";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string PFO_SourceTypeCode = "来源类型编码";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string PFO_SourceTypeName = "来源类型名称";

                /// <summary>
                /// 订单总额
                /// </summary>
                public const string PFO_TotalAmount = "订单总额";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string PFO_StatusCode = "单据状态编码";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string PFO_StatusName = "单据状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string PFO_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string PFO_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string PFO_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string PFO_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string PFO_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string PFO_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string PFO_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string PFO_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 采购预测订单ID
                /// </summary>
                public const string PFO_ID = "PFO_ID";

                /// <summary>
                /// 单号
                /// </summary>
                public const string PFO_No = "PFO_No";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string PFO_Org_ID = "PFO_Org_ID";

                /// <summary>
                /// 供应商ID
                /// </summary>
                public const string PFO_SUPP_ID = "PFO_SUPP_ID";

                /// <summary>
                /// 供应商名称
                /// </summary>
                public const string PFO_SUPP_Name = "PFO_SUPP_Name";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string PFO_SourceTypeCode = "PFO_SourceTypeCode";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string PFO_SourceTypeName = "PFO_SourceTypeName";

                /// <summary>
                /// 订单总额
                /// </summary>
                public const string PFO_TotalAmount = "PFO_TotalAmount";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string PFO_StatusCode = "PFO_StatusCode";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string PFO_StatusName = "PFO_StatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string PFO_Remark = "PFO_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string PFO_IsValid = "PFO_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string PFO_CreatedBy = "PFO_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string PFO_CreatedTime = "PFO_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string PFO_UpdatedBy = "PFO_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string PFO_UpdatedTime = "PFO_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string PFO_VersionNo = "PFO_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string PFO_TransID = "PFO_TransID";

            }

        }
        /// <summary>
        /// 采购预测订单明细
        /// </summary>
        public class PIS_PurchaseForecastOrderDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 采购预测订单明细ID
                /// </summary>
                public const string PFOD_ID = "采购预测订单明细ID";

                /// <summary>
                /// 采购预测订单ID
                /// </summary>
                public const string PFOD_PFO_ID = "采购预测订单ID";

                /// <summary>
                /// 订单号
                /// </summary>
                public const string PFOD_PFO_No = "订单号";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string PFOD_AutoPartsBarcode = "配件条码";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string PFOD_ThirdCode = "第三方编码";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string PFOD_OEMCode = "原厂编码";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string PFOD_AutoPartsName = "配件名称";

                /// <summary>
                /// 配件品牌
                /// </summary>
                public const string PFOD_AutoPartsBrand = "配件品牌";

                /// <summary>
                /// 规格型号
                /// </summary>
                public const string PFOD_AutoPartsSpec = "规格型号";

                /// <summary>
                /// 配件级别
                /// </summary>
                public const string PFOD_AutoPartsLevel = "配件级别";

                /// <summary>
                /// 计量单位
                /// </summary>
                public const string PFOD_UOM = "计量单位";

                /// <summary>
                /// 汽车品牌
                /// </summary>
                public const string PFOD_VehicleBrand = "汽车品牌";

                /// <summary>
                /// 车系
                /// </summary>
                public const string PFOD_VehicleInspire = "车系";

                /// <summary>
                /// 排量
                /// </summary>
                public const string PFOD_VehicleCapacity = "排量";

                /// <summary>
                /// 年款
                /// </summary>
                public const string PFOD_VehicleYearModel = "年款";

                /// <summary>
                /// 变速类型
                /// </summary>
                public const string PFOD_VehicleGearboxType = "变速类型";

                /// <summary>
                /// 数量
                /// </summary>
                public const string PFOD_Qty = "数量";

                /// <summary>
                /// 最后一次采购单价
                /// </summary>
                public const string PFOD_LastUnitPrice = "最后一次采购单价";

                /// <summary>
                /// 有效
                /// </summary>
                public const string PFOD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string PFOD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string PFOD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string PFOD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string PFOD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string PFOD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string PFOD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 采购预测订单明细ID
                /// </summary>
                public const string PFOD_ID = "PFOD_ID";

                /// <summary>
                /// 采购预测订单ID
                /// </summary>
                public const string PFOD_PFO_ID = "PFOD_PFO_ID";

                /// <summary>
                /// 订单号
                /// </summary>
                public const string PFOD_PFO_No = "PFOD_PFO_No";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string PFOD_AutoPartsBarcode = "PFOD_AutoPartsBarcode";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string PFOD_ThirdCode = "PFOD_ThirdCode";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string PFOD_OEMCode = "PFOD_OEMCode";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string PFOD_AutoPartsName = "PFOD_AutoPartsName";

                /// <summary>
                /// 配件品牌
                /// </summary>
                public const string PFOD_AutoPartsBrand = "PFOD_AutoPartsBrand";

                /// <summary>
                /// 规格型号
                /// </summary>
                public const string PFOD_AutoPartsSpec = "PFOD_AutoPartsSpec";

                /// <summary>
                /// 配件级别
                /// </summary>
                public const string PFOD_AutoPartsLevel = "PFOD_AutoPartsLevel";

                /// <summary>
                /// 计量单位
                /// </summary>
                public const string PFOD_UOM = "PFOD_UOM";

                /// <summary>
                /// 汽车品牌
                /// </summary>
                public const string PFOD_VehicleBrand = "PFOD_VehicleBrand";

                /// <summary>
                /// 车系
                /// </summary>
                public const string PFOD_VehicleInspire = "PFOD_VehicleInspire";

                /// <summary>
                /// 排量
                /// </summary>
                public const string PFOD_VehicleCapacity = "PFOD_VehicleCapacity";

                /// <summary>
                /// 年款
                /// </summary>
                public const string PFOD_VehicleYearModel = "PFOD_VehicleYearModel";

                /// <summary>
                /// 变速类型
                /// </summary>
                public const string PFOD_VehicleGearboxType = "PFOD_VehicleGearboxType";

                /// <summary>
                /// 数量
                /// </summary>
                public const string PFOD_Qty = "PFOD_Qty";

                /// <summary>
                /// 最后一次采购单价
                /// </summary>
                public const string PFOD_LastUnitPrice = "PFOD_LastUnitPrice";

                /// <summary>
                /// 有效
                /// </summary>
                public const string PFOD_IsValid = "PFOD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string PFOD_CreatedBy = "PFOD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string PFOD_CreatedTime = "PFOD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string PFOD_UpdatedBy = "PFOD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string PFOD_UpdatedTime = "PFOD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string PFOD_VersionNo = "PFOD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string PFOD_TransID = "PFOD_TransID";

            }

        }
        /// <summary>
        /// 仓库
        /// </summary>
        public class PIS_Warehouse
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string WH_ID = "仓库ID";

                /// <summary>
                /// 仓库编号
                /// </summary>
                public const string WH_No = "仓库编号";

                /// <summary>
                /// 仓库名称
                /// </summary>
                public const string WH_Name = "仓库名称";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string WH_Org_ID = "组织ID";

                /// <summary>
                /// 仓库地址
                /// </summary>
                public const string WH_Address = "仓库地址";

                /// <summary>
                /// 仓库描述
                /// </summary>
                public const string WH_Description = "仓库描述";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WH_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WH_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WH_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WH_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WH_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WH_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WH_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string WH_ID = "WH_ID";

                /// <summary>
                /// 仓库编号
                /// </summary>
                public const string WH_No = "WH_No";

                /// <summary>
                /// 仓库名称
                /// </summary>
                public const string WH_Name = "WH_Name";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string WH_Org_ID = "WH_Org_ID";

                /// <summary>
                /// 仓库地址
                /// </summary>
                public const string WH_Address = "WH_Address";

                /// <summary>
                /// 仓库描述
                /// </summary>
                public const string WH_Description = "WH_Description";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WH_IsValid = "WH_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WH_CreatedBy = "WH_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WH_CreatedTime = "WH_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WH_UpdatedBy = "WH_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WH_UpdatedTime = "WH_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WH_VersionNo = "WH_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WH_TransID = "WH_TransID";

            }

        }
        /// <summary>
        /// 仓位
        /// </summary>
        public class PIS_WarehouseBin
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string WHB_ID = "仓位ID";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string WHB_WH_ID = "仓库ID";

                /// <summary>
                /// 仓位名称
                /// </summary>
                public const string WHB_Name = "仓位名称";

                /// <summary>
                /// 仓位描述
                /// </summary>
                public const string WHB_Description = "仓位描述";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WHB_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WHB_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WHB_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WHB_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WHB_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WHB_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WHB_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string WHB_ID = "WHB_ID";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string WHB_WH_ID = "WHB_WH_ID";

                /// <summary>
                /// 仓位名称
                /// </summary>
                public const string WHB_Name = "WHB_Name";

                /// <summary>
                /// 仓位描述
                /// </summary>
                public const string WHB_Description = "WHB_Description";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WHB_IsValid = "WHB_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WHB_CreatedBy = "WHB_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WHB_CreatedTime = "WHB_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WHB_UpdatedBy = "WHB_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WHB_UpdatedTime = "WHB_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WHB_VersionNo = "WHB_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WHB_TransID = "WHB_TransID";

            }

        }
        /// <summary>
        /// 出库单
        /// </summary>
        public class PIS_StockOutBill
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 出库单ID
                /// </summary>
                public const string SOB_ID = "出库单ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string SOB_Org_ID = "组织ID";

                /// <summary>
                /// 单号
                /// </summary>
                public const string SOB_No = "单号";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string SOB_SourceTypeCode = "来源类型编码";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string SOB_SourceTypeName = "来源类型名称";

                /// <summary>
                /// 来源单号
                /// </summary>
                public const string SOB_SourceNo = "来源单号";

                /// <summary>
                /// 供应商ID
                /// </summary>
                public const string SOB_SUPP_ID = "供应商ID";

                /// <summary>
                /// 供应商名称
                /// </summary>
                public const string SOB_SUPP_Name = "供应商名称";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string SOB_StatusCode = "单据状态编码";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string SOB_StatusName = "单据状态名称";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string SOB_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string SOB_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SOB_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SOB_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SOB_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SOB_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SOB_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SOB_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SOB_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SOB_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 出库单ID
                /// </summary>
                public const string SOB_ID = "SOB_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string SOB_Org_ID = "SOB_Org_ID";

                /// <summary>
                /// 单号
                /// </summary>
                public const string SOB_No = "SOB_No";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string SOB_SourceTypeCode = "SOB_SourceTypeCode";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string SOB_SourceTypeName = "SOB_SourceTypeName";

                /// <summary>
                /// 来源单号
                /// </summary>
                public const string SOB_SourceNo = "SOB_SourceNo";

                /// <summary>
                /// 供应商ID
                /// </summary>
                public const string SOB_SUPP_ID = "SOB_SUPP_ID";

                /// <summary>
                /// 供应商名称
                /// </summary>
                public const string SOB_SUPP_Name = "SOB_SUPP_Name";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string SOB_StatusCode = "SOB_StatusCode";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string SOB_StatusName = "SOB_StatusName";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string SOB_ApprovalStatusCode = "SOB_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string SOB_ApprovalStatusName = "SOB_ApprovalStatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SOB_Remark = "SOB_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SOB_IsValid = "SOB_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SOB_CreatedBy = "SOB_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SOB_CreatedTime = "SOB_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SOB_UpdatedBy = "SOB_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SOB_UpdatedTime = "SOB_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SOB_VersionNo = "SOB_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SOB_TransID = "SOB_TransID";

            }

        }
        /// <summary>
        /// 出库单明细
        /// </summary>
        public class PIS_StockOutBillDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 出库单明细ID
                /// </summary>
                public const string SOBD_ID = "出库单明细ID";

                /// <summary>
                /// 出库单ID
                /// </summary>
                public const string SOBD_SOB_ID = "出库单ID";

                /// <summary>
                /// 出库单号
                /// </summary>
                public const string SOBD_SOB_No = "出库单号";

                /// <summary>
                /// 来源单明细ID
                /// </summary>
                public const string SOBD_SourceDetailID = "来源单明细ID";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string SOBD_Barcode = "配件条码";

                /// <summary>
                /// 配件批次号
                /// </summary>
                public const string SOBD_BatchNo = "配件批次号";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string SOBD_ThirdNo = "第三方编码";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string SOBD_OEMNo = "原厂编码";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string SOBD_Name = "配件名称";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string SOBD_Specification = "配件规格型号";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string SOBD_WH_ID = "仓库ID";

                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string SOBD_WHB_ID = "仓位ID";

                /// <summary>
                /// 进货单价
                /// </summary>
                public const string SOBD_UnitCostPrice = "进货单价";

                /// <summary>
                /// 出库数量
                /// </summary>
                public const string SOBD_Qty = "出库数量";

                /// <summary>
                /// 单位
                /// </summary>
                public const string SOBD_UOM = "单位";

                /// <summary>
                /// 销售单价
                /// </summary>
                public const string SOBD_UnitSalePrice = "销售单价";

                /// <summary>
                /// 金额
                /// </summary>
                public const string SOBD_Amount = "金额";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SOBD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SOBD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SOBD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SOBD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SOBD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SOBD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SOBD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 出库单明细ID
                /// </summary>
                public const string SOBD_ID = "SOBD_ID";

                /// <summary>
                /// 出库单ID
                /// </summary>
                public const string SOBD_SOB_ID = "SOBD_SOB_ID";

                /// <summary>
                /// 出库单号
                /// </summary>
                public const string SOBD_SOB_No = "SOBD_SOB_No";

                /// <summary>
                /// 来源单明细ID
                /// </summary>
                public const string SOBD_SourceDetailID = "SOBD_SourceDetailID";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string SOBD_Barcode = "SOBD_Barcode";

                /// <summary>
                /// 配件批次号
                /// </summary>
                public const string SOBD_BatchNo = "SOBD_BatchNo";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string SOBD_ThirdNo = "SOBD_ThirdNo";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string SOBD_OEMNo = "SOBD_OEMNo";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string SOBD_Name = "SOBD_Name";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string SOBD_Specification = "SOBD_Specification";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string SOBD_WH_ID = "SOBD_WH_ID";

                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string SOBD_WHB_ID = "SOBD_WHB_ID";

                /// <summary>
                /// 进货单价
                /// </summary>
                public const string SOBD_UnitCostPrice = "SOBD_UnitCostPrice";

                /// <summary>
                /// 出库数量
                /// </summary>
                public const string SOBD_Qty = "SOBD_Qty";

                /// <summary>
                /// 单位
                /// </summary>
                public const string SOBD_UOM = "SOBD_UOM";

                /// <summary>
                /// 销售单价
                /// </summary>
                public const string SOBD_UnitSalePrice = "SOBD_UnitSalePrice";

                /// <summary>
                /// 金额
                /// </summary>
                public const string SOBD_Amount = "SOBD_Amount";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SOBD_IsValid = "SOBD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SOBD_CreatedBy = "SOBD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SOBD_CreatedTime = "SOBD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SOBD_UpdatedBy = "SOBD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SOBD_UpdatedTime = "SOBD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SOBD_VersionNo = "SOBD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SOBD_TransID = "SOBD_TransID";

            }

        }
        /// <summary>
        /// 供应商管理
        /// </summary>
        public class PIS_Supplier
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string SUPP_ID = "ID";

                /// <summary>
                /// 编码
                /// </summary>
                public const string SUPP_Code = "编码";

                /// <summary>
                /// 名称
                /// </summary>
                public const string SUPP_Name = "名称";

                /// <summary>
                /// 简称
                /// </summary>
                public const string SUPP_ShortName = "简称";

                /// <summary>
                /// 联系人
                /// </summary>
                public const string SUPP_Contacter = "联系人";

                /// <summary>
                /// 固定号码
                /// </summary>
                public const string SUPP_Tel = "固定号码";

                /// <summary>
                /// 电话号码
                /// </summary>
                public const string SUPP_Phone = "电话号码";

                /// <summary>
                /// QQ号码
                /// </summary>
                public const string SUPP_QQ = "QQ号码";

                /// <summary>
                /// 地区
                /// </summary>
                public const string SUPP_Territory = "地区";

                /// <summary>
                /// 省
                /// </summary>
                public const string SUPP_Prov_Code = "省";

                /// <summary>
                /// 市
                /// </summary>
                public const string SUPP_City_Code = "市";

                /// <summary>
                /// 区
                /// </summary>
                public const string SUPP_Dist_Code = "区";

                /// <summary>
                /// 地址
                /// </summary>
                public const string SUPP_Address = "地址";

                /// <summary>
                /// 评估等级
                /// </summary>
                public const string SUPP_EvaluateLevel = "评估等级";

                /// <summary>
                /// 最近评估日
                /// </summary>
                public const string SUPP_LastEvaluateDate = "最近评估日";

                /// <summary>
                /// 开户行
                /// </summary>
                public const string SUPP_BankName = "开户行";

                /// <summary>
                /// 开户名
                /// </summary>
                public const string SUPP_BankAccountName = "开户名";

                /// <summary>
                /// 账号
                /// </summary>
                public const string SUPP_BankAccountNo = "账号";

                /// <summary>
                /// 主营配件
                /// </summary>
                public const string SUPP_MainAutoParts = "主营配件";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SUPP_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SUPP_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SUPP_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SUPP_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SUPP_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SUPP_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SUPP_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SUPP_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string SUPP_ID = "SUPP_ID";

                /// <summary>
                /// 编码
                /// </summary>
                public const string SUPP_Code = "SUPP_Code";

                /// <summary>
                /// 名称
                /// </summary>
                public const string SUPP_Name = "SUPP_Name";

                /// <summary>
                /// 简称
                /// </summary>
                public const string SUPP_ShortName = "SUPP_ShortName";

                /// <summary>
                /// 联系人
                /// </summary>
                public const string SUPP_Contacter = "SUPP_Contacter";

                /// <summary>
                /// 固定号码
                /// </summary>
                public const string SUPP_Tel = "SUPP_Tel";

                /// <summary>
                /// 电话号码
                /// </summary>
                public const string SUPP_Phone = "SUPP_Phone";

                /// <summary>
                /// QQ号码
                /// </summary>
                public const string SUPP_QQ = "SUPP_QQ";

                /// <summary>
                /// 地区
                /// </summary>
                public const string SUPP_Territory = "SUPP_Territory";

                /// <summary>
                /// 省
                /// </summary>
                public const string SUPP_Prov_Code = "SUPP_Prov_Code";

                /// <summary>
                /// 市
                /// </summary>
                public const string SUPP_City_Code = "SUPP_City_Code";

                /// <summary>
                /// 区
                /// </summary>
                public const string SUPP_Dist_Code = "SUPP_Dist_Code";

                /// <summary>
                /// 地址
                /// </summary>
                public const string SUPP_Address = "SUPP_Address";

                /// <summary>
                /// 评估等级
                /// </summary>
                public const string SUPP_EvaluateLevel = "SUPP_EvaluateLevel";

                /// <summary>
                /// 最近评估日
                /// </summary>
                public const string SUPP_LastEvaluateDate = "SUPP_LastEvaluateDate";

                /// <summary>
                /// 开户行
                /// </summary>
                public const string SUPP_BankName = "SUPP_BankName";

                /// <summary>
                /// 开户名
                /// </summary>
                public const string SUPP_BankAccountName = "SUPP_BankAccountName";

                /// <summary>
                /// 账号
                /// </summary>
                public const string SUPP_BankAccountNo = "SUPP_BankAccountNo";

                /// <summary>
                /// 主营配件
                /// </summary>
                public const string SUPP_MainAutoParts = "SUPP_MainAutoParts";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SUPP_Remark = "SUPP_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SUPP_IsValid = "SUPP_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SUPP_CreatedBy = "SUPP_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SUPP_CreatedTime = "SUPP_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SUPP_UpdatedBy = "SUPP_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SUPP_UpdatedTime = "SUPP_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SUPP_VersionNo = "SUPP_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SUPP_TransID = "SUPP_TransID";

            }

        }
        /// <summary>
        /// 库存
        /// </summary>
        public class PIS_Inventory
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string INV_ID = "ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string INV_Org_ID = "组织ID";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string INV_WH_ID = "仓库ID";

                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string INV_WHB_ID = "仓位ID";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string INV_ThirdNo = "第三方编码";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string INV_OEMNo = "原厂编码";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string INV_Barcode = "配件条码";

                /// <summary>
                /// 配件批次号
                /// </summary>
                public const string INV_BatchNo = "配件批次号";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string INV_Name = "配件名称";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string INV_Specification = "配件规格型号";

                /// <summary>
                /// 供应商ID
                /// </summary>
                public const string INV_SUPP_ID = "供应商ID";

                /// <summary>
                /// 数量
                /// </summary>
                public const string INV_Qty = "数量";

                /// <summary>
                /// 采购单价
                /// </summary>
                public const string INV_PurchaseUnitPrice = "采购单价";

                /// <summary>
                /// 有效
                /// </summary>
                public const string INV_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string INV_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string INV_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string INV_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string INV_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string INV_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string INV_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string INV_ID = "INV_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string INV_Org_ID = "INV_Org_ID";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string INV_WH_ID = "INV_WH_ID";

                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string INV_WHB_ID = "INV_WHB_ID";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string INV_ThirdNo = "INV_ThirdNo";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string INV_OEMNo = "INV_OEMNo";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string INV_Barcode = "INV_Barcode";

                /// <summary>
                /// 配件批次号
                /// </summary>
                public const string INV_BatchNo = "INV_BatchNo";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string INV_Name = "INV_Name";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string INV_Specification = "INV_Specification";

                /// <summary>
                /// 供应商ID
                /// </summary>
                public const string INV_SUPP_ID = "INV_SUPP_ID";

                /// <summary>
                /// 数量
                /// </summary>
                public const string INV_Qty = "INV_Qty";

                /// <summary>
                /// 采购单价
                /// </summary>
                public const string INV_PurchaseUnitPrice = "INV_PurchaseUnitPrice";

                /// <summary>
                /// 有效
                /// </summary>
                public const string INV_IsValid = "INV_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string INV_CreatedBy = "INV_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string INV_CreatedTime = "INV_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string INV_UpdatedBy = "INV_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string INV_UpdatedTime = "INV_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string INV_VersionNo = "INV_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string INV_TransID = "INV_TransID";

            }

        }
        /// <summary>
        /// 库存图片
        /// </summary>
        public class PIS_InventoryPicture
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string INVP_ID = "ID";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string INVP_SourceTypeCode = "来源类型编码";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string INVP_SourceTypeName = "来源类型名称";

                /// <summary>
                /// 配件ID
                /// </summary>
                public const string INVP_AutoPartsID = "配件ID";

                /// <summary>
                /// 条码
                /// </summary>
                public const string INVP_Barcode = "条码";

                /// <summary>
                /// 图片名称
                /// </summary>
                public const string INVP_PictureName = "图片名称";

                /// <summary>
                /// 有效
                /// </summary>
                public const string INVP_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string INVP_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string INVP_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string INVP_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string INVP_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string INVP_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string INVP_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string INVP_ID = "INVP_ID";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string INVP_SourceTypeCode = "INVP_SourceTypeCode";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string INVP_SourceTypeName = "INVP_SourceTypeName";

                /// <summary>
                /// 配件ID
                /// </summary>
                public const string INVP_AutoPartsID = "INVP_AutoPartsID";

                /// <summary>
                /// 条码
                /// </summary>
                public const string INVP_Barcode = "INVP_Barcode";

                /// <summary>
                /// 图片名称
                /// </summary>
                public const string INVP_PictureName = "INVP_PictureName";

                /// <summary>
                /// 有效
                /// </summary>
                public const string INVP_IsValid = "INVP_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string INVP_CreatedBy = "INVP_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string INVP_CreatedTime = "INVP_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string INVP_UpdatedBy = "INVP_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string INVP_UpdatedTime = "INVP_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string INVP_VersionNo = "INVP_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string INVP_TransID = "INVP_TransID";

            }

        }
        /// <summary>
        /// 库存异动日志
        /// </summary>
        public class PIS_InventoryTransLog
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 库存异动日志ID
                /// </summary>
                public const string ITL_ID = "库存异动日志ID";

                /// <summary>
                /// 异动类型
                /// </summary>
                public const string ITL_TransType = "异动类型";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string ITL_Org_ID = "组织ID";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string ITL_WH_ID = "仓库ID";

                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string ITL_WHB_ID = "仓位ID";

                /// <summary>
                /// 业务单号
                /// </summary>
                public const string ITL_BusinessNo = "业务单号";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string ITL_Barcode = "配件条码";

                /// <summary>
                /// 配件批次号
                /// </summary>
                public const string ITL_BatchNo = "配件批次号";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string ITL_Name = "配件名称";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string ITL_Specification = "配件规格型号";

                /// <summary>
                /// 单位成本
                /// </summary>
                public const string ITL_UnitCostPrice = "单位成本";

                /// <summary>
                /// 单位销价
                /// </summary>
                public const string ITL_UnitSalePrice = "单位销价";

                /// <summary>
                /// 异动数量
                /// </summary>
                public const string ITL_Qty = "异动数量";

                /// <summary>
                /// 异动后库存数量
                /// </summary>
                public const string ITL_AfterTransQty = "异动后库存数量";

                /// <summary>
                /// 出发地
                /// </summary>
                public const string ITL_Source = "出发地";

                /// <summary>
                /// 目的地
                /// </summary>
                public const string ITL_Destination = "目的地";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ITL_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ITL_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ITL_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ITL_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ITL_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ITL_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ITL_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 库存异动日志ID
                /// </summary>
                public const string ITL_ID = "ITL_ID";

                /// <summary>
                /// 异动类型
                /// </summary>
                public const string ITL_TransType = "ITL_TransType";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string ITL_Org_ID = "ITL_Org_ID";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string ITL_WH_ID = "ITL_WH_ID";

                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string ITL_WHB_ID = "ITL_WHB_ID";

                /// <summary>
                /// 业务单号
                /// </summary>
                public const string ITL_BusinessNo = "ITL_BusinessNo";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string ITL_Barcode = "ITL_Barcode";

                /// <summary>
                /// 配件批次号
                /// </summary>
                public const string ITL_BatchNo = "ITL_BatchNo";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string ITL_Name = "ITL_Name";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string ITL_Specification = "ITL_Specification";

                /// <summary>
                /// 单位成本
                /// </summary>
                public const string ITL_UnitCostPrice = "ITL_UnitCostPrice";

                /// <summary>
                /// 单位销价
                /// </summary>
                public const string ITL_UnitSalePrice = "ITL_UnitSalePrice";

                /// <summary>
                /// 异动数量
                /// </summary>
                public const string ITL_Qty = "ITL_Qty";

                /// <summary>
                /// 异动后库存数量
                /// </summary>
                public const string ITL_AfterTransQty = "ITL_AfterTransQty";

                /// <summary>
                /// 出发地
                /// </summary>
                public const string ITL_Source = "ITL_Source";

                /// <summary>
                /// 目的地
                /// </summary>
                public const string ITL_Destination = "ITL_Destination";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ITL_IsValid = "ITL_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ITL_CreatedBy = "ITL_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ITL_CreatedTime = "ITL_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ITL_UpdatedBy = "ITL_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ITL_UpdatedTime = "ITL_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ITL_VersionNo = "ITL_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ITL_TransID = "ITL_TransID";

            }

        }
        /// <summary>
        /// 共享库存
        /// </summary>
        public class PIS_ShareInventory
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string SI_ID = "ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string SI_Org_ID = "组织ID";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string SI_WH_ID = "仓库ID";

                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string SI_WHB_ID = "仓位ID";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string SI_ThirdNo = "第三方编码";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string SI_OEMNo = "原厂编码";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string SI_Barcode = "配件条码";

                /// <summary>
                /// 配件批次号
                /// </summary>
                public const string SI_BatchNo = "配件批次号";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string SI_Name = "配件名称";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string SI_Specification = "配件规格型号";

                /// <summary>
                /// 供应商ID
                /// </summary>
                public const string SI_SUPP_ID = "供应商ID";

                /// <summary>
                /// 数量
                /// </summary>
                public const string SI_Qty = "数量";

                /// <summary>
                /// 采购单价可见
                /// </summary>
                public const string SI_PurchasePriceIsVisible = "采购单价可见";

                /// <summary>
                /// 采购单价
                /// </summary>
                public const string SI_PurchaseUnitPrice = "采购单价";

                /// <summary>
                /// 普通客户销售单价
                /// </summary>
                public const string SI_PriceOfGeneralCustomer = "普通客户销售单价";

                /// <summary>
                /// 一般汽修商户销售单价
                /// </summary>
                public const string SI_PriceOfCommonAutoFactory = "一般汽修商户销售单价";

                /// <summary>
                /// 平台内汽修商销售单价
                /// </summary>
                public const string SI_PriceOfPlatformAutoFactory = "平台内汽修商销售单价";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SI_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SI_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SI_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SI_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SI_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SI_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SI_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string SI_ID = "SI_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string SI_Org_ID = "SI_Org_ID";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string SI_WH_ID = "SI_WH_ID";

                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string SI_WHB_ID = "SI_WHB_ID";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string SI_ThirdNo = "SI_ThirdNo";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string SI_OEMNo = "SI_OEMNo";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string SI_Barcode = "SI_Barcode";

                /// <summary>
                /// 配件批次号
                /// </summary>
                public const string SI_BatchNo = "SI_BatchNo";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string SI_Name = "SI_Name";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string SI_Specification = "SI_Specification";

                /// <summary>
                /// 供应商ID
                /// </summary>
                public const string SI_SUPP_ID = "SI_SUPP_ID";

                /// <summary>
                /// 数量
                /// </summary>
                public const string SI_Qty = "SI_Qty";

                /// <summary>
                /// 采购单价可见
                /// </summary>
                public const string SI_PurchasePriceIsVisible = "SI_PurchasePriceIsVisible";

                /// <summary>
                /// 采购单价
                /// </summary>
                public const string SI_PurchaseUnitPrice = "SI_PurchaseUnitPrice";

                /// <summary>
                /// 普通客户销售单价
                /// </summary>
                public const string SI_PriceOfGeneralCustomer = "SI_PriceOfGeneralCustomer";

                /// <summary>
                /// 一般汽修商户销售单价
                /// </summary>
                public const string SI_PriceOfCommonAutoFactory = "SI_PriceOfCommonAutoFactory";

                /// <summary>
                /// 平台内汽修商销售单价
                /// </summary>
                public const string SI_PriceOfPlatformAutoFactory = "SI_PriceOfPlatformAutoFactory";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SI_IsValid = "SI_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SI_CreatedBy = "SI_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SI_CreatedTime = "SI_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SI_UpdatedBy = "SI_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SI_UpdatedTime = "SI_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SI_VersionNo = "SI_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SI_TransID = "SI_TransID";

            }

        }
        /// <summary>
        /// 盘点任务
        /// </summary>
        public class PIS_StocktakingTask
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 盘点任务ID
                /// </summary>
                public const string ST_ID = "盘点任务ID";

                /// <summary>
                /// 盘点单号
                /// </summary>
                public const string ST_No = "盘点单号";

                /// <summary>
                /// 盘点次数
                /// </summary>
                public const string ST_CheckAmount = "盘点次数";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string ST_Org_ID = "组织ID";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string ST_WH_ID = "仓库ID";

                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string ST_WHB_ID = "仓位ID";

                /// <summary>
                /// 开始时间
                /// </summary>
                public const string ST_StartTime = "开始时间";

                /// <summary>
                /// 结束时间
                /// </summary>
                public const string ST_EndTime = "结束时间";

                /// <summary>
                /// 显示成本
                /// </summary>
                public const string ST_IsShowCost = "显示成本";

                /// <summary>
                /// 应有库存量
                /// </summary>
                public const string ST_DueQty = "应有库存量";

                /// <summary>
                /// 实际库存量
                /// </summary>
                public const string ST_ActualQty = "实际库存量";

                /// <summary>
                /// 数量损失率
                /// </summary>
                public const string ST_QtyLossRatio = "数量损失率";

                /// <summary>
                /// 应有库存金额
                /// </summary>
                public const string ST_DueAmount = "应有库存金额";

                /// <summary>
                /// 实际库存金额
                /// </summary>
                public const string ST_ActualAmount = "实际库存金额";

                /// <summary>
                /// 金额损失率
                /// </summary>
                public const string ST_AmountLossRatio = "金额损失率";

                /// <summary>
                /// 盘点结果编码
                /// </summary>
                public const string ST_CheckResultCode = "盘点结果编码";

                /// <summary>
                /// 盘点结果名称
                /// </summary>
                public const string ST_CheckResultName = "盘点结果名称";

                /// <summary>
                /// 盘点单状态编码
                /// </summary>
                public const string ST_StatusCode = "盘点单状态编码";

                /// <summary>
                /// 盘点单状态名称
                /// </summary>
                public const string ST_StatusName = "盘点单状态名称";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string ST_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string ST_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string ST_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ST_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ST_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ST_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ST_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ST_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ST_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ST_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 盘点任务ID
                /// </summary>
                public const string ST_ID = "ST_ID";

                /// <summary>
                /// 盘点单号
                /// </summary>
                public const string ST_No = "ST_No";

                /// <summary>
                /// 盘点次数
                /// </summary>
                public const string ST_CheckAmount = "ST_CheckAmount";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string ST_Org_ID = "ST_Org_ID";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string ST_WH_ID = "ST_WH_ID";

                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string ST_WHB_ID = "ST_WHB_ID";

                /// <summary>
                /// 开始时间
                /// </summary>
                public const string ST_StartTime = "ST_StartTime";

                /// <summary>
                /// 结束时间
                /// </summary>
                public const string ST_EndTime = "ST_EndTime";

                /// <summary>
                /// 显示成本
                /// </summary>
                public const string ST_IsShowCost = "ST_IsShowCost";

                /// <summary>
                /// 应有库存量
                /// </summary>
                public const string ST_DueQty = "ST_DueQty";

                /// <summary>
                /// 实际库存量
                /// </summary>
                public const string ST_ActualQty = "ST_ActualQty";

                /// <summary>
                /// 数量损失率
                /// </summary>
                public const string ST_QtyLossRatio = "ST_QtyLossRatio";

                /// <summary>
                /// 应有库存金额
                /// </summary>
                public const string ST_DueAmount = "ST_DueAmount";

                /// <summary>
                /// 实际库存金额
                /// </summary>
                public const string ST_ActualAmount = "ST_ActualAmount";

                /// <summary>
                /// 金额损失率
                /// </summary>
                public const string ST_AmountLossRatio = "ST_AmountLossRatio";

                /// <summary>
                /// 盘点结果编码
                /// </summary>
                public const string ST_CheckResultCode = "ST_CheckResultCode";

                /// <summary>
                /// 盘点结果名称
                /// </summary>
                public const string ST_CheckResultName = "ST_CheckResultName";

                /// <summary>
                /// 盘点单状态编码
                /// </summary>
                public const string ST_StatusCode = "ST_StatusCode";

                /// <summary>
                /// 盘点单状态名称
                /// </summary>
                public const string ST_StatusName = "ST_StatusName";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string ST_ApprovalStatusCode = "ST_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string ST_ApprovalStatusName = "ST_ApprovalStatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string ST_Remark = "ST_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string ST_IsValid = "ST_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string ST_CreatedBy = "ST_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string ST_CreatedTime = "ST_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string ST_UpdatedBy = "ST_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string ST_UpdatedTime = "ST_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string ST_VersionNo = "ST_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string ST_TransID = "ST_TransID";

            }

        }
        /// <summary>
        /// 盘点任务明细
        /// </summary>
        public class PIS_StocktakingTaskDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 盘点任务明细ID
                /// </summary>
                public const string STD_ID = "盘点任务明细ID";

                /// <summary>
                /// 盘点任务ID
                /// </summary>
                public const string STD_TB_ID = "盘点任务ID";

                /// <summary>
                /// 盘点单号
                /// </summary>
                public const string STD_TB_No = "盘点单号";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string STD_WH_ID = "仓库ID";

                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string STD_WHB_ID = "仓位ID";

                /// <summary>
                /// 应有量
                /// </summary>
                public const string STD_DueQty = "应有量";

                /// <summary>
                /// 实际量
                /// </summary>
                public const string STD_ActualQty = "实际量";

                /// <summary>
                /// 差异数量
                /// </summary>
                public const string STD_AdjustQty = "差异数量";

                /// <summary>
                /// 允差数量
                /// </summary>
                public const string STD_ApprDiffQty = "允差数量";

                /// <summary>
                /// 数量允差比
                /// </summary>
                public const string STD_ApprDiffQtyRate = "数量允差比";

                /// <summary>
                /// 调整数量
                /// </summary>
                public const string STD_SnapshotQty = "调整数量";

                /// <summary>
                /// 应有金额
                /// </summary>
                public const string STD_DueAmount = "应有金额";

                /// <summary>
                /// 实际金额
                /// </summary>
                public const string STD_ActualAmount = "实际金额";

                /// <summary>
                /// 金额损失率
                /// </summary>
                public const string STD_AmountLossRatio = "金额损失率";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string STD_Barcode = "配件条码";

                /// <summary>
                /// 配件批次号
                /// </summary>
                public const string STD_BatchNo = "配件批次号";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string STD_ThirdNo = "第三方编码";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string STD_OEMNo = "原厂编码";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string STD_Name = "配件名称";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string STD_Specification = "配件规格型号";

                /// <summary>
                /// 单位
                /// </summary>
                public const string STD_UOM = "单位";

                /// <summary>
                /// 有效
                /// </summary>
                public const string STD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string STD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string STD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string STD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string STD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string STD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string STD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 盘点任务明细ID
                /// </summary>
                public const string STD_ID = "STD_ID";

                /// <summary>
                /// 盘点任务ID
                /// </summary>
                public const string STD_TB_ID = "STD_TB_ID";

                /// <summary>
                /// 盘点单号
                /// </summary>
                public const string STD_TB_No = "STD_TB_No";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string STD_WH_ID = "STD_WH_ID";

                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string STD_WHB_ID = "STD_WHB_ID";

                /// <summary>
                /// 应有量
                /// </summary>
                public const string STD_DueQty = "STD_DueQty";

                /// <summary>
                /// 实际量
                /// </summary>
                public const string STD_ActualQty = "STD_ActualQty";

                /// <summary>
                /// 差异数量
                /// </summary>
                public const string STD_AdjustQty = "STD_AdjustQty";

                /// <summary>
                /// 允差数量
                /// </summary>
                public const string STD_ApprDiffQty = "STD_ApprDiffQty";

                /// <summary>
                /// 数量允差比
                /// </summary>
                public const string STD_ApprDiffQtyRate = "STD_ApprDiffQtyRate";

                /// <summary>
                /// 调整数量
                /// </summary>
                public const string STD_SnapshotQty = "STD_SnapshotQty";

                /// <summary>
                /// 应有金额
                /// </summary>
                public const string STD_DueAmount = "STD_DueAmount";

                /// <summary>
                /// 实际金额
                /// </summary>
                public const string STD_ActualAmount = "STD_ActualAmount";

                /// <summary>
                /// 金额损失率
                /// </summary>
                public const string STD_AmountLossRatio = "STD_AmountLossRatio";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string STD_Barcode = "STD_Barcode";

                /// <summary>
                /// 配件批次号
                /// </summary>
                public const string STD_BatchNo = "STD_BatchNo";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string STD_ThirdNo = "STD_ThirdNo";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string STD_OEMNo = "STD_OEMNo";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string STD_Name = "STD_Name";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string STD_Specification = "STD_Specification";

                /// <summary>
                /// 单位
                /// </summary>
                public const string STD_UOM = "STD_UOM";

                /// <summary>
                /// 有效
                /// </summary>
                public const string STD_IsValid = "STD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string STD_CreatedBy = "STD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string STD_CreatedTime = "STD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string STD_UpdatedBy = "STD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string STD_UpdatedTime = "STD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string STD_VersionNo = "STD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string STD_TransID = "STD_TransID";

            }

        }
        /// <summary>
        /// 普通客户
        /// </summary>
        public class PIS_GeneralCustomer
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 普通客户ID
                /// </summary>
                public const string GC_ID = "普通客户ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string GC_Org_ID = "组织ID";

                /// <summary>
                /// 姓名
                /// </summary>
                public const string GC_Name = "姓名";

                /// <summary>
                /// 手机号码
                /// </summary>
                public const string GC_PhoneNo = "手机号码";

                /// <summary>
                /// 地址
                /// </summary>
                public const string GC_Address = "地址";

                /// <summary>
                /// 信用额度
                /// </summary>
                public const string GC_CreditAmount = "信用额度";

                /// <summary>
                /// 默认支付类型编码
                /// </summary>
                public const string GC_PaymentTypeCode = "默认支付类型编码";

                /// <summary>
                /// 默认支付类型名称
                /// </summary>
                public const string GC_PaymentTypeName = "默认支付类型名称";

                /// <summary>
                /// 默认开票类型编码
                /// </summary>
                public const string GC_BillingTypeCode = "默认开票类型编码";

                /// <summary>
                /// 默认开票类型名称
                /// </summary>
                public const string GC_BillingTypeName = "默认开票类型名称";

                /// <summary>
                /// 默认物流人员类型编码
                /// </summary>
                public const string GC_DeliveryTypeCode = "默认物流人员类型编码";

                /// <summary>
                /// 默认物流人员类型名称
                /// </summary>
                public const string GC_DeliveryTypeName = "默认物流人员类型名称";

                /// <summary>
                /// 默认物流人员ID
                /// </summary>
                public const string GC_DeliveryByID = "默认物流人员ID";

                /// <summary>
                /// 默认物流人员名称
                /// </summary>
                public const string GC_DeliveryByName = "默认物流人员名称";

                /// <summary>
                /// 默认物流人员手机号
                /// </summary>
                public const string GC_DeliveryByPhoneNo = "默认物流人员手机号";

                /// <summary>
                /// 终止销售
                /// </summary>
                public const string GC_IsEndSales = "终止销售";

                /// <summary>
                /// 配件价格类别
                /// </summary>
                public const string GC_AutoPartsPriceType = "配件价格类别";

                /// <summary>
                /// 备注
                /// </summary>
                public const string GC_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string GC_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string GC_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string GC_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string GC_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string GC_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string GC_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string GC_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 普通客户ID
                /// </summary>
                public const string GC_ID = "GC_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string GC_Org_ID = "GC_Org_ID";

                /// <summary>
                /// 姓名
                /// </summary>
                public const string GC_Name = "GC_Name";

                /// <summary>
                /// 手机号码
                /// </summary>
                public const string GC_PhoneNo = "GC_PhoneNo";

                /// <summary>
                /// 地址
                /// </summary>
                public const string GC_Address = "GC_Address";

                /// <summary>
                /// 信用额度
                /// </summary>
                public const string GC_CreditAmount = "GC_CreditAmount";

                /// <summary>
                /// 默认支付类型编码
                /// </summary>
                public const string GC_PaymentTypeCode = "GC_PaymentTypeCode";

                /// <summary>
                /// 默认支付类型名称
                /// </summary>
                public const string GC_PaymentTypeName = "GC_PaymentTypeName";

                /// <summary>
                /// 默认开票类型编码
                /// </summary>
                public const string GC_BillingTypeCode = "GC_BillingTypeCode";

                /// <summary>
                /// 默认开票类型名称
                /// </summary>
                public const string GC_BillingTypeName = "GC_BillingTypeName";

                /// <summary>
                /// 默认物流人员类型编码
                /// </summary>
                public const string GC_DeliveryTypeCode = "GC_DeliveryTypeCode";

                /// <summary>
                /// 默认物流人员类型名称
                /// </summary>
                public const string GC_DeliveryTypeName = "GC_DeliveryTypeName";

                /// <summary>
                /// 默认物流人员ID
                /// </summary>
                public const string GC_DeliveryByID = "GC_DeliveryByID";

                /// <summary>
                /// 默认物流人员名称
                /// </summary>
                public const string GC_DeliveryByName = "GC_DeliveryByName";

                /// <summary>
                /// 默认物流人员手机号
                /// </summary>
                public const string GC_DeliveryByPhoneNo = "GC_DeliveryByPhoneNo";

                /// <summary>
                /// 终止销售
                /// </summary>
                public const string GC_IsEndSales = "GC_IsEndSales";

                /// <summary>
                /// 配件价格类别
                /// </summary>
                public const string GC_AutoPartsPriceType = "GC_AutoPartsPriceType";

                /// <summary>
                /// 备注
                /// </summary>
                public const string GC_Remark = "GC_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string GC_IsValid = "GC_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string GC_CreatedBy = "GC_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string GC_CreatedTime = "GC_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string GC_UpdatedBy = "GC_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string GC_UpdatedTime = "GC_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string GC_VersionNo = "GC_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string GC_TransID = "GC_TransID";

            }

        }
        /// <summary>
        /// 汽修商客户
        /// </summary>
        public class PIS_AutoFactoryCustomer
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 汽修商客户ID
                /// </summary>
                public const string AFC_ID = "汽修商客户ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string AFC_Org_ID = "组织ID";

                /// <summary>
                /// 是否平台商户
                /// </summary>
                public const string AFC_IsPlatform = "是否平台商户";

                /// <summary>
                /// 汽修商编码
                /// </summary>
                public const string AFC_Code = "汽修商编码";

                /// <summary>
                /// 汽修商名称
                /// </summary>
                public const string AFC_Name = "汽修商名称";

                /// <summary>
                /// 汽修商联系人
                /// </summary>
                public const string AFC_Contacter = "汽修商联系人";

                /// <summary>
                /// 汽修商联系方式
                /// </summary>
                public const string AFC_PhoneNo = "汽修商联系方式";

                /// <summary>
                /// 汽修商地址
                /// </summary>
                public const string AFC_Address = "汽修商地址";

                /// <summary>
                /// 汽修组织编码
                /// </summary>
                public const string AFC_AROrg_Code = "汽修组织编码";

                /// <summary>
                /// 汽修组织名称
                /// </summary>
                public const string AFC_AROrg_Name = "汽修组织名称";

                /// <summary>
                /// 汽修组织联系人
                /// </summary>
                public const string AFC_AROrg_Contacter = "汽修组织联系人";

                /// <summary>
                /// 汽修组织联系方式
                /// </summary>
                public const string AFC_AROrg_Phone = "汽修组织联系方式";

                /// <summary>
                /// 汽修组织地址
                /// </summary>
                public const string AFC_AROrg_Address = "汽修组织地址";

                /// <summary>
                /// 信用额度
                /// </summary>
                public const string AFC_CreditAmount = "信用额度";

                /// <summary>
                /// 默认支付类型编码
                /// </summary>
                public const string AFC_PaymentTypeCode = "默认支付类型编码";

                /// <summary>
                /// 默认支付类型名称
                /// </summary>
                public const string AFC_PaymentTypeName = "默认支付类型名称";

                /// <summary>
                /// 默认开票类型编码
                /// </summary>
                public const string AFC_BillingTypeCode = "默认开票类型编码";

                /// <summary>
                /// 默认开票类型名称
                /// </summary>
                public const string AFC_BillingTypeName = "默认开票类型名称";

                /// <summary>
                /// 默认物流人员类型编码
                /// </summary>
                public const string AFC_DeliveryTypeCode = "默认物流人员类型编码";

                /// <summary>
                /// 默认物流人员类型名称
                /// </summary>
                public const string AFC_DeliveryTypeName = "默认物流人员类型名称";

                /// <summary>
                /// 默认物流人员ID
                /// </summary>
                public const string AFC_DeliveryByID = "默认物流人员ID";

                /// <summary>
                /// 默认物流人员名称
                /// </summary>
                public const string AFC_DeliveryByName = "默认物流人员名称";

                /// <summary>
                /// 默认物流人员手机号
                /// </summary>
                public const string AFC_DeliveryByPhoneNo = "默认物流人员手机号";

                /// <summary>
                /// 终止销售
                /// </summary>
                public const string AFC_IsEndSales = "终止销售";

                /// <summary>
                /// 配件价格类别
                /// </summary>
                public const string AFC_AutoPartsPriceType = "配件价格类别";

                /// <summary>
                /// 备注
                /// </summary>
                public const string AFC_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string AFC_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string AFC_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string AFC_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string AFC_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string AFC_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string AFC_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string AFC_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 汽修商客户ID
                /// </summary>
                public const string AFC_ID = "AFC_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string AFC_Org_ID = "AFC_Org_ID";

                /// <summary>
                /// 是否平台商户
                /// </summary>
                public const string AFC_IsPlatform = "AFC_IsPlatform";

                /// <summary>
                /// 汽修商编码
                /// </summary>
                public const string AFC_Code = "AFC_Code";

                /// <summary>
                /// 汽修商名称
                /// </summary>
                public const string AFC_Name = "AFC_Name";

                /// <summary>
                /// 汽修商联系人
                /// </summary>
                public const string AFC_Contacter = "AFC_Contacter";

                /// <summary>
                /// 汽修商联系方式
                /// </summary>
                public const string AFC_PhoneNo = "AFC_PhoneNo";

                /// <summary>
                /// 汽修商地址
                /// </summary>
                public const string AFC_Address = "AFC_Address";

                /// <summary>
                /// 汽修组织编码
                /// </summary>
                public const string AFC_AROrg_Code = "AFC_AROrg_Code";

                /// <summary>
                /// 汽修组织名称
                /// </summary>
                public const string AFC_AROrg_Name = "AFC_AROrg_Name";

                /// <summary>
                /// 汽修组织联系人
                /// </summary>
                public const string AFC_AROrg_Contacter = "AFC_AROrg_Contacter";

                /// <summary>
                /// 汽修组织联系方式
                /// </summary>
                public const string AFC_AROrg_Phone = "AFC_AROrg_Phone";

                /// <summary>
                /// 汽修组织地址
                /// </summary>
                public const string AFC_AROrg_Address = "AFC_AROrg_Address";

                /// <summary>
                /// 信用额度
                /// </summary>
                public const string AFC_CreditAmount = "AFC_CreditAmount";

                /// <summary>
                /// 默认支付类型编码
                /// </summary>
                public const string AFC_PaymentTypeCode = "AFC_PaymentTypeCode";

                /// <summary>
                /// 默认支付类型名称
                /// </summary>
                public const string AFC_PaymentTypeName = "AFC_PaymentTypeName";

                /// <summary>
                /// 默认开票类型编码
                /// </summary>
                public const string AFC_BillingTypeCode = "AFC_BillingTypeCode";

                /// <summary>
                /// 默认开票类型名称
                /// </summary>
                public const string AFC_BillingTypeName = "AFC_BillingTypeName";

                /// <summary>
                /// 默认物流人员类型编码
                /// </summary>
                public const string AFC_DeliveryTypeCode = "AFC_DeliveryTypeCode";

                /// <summary>
                /// 默认物流人员类型名称
                /// </summary>
                public const string AFC_DeliveryTypeName = "AFC_DeliveryTypeName";

                /// <summary>
                /// 默认物流人员ID
                /// </summary>
                public const string AFC_DeliveryByID = "AFC_DeliveryByID";

                /// <summary>
                /// 默认物流人员名称
                /// </summary>
                public const string AFC_DeliveryByName = "AFC_DeliveryByName";

                /// <summary>
                /// 默认物流人员手机号
                /// </summary>
                public const string AFC_DeliveryByPhoneNo = "AFC_DeliveryByPhoneNo";

                /// <summary>
                /// 终止销售
                /// </summary>
                public const string AFC_IsEndSales = "AFC_IsEndSales";

                /// <summary>
                /// 配件价格类别
                /// </summary>
                public const string AFC_AutoPartsPriceType = "AFC_AutoPartsPriceType";

                /// <summary>
                /// 备注
                /// </summary>
                public const string AFC_Remark = "AFC_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string AFC_IsValid = "AFC_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string AFC_CreatedBy = "AFC_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string AFC_CreatedTime = "AFC_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string AFC_UpdatedBy = "AFC_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string AFC_UpdatedTime = "AFC_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string AFC_VersionNo = "AFC_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string AFC_TransID = "AFC_TransID";

            }

        }
        /// <summary>
        /// 调拨单
        /// </summary>
        public class PIS_TransferBill
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 调拨单ID
                /// </summary>
                public const string TB_ID = "调拨单ID";

                /// <summary>
                /// 单号
                /// </summary>
                public const string TB_No = "单号";

                /// <summary>
                /// 单据类型编码
                /// </summary>
                public const string TB_TypeCode = "单据类型编码";

                /// <summary>
                /// 单据类型名称
                /// </summary>
                public const string TB_TypeName = "单据类型名称";

                /// <summary>
                /// 调拨类型编码
                /// </summary>
                public const string TB_TransferTypeCode = "调拨类型编码";

                /// <summary>
                /// 调拨类型名称
                /// </summary>
                public const string TB_TransferTypeName = "调拨类型名称";

                /// <summary>
                /// 调出组织ID
                /// </summary>
                public const string TB_TransferOutOrgId = "调出组织ID";

                /// <summary>
                /// 调出组织名称
                /// </summary>
                public const string TB_TransferOutOrgName = "调出组织名称";

                /// <summary>
                /// 调入组织ID
                /// </summary>
                public const string TB_TransferInOrgId = "调入组织ID";

                /// <summary>
                /// 调入组织名称
                /// </summary>
                public const string TB_TransferInOrgName = "调入组织名称";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string TB_StatusCode = "单据状态编码";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string TB_StatusName = "单据状态名称";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string TB_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string TB_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string TB_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string TB_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string TB_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string TB_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string TB_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string TB_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string TB_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string TB_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 调拨单ID
                /// </summary>
                public const string TB_ID = "TB_ID";

                /// <summary>
                /// 单号
                /// </summary>
                public const string TB_No = "TB_No";

                /// <summary>
                /// 单据类型编码
                /// </summary>
                public const string TB_TypeCode = "TB_TypeCode";

                /// <summary>
                /// 单据类型名称
                /// </summary>
                public const string TB_TypeName = "TB_TypeName";

                /// <summary>
                /// 调拨类型编码
                /// </summary>
                public const string TB_TransferTypeCode = "TB_TransferTypeCode";

                /// <summary>
                /// 调拨类型名称
                /// </summary>
                public const string TB_TransferTypeName = "TB_TransferTypeName";

                /// <summary>
                /// 调出组织ID
                /// </summary>
                public const string TB_TransferOutOrgId = "TB_TransferOutOrgId";

                /// <summary>
                /// 调出组织名称
                /// </summary>
                public const string TB_TransferOutOrgName = "TB_TransferOutOrgName";

                /// <summary>
                /// 调入组织ID
                /// </summary>
                public const string TB_TransferInOrgId = "TB_TransferInOrgId";

                /// <summary>
                /// 调入组织名称
                /// </summary>
                public const string TB_TransferInOrgName = "TB_TransferInOrgName";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string TB_StatusCode = "TB_StatusCode";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string TB_StatusName = "TB_StatusName";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string TB_ApprovalStatusCode = "TB_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string TB_ApprovalStatusName = "TB_ApprovalStatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string TB_Remark = "TB_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string TB_IsValid = "TB_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string TB_CreatedBy = "TB_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string TB_CreatedTime = "TB_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string TB_UpdatedBy = "TB_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string TB_UpdatedTime = "TB_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string TB_VersionNo = "TB_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string TB_TransID = "TB_TransID";

            }

        }
        /// <summary>
        /// 调拨单明细
        /// </summary>
        public class PIS_TransferBillDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 调拨单明细ID
                /// </summary>
                public const string TBD_ID = "调拨单明细ID";

                /// <summary>
                /// 调拨单ID
                /// </summary>
                public const string TBD_TB_ID = "调拨单ID";

                /// <summary>
                /// 调拨单号
                /// </summary>
                public const string TBD_TB_No = "调拨单号";

                /// <summary>
                /// 调出仓库ID
                /// </summary>
                public const string TBD_TransOutWhId = "调出仓库ID";

                /// <summary>
                /// 调出仓位ID
                /// </summary>
                public const string TBD_TransOutBinId = "调出仓位ID";

                /// <summary>
                /// 调入仓库ID
                /// </summary>
                public const string TBD_TransInWhId = "调入仓库ID";

                /// <summary>
                /// 调入仓位ID
                /// </summary>
                public const string TBD_TransInBinId = "调入仓位ID";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string TBD_Barcode = "配件条码";

                /// <summary>
                /// 调出配件批次号
                /// </summary>
                public const string TBD_TransOutBatchNo = "调出配件批次号";

                /// <summary>
                /// 调入配件批次号
                /// </summary>
                public const string TBD_TransInBatchNo = "调入配件批次号";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string TBD_ThirdNo = "第三方编码";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string TBD_OEMNo = "原厂编码";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string TBD_Name = "配件名称";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string TBD_Specification = "配件规格型号";

                /// <summary>
                /// 供应商ID
                /// </summary>
                public const string TBD_SUPP_ID = "供应商ID";

                /// <summary>
                /// 数量
                /// </summary>
                public const string TBD_Qty = "数量";

                /// <summary>
                /// 单位
                /// </summary>
                public const string TBD_UOM = "单位";

                /// <summary>
                /// 源库存单价
                /// </summary>
                public const string TBD_SourUnitPrice = "源库存单价";

                /// <summary>
                /// 入库单价
                /// </summary>
                public const string TBD_DestUnitPrice = "入库单价";

                /// <summary>
                /// 已结算标志
                /// </summary>
                public const string TBD_IsSettled = "已结算标志";

                /// <summary>
                /// 有效
                /// </summary>
                public const string TBD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string TBD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string TBD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string TBD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string TBD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string TBD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string TBD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 调拨单明细ID
                /// </summary>
                public const string TBD_ID = "TBD_ID";

                /// <summary>
                /// 调拨单ID
                /// </summary>
                public const string TBD_TB_ID = "TBD_TB_ID";

                /// <summary>
                /// 调拨单号
                /// </summary>
                public const string TBD_TB_No = "TBD_TB_No";

                /// <summary>
                /// 调出仓库ID
                /// </summary>
                public const string TBD_TransOutWhId = "TBD_TransOutWhId";

                /// <summary>
                /// 调出仓位ID
                /// </summary>
                public const string TBD_TransOutBinId = "TBD_TransOutBinId";

                /// <summary>
                /// 调入仓库ID
                /// </summary>
                public const string TBD_TransInWhId = "TBD_TransInWhId";

                /// <summary>
                /// 调入仓位ID
                /// </summary>
                public const string TBD_TransInBinId = "TBD_TransInBinId";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string TBD_Barcode = "TBD_Barcode";

                /// <summary>
                /// 调出配件批次号
                /// </summary>
                public const string TBD_TransOutBatchNo = "TBD_TransOutBatchNo";

                /// <summary>
                /// 调入配件批次号
                /// </summary>
                public const string TBD_TransInBatchNo = "TBD_TransInBatchNo";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string TBD_ThirdNo = "TBD_ThirdNo";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string TBD_OEMNo = "TBD_OEMNo";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string TBD_Name = "TBD_Name";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string TBD_Specification = "TBD_Specification";

                /// <summary>
                /// 供应商ID
                /// </summary>
                public const string TBD_SUPP_ID = "TBD_SUPP_ID";

                /// <summary>
                /// 数量
                /// </summary>
                public const string TBD_Qty = "TBD_Qty";

                /// <summary>
                /// 单位
                /// </summary>
                public const string TBD_UOM = "TBD_UOM";

                /// <summary>
                /// 源库存单价
                /// </summary>
                public const string TBD_SourUnitPrice = "TBD_SourUnitPrice";

                /// <summary>
                /// 入库单价
                /// </summary>
                public const string TBD_DestUnitPrice = "TBD_DestUnitPrice";

                /// <summary>
                /// 已结算标志
                /// </summary>
                public const string TBD_IsSettled = "TBD_IsSettled";

                /// <summary>
                /// 有效
                /// </summary>
                public const string TBD_IsValid = "TBD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string TBD_CreatedBy = "TBD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string TBD_CreatedTime = "TBD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string TBD_UpdatedBy = "TBD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string TBD_UpdatedTime = "TBD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string TBD_VersionNo = "TBD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string TBD_TransID = "TBD_TransID";

            }

        }
        /// <summary>
        /// 入库单
        /// </summary>
        public class PIS_StockInBill
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 入库单ID
                /// </summary>
                public const string SIB_ID = "入库单ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string SIB_Org_ID = "组织ID";

                /// <summary>
                /// 单号
                /// </summary>
                public const string SIB_No = "单号";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string SIB_SourceTypeCode = "来源类型编码";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string SIB_SourceTypeName = "来源类型名称";

                /// <summary>
                /// 来源单号
                /// </summary>
                public const string SIB_SourceNo = "来源单号";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string SIB_StatusCode = "单据状态编码";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string SIB_StatusName = "单据状态名称";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string SIB_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string SIB_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SIB_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SIB_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SIB_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SIB_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SIB_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SIB_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SIB_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SIB_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 入库单ID
                /// </summary>
                public const string SIB_ID = "SIB_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string SIB_Org_ID = "SIB_Org_ID";

                /// <summary>
                /// 单号
                /// </summary>
                public const string SIB_No = "SIB_No";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string SIB_SourceTypeCode = "SIB_SourceTypeCode";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string SIB_SourceTypeName = "SIB_SourceTypeName";

                /// <summary>
                /// 来源单号
                /// </summary>
                public const string SIB_SourceNo = "SIB_SourceNo";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string SIB_StatusCode = "SIB_StatusCode";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string SIB_StatusName = "SIB_StatusName";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string SIB_ApprovalStatusCode = "SIB_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string SIB_ApprovalStatusName = "SIB_ApprovalStatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SIB_Remark = "SIB_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SIB_IsValid = "SIB_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SIB_CreatedBy = "SIB_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SIB_CreatedTime = "SIB_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SIB_UpdatedBy = "SIB_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SIB_UpdatedTime = "SIB_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SIB_VersionNo = "SIB_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SIB_TransID = "SIB_TransID";

            }

        }
        /// <summary>
        /// 入库单明细
        /// </summary>
        public class PIS_StockInDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 入库单明细ID
                /// </summary>
                public const string SID_ID = "入库单明细ID";

                /// <summary>
                /// 入库单ID
                /// </summary>
                public const string SID_SIB_ID = "入库单ID";

                /// <summary>
                /// 入库单号
                /// </summary>
                public const string SID_SIB_No = "入库单号";

                /// <summary>
                /// 来源单明细ID
                /// </summary>
                public const string SID_SourceDetailID = "来源单明细ID";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string SID_Barcode = "配件条码";

                /// <summary>
                /// 配件批次号
                /// </summary>
                public const string SID_BatchNo = "配件批次号";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string SID_ThirdNo = "第三方编码";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string SID_OEMNo = "原厂编码";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string SID_Name = "配件名称";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string SID_Specification = "配件规格型号";

                /// <summary>
                /// 供应商ID
                /// </summary>
                public const string SID_SUPP_ID = "供应商ID";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string SID_WH_ID = "仓库ID";

                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string SID_WHB_ID = "仓位ID";

                /// <summary>
                /// 入库数量
                /// </summary>
                public const string SID_Qty = "入库数量";

                /// <summary>
                /// 单位
                /// </summary>
                public const string SID_UOM = "单位";

                /// <summary>
                /// 入库单价
                /// </summary>
                public const string SID_UnitCostPrice = "入库单价";

                /// <summary>
                /// 入库金额
                /// </summary>
                public const string SID_Amount = "入库金额";

                /// <summary>
                /// 已结算标志
                /// </summary>
                public const string SID_IsSettled = "已结算标志";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SID_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SID_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SID_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SID_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SID_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SID_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SID_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 入库单明细ID
                /// </summary>
                public const string SID_ID = "SID_ID";

                /// <summary>
                /// 入库单ID
                /// </summary>
                public const string SID_SIB_ID = "SID_SIB_ID";

                /// <summary>
                /// 入库单号
                /// </summary>
                public const string SID_SIB_No = "SID_SIB_No";

                /// <summary>
                /// 来源单明细ID
                /// </summary>
                public const string SID_SourceDetailID = "SID_SourceDetailID";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string SID_Barcode = "SID_Barcode";

                /// <summary>
                /// 配件批次号
                /// </summary>
                public const string SID_BatchNo = "SID_BatchNo";

                /// <summary>
                /// 第三方编码
                /// </summary>
                public const string SID_ThirdNo = "SID_ThirdNo";

                /// <summary>
                /// 原厂编码
                /// </summary>
                public const string SID_OEMNo = "SID_OEMNo";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string SID_Name = "SID_Name";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string SID_Specification = "SID_Specification";

                /// <summary>
                /// 供应商ID
                /// </summary>
                public const string SID_SUPP_ID = "SID_SUPP_ID";

                /// <summary>
                /// 仓库ID
                /// </summary>
                public const string SID_WH_ID = "SID_WH_ID";

                /// <summary>
                /// 仓位ID
                /// </summary>
                public const string SID_WHB_ID = "SID_WHB_ID";

                /// <summary>
                /// 入库数量
                /// </summary>
                public const string SID_Qty = "SID_Qty";

                /// <summary>
                /// 单位
                /// </summary>
                public const string SID_UOM = "SID_UOM";

                /// <summary>
                /// 入库单价
                /// </summary>
                public const string SID_UnitCostPrice = "SID_UnitCostPrice";

                /// <summary>
                /// 入库金额
                /// </summary>
                public const string SID_Amount = "SID_Amount";

                /// <summary>
                /// 已结算标志
                /// </summary>
                public const string SID_IsSettled = "SID_IsSettled";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SID_IsValid = "SID_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SID_CreatedBy = "SID_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SID_CreatedTime = "SID_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SID_UpdatedBy = "SID_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SID_UpdatedTime = "SID_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SID_VersionNo = "SID_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SID_TransID = "SID_TransID";

            }

        }
        /// <summary>
        /// 微信菜单
        /// </summary>
        public class WC_Menu
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 菜单ID
                /// </summary>
                public const string WM_ID = "菜单ID";

                /// <summary>
                /// 平台编码
                /// </summary>
                public const string WM_PlatformCode = "平台编码";

                /// <summary>
                /// 父菜单
                /// </summary>
                public const string WM_ParentID = "父菜单";

                /// <summary>
                /// 是否叶节点
                /// </summary>
                public const string WM_Isleff = "是否叶节点";

                /// <summary>
                /// 菜单名称
                /// </summary>
                public const string WM_Name = "菜单名称";

                /// <summary>
                /// 菜单类型
                /// </summary>
                public const string WM_Type = "菜单类型";

                /// <summary>
                /// 菜单Key
                /// </summary>
                public const string WM_Key = "菜单Key";

                /// <summary>
                /// 菜单URL
                /// </summary>
                public const string WM_URL = "菜单URL";

                /// <summary>
                /// MediaID
                /// </summary>
                public const string WM_MediaID = "MediaID";

                /// <summary>
                /// 显示顺序
                /// </summary>
                public const string WM_Index = "显示顺序";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WM_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WM_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WM_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WM_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WM_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WM_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WM_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 菜单ID
                /// </summary>
                public const string WM_ID = "WM_ID";

                /// <summary>
                /// 平台编码
                /// </summary>
                public const string WM_PlatformCode = "WM_PlatformCode";

                /// <summary>
                /// 父菜单
                /// </summary>
                public const string WM_ParentID = "WM_ParentID";

                /// <summary>
                /// 是否叶节点
                /// </summary>
                public const string WM_Isleff = "WM_Isleff";

                /// <summary>
                /// 菜单名称
                /// </summary>
                public const string WM_Name = "WM_Name";

                /// <summary>
                /// 菜单类型
                /// </summary>
                public const string WM_Type = "WM_Type";

                /// <summary>
                /// 菜单Key
                /// </summary>
                public const string WM_Key = "WM_Key";

                /// <summary>
                /// 菜单URL
                /// </summary>
                public const string WM_URL = "WM_URL";

                /// <summary>
                /// MediaID
                /// </summary>
                public const string WM_MediaID = "WM_MediaID";

                /// <summary>
                /// 显示顺序
                /// </summary>
                public const string WM_Index = "WM_Index";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WM_IsValid = "WM_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WM_CreatedBy = "WM_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WM_CreatedTime = "WM_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WM_UpdatedBy = "WM_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WM_UpdatedTime = "WM_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WM_VersionNo = "WM_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WM_TransID = "WM_TransID";

            }

        }
        /// <summary>
        /// 微信消息
        /// </summary>
        public class WC_Message
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string WMsg_ID = "ID";

                /// <summary>
                /// 开发者微信号
                /// </summary>
                public const string WMsg_ToUserName = "开发者微信号";

                /// <summary>
                /// 发送方OpenID
                /// </summary>
                public const string WMsg_FromUserName = "发送方OpenID";

                /// <summary>
                /// 消息创建时间
                /// </summary>
                public const string WMsg_CreateTime = "消息创建时间";

                /// <summary>
                /// 消息体
                /// </summary>
                public const string WMsg_MessageBody = "消息体";

                /// <summary>
                /// 消息类型
                /// </summary>
                public const string WMsg_MsgType = "消息类型";

                /// <summary>
                /// 文本消息内容
                /// </summary>
                public const string WMsg_Content = "文本消息内容";

                /// <summary>
                /// 消息ID
                /// </summary>
                public const string WMsg_MsgId = "消息ID";

                /// <summary>
                /// 图片链接
                /// </summary>
                public const string WMsg_PicUrl = "图片链接";

                /// <summary>
                /// 图片消息媒体id
                /// </summary>
                public const string WMsg_MediaId = "图片消息媒体id";

                /// <summary>
                /// 语音格式
                /// </summary>
                public const string WMsg_Format = "语音格式";

                /// <summary>
                /// 视频消息缩略图的媒体id
                /// </summary>
                public const string WMsg_ThumbMediaId = "视频消息缩略图的媒体id";

                /// <summary>
                /// 地理位置维度
                /// </summary>
                public const string WMsg_Location_X = "地理位置维度";

                /// <summary>
                /// 地理位置经度
                /// </summary>
                public const string WMsg_Location_Y = "地理位置经度";

                /// <summary>
                /// 地图缩放大小
                /// </summary>
                public const string WMsg_Scale = "地图缩放大小";

                /// <summary>
                /// 地理位置信息
                /// </summary>
                public const string WMsg_Label = "地理位置信息";

                /// <summary>
                /// 消息标题
                /// </summary>
                public const string WMsg_Title = "消息标题";

                /// <summary>
                /// 消息描述
                /// </summary>
                public const string WMsg_Description = "消息描述";

                /// <summary>
                /// 消息链接
                /// </summary>
                public const string WMsg_Url = "消息链接";

                /// <summary>
                /// 事件类型
                /// </summary>
                public const string WMsg_Event = "事件类型";

                /// <summary>
                /// 事件KEY值
                /// </summary>
                public const string WMsg_EventKey = "事件KEY值";

                /// <summary>
                /// 二维码的ticket
                /// </summary>
                public const string WMsg_Ticket = "二维码的ticket";

                /// <summary>
                /// 地理位置纬度
                /// </summary>
                public const string WMsg_Latitude = "地理位置纬度";

                /// <summary>
                /// 地理位置经度
                /// </summary>
                public const string WMsg_Longitude = "地理位置经度";

                /// <summary>
                /// 地理位置精度
                /// </summary>
                public const string WMsg_Precision = "地理位置精度";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WMsg_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WMsg_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WMsg_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WMsg_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WMsg_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WMsg_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WMsg_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string WMsg_ID = "WMsg_ID";

                /// <summary>
                /// 开发者微信号
                /// </summary>
                public const string WMsg_ToUserName = "WMsg_ToUserName";

                /// <summary>
                /// 发送方OpenID
                /// </summary>
                public const string WMsg_FromUserName = "WMsg_FromUserName";

                /// <summary>
                /// 消息创建时间
                /// </summary>
                public const string WMsg_CreateTime = "WMsg_CreateTime";

                /// <summary>
                /// 消息体
                /// </summary>
                public const string WMsg_MessageBody = "WMsg_MessageBody";

                /// <summary>
                /// 消息类型
                /// </summary>
                public const string WMsg_MsgType = "WMsg_MsgType";

                /// <summary>
                /// 文本消息内容
                /// </summary>
                public const string WMsg_Content = "WMsg_Content";

                /// <summary>
                /// 消息ID
                /// </summary>
                public const string WMsg_MsgId = "WMsg_MsgId";

                /// <summary>
                /// 图片链接
                /// </summary>
                public const string WMsg_PicUrl = "WMsg_PicUrl";

                /// <summary>
                /// 图片消息媒体id
                /// </summary>
                public const string WMsg_MediaId = "WMsg_MediaId";

                /// <summary>
                /// 语音格式
                /// </summary>
                public const string WMsg_Format = "WMsg_Format";

                /// <summary>
                /// 视频消息缩略图的媒体id
                /// </summary>
                public const string WMsg_ThumbMediaId = "WMsg_ThumbMediaId";

                /// <summary>
                /// 地理位置维度
                /// </summary>
                public const string WMsg_Location_X = "WMsg_Location_X";

                /// <summary>
                /// 地理位置经度
                /// </summary>
                public const string WMsg_Location_Y = "WMsg_Location_Y";

                /// <summary>
                /// 地图缩放大小
                /// </summary>
                public const string WMsg_Scale = "WMsg_Scale";

                /// <summary>
                /// 地理位置信息
                /// </summary>
                public const string WMsg_Label = "WMsg_Label";

                /// <summary>
                /// 消息标题
                /// </summary>
                public const string WMsg_Title = "WMsg_Title";

                /// <summary>
                /// 消息描述
                /// </summary>
                public const string WMsg_Description = "WMsg_Description";

                /// <summary>
                /// 消息链接
                /// </summary>
                public const string WMsg_Url = "WMsg_Url";

                /// <summary>
                /// 事件类型
                /// </summary>
                public const string WMsg_Event = "WMsg_Event";

                /// <summary>
                /// 事件KEY值
                /// </summary>
                public const string WMsg_EventKey = "WMsg_EventKey";

                /// <summary>
                /// 二维码的ticket
                /// </summary>
                public const string WMsg_Ticket = "WMsg_Ticket";

                /// <summary>
                /// 地理位置纬度
                /// </summary>
                public const string WMsg_Latitude = "WMsg_Latitude";

                /// <summary>
                /// 地理位置经度
                /// </summary>
                public const string WMsg_Longitude = "WMsg_Longitude";

                /// <summary>
                /// 地理位置精度
                /// </summary>
                public const string WMsg_Precision = "WMsg_Precision";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WMsg_IsValid = "WMsg_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WMsg_CreatedBy = "WMsg_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WMsg_CreatedTime = "WMsg_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WMsg_UpdatedBy = "WMsg_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WMsg_UpdatedTime = "WMsg_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WMsg_VersionNo = "WMsg_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WMsg_TransID = "WMsg_TransID";

            }

        }
        /// <summary>
        /// 微信用户
        /// </summary>
        public class WC_User
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string WU_ID = "ID";

                /// <summary>
                /// 用户的标识
                /// </summary>
                public const string WU_OpenID = "用户的标识";

                /// <summary>
                /// 昵称
                /// </summary>
                public const string WU_NickName = "昵称";

                /// <summary>
                /// 性别
                /// </summary>
                public const string WU_Sex = "性别";

                /// <summary>
                /// 用户所在城市
                /// </summary>
                public const string WU_City = "用户所在城市";

                /// <summary>
                /// 用户所在国家
                /// </summary>
                public const string WU_Country = "用户所在国家";

                /// <summary>
                /// 用户所在省份
                /// </summary>
                public const string WU_Province = "用户所在省份";

                /// <summary>
                /// 用户的语言
                /// </summary>
                public const string WU_Language = "用户的语言";

                /// <summary>
                /// 用户头像
                /// </summary>
                public const string WU_HeadImgURL = "用户头像";

                /// <summary>
                /// 微信用户关注公众号的标识
                /// </summary>
                public const string WU_Subscribe = "微信用户关注公众号的标识";

                /// <summary>
                /// 用户关注时间戳
                /// </summary>
                public const string WU_SubscribeTime = "用户关注时间戳";

                /// <summary>
                /// 开放平台ID
                /// </summary>
                public const string WU_UnionID = "开放平台ID";

                /// <summary>
                /// 公众号运营者对粉丝的备注
                /// </summary>
                public const string WU_Remark = "公众号运营者对粉丝的备注";

                /// <summary>
                /// 用户所在的分组ID
                /// </summary>
                public const string WU_GroupID = "用户所在的分组ID";

                /// <summary>
                /// 用户特权信息
                /// </summary>
                public const string WU_Privilege = "用户特权信息";

                /// <summary>
                /// 最近一次关注时间
                /// </summary>
                public const string WU_SubscribeTimeNew = "最近一次关注时间";

                /// <summary>
                /// 最近一次取消关注时间
                /// </summary>
                public const string WU_UnSubscribeTimeNew = "最近一次取消关注时间";

                /// <summary>
                /// 最近一次关注的来源组织
                /// </summary>
                public const string WU_Org_ID = "最近一次关注的来源组织";

                /// <summary>
                /// 最近一次访问时间
                /// </summary>
                public const string WU_LastVisitTime = "最近一次访问时间";

                /// <summary>
                /// 最近一次经度
                /// </summary>
                public const string WU_Latitude = "最近一次经度";

                /// <summary>
                /// 最近一次经度
                /// </summary>
                public const string WU_Longitude = "最近一次经度";

                /// <summary>
                /// 最近一次坐标更新时间
                /// </summary>
                public const string WU_LocationUpdTime = "最近一次坐标更新时间";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WU_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WU_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WU_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WU_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WU_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WU_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WU_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string WU_ID = "WU_ID";

                /// <summary>
                /// 用户的标识
                /// </summary>
                public const string WU_OpenID = "WU_OpenID";

                /// <summary>
                /// 昵称
                /// </summary>
                public const string WU_NickName = "WU_NickName";

                /// <summary>
                /// 性别
                /// </summary>
                public const string WU_Sex = "WU_Sex";

                /// <summary>
                /// 用户所在城市
                /// </summary>
                public const string WU_City = "WU_City";

                /// <summary>
                /// 用户所在国家
                /// </summary>
                public const string WU_Country = "WU_Country";

                /// <summary>
                /// 用户所在省份
                /// </summary>
                public const string WU_Province = "WU_Province";

                /// <summary>
                /// 用户的语言
                /// </summary>
                public const string WU_Language = "WU_Language";

                /// <summary>
                /// 用户头像
                /// </summary>
                public const string WU_HeadImgURL = "WU_HeadImgURL";

                /// <summary>
                /// 微信用户关注公众号的标识
                /// </summary>
                public const string WU_Subscribe = "WU_Subscribe";

                /// <summary>
                /// 用户关注时间戳
                /// </summary>
                public const string WU_SubscribeTime = "WU_SubscribeTime";

                /// <summary>
                /// 开放平台ID
                /// </summary>
                public const string WU_UnionID = "WU_UnionID";

                /// <summary>
                /// 公众号运营者对粉丝的备注
                /// </summary>
                public const string WU_Remark = "WU_Remark";

                /// <summary>
                /// 用户所在的分组ID
                /// </summary>
                public const string WU_GroupID = "WU_GroupID";

                /// <summary>
                /// 用户特权信息
                /// </summary>
                public const string WU_Privilege = "WU_Privilege";

                /// <summary>
                /// 最近一次关注时间
                /// </summary>
                public const string WU_SubscribeTimeNew = "WU_SubscribeTimeNew";

                /// <summary>
                /// 最近一次取消关注时间
                /// </summary>
                public const string WU_UnSubscribeTimeNew = "WU_UnSubscribeTimeNew";

                /// <summary>
                /// 最近一次关注的来源组织
                /// </summary>
                public const string WU_Org_ID = "WU_Org_ID";

                /// <summary>
                /// 最近一次访问时间
                /// </summary>
                public const string WU_LastVisitTime = "WU_LastVisitTime";

                /// <summary>
                /// 最近一次经度
                /// </summary>
                public const string WU_Latitude = "WU_Latitude";

                /// <summary>
                /// 最近一次经度
                /// </summary>
                public const string WU_Longitude = "WU_Longitude";

                /// <summary>
                /// 最近一次坐标更新时间
                /// </summary>
                public const string WU_LocationUpdTime = "WU_LocationUpdTime";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WU_IsValid = "WU_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WU_CreatedBy = "WU_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WU_CreatedTime = "WU_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WU_UpdatedBy = "WU_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WU_UpdatedTime = "WU_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WU_VersionNo = "WU_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WU_TransID = "WU_TransID";

            }

        }
        /// <summary>
        /// 微信用户明细
        /// </summary>
        public class WC_UserDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 微信用户明细ID
                /// </summary>
                public const string WUD_ID = "微信用户明细ID";

                /// <summary>
                /// 开放平台ID
                /// </summary>
                public const string WUD_UnionID = "开放平台ID";

                /// <summary>
                /// OpenID
                /// </summary>
                public const string WUD_OpenID = "OpenID";

                /// <summary>
                /// 用户类型
                /// </summary>
                public const string WUD_Type = "用户类型";

                /// <summary>
                /// 认证时间
                /// </summary>
                public const string WUD_CertificationTime = "认证时间";

                /// <summary>
                /// 认证标识
                /// </summary>
                public const string WUD_Mark = "认证标识";

                /// <summary>
                /// 用户名称
                /// </summary>
                public const string WUD_Name = "用户名称";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string WUD_UserID = "用户ID";

                /// <summary>
                /// 是否商户管理者
                /// </summary>
                public const string WUD_IsManager = "是否商户管理者";

                /// <summary>
                /// 是否允许当前商户被多次认证
                /// </summary>
                public const string WUD_AllowMultipleCertificate = "是否允许当前商户被多次认证";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WUD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WUD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WUD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WUD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WUD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WUD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WUD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 微信用户明细ID
                /// </summary>
                public const string WUD_ID = "WUD_ID";

                /// <summary>
                /// 开放平台ID
                /// </summary>
                public const string WUD_UnionID = "WUD_UnionID";

                /// <summary>
                /// OpenID
                /// </summary>
                public const string WUD_OpenID = "WUD_OpenID";

                /// <summary>
                /// 用户类型
                /// </summary>
                public const string WUD_Type = "WUD_Type";

                /// <summary>
                /// 认证时间
                /// </summary>
                public const string WUD_CertificationTime = "WUD_CertificationTime";

                /// <summary>
                /// 认证标识
                /// </summary>
                public const string WUD_Mark = "WUD_Mark";

                /// <summary>
                /// 用户名称
                /// </summary>
                public const string WUD_Name = "WUD_Name";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string WUD_UserID = "WUD_UserID";

                /// <summary>
                /// 是否商户管理者
                /// </summary>
                public const string WUD_IsManager = "WUD_IsManager";

                /// <summary>
                /// 是否允许当前商户被多次认证
                /// </summary>
                public const string WUD_AllowMultipleCertificate = "WUD_AllowMultipleCertificate";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WUD_IsValid = "WUD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WUD_CreatedBy = "WUD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WUD_CreatedTime = "WUD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WUD_UpdatedBy = "WUD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WUD_UpdatedTime = "WUD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WUD_VersionNo = "WUD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WUD_TransID = "WUD_TransID";

            }

        }
        /// <summary>
        /// 微信用户明细异动
        /// </summary>
        public class WC_UserDetailTrans
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 明细异动ID
                /// </summary>
                public const string WUDT_ID = "明细异动ID";

                /// <summary>
                /// 开放平台ID
                /// </summary>
                public const string WUDT_UnionID = "开放平台ID";

                /// <summary>
                /// OpenID
                /// </summary>
                public const string WUDT_OpenID = "OpenID";

                /// <summary>
                /// 认证标识
                /// </summary>
                public const string WUDT_Mark = "认证标识";

                /// <summary>
                /// 用户名称
                /// </summary>
                public const string WUDT_Name = "用户名称";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string WUDT_UserID = "用户ID";

                /// <summary>
                /// 异动类型
                /// </summary>
                public const string WUDT_Type = "异动类型";

                /// <summary>
                /// 认证类型
                /// </summary>
                public const string WUDT_CertificationType = "认证类型";

                /// <summary>
                /// 异动时间
                /// </summary>
                public const string WUDT_Time = "异动时间";

                /// <summary>
                /// 备注
                /// </summary>
                public const string WUDT_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WUDT_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WUDT_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WUDT_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WUDT_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WUDT_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WUDT_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WUDT_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 明细异动ID
                /// </summary>
                public const string WUDT_ID = "WUDT_ID";

                /// <summary>
                /// 开放平台ID
                /// </summary>
                public const string WUDT_UnionID = "WUDT_UnionID";

                /// <summary>
                /// OpenID
                /// </summary>
                public const string WUDT_OpenID = "WUDT_OpenID";

                /// <summary>
                /// 认证标识
                /// </summary>
                public const string WUDT_Mark = "WUDT_Mark";

                /// <summary>
                /// 用户名称
                /// </summary>
                public const string WUDT_Name = "WUDT_Name";

                /// <summary>
                /// 用户ID
                /// </summary>
                public const string WUDT_UserID = "WUDT_UserID";

                /// <summary>
                /// 异动类型
                /// </summary>
                public const string WUDT_Type = "WUDT_Type";

                /// <summary>
                /// 认证类型
                /// </summary>
                public const string WUDT_CertificationType = "WUDT_CertificationType";

                /// <summary>
                /// 异动时间
                /// </summary>
                public const string WUDT_Time = "WUDT_Time";

                /// <summary>
                /// 备注
                /// </summary>
                public const string WUDT_Remark = "WUDT_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WUDT_IsValid = "WUDT_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WUDT_CreatedBy = "WUDT_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WUDT_CreatedTime = "WUDT_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WUDT_UpdatedBy = "WUDT_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WUDT_UpdatedTime = "WUDT_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WUDT_VersionNo = "WUDT_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WUDT_TransID = "WUDT_TransID";

            }

        }
        /// <summary>
        /// 微信用户关注明细
        /// </summary>
        public class WC_UserSubscribeTrans
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 微信用户关注明细ID
                /// </summary>
                public const string WUST_ID = "微信用户关注明细ID";

                /// <summary>
                /// 开放平台ID
                /// </summary>
                public const string WUST_UnionID = "开放平台ID";

                /// <summary>
                /// OpenID
                /// </summary>
                public const string WUST_OpenID = "OpenID";

                /// <summary>
                /// 事件
                /// </summary>
                public const string WUST_Event = "事件";

                /// <summary>
                /// 异动时间
                /// </summary>
                public const string WUST_Time = "异动时间";

                /// <summary>
                /// 备注
                /// </summary>
                public const string WUST_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WUST_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WUST_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WUST_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WUST_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WUST_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WUST_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WUST_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 微信用户关注明细ID
                /// </summary>
                public const string WUST_ID = "WUST_ID";

                /// <summary>
                /// 开放平台ID
                /// </summary>
                public const string WUST_UnionID = "WUST_UnionID";

                /// <summary>
                /// OpenID
                /// </summary>
                public const string WUST_OpenID = "WUST_OpenID";

                /// <summary>
                /// 事件
                /// </summary>
                public const string WUST_Event = "WUST_Event";

                /// <summary>
                /// 异动时间
                /// </summary>
                public const string WUST_Time = "WUST_Time";

                /// <summary>
                /// 备注
                /// </summary>
                public const string WUST_Remark = "WUST_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WUST_IsValid = "WUST_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WUST_CreatedBy = "WUST_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WUST_CreatedTime = "WUST_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WUST_UpdatedBy = "WUST_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WUST_UpdatedTime = "WUST_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WUST_VersionNo = "WUST_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WUST_TransID = "WUST_TransID";

            }

        }
        /// <summary>
        /// 物流订单
        /// </summary>
        public class SD_LogisticsBill
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 物流订单ID
                /// </summary>
                public const string LB_ID = "物流订单ID";

                /// <summary>
                /// 物流单号
                /// </summary>
                public const string LB_No = "物流单号";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string LB_Org_ID = "组织ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string LB_Org_Name = "组织名称";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string LB_SourceTypeCode = "来源类型编码";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string LB_SourceTypeName = "来源类型名称";

                /// <summary>
                /// 物流单来源单号
                /// </summary>
                public const string LB_SourceNo = "物流单来源单号";

                /// <summary>
                /// 物流人员类型编码
                /// </summary>
                public const string LB_SourceCode = "物流人员类型编码";

                /// <summary>
                /// 物流人员类型名称
                /// </summary>
                public const string LB_SourceName = "物流人员类型名称";

                /// <summary>
                /// 物流人员ID
                /// </summary>
                public const string LB_DeliveryByID = "物流人员ID";

                /// <summary>
                /// 物流人员名称
                /// </summary>
                public const string LB_DeliveryBy = "物流人员名称";

                /// <summary>
                /// 物流人员手机号
                /// </summary>
                public const string LB_PhoneNo = "物流人员手机号";

                /// <summary>
                /// 物流人员接单时间
                /// </summary>
                public const string LB_AcceptTime = "物流人员接单时间";

                /// <summary>
                /// 物流人员接单图片路径1
                /// </summary>
                public const string LB_AcceptPicPath1 = "物流人员接单图片路径1";

                /// <summary>
                /// 物流人员接单图片路径2
                /// </summary>
                public const string LB_AcceptPicPath2 = "物流人员接单图片路径2";

                /// <summary>
                /// 收件人姓名
                /// </summary>
                public const string LB_Receiver = "收件人姓名";

                /// <summary>
                /// 收件人地址
                /// </summary>
                public const string LB_ReceiverAddress = "收件人地址";

                /// <summary>
                /// 收件人邮编
                /// </summary>
                public const string LB_ReceiverPostcode = "收件人邮编";

                /// <summary>
                /// 收件人手机号
                /// </summary>
                public const string LB_ReceiverPhoneNo = "收件人手机号";

                /// <summary>
                /// 签收人
                /// </summary>
                public const string LB_ReceivedBy = "签收人";

                /// <summary>
                /// 签收时间
                /// </summary>
                public const string LB_ReceivedTime = "签收时间";

                /// <summary>
                /// 签收拍照图片路径1
                /// </summary>
                public const string LB_ReceivedPicPath1 = "签收拍照图片路径1";

                /// <summary>
                /// 签收拍照图片路径2
                /// </summary>
                public const string LB_ReceivedPicPath2 = "签收拍照图片路径2";

                /// <summary>
                /// 物流费
                /// </summary>
                public const string LB_Fee = "物流费";

                /// <summary>
                /// 赔偿金
                /// </summary>
                public const string LB_Indemnification = "赔偿金";

                /// <summary>
                /// 应收金额
                /// </summary>
                public const string LB_AccountReceivableAmount = "应收金额";

                /// <summary>
                /// 物流费付款状态编码
                /// </summary>
                public const string LB_PayStautsCode = "物流费付款状态编码";

                /// <summary>
                /// 物流费付款状态名称
                /// </summary>
                public const string LB_PayStautsName = "物流费付款状态名称";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string LB_StatusCode = "单据状态编码";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string LB_StatusName = "单据状态名称";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string LB_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string LB_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string LB_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string LB_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string LB_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string LB_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string LB_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string LB_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string LB_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string LB_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 物流订单ID
                /// </summary>
                public const string LB_ID = "LB_ID";

                /// <summary>
                /// 物流单号
                /// </summary>
                public const string LB_No = "LB_No";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string LB_Org_ID = "LB_Org_ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string LB_Org_Name = "LB_Org_Name";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string LB_SourceTypeCode = "LB_SourceTypeCode";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string LB_SourceTypeName = "LB_SourceTypeName";

                /// <summary>
                /// 物流单来源单号
                /// </summary>
                public const string LB_SourceNo = "LB_SourceNo";

                /// <summary>
                /// 物流人员类型编码
                /// </summary>
                public const string LB_SourceCode = "LB_SourceCode";

                /// <summary>
                /// 物流人员类型名称
                /// </summary>
                public const string LB_SourceName = "LB_SourceName";

                /// <summary>
                /// 物流人员ID
                /// </summary>
                public const string LB_DeliveryByID = "LB_DeliveryByID";

                /// <summary>
                /// 物流人员名称
                /// </summary>
                public const string LB_DeliveryBy = "LB_DeliveryBy";

                /// <summary>
                /// 物流人员手机号
                /// </summary>
                public const string LB_PhoneNo = "LB_PhoneNo";

                /// <summary>
                /// 物流人员接单时间
                /// </summary>
                public const string LB_AcceptTime = "LB_AcceptTime";

                /// <summary>
                /// 物流人员接单图片路径1
                /// </summary>
                public const string LB_AcceptPicPath1 = "LB_AcceptPicPath1";

                /// <summary>
                /// 物流人员接单图片路径2
                /// </summary>
                public const string LB_AcceptPicPath2 = "LB_AcceptPicPath2";

                /// <summary>
                /// 收件人姓名
                /// </summary>
                public const string LB_Receiver = "LB_Receiver";

                /// <summary>
                /// 收件人地址
                /// </summary>
                public const string LB_ReceiverAddress = "LB_ReceiverAddress";

                /// <summary>
                /// 收件人邮编
                /// </summary>
                public const string LB_ReceiverPostcode = "LB_ReceiverPostcode";

                /// <summary>
                /// 收件人手机号
                /// </summary>
                public const string LB_ReceiverPhoneNo = "LB_ReceiverPhoneNo";

                /// <summary>
                /// 签收人
                /// </summary>
                public const string LB_ReceivedBy = "LB_ReceivedBy";

                /// <summary>
                /// 签收时间
                /// </summary>
                public const string LB_ReceivedTime = "LB_ReceivedTime";

                /// <summary>
                /// 签收拍照图片路径1
                /// </summary>
                public const string LB_ReceivedPicPath1 = "LB_ReceivedPicPath1";

                /// <summary>
                /// 签收拍照图片路径2
                /// </summary>
                public const string LB_ReceivedPicPath2 = "LB_ReceivedPicPath2";

                /// <summary>
                /// 物流费
                /// </summary>
                public const string LB_Fee = "LB_Fee";

                /// <summary>
                /// 赔偿金
                /// </summary>
                public const string LB_Indemnification = "LB_Indemnification";

                /// <summary>
                /// 应收金额
                /// </summary>
                public const string LB_AccountReceivableAmount = "LB_AccountReceivableAmount";

                /// <summary>
                /// 物流费付款状态编码
                /// </summary>
                public const string LB_PayStautsCode = "LB_PayStautsCode";

                /// <summary>
                /// 物流费付款状态名称
                /// </summary>
                public const string LB_PayStautsName = "LB_PayStautsName";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string LB_StatusCode = "LB_StatusCode";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string LB_StatusName = "LB_StatusName";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string LB_ApprovalStatusCode = "LB_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string LB_ApprovalStatusName = "LB_ApprovalStatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string LB_Remark = "LB_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string LB_IsValid = "LB_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string LB_CreatedBy = "LB_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string LB_CreatedTime = "LB_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string LB_UpdatedBy = "LB_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string LB_UpdatedTime = "LB_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string LB_VersionNo = "LB_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string LB_TransID = "LB_TransID";

            }

        }
        /// <summary>
        /// 物流订单明细
        /// </summary>
        public class SD_LogisticsBillDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 物流订单明细ID
                /// </summary>
                public const string LBD_ID = "物流订单明细ID";

                /// <summary>
                /// 物流订单ID
                /// </summary>
                public const string LBD_LB_ID = "物流订单ID";

                /// <summary>
                /// 物流单号
                /// </summary>
                public const string LBD_LB_No = "物流单号";

                /// <summary>
                /// 条码
                /// </summary>
                public const string LBD_Barcode = "条码";

                /// <summary>
                /// 配件批次号（原库存批次）
                /// </summary>
                public const string LBD_BatchNo = "配件批次号（原库存批次）";

                /// <summary>
                /// 配件批次号（汽修厂用）
                /// </summary>
                public const string LBD_BatchNoNew = "配件批次号（汽修厂用）";

                /// <summary>
                /// 名称
                /// </summary>
                public const string LBD_Name = "名称";

                /// <summary>
                /// 规格型号
                /// </summary>
                public const string LBD_Specification = "规格型号";

                /// <summary>
                /// 单位
                /// </summary>
                public const string LBD_UOM = "单位";

                /// <summary>
                /// 配送数量
                /// </summary>
                public const string LBD_DeliveryQty = "配送数量";

                /// <summary>
                /// 签收数量
                /// </summary>
                public const string LBD_SignQty = "签收数量";

                /// <summary>
                /// 拒收数量
                /// </summary>
                public const string LBD_RejectQty = "拒收数量";

                /// <summary>
                /// 丢失数量
                /// </summary>
                public const string LBD_LoseQty = "丢失数量";

                /// <summary>
                /// 赔偿金
                /// </summary>
                public const string LBD_Indemnification = "赔偿金";

                /// <summary>
                /// 应收金额
                /// </summary>
                public const string LBD_AccountReceivableAmount = "应收金额";

                /// <summary>
                /// 状态编码
                /// </summary>
                public const string LBD_StatusCode = "状态编码";

                /// <summary>
                /// 状态名称
                /// </summary>
                public const string LBD_StatusName = "状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string LBD_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string LBD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string LBD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string LBD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string LBD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string LBD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string LBD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string LBD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 物流订单明细ID
                /// </summary>
                public const string LBD_ID = "LBD_ID";

                /// <summary>
                /// 物流订单ID
                /// </summary>
                public const string LBD_LB_ID = "LBD_LB_ID";

                /// <summary>
                /// 物流单号
                /// </summary>
                public const string LBD_LB_No = "LBD_LB_No";

                /// <summary>
                /// 条码
                /// </summary>
                public const string LBD_Barcode = "LBD_Barcode";

                /// <summary>
                /// 配件批次号（原库存批次）
                /// </summary>
                public const string LBD_BatchNo = "LBD_BatchNo";

                /// <summary>
                /// 配件批次号（汽修厂用）
                /// </summary>
                public const string LBD_BatchNoNew = "LBD_BatchNoNew";

                /// <summary>
                /// 名称
                /// </summary>
                public const string LBD_Name = "LBD_Name";

                /// <summary>
                /// 规格型号
                /// </summary>
                public const string LBD_Specification = "LBD_Specification";

                /// <summary>
                /// 单位
                /// </summary>
                public const string LBD_UOM = "LBD_UOM";

                /// <summary>
                /// 配送数量
                /// </summary>
                public const string LBD_DeliveryQty = "LBD_DeliveryQty";

                /// <summary>
                /// 签收数量
                /// </summary>
                public const string LBD_SignQty = "LBD_SignQty";

                /// <summary>
                /// 拒收数量
                /// </summary>
                public const string LBD_RejectQty = "LBD_RejectQty";

                /// <summary>
                /// 丢失数量
                /// </summary>
                public const string LBD_LoseQty = "LBD_LoseQty";

                /// <summary>
                /// 赔偿金
                /// </summary>
                public const string LBD_Indemnification = "LBD_Indemnification";

                /// <summary>
                /// 应收金额
                /// </summary>
                public const string LBD_AccountReceivableAmount = "LBD_AccountReceivableAmount";

                /// <summary>
                /// 状态编码
                /// </summary>
                public const string LBD_StatusCode = "LBD_StatusCode";

                /// <summary>
                /// 状态名称
                /// </summary>
                public const string LBD_StatusName = "LBD_StatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string LBD_Remark = "LBD_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string LBD_IsValid = "LBD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string LBD_CreatedBy = "LBD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string LBD_CreatedTime = "LBD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string LBD_UpdatedBy = "LBD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string LBD_UpdatedTime = "LBD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string LBD_VersionNo = "LBD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string LBD_TransID = "LBD_TransID";

            }

        }
        /// <summary>
        /// 物流订单异动
        /// </summary>
        public class SD_LogisticsBillTrans
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 物流订单异动ID
                /// </summary>
                public const string LBT_ID = "物流订单异动ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string LBT_Org_ID = "组织ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string LBT_Org_Name = "组织名称";

                /// <summary>
                /// 物流订单ID
                /// </summary>
                public const string LBT_LB_ID = "物流订单ID";

                /// <summary>
                /// 单号
                /// </summary>
                public const string LBT_LB_NO = "单号";

                /// <summary>
                /// 异动时间
                /// </summary>
                public const string LBT_Time = "异动时间";

                /// <summary>
                /// 异动状态
                /// </summary>
                public const string LBT_Status = "异动状态";

                /// <summary>
                /// 备注
                /// </summary>
                public const string LBT_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string LBT_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string LBT_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string LBT_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string LBT_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string LBT_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string LBT_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string LBT_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 物流订单异动ID
                /// </summary>
                public const string LBT_ID = "LBT_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string LBT_Org_ID = "LBT_Org_ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string LBT_Org_Name = "LBT_Org_Name";

                /// <summary>
                /// 物流订单ID
                /// </summary>
                public const string LBT_LB_ID = "LBT_LB_ID";

                /// <summary>
                /// 单号
                /// </summary>
                public const string LBT_LB_NO = "LBT_LB_NO";

                /// <summary>
                /// 异动时间
                /// </summary>
                public const string LBT_Time = "LBT_Time";

                /// <summary>
                /// 异动状态
                /// </summary>
                public const string LBT_Status = "LBT_Status";

                /// <summary>
                /// 备注
                /// </summary>
                public const string LBT_Remark = "LBT_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string LBT_IsValid = "LBT_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string LBT_CreatedBy = "LBT_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string LBT_CreatedTime = "LBT_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string LBT_UpdatedBy = "LBT_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string LBT_UpdatedTime = "LBT_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string LBT_VersionNo = "LBT_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string LBT_TransID = "LBT_TransID";

            }

        }
        /// <summary>
        /// 销售订单
        /// </summary>
        public class SD_SalesOrder
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 销售订单ID
                /// </summary>
                public const string SO_ID = "销售订单ID";

                /// <summary>
                /// 单据编号
                /// </summary>
                public const string SO_No = "单据编号";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string SO_Org_ID = "组织ID";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string SO_SourceTypeCode = "来源类型编码";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string SO_SourceTypeName = "来源类型名称";

                /// <summary>
                /// 来源单号
                /// </summary>
                public const string SO_SourceNo = "来源单号";

                /// <summary>
                /// 客户类型编码
                /// </summary>
                public const string SO_CustomerTypeCode = "客户类型编码";

                /// <summary>
                /// 客户类型名称
                /// </summary>
                public const string SO_CustomerTypeName = "客户类型名称";

                /// <summary>
                /// 客户ID
                /// </summary>
                public const string SO_CustomerID = "客户ID";

                /// <summary>
                /// 客户名称
                /// </summary>
                public const string SO_CustomerName = "客户名称";

                /// <summary>
                /// 是否价格含税
                /// </summary>
                public const string SO_IsPriceIncludeTax = "是否价格含税";

                /// <summary>
                /// 税率
                /// </summary>
                public const string SO_TaxRate = "税率";

                /// <summary>
                /// 税额
                /// </summary>
                public const string SO_TotalTax = "税额";

                /// <summary>
                /// 总金额
                /// </summary>
                public const string SO_TotalAmount = "总金额";

                /// <summary>
                /// 未税总金额
                /// </summary>
                public const string SO_TotalNetAmount = "未税总金额";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string SO_StatusCode = "单据状态编码";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string SO_StatusName = "单据状态名称";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string SO_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string SO_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 配件价格类别
                /// </summary>
                public const string SO_AutoPartsPriceType = "配件价格类别";

                /// <summary>
                /// 业务员ID
                /// </summary>
                public const string SO_SalesByID = "业务员ID";

                /// <summary>
                /// 业务员名称
                /// </summary>
                public const string SO_SalesByName = "业务员名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SO_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SO_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SO_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SO_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SO_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SO_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SO_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SO_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 销售订单ID
                /// </summary>
                public const string SO_ID = "SO_ID";

                /// <summary>
                /// 单据编号
                /// </summary>
                public const string SO_No = "SO_No";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string SO_Org_ID = "SO_Org_ID";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string SO_SourceTypeCode = "SO_SourceTypeCode";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string SO_SourceTypeName = "SO_SourceTypeName";

                /// <summary>
                /// 来源单号
                /// </summary>
                public const string SO_SourceNo = "SO_SourceNo";

                /// <summary>
                /// 客户类型编码
                /// </summary>
                public const string SO_CustomerTypeCode = "SO_CustomerTypeCode";

                /// <summary>
                /// 客户类型名称
                /// </summary>
                public const string SO_CustomerTypeName = "SO_CustomerTypeName";

                /// <summary>
                /// 客户ID
                /// </summary>
                public const string SO_CustomerID = "SO_CustomerID";

                /// <summary>
                /// 客户名称
                /// </summary>
                public const string SO_CustomerName = "SO_CustomerName";

                /// <summary>
                /// 是否价格含税
                /// </summary>
                public const string SO_IsPriceIncludeTax = "SO_IsPriceIncludeTax";

                /// <summary>
                /// 税率
                /// </summary>
                public const string SO_TaxRate = "SO_TaxRate";

                /// <summary>
                /// 税额
                /// </summary>
                public const string SO_TotalTax = "SO_TotalTax";

                /// <summary>
                /// 总金额
                /// </summary>
                public const string SO_TotalAmount = "SO_TotalAmount";

                /// <summary>
                /// 未税总金额
                /// </summary>
                public const string SO_TotalNetAmount = "SO_TotalNetAmount";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string SO_StatusCode = "SO_StatusCode";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string SO_StatusName = "SO_StatusName";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string SO_ApprovalStatusCode = "SO_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string SO_ApprovalStatusName = "SO_ApprovalStatusName";

                /// <summary>
                /// 配件价格类别
                /// </summary>
                public const string SO_AutoPartsPriceType = "SO_AutoPartsPriceType";

                /// <summary>
                /// 业务员ID
                /// </summary>
                public const string SO_SalesByID = "SO_SalesByID";

                /// <summary>
                /// 业务员名称
                /// </summary>
                public const string SO_SalesByName = "SO_SalesByName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SO_Remark = "SO_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SO_IsValid = "SO_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SO_CreatedBy = "SO_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SO_CreatedTime = "SO_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SO_UpdatedBy = "SO_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SO_UpdatedTime = "SO_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SO_VersionNo = "SO_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SO_TransID = "SO_TransID";

            }

        }
        /// <summary>
        /// 销售订单明细
        /// </summary>
        public class SD_SalesOrderDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 销售订单明细ID
                /// </summary>
                public const string SOD_ID = "销售订单明细ID";

                /// <summary>
                /// 销售订单ID
                /// </summary>
                public const string SOD_SO_ID = "销售订单ID";

                /// <summary>
                /// 计价基准
                /// </summary>
                public const string SOD_SalePriceRate = "计价基准";

                /// <summary>
                /// 计价基准可改
                /// </summary>
                public const string SOD_SalePriceRateIsChangeable = "计价基准可改";

                /// <summary>
                /// 价格是否含税
                /// </summary>
                public const string SOD_PriceIsIncludeTax = "价格是否含税";

                /// <summary>
                /// 税率
                /// </summary>
                public const string SOD_TaxRate = "税率";

                /// <summary>
                /// 税额
                /// </summary>
                public const string SOD_TotalTax = "税额";

                /// <summary>
                /// 销售数量
                /// </summary>
                public const string SOD_Qty = "销售数量";

                /// <summary>
                /// 签收数量
                /// </summary>
                public const string SOD_SignQty = "签收数量";

                /// <summary>
                /// 拒收数量
                /// </summary>
                public const string SOD_RejectQty = "拒收数量";

                /// <summary>
                /// 丢失数量
                /// </summary>
                public const string SOD_LoseQty = "丢失数量";

                /// <summary>
                /// 单价
                /// </summary>
                public const string SOD_UnitPrice = "单价";

                /// <summary>
                /// 总金额
                /// </summary>
                public const string SOD_TotalAmount = "总金额";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string SOD_Barcode = "配件条码";

                /// <summary>
                /// 配件批次号（原库存批次）
                /// </summary>
                public const string SOD_BatchNo = "配件批次号（原库存批次）";

                /// <summary>
                /// 配件批次号（汽修厂用）
                /// </summary>
                public const string SOD_BatchNoNew = "配件批次号（汽修厂用）";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string SOD_Name = "配件名称";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string SOD_Specification = "配件规格型号";

                /// <summary>
                /// 单位
                /// </summary>
                public const string SOD_UOM = "单位";

                /// <summary>
                /// 入库组织ID
                /// </summary>
                public const string SOD_StockInOrgID = "入库组织ID";

                /// <summary>
                /// 入库组织编码
                /// </summary>
                public const string SOD_StockInOrgCode = "入库组织编码";

                /// <summary>
                /// 入库组织名称
                /// </summary>
                public const string SOD_StockInOrgName = "入库组织名称";

                /// <summary>
                /// 入库仓库ID
                /// </summary>
                public const string SOD_StockInWarehouseID = "入库仓库ID";

                /// <summary>
                /// 入库仓库名称
                /// </summary>
                public const string SOD_StockInWarehouseName = "入库仓库名称";

                /// <summary>
                /// 入库仓位ID
                /// </summary>
                public const string SOD_StockInBinID = "入库仓位ID";

                /// <summary>
                /// 入库仓位名称
                /// </summary>
                public const string SOD_StockInBinName = "入库仓位名称";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string SOD_StatusCode = "单据状态编码";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string SOD_StatusName = "单据状态名称";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string SOD_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string SOD_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SOD_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SOD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SOD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SOD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SOD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SOD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SOD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SOD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 销售订单明细ID
                /// </summary>
                public const string SOD_ID = "SOD_ID";

                /// <summary>
                /// 销售订单ID
                /// </summary>
                public const string SOD_SO_ID = "SOD_SO_ID";

                /// <summary>
                /// 计价基准
                /// </summary>
                public const string SOD_SalePriceRate = "SOD_SalePriceRate";

                /// <summary>
                /// 计价基准可改
                /// </summary>
                public const string SOD_SalePriceRateIsChangeable = "SOD_SalePriceRateIsChangeable";

                /// <summary>
                /// 价格是否含税
                /// </summary>
                public const string SOD_PriceIsIncludeTax = "SOD_PriceIsIncludeTax";

                /// <summary>
                /// 税率
                /// </summary>
                public const string SOD_TaxRate = "SOD_TaxRate";

                /// <summary>
                /// 税额
                /// </summary>
                public const string SOD_TotalTax = "SOD_TotalTax";

                /// <summary>
                /// 销售数量
                /// </summary>
                public const string SOD_Qty = "SOD_Qty";

                /// <summary>
                /// 签收数量
                /// </summary>
                public const string SOD_SignQty = "SOD_SignQty";

                /// <summary>
                /// 拒收数量
                /// </summary>
                public const string SOD_RejectQty = "SOD_RejectQty";

                /// <summary>
                /// 丢失数量
                /// </summary>
                public const string SOD_LoseQty = "SOD_LoseQty";

                /// <summary>
                /// 单价
                /// </summary>
                public const string SOD_UnitPrice = "SOD_UnitPrice";

                /// <summary>
                /// 总金额
                /// </summary>
                public const string SOD_TotalAmount = "SOD_TotalAmount";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string SOD_Barcode = "SOD_Barcode";

                /// <summary>
                /// 配件批次号（原库存批次）
                /// </summary>
                public const string SOD_BatchNo = "SOD_BatchNo";

                /// <summary>
                /// 配件批次号（汽修厂用）
                /// </summary>
                public const string SOD_BatchNoNew = "SOD_BatchNoNew";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string SOD_Name = "SOD_Name";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string SOD_Specification = "SOD_Specification";

                /// <summary>
                /// 单位
                /// </summary>
                public const string SOD_UOM = "SOD_UOM";

                /// <summary>
                /// 入库组织ID
                /// </summary>
                public const string SOD_StockInOrgID = "SOD_StockInOrgID";

                /// <summary>
                /// 入库组织编码
                /// </summary>
                public const string SOD_StockInOrgCode = "SOD_StockInOrgCode";

                /// <summary>
                /// 入库组织名称
                /// </summary>
                public const string SOD_StockInOrgName = "SOD_StockInOrgName";

                /// <summary>
                /// 入库仓库ID
                /// </summary>
                public const string SOD_StockInWarehouseID = "SOD_StockInWarehouseID";

                /// <summary>
                /// 入库仓库名称
                /// </summary>
                public const string SOD_StockInWarehouseName = "SOD_StockInWarehouseName";

                /// <summary>
                /// 入库仓位ID
                /// </summary>
                public const string SOD_StockInBinID = "SOD_StockInBinID";

                /// <summary>
                /// 入库仓位名称
                /// </summary>
                public const string SOD_StockInBinName = "SOD_StockInBinName";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string SOD_StatusCode = "SOD_StatusCode";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string SOD_StatusName = "SOD_StatusName";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string SOD_ApprovalStatusCode = "SOD_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string SOD_ApprovalStatusName = "SOD_ApprovalStatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SOD_Remark = "SOD_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SOD_IsValid = "SOD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SOD_CreatedBy = "SOD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SOD_CreatedTime = "SOD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SOD_UpdatedBy = "SOD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SOD_UpdatedTime = "SOD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SOD_VersionNo = "SOD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SOD_TransID = "SOD_TransID";

            }

        }
        /// <summary>
        /// 下发路径
        /// </summary>
        public class SD_DistributePath
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 下发路径ID
                /// </summary>
                public const string DP_ID = "下发路径ID";

                /// <summary>
                /// 来源组织
                /// </summary>
                public const string DP_Org_ID_From = "来源组织";

                /// <summary>
                /// 目标组织
                /// </summary>
                public const string DP_Org_ID_To = "目标组织";

                /// <summary>
                /// 下发人
                /// </summary>
                public const string DP_SendPerson = "下发人";

                /// <summary>
                /// 下发数据ID
                /// </summary>
                public const string DP_SendDataID = "下发数据ID";

                /// <summary>
                /// 下发数据类型编码
                /// </summary>
                public const string DP_SendDataTypeCode = "下发数据类型编码";

                /// <summary>
                /// 下发数据类型名称
                /// </summary>
                public const string DP_SendDataTypeName = "下发数据类型名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string DP_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string DP_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string DP_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string DP_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string DP_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string DP_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string DP_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string DP_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 下发路径ID
                /// </summary>
                public const string DP_ID = "DP_ID";

                /// <summary>
                /// 来源组织
                /// </summary>
                public const string DP_Org_ID_From = "DP_Org_ID_From";

                /// <summary>
                /// 目标组织
                /// </summary>
                public const string DP_Org_ID_To = "DP_Org_ID_To";

                /// <summary>
                /// 下发人
                /// </summary>
                public const string DP_SendPerson = "DP_SendPerson";

                /// <summary>
                /// 下发数据ID
                /// </summary>
                public const string DP_SendDataID = "DP_SendDataID";

                /// <summary>
                /// 下发数据类型编码
                /// </summary>
                public const string DP_SendDataTypeCode = "DP_SendDataTypeCode";

                /// <summary>
                /// 下发数据类型名称
                /// </summary>
                public const string DP_SendDataTypeName = "DP_SendDataTypeName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string DP_Remark = "DP_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string DP_IsValid = "DP_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string DP_CreatedBy = "DP_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string DP_CreatedTime = "DP_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string DP_UpdatedBy = "DP_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string DP_UpdatedTime = "DP_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string DP_VersionNo = "DP_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string DP_TransID = "DP_TransID";

            }

        }
        /// <summary>
        /// 销售模板
        /// </summary>
        public class SD_SalesTemplate
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 销售模板ID
                /// </summary>
                public const string SasT_ID = "销售模板ID";

                /// <summary>
                /// 销售模板名称
                /// </summary>
                public const string SasT_Name = "销售模板名称";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string SasT_Org_ID = "组织ID";

                /// <summary>
                /// 汽修商户编码
                /// </summary>
                public const string SasT_AutoFactoryCode = "汽修商户编码";

                /// <summary>
                /// 汽修商户名称
                /// </summary>
                public const string SasT_AutoFactoryName = "汽修商户名称";

                /// <summary>
                /// 汽修商客户ID
                /// </summary>
                public const string SasT_CustomerID = "汽修商客户ID";

                /// <summary>
                /// 汽修商客户名称
                /// </summary>
                public const string SasT_CustomerName = "汽修商客户名称";

                /// <summary>
                /// 汽修商组织编码
                /// </summary>
                public const string SasT_AutoFactoryOrgCode = "汽修商组织编码";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string SasT_ApprovalStatusCode = "审核状态编码";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string SasT_ApprovalStatusName = "审核状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SasT_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SasT_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SasT_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SasT_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SasT_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SasT_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SasT_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SasT_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 销售模板ID
                /// </summary>
                public const string SasT_ID = "SasT_ID";

                /// <summary>
                /// 销售模板名称
                /// </summary>
                public const string SasT_Name = "SasT_Name";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string SasT_Org_ID = "SasT_Org_ID";

                /// <summary>
                /// 汽修商户编码
                /// </summary>
                public const string SasT_AutoFactoryCode = "SasT_AutoFactoryCode";

                /// <summary>
                /// 汽修商户名称
                /// </summary>
                public const string SasT_AutoFactoryName = "SasT_AutoFactoryName";

                /// <summary>
                /// 汽修商客户ID
                /// </summary>
                public const string SasT_CustomerID = "SasT_CustomerID";

                /// <summary>
                /// 汽修商客户名称
                /// </summary>
                public const string SasT_CustomerName = "SasT_CustomerName";

                /// <summary>
                /// 汽修商组织编码
                /// </summary>
                public const string SasT_AutoFactoryOrgCode = "SasT_AutoFactoryOrgCode";

                /// <summary>
                /// 审核状态编码
                /// </summary>
                public const string SasT_ApprovalStatusCode = "SasT_ApprovalStatusCode";

                /// <summary>
                /// 审核状态名称
                /// </summary>
                public const string SasT_ApprovalStatusName = "SasT_ApprovalStatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SasT_Remark = "SasT_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SasT_IsValid = "SasT_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SasT_CreatedBy = "SasT_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SasT_CreatedTime = "SasT_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SasT_UpdatedBy = "SasT_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SasT_UpdatedTime = "SasT_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SasT_VersionNo = "SasT_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SasT_TransID = "SasT_TransID";

            }

        }
        /// <summary>
        /// 销售模板明细
        /// </summary>
        public class SD_SalesTemplateDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 销售模板明细ID
                /// </summary>
                public const string SasTD_ID = "销售模板明细ID";

                /// <summary>
                /// 销售模板ID
                /// </summary>
                public const string SasTD_SasT_ID = "销售模板ID";

                /// <summary>
                /// 价格是否含税
                /// </summary>
                public const string SasTD_PriceIsIncludeTax = "价格是否含税";

                /// <summary>
                /// 税率
                /// </summary>
                public const string SasTD_TaxRate = "税率";

                /// <summary>
                /// 税额
                /// </summary>
                public const string SasTD_TotalTax = "税额";

                /// <summary>
                /// 数量
                /// </summary>
                public const string SasTD_Qty = "数量";

                /// <summary>
                /// 单价
                /// </summary>
                public const string SasTD_UnitPrice = "单价";

                /// <summary>
                /// 总金额
                /// </summary>
                public const string SasTD_TotalAmount = "总金额";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string SasTD_Barcode = "配件条码";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string SasTD_Name = "配件名称";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string SasTD_Specification = "配件规格型号";

                /// <summary>
                /// 单位
                /// </summary>
                public const string SasTD_UOM = "单位";

                /// <summary>
                /// 汽修商组织ID
                /// </summary>
                public const string SasTD_AutoFactoryOrgID = "汽修商组织ID";

                /// <summary>
                /// 汽修商组织名称
                /// </summary>
                public const string SasTD_AutoFactoryOrgName = "汽修商组织名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SasTD_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SasTD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SasTD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SasTD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SasTD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SasTD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SasTD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SasTD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 销售模板明细ID
                /// </summary>
                public const string SasTD_ID = "SasTD_ID";

                /// <summary>
                /// 销售模板ID
                /// </summary>
                public const string SasTD_SasT_ID = "SasTD_SasT_ID";

                /// <summary>
                /// 价格是否含税
                /// </summary>
                public const string SasTD_PriceIsIncludeTax = "SasTD_PriceIsIncludeTax";

                /// <summary>
                /// 税率
                /// </summary>
                public const string SasTD_TaxRate = "SasTD_TaxRate";

                /// <summary>
                /// 税额
                /// </summary>
                public const string SasTD_TotalTax = "SasTD_TotalTax";

                /// <summary>
                /// 数量
                /// </summary>
                public const string SasTD_Qty = "SasTD_Qty";

                /// <summary>
                /// 单价
                /// </summary>
                public const string SasTD_UnitPrice = "SasTD_UnitPrice";

                /// <summary>
                /// 总金额
                /// </summary>
                public const string SasTD_TotalAmount = "SasTD_TotalAmount";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string SasTD_Barcode = "SasTD_Barcode";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string SasTD_Name = "SasTD_Name";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string SasTD_Specification = "SasTD_Specification";

                /// <summary>
                /// 单位
                /// </summary>
                public const string SasTD_UOM = "SasTD_UOM";

                /// <summary>
                /// 汽修商组织ID
                /// </summary>
                public const string SasTD_AutoFactoryOrgID = "SasTD_AutoFactoryOrgID";

                /// <summary>
                /// 汽修商组织名称
                /// </summary>
                public const string SasTD_AutoFactoryOrgName = "SasTD_AutoFactoryOrgName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SasTD_Remark = "SasTD_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SasTD_IsValid = "SasTD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SasTD_CreatedBy = "SasTD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SasTD_CreatedTime = "SasTD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SasTD_UpdatedBy = "SasTD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SasTD_UpdatedTime = "SasTD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SasTD_VersionNo = "SasTD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SasTD_TransID = "SasTD_TransID";

            }

        }
        /// <summary>
        /// 销售预测订单
        /// </summary>
        public class SD_SalesForecastOrder
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 销售预测订单ID
                /// </summary>
                public const string SFO_ID = "销售预测订单ID";

                /// <summary>
                /// 单据编号
                /// </summary>
                public const string SFO_No = "单据编号";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string SFO_Org_ID = "组织ID";

                /// <summary>
                /// 汽修商户编码
                /// </summary>
                public const string SFO_AutoFactoryCode = "汽修商户编码";

                /// <summary>
                /// 汽修商户名称
                /// </summary>
                public const string SFO_AutoFactoryName = "汽修商户名称";

                /// <summary>
                /// 汽修商客户ID
                /// </summary>
                public const string SFO_CustomerID = "汽修商客户ID";

                /// <summary>
                /// 汽修商客户名称
                /// </summary>
                public const string SFO_CustomerName = "汽修商客户名称";

                /// <summary>
                /// 汽修商组织编码
                /// </summary>
                public const string SFO_AutoFactoryOrgCode = "汽修商组织编码";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string SFO_SourceTypeCode = "来源类型编码";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string SFO_SourceTypeName = "来源类型名称";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string SFO_StatusCode = "单据状态编码";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string SFO_StatusName = "单据状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SFO_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SFO_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SFO_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SFO_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SFO_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SFO_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SFO_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SFO_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 销售预测订单ID
                /// </summary>
                public const string SFO_ID = "SFO_ID";

                /// <summary>
                /// 单据编号
                /// </summary>
                public const string SFO_No = "SFO_No";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string SFO_Org_ID = "SFO_Org_ID";

                /// <summary>
                /// 汽修商户编码
                /// </summary>
                public const string SFO_AutoFactoryCode = "SFO_AutoFactoryCode";

                /// <summary>
                /// 汽修商户名称
                /// </summary>
                public const string SFO_AutoFactoryName = "SFO_AutoFactoryName";

                /// <summary>
                /// 汽修商客户ID
                /// </summary>
                public const string SFO_CustomerID = "SFO_CustomerID";

                /// <summary>
                /// 汽修商客户名称
                /// </summary>
                public const string SFO_CustomerName = "SFO_CustomerName";

                /// <summary>
                /// 汽修商组织编码
                /// </summary>
                public const string SFO_AutoFactoryOrgCode = "SFO_AutoFactoryOrgCode";

                /// <summary>
                /// 来源类型编码
                /// </summary>
                public const string SFO_SourceTypeCode = "SFO_SourceTypeCode";

                /// <summary>
                /// 来源类型名称
                /// </summary>
                public const string SFO_SourceTypeName = "SFO_SourceTypeName";

                /// <summary>
                /// 单据状态编码
                /// </summary>
                public const string SFO_StatusCode = "SFO_StatusCode";

                /// <summary>
                /// 单据状态名称
                /// </summary>
                public const string SFO_StatusName = "SFO_StatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SFO_Remark = "SFO_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SFO_IsValid = "SFO_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SFO_CreatedBy = "SFO_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SFO_CreatedTime = "SFO_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SFO_UpdatedBy = "SFO_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SFO_UpdatedTime = "SFO_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SFO_VersionNo = "SFO_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SFO_TransID = "SFO_TransID";

            }

        }
        /// <summary>
        /// 销售预测订单明细
        /// </summary>
        public class SD_SalesForecastOrderDetail
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 销售预测订单明细ID
                /// </summary>
                public const string SFOD_ID = "销售预测订单明细ID";

                /// <summary>
                /// 销售预测订单ID
                /// </summary>
                public const string SFOD_ST_ID = "销售预测订单ID";

                /// <summary>
                /// 价格是否含税
                /// </summary>
                public const string SFOD_PriceIsIncludeTax = "价格是否含税";

                /// <summary>
                /// 税率
                /// </summary>
                public const string SFOD_TaxRate = "税率";

                /// <summary>
                /// 税额
                /// </summary>
                public const string SFOD_TotalTax = "税额";

                /// <summary>
                /// 数量
                /// </summary>
                public const string SFOD_Qty = "数量";

                /// <summary>
                /// 单价
                /// </summary>
                public const string SFOD_UnitPrice = "单价";

                /// <summary>
                /// 总金额
                /// </summary>
                public const string SFOD_TotalAmount = "总金额";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string SFOD_Barcode = "配件条码";

                /// <summary>
                /// 配件批次号（汽修厂用）
                /// </summary>
                public const string SFOD_BatchNoNew = "配件批次号（汽修厂用）";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string SFOD_Name = "配件名称";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string SFOD_Specification = "配件规格型号";

                /// <summary>
                /// 单位
                /// </summary>
                public const string SFOD_UOM = "单位";

                /// <summary>
                /// 汽修商组织ID
                /// </summary>
                public const string SFOD_AutoFactoryOrgID = "汽修商组织ID";

                /// <summary>
                /// 汽修商组织名称
                /// </summary>
                public const string SFOD_AutoFactoryOrgName = "汽修商组织名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SFOD_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SFOD_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SFOD_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SFOD_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SFOD_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SFOD_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SFOD_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SFOD_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 销售预测订单明细ID
                /// </summary>
                public const string SFOD_ID = "SFOD_ID";

                /// <summary>
                /// 销售预测订单ID
                /// </summary>
                public const string SFOD_ST_ID = "SFOD_ST_ID";

                /// <summary>
                /// 价格是否含税
                /// </summary>
                public const string SFOD_PriceIsIncludeTax = "SFOD_PriceIsIncludeTax";

                /// <summary>
                /// 税率
                /// </summary>
                public const string SFOD_TaxRate = "SFOD_TaxRate";

                /// <summary>
                /// 税额
                /// </summary>
                public const string SFOD_TotalTax = "SFOD_TotalTax";

                /// <summary>
                /// 数量
                /// </summary>
                public const string SFOD_Qty = "SFOD_Qty";

                /// <summary>
                /// 单价
                /// </summary>
                public const string SFOD_UnitPrice = "SFOD_UnitPrice";

                /// <summary>
                /// 总金额
                /// </summary>
                public const string SFOD_TotalAmount = "SFOD_TotalAmount";

                /// <summary>
                /// 配件条码
                /// </summary>
                public const string SFOD_Barcode = "SFOD_Barcode";

                /// <summary>
                /// 配件批次号（汽修厂用）
                /// </summary>
                public const string SFOD_BatchNoNew = "SFOD_BatchNoNew";

                /// <summary>
                /// 配件名称
                /// </summary>
                public const string SFOD_Name = "SFOD_Name";

                /// <summary>
                /// 配件规格型号
                /// </summary>
                public const string SFOD_Specification = "SFOD_Specification";

                /// <summary>
                /// 单位
                /// </summary>
                public const string SFOD_UOM = "SFOD_UOM";

                /// <summary>
                /// 汽修商组织ID
                /// </summary>
                public const string SFOD_AutoFactoryOrgID = "SFOD_AutoFactoryOrgID";

                /// <summary>
                /// 汽修商组织名称
                /// </summary>
                public const string SFOD_AutoFactoryOrgName = "SFOD_AutoFactoryOrgName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string SFOD_Remark = "SFOD_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string SFOD_IsValid = "SFOD_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string SFOD_CreatedBy = "SFOD_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string SFOD_CreatedTime = "SFOD_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string SFOD_UpdatedBy = "SFOD_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string SFOD_UpdatedTime = "SFOD_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string SFOD_VersionNo = "SFOD_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string SFOD_TransID = "SFOD_TransID";

            }

        }
        /// <summary>
        /// 系统作业
        /// </summary>
        public class CSM_BatchJob
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string BJ_ID = "ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string BJ_Org_ID = "组织ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string BJ_Org_Name = "组织名称";

                /// <summary>
                /// 作业编码
                /// </summary>
                public const string BJ_Code = "作业编码";

                /// <summary>
                /// 作业名称
                /// </summary>
                public const string BJ_Name = "作业名称";

                /// <summary>
                /// 作业分组
                /// </summary>
                public const string BJ_GroupName = "作业分组";

                /// <summary>
                /// 作业方式（消息推送，调度执行）
                /// </summary>
                public const string BJ_Pattern = "作业方式（消息推送，调度执行）";

                /// <summary>
                /// 消息类别（PC端，APP端，微信）
                /// </summary>
                public const string BJ_PushMode = "消息类别（PC端，APP端，微信）";

                /// <summary>
                /// 业务类别（验车，交强险到期，商业险到期，换驾驶证，生日祝福）
                /// </summary>
                public const string BJ_BusinessType = "业务类别（验车，交强险到期，商业险到期，换驾驶证，生日祝福）";

                /// <summary>
                /// 类全名
                /// </summary>
                public const string BJ_FullClassName = "类全名";

                /// <summary>
                /// 执行类型
                /// </summary>
                public const string BJ_ExecutionType = "执行类型";

                /// <summary>
                /// 执行一次的时间
                /// </summary>
                public const string BJ_ExecuteTime = "执行一次的时间";

                /// <summary>
                /// 执行间隔
                /// </summary>
                public const string BJ_ExecutionInterval = "执行间隔";

                /// <summary>
                /// 执行间隔值
                /// </summary>
                public const string BJ_ExecutionIntervalValue = "执行间隔值";

                /// <summary>
                /// 日执行类型
                /// </summary>
                public const string BJ_DayExecutionType = "日执行类型";

                /// <summary>
                /// 日一次执行时间
                /// </summary>
                public const string BJ_DayExecutionTime = "日一次执行时间";

                /// <summary>
                /// 日执行间隔
                /// </summary>
                public const string BJ_DayExecutionFrequency = "日执行间隔";

                /// <summary>
                /// 日执行间隔值
                /// </summary>
                public const string BJ_DayExecutionIntervalValue = "日执行间隔值";

                /// <summary>
                /// 日执行间隔的开始时间
                /// </summary>
                public const string BJ_DayExecutionStartTime = "日执行间隔的开始时间";

                /// <summary>
                /// 日执行间隔的结束时间
                /// </summary>
                public const string BJ_DayExecutionEndTime = "日执行间隔的结束时间";

                /// <summary>
                /// 计划生效时间
                /// </summary>
                public const string BJ_PlanStartDate = "计划生效时间";

                /// <summary>
                /// 计划失效时间
                /// </summary>
                public const string BJ_PlanEndDate = "计划失效时间";

                /// <summary>
                /// Cron表达式
                /// </summary>
                public const string BJ_CronExpression = "Cron表达式";

                /// <summary>
                /// 计划说明
                /// </summary>
                public const string BJ_Remark = "计划说明";

                /// <summary>
                /// 有效
                /// </summary>
                public const string BJ_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string BJ_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string BJ_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string BJ_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string BJ_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string BJ_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string BJ_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string BJ_ID = "BJ_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string BJ_Org_ID = "BJ_Org_ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string BJ_Org_Name = "BJ_Org_Name";

                /// <summary>
                /// 作业编码
                /// </summary>
                public const string BJ_Code = "BJ_Code";

                /// <summary>
                /// 作业名称
                /// </summary>
                public const string BJ_Name = "BJ_Name";

                /// <summary>
                /// 作业分组
                /// </summary>
                public const string BJ_GroupName = "BJ_GroupName";

                /// <summary>
                /// 作业方式（消息推送，调度执行）
                /// </summary>
                public const string BJ_Pattern = "BJ_Pattern";

                /// <summary>
                /// 消息类别（PC端，APP端，微信）
                /// </summary>
                public const string BJ_PushMode = "BJ_PushMode";

                /// <summary>
                /// 业务类别（验车，交强险到期，商业险到期，换驾驶证，生日祝福）
                /// </summary>
                public const string BJ_BusinessType = "BJ_BusinessType";

                /// <summary>
                /// 类全名
                /// </summary>
                public const string BJ_FullClassName = "BJ_FullClassName";

                /// <summary>
                /// 执行类型
                /// </summary>
                public const string BJ_ExecutionType = "BJ_ExecutionType";

                /// <summary>
                /// 执行一次的时间
                /// </summary>
                public const string BJ_ExecuteTime = "BJ_ExecuteTime";

                /// <summary>
                /// 执行间隔
                /// </summary>
                public const string BJ_ExecutionInterval = "BJ_ExecutionInterval";

                /// <summary>
                /// 执行间隔值
                /// </summary>
                public const string BJ_ExecutionIntervalValue = "BJ_ExecutionIntervalValue";

                /// <summary>
                /// 日执行类型
                /// </summary>
                public const string BJ_DayExecutionType = "BJ_DayExecutionType";

                /// <summary>
                /// 日一次执行时间
                /// </summary>
                public const string BJ_DayExecutionTime = "BJ_DayExecutionTime";

                /// <summary>
                /// 日执行间隔
                /// </summary>
                public const string BJ_DayExecutionFrequency = "BJ_DayExecutionFrequency";

                /// <summary>
                /// 日执行间隔值
                /// </summary>
                public const string BJ_DayExecutionIntervalValue = "BJ_DayExecutionIntervalValue";

                /// <summary>
                /// 日执行间隔的开始时间
                /// </summary>
                public const string BJ_DayExecutionStartTime = "BJ_DayExecutionStartTime";

                /// <summary>
                /// 日执行间隔的结束时间
                /// </summary>
                public const string BJ_DayExecutionEndTime = "BJ_DayExecutionEndTime";

                /// <summary>
                /// 计划生效时间
                /// </summary>
                public const string BJ_PlanStartDate = "BJ_PlanStartDate";

                /// <summary>
                /// 计划失效时间
                /// </summary>
                public const string BJ_PlanEndDate = "BJ_PlanEndDate";

                /// <summary>
                /// Cron表达式
                /// </summary>
                public const string BJ_CronExpression = "BJ_CronExpression";

                /// <summary>
                /// 计划说明
                /// </summary>
                public const string BJ_Remark = "BJ_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string BJ_IsValid = "BJ_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string BJ_CreatedBy = "BJ_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string BJ_CreatedTime = "BJ_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string BJ_UpdatedBy = "BJ_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string BJ_UpdatedTime = "BJ_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string BJ_VersionNo = "BJ_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string BJ_TransID = "BJ_TransID";

            }

        }
        /// <summary>
        /// 系统作业日志
        /// </summary>
        public class CSM_BatchJobLog
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string BJL_ID = "ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string BJL_Org_ID = "组织ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string BJL_Org_Name = "组织名称";

                /// <summary>
                /// 作业编码
                /// </summary>
                public const string BJL_Code = "作业编码";

                /// <summary>
                /// 作业名称
                /// </summary>
                public const string BJL_Name = "作业名称";

                /// <summary>
                /// 作业方式（消息推送，调度执行）
                /// </summary>
                public const string BJL_Pattern = "作业方式（消息推送，调度执行）";

                /// <summary>
                /// 推送方式（PC端，APP端，微信）
                /// </summary>
                public const string BJL_PushMode = "推送方式（PC端，APP端，微信）";

                /// <summary>
                /// 执行开始时间
                /// </summary>
                public const string BJL_ExectueStartDate = "执行开始时间";

                /// <summary>
                /// 执行结束时间
                /// </summary>
                public const string BJL_ExectueEndDate = "执行结束时间";

                /// <summary>
                /// 启动方式（自动，手动）
                /// </summary>
                public const string BJL_StartMode = "启动方式（自动，手动）";

                /// <summary>
                /// 日志明细
                /// </summary>
                public const string BJL_Details = "日志明细";

                /// <summary>
                /// 有效
                /// </summary>
                public const string BJL_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string BJL_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string BJL_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string BJL_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string BJL_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string BJL_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string BJL_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string BJL_ID = "BJL_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string BJL_Org_ID = "BJL_Org_ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string BJL_Org_Name = "BJL_Org_Name";

                /// <summary>
                /// 作业编码
                /// </summary>
                public const string BJL_Code = "BJL_Code";

                /// <summary>
                /// 作业名称
                /// </summary>
                public const string BJL_Name = "BJL_Name";

                /// <summary>
                /// 作业方式（消息推送，调度执行）
                /// </summary>
                public const string BJL_Pattern = "BJL_Pattern";

                /// <summary>
                /// 推送方式（PC端，APP端，微信）
                /// </summary>
                public const string BJL_PushMode = "BJL_PushMode";

                /// <summary>
                /// 执行开始时间
                /// </summary>
                public const string BJL_ExectueStartDate = "BJL_ExectueStartDate";

                /// <summary>
                /// 执行结束时间
                /// </summary>
                public const string BJL_ExectueEndDate = "BJL_ExectueEndDate";

                /// <summary>
                /// 启动方式（自动，手动）
                /// </summary>
                public const string BJL_StartMode = "BJL_StartMode";

                /// <summary>
                /// 日志明细
                /// </summary>
                public const string BJL_Details = "BJL_Details";

                /// <summary>
                /// 有效
                /// </summary>
                public const string BJL_IsValid = "BJL_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string BJL_CreatedBy = "BJL_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string BJL_CreatedTime = "BJL_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string BJL_UpdatedBy = "BJL_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string BJL_UpdatedTime = "BJL_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string BJL_VersionNo = "BJL_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string BJL_TransID = "BJL_TransID";

            }

        }
        /// <summary>
        /// 业务提醒日志
        /// </summary>
        public class CSM_BusinessRemindLog
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string BRL_ID = "ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string BRL_Org_ID = "组织ID";

                /// <summary>
                /// 组织
                /// </summary>
                public const string BRL_Org_Name = "组织";

                /// <summary>
                /// 业务类别
                /// </summary>
                public const string BRL_BJ_BusinessType = "业务类别";

                /// <summary>
                /// 被提醒对象类别
                /// </summary>
                public const string BRL_RemindObjectType = "被提醒对象类别";

                /// <summary>
                /// 被提醒对象
                /// </summary>
                public const string BRL_RemindObject = "被提醒对象";

                /// <summary>
                /// 交强险保险公司
                /// </summary>
                public const string BRL_ComInsuranceCompany = "交强险保险公司";

                /// <summary>
                /// 商业险保险公司
                /// </summary>
                public const string BRL_BusInsuranceCompany = "商业险保险公司";

                /// <summary>
                /// 相关日期
                /// </summary>
                public const string BRL_RelateDate = "相关日期";

                /// <summary>
                /// 备注
                /// </summary>
                public const string BRL_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string BRL_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string BRL_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string BRL_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string BRL_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string BRL_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string BRL_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string BRL_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string BRL_ID = "BRL_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string BRL_Org_ID = "BRL_Org_ID";

                /// <summary>
                /// 组织
                /// </summary>
                public const string BRL_Org_Name = "BRL_Org_Name";

                /// <summary>
                /// 业务类别
                /// </summary>
                public const string BRL_BJ_BusinessType = "BRL_BJ_BusinessType";

                /// <summary>
                /// 被提醒对象类别
                /// </summary>
                public const string BRL_RemindObjectType = "BRL_RemindObjectType";

                /// <summary>
                /// 被提醒对象
                /// </summary>
                public const string BRL_RemindObject = "BRL_RemindObject";

                /// <summary>
                /// 交强险保险公司
                /// </summary>
                public const string BRL_ComInsuranceCompany = "BRL_ComInsuranceCompany";

                /// <summary>
                /// 商业险保险公司
                /// </summary>
                public const string BRL_BusInsuranceCompany = "BRL_BusInsuranceCompany";

                /// <summary>
                /// 相关日期
                /// </summary>
                public const string BRL_RelateDate = "BRL_RelateDate";

                /// <summary>
                /// 备注
                /// </summary>
                public const string BRL_Remark = "BRL_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string BRL_IsValid = "BRL_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string BRL_CreatedBy = "BRL_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string BRL_CreatedTime = "BRL_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string BRL_UpdatedBy = "BRL_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string BRL_UpdatedTime = "BRL_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string BRL_VersionNo = "BRL_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string BRL_TransID = "BRL_TransID";

            }

        }
        /// <summary>
        /// 消息推送接收日志
        /// </summary>
        public class CSM_PushMesageLog
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string PML_ID = "ID";

                /// <summary>
                /// 业务提醒ID
                /// </summary>
                public const string PML_BRL_ID = "业务提醒ID";

                /// <summary>
                /// 推送方式
                /// </summary>
                public const string PML_PushMode = "推送方式";

                /// <summary>
                /// 推送内容
                /// </summary>
                public const string PML_Content = "推送内容";

                /// <summary>
                /// 推送者类别
                /// </summary>
                public const string PML_SenderType = "推送者类别";

                /// <summary>
                /// 推送者
                /// </summary>
                public const string PML_Sender = "推送者";

                /// <summary>
                /// 推送时间
                /// </summary>
                public const string PML_SendTime = "推送时间";

                /// <summary>
                /// 推送状态
                /// </summary>
                public const string PML_SendStatus = "推送状态";

                /// <summary>
                /// 接收者类别
                /// </summary>
                public const string PML_ReceiverType = "接收者类别";

                /// <summary>
                /// 接收者
                /// </summary>
                public const string PML_Receiver = "接收者";

                /// <summary>
                /// 接收时间
                /// </summary>
                public const string PML_ReceiveTime = "接收时间";

                /// <summary>
                /// 接收状态
                /// </summary>
                public const string PML_ReceiveStatus = "接收状态";

                /// <summary>
                /// 作业ID
                /// </summary>
                public const string PML_BJ_ID = "作业ID";

                /// <summary>
                /// 作业名称
                /// </summary>
                public const string PML_BJ_Name = "作业名称";

                /// <summary>
                /// 业务消息类别
                /// </summary>
                public const string PML_BusMsgType = "业务消息类别";

                /// <summary>
                /// 业务单号
                /// </summary>
                public const string PML_BussinessCode = "业务单号";

                /// <summary>
                /// JSON格式内容
                /// </summary>
                public const string PML_JsonFormatContent = "JSON格式内容";

                /// <summary>
                /// 备注
                /// </summary>
                public const string PML_Remark = "备注";

                /// <summary>
                /// 是否需要跟踪
                /// </summary>
                public const string PML_IsNeedTrack = "是否需要跟踪";

                /// <summary>
                /// 跟踪人
                /// </summary>
                public const string PML_TrackedBy = "跟踪人";

                /// <summary>
                /// 跟踪状态
                /// </summary>
                public const string PML_TrackStatus = "跟踪状态";

                /// <summary>
                /// 有效
                /// </summary>
                public const string PML_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string PML_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string PML_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string PML_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string PML_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string PML_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string PML_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// ID
                /// </summary>
                public const string PML_ID = "PML_ID";

                /// <summary>
                /// 业务提醒ID
                /// </summary>
                public const string PML_BRL_ID = "PML_BRL_ID";

                /// <summary>
                /// 推送方式
                /// </summary>
                public const string PML_PushMode = "PML_PushMode";

                /// <summary>
                /// 推送内容
                /// </summary>
                public const string PML_Content = "PML_Content";

                /// <summary>
                /// 推送者类别
                /// </summary>
                public const string PML_SenderType = "PML_SenderType";

                /// <summary>
                /// 推送者
                /// </summary>
                public const string PML_Sender = "PML_Sender";

                /// <summary>
                /// 推送时间
                /// </summary>
                public const string PML_SendTime = "PML_SendTime";

                /// <summary>
                /// 推送状态
                /// </summary>
                public const string PML_SendStatus = "PML_SendStatus";

                /// <summary>
                /// 接收者类别
                /// </summary>
                public const string PML_ReceiverType = "PML_ReceiverType";

                /// <summary>
                /// 接收者
                /// </summary>
                public const string PML_Receiver = "PML_Receiver";

                /// <summary>
                /// 接收时间
                /// </summary>
                public const string PML_ReceiveTime = "PML_ReceiveTime";

                /// <summary>
                /// 接收状态
                /// </summary>
                public const string PML_ReceiveStatus = "PML_ReceiveStatus";

                /// <summary>
                /// 作业ID
                /// </summary>
                public const string PML_BJ_ID = "PML_BJ_ID";

                /// <summary>
                /// 作业名称
                /// </summary>
                public const string PML_BJ_Name = "PML_BJ_Name";

                /// <summary>
                /// 业务消息类别
                /// </summary>
                public const string PML_BusMsgType = "PML_BusMsgType";

                /// <summary>
                /// 业务单号
                /// </summary>
                public const string PML_BussinessCode = "PML_BussinessCode";

                /// <summary>
                /// JSON格式内容
                /// </summary>
                public const string PML_JsonFormatContent = "PML_JsonFormatContent";

                /// <summary>
                /// 备注
                /// </summary>
                public const string PML_Remark = "PML_Remark";

                /// <summary>
                /// 是否需要跟踪
                /// </summary>
                public const string PML_IsNeedTrack = "PML_IsNeedTrack";

                /// <summary>
                /// 跟踪人
                /// </summary>
                public const string PML_TrackedBy = "PML_TrackedBy";

                /// <summary>
                /// 跟踪状态
                /// </summary>
                public const string PML_TrackStatus = "PML_TrackStatus";

                /// <summary>
                /// 有效
                /// </summary>
                public const string PML_IsValid = "PML_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string PML_CreatedBy = "PML_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string PML_CreatedTime = "PML_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string PML_UpdatedBy = "PML_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string PML_UpdatedTime = "PML_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string PML_VersionNo = "PML_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string PML_TransID = "PML_TransID";

            }

        }
        /// <summary>
        /// 电子钱包
        /// </summary>
        public class EWM_Wallet
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 钱包ID
                /// </summary>
                public const string Wal_ID = "钱包ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string Wal_Org_ID = "组织ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string Wal_Org_Name = "组织名称";

                /// <summary>
                /// 钱包账号
                /// </summary>
                public const string Wal_No = "钱包账号";

                /// <summary>
                /// 钱包来源类型编码
                /// </summary>
                public const string Wal_SourceTypeCode = "钱包来源类型编码";

                /// <summary>
                /// 钱包来源类型名称
                /// </summary>
                public const string Wal_SourceTypeName = "钱包来源类型名称";

                /// <summary>
                /// 来源账号
                /// </summary>
                public const string Wal_SourceNo = "来源账号";

                /// <summary>
                /// 钱包所有人类别编码
                /// </summary>
                public const string Wal_OwnerTypeCode = "钱包所有人类别编码";

                /// <summary>
                /// 钱包所有人类别名称
                /// </summary>
                public const string Wal_OwnerTypeName = "钱包所有人类别名称";

                /// <summary>
                /// 开户人ID
                /// </summary>
                public const string Wal_CustomerID = "开户人ID";

                /// <summary>
                /// 开户人姓名
                /// </summary>
                public const string Wal_CustomerName = "开户人姓名";

                /// <summary>
                /// 汽修商户编码
                /// </summary>
                public const string Wal_AutoFactoryCode = "汽修商户编码";

                /// <summary>
                /// 汽修商户组织编码
                /// </summary>
                public const string Wal_AutoFactoryOrgCode = "汽修商户组织编码";

                /// <summary>
                /// 交易密码
                /// </summary>
                public const string Wal_TradingPassword = "交易密码";

                /// <summary>
                /// 可用余额
                /// </summary>
                public const string Wal_AvailableBalance = "可用余额";

                /// <summary>
                /// 冻结余额
                /// </summary>
                public const string Wal_FreezingBalance = "冻结余额";

                /// <summary>
                /// 充值基数
                /// </summary>
                public const string Wal_DepositBaseAmount = "充值基数";

                /// <summary>
                /// 推荐员工ID
                /// </summary>
                public const string Wal_RecommendEmployeeID = "推荐员工ID";

                /// <summary>
                /// 推荐员工
                /// </summary>
                public const string Wal_RecommendEmployee = "推荐员工";

                /// <summary>
                /// 开户组织ID
                /// </summary>
                public const string Wal_CreatedByOrgID = "开户组织ID";

                /// <summary>
                /// 开户组织名称
                /// </summary>
                public const string Wal_CreatedByOrgName = "开户组织名称";

                /// <summary>
                /// 生效时间
                /// </summary>
                public const string Wal_EffectiveTime = "生效时间";

                /// <summary>
                /// 失效时间
                /// </summary>
                public const string Wal_IneffectiveTime = "失效时间";

                /// <summary>
                /// 钱包状态编码
                /// </summary>
                public const string Wal_StatusCode = "钱包状态编码";

                /// <summary>
                /// 钱包状态名称
                /// </summary>
                public const string Wal_StatusName = "钱包状态名称";

                /// <summary>
                /// 备注
                /// </summary>
                public const string Wal_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Wal_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Wal_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Wal_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Wal_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Wal_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Wal_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Wal_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 钱包ID
                /// </summary>
                public const string Wal_ID = "Wal_ID";

                /// <summary>
                /// 组织ID
                /// </summary>
                public const string Wal_Org_ID = "Wal_Org_ID";

                /// <summary>
                /// 组织名称
                /// </summary>
                public const string Wal_Org_Name = "Wal_Org_Name";

                /// <summary>
                /// 钱包账号
                /// </summary>
                public const string Wal_No = "Wal_No";

                /// <summary>
                /// 钱包来源类型编码
                /// </summary>
                public const string Wal_SourceTypeCode = "Wal_SourceTypeCode";

                /// <summary>
                /// 钱包来源类型名称
                /// </summary>
                public const string Wal_SourceTypeName = "Wal_SourceTypeName";

                /// <summary>
                /// 来源账号
                /// </summary>
                public const string Wal_SourceNo = "Wal_SourceNo";

                /// <summary>
                /// 钱包所有人类别编码
                /// </summary>
                public const string Wal_OwnerTypeCode = "Wal_OwnerTypeCode";

                /// <summary>
                /// 钱包所有人类别名称
                /// </summary>
                public const string Wal_OwnerTypeName = "Wal_OwnerTypeName";

                /// <summary>
                /// 开户人ID
                /// </summary>
                public const string Wal_CustomerID = "Wal_CustomerID";

                /// <summary>
                /// 开户人姓名
                /// </summary>
                public const string Wal_CustomerName = "Wal_CustomerName";

                /// <summary>
                /// 汽修商户编码
                /// </summary>
                public const string Wal_AutoFactoryCode = "Wal_AutoFactoryCode";

                /// <summary>
                /// 汽修商户组织编码
                /// </summary>
                public const string Wal_AutoFactoryOrgCode = "Wal_AutoFactoryOrgCode";

                /// <summary>
                /// 交易密码
                /// </summary>
                public const string Wal_TradingPassword = "Wal_TradingPassword";

                /// <summary>
                /// 可用余额
                /// </summary>
                public const string Wal_AvailableBalance = "Wal_AvailableBalance";

                /// <summary>
                /// 冻结余额
                /// </summary>
                public const string Wal_FreezingBalance = "Wal_FreezingBalance";

                /// <summary>
                /// 充值基数
                /// </summary>
                public const string Wal_DepositBaseAmount = "Wal_DepositBaseAmount";

                /// <summary>
                /// 推荐员工ID
                /// </summary>
                public const string Wal_RecommendEmployeeID = "Wal_RecommendEmployeeID";

                /// <summary>
                /// 推荐员工
                /// </summary>
                public const string Wal_RecommendEmployee = "Wal_RecommendEmployee";

                /// <summary>
                /// 开户组织ID
                /// </summary>
                public const string Wal_CreatedByOrgID = "Wal_CreatedByOrgID";

                /// <summary>
                /// 开户组织名称
                /// </summary>
                public const string Wal_CreatedByOrgName = "Wal_CreatedByOrgName";

                /// <summary>
                /// 生效时间
                /// </summary>
                public const string Wal_EffectiveTime = "Wal_EffectiveTime";

                /// <summary>
                /// 失效时间
                /// </summary>
                public const string Wal_IneffectiveTime = "Wal_IneffectiveTime";

                /// <summary>
                /// 钱包状态编码
                /// </summary>
                public const string Wal_StatusCode = "Wal_StatusCode";

                /// <summary>
                /// 钱包状态名称
                /// </summary>
                public const string Wal_StatusName = "Wal_StatusName";

                /// <summary>
                /// 备注
                /// </summary>
                public const string Wal_Remark = "Wal_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string Wal_IsValid = "Wal_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string Wal_CreatedBy = "Wal_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string Wal_CreatedTime = "Wal_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string Wal_UpdatedBy = "Wal_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string Wal_UpdatedTime = "Wal_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string Wal_VersionNo = "Wal_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string Wal_TransID = "Wal_TransID";

            }

        }
        /// <summary>
        /// 电子钱包异动
        /// </summary>
        public class EWM_WalletTrans
        {
            /// <summary>
            /// 字段中文名称
            /// </summary>
            public class Name
            {
                /// <summary>
                /// 钱包异动ID
                /// </summary>
                public const string WalT_ID = "钱包异动ID";

                /// <summary>
                /// 受理组织ID
                /// </summary>
                public const string WalT_Org_ID = "受理组织ID";

                /// <summary>
                /// 受理组织名称
                /// </summary>
                public const string WalT_Org_Name = "受理组织名称";

                /// <summary>
                /// 钱包ID
                /// </summary>
                public const string WalT_Wal_ID = "钱包ID";

                /// <summary>
                /// 钱包账号
                /// </summary>
                public const string WalT_Wal_No = "钱包账号";

                /// <summary>
                /// 异动时间
                /// </summary>
                public const string WalT_Time = "异动时间";

                /// <summary>
                /// 异动类型编码
                /// </summary>
                public const string WalT_TypeCode = "异动类型编码";

                /// <summary>
                /// 异动类型名称
                /// </summary>
                public const string WalT_TypeName = "异动类型名称";

                /// <summary>
                /// 充值方式编码
                /// </summary>
                public const string WalT_RechargeTypeCode = "充值方式编码";

                /// <summary>
                /// 充值方式名称
                /// </summary>
                public const string WalT_RechargeTypeName = "充值方式名称";

                /// <summary>
                /// 通道编码
                /// </summary>
                public const string WalT_ChannelCode = "通道编码";

                /// <summary>
                /// 通道名称
                /// </summary>
                public const string WalT_ChannelName = "通道名称";

                /// <summary>
                /// 异动金额
                /// </summary>
                public const string WalT_Amount = "异动金额";

                /// <summary>
                /// 单据编号
                /// </summary>
                public const string WalT_BillNo = "单据编号";

                /// <summary>
                /// 备注
                /// </summary>
                public const string WalT_Remark = "备注";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WalT_IsValid = "有效";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WalT_CreatedBy = "创建人";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WalT_CreatedTime = "创建时间";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WalT_UpdatedBy = "修改人";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WalT_UpdatedTime = "修改时间";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WalT_VersionNo = "版本号";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WalT_TransID = "事务编号";

            }

            /// <summary>
            /// 字段英文名称
            /// </summary>
            public class Code
            {
                /// <summary>
                /// 钱包异动ID
                /// </summary>
                public const string WalT_ID = "WalT_ID";

                /// <summary>
                /// 受理组织ID
                /// </summary>
                public const string WalT_Org_ID = "WalT_Org_ID";

                /// <summary>
                /// 受理组织名称
                /// </summary>
                public const string WalT_Org_Name = "WalT_Org_Name";

                /// <summary>
                /// 钱包ID
                /// </summary>
                public const string WalT_Wal_ID = "WalT_Wal_ID";

                /// <summary>
                /// 钱包账号
                /// </summary>
                public const string WalT_Wal_No = "WalT_Wal_No";

                /// <summary>
                /// 异动时间
                /// </summary>
                public const string WalT_Time = "WalT_Time";

                /// <summary>
                /// 异动类型编码
                /// </summary>
                public const string WalT_TypeCode = "WalT_TypeCode";

                /// <summary>
                /// 异动类型名称
                /// </summary>
                public const string WalT_TypeName = "WalT_TypeName";

                /// <summary>
                /// 充值方式编码
                /// </summary>
                public const string WalT_RechargeTypeCode = "WalT_RechargeTypeCode";

                /// <summary>
                /// 充值方式名称
                /// </summary>
                public const string WalT_RechargeTypeName = "WalT_RechargeTypeName";

                /// <summary>
                /// 通道编码
                /// </summary>
                public const string WalT_ChannelCode = "WalT_ChannelCode";

                /// <summary>
                /// 通道名称
                /// </summary>
                public const string WalT_ChannelName = "WalT_ChannelName";

                /// <summary>
                /// 异动金额
                /// </summary>
                public const string WalT_Amount = "WalT_Amount";

                /// <summary>
                /// 单据编号
                /// </summary>
                public const string WalT_BillNo = "WalT_BillNo";

                /// <summary>
                /// 备注
                /// </summary>
                public const string WalT_Remark = "WalT_Remark";

                /// <summary>
                /// 有效
                /// </summary>
                public const string WalT_IsValid = "WalT_IsValid";

                /// <summary>
                /// 创建人
                /// </summary>
                public const string WalT_CreatedBy = "WalT_CreatedBy";

                /// <summary>
                /// 创建时间
                /// </summary>
                public const string WalT_CreatedTime = "WalT_CreatedTime";

                /// <summary>
                /// 修改人
                /// </summary>
                public const string WalT_UpdatedBy = "WalT_UpdatedBy";

                /// <summary>
                /// 修改时间
                /// </summary>
                public const string WalT_UpdatedTime = "WalT_UpdatedTime";

                /// <summary>
                /// 版本号
                /// </summary>
                public const string WalT_VersionNo = "WalT_VersionNo";

                /// <summary>
                /// 事务编号
                /// </summary>
                public const string WalT_TransID = "WalT_TransID";

            }

        }

    }

}
