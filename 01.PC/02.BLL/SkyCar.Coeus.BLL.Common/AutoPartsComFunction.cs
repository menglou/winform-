using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.Common.Log;
using System.Windows.Forms;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;

namespace SkyCar.Coeus.BLL.COM
{
    /// <summary>
    /// 配件相关共通方法
    /// </summary>
    public class AutoPartsComFunction
    {
        private static readonly BLLBase _bllBase = new BLLBase(Trans.COM);

        #region 配件档案相关

        /// <summary>
        /// 检查配件档案是否已存在
        /// </summary>
        /// <param name="paramAutoPartsArchive">配件档案TBModel</param>
        /// <param name="paramAutoPartsBarCode">如果配件不存在，此处返回配件的条码;如果配件存在，返回已有条码</param>
        /// <returns>true：配件存在，false：配件不存在</returns>
        public static bool AutoPartsArchiveExist(MDLBS_AutoPartsArchive paramAutoPartsArchive, out string paramAutoPartsBarCode)
        {
            paramAutoPartsBarCode = null;
            //配件档案是否存在
            var archiveExist = false;
            MDLBS_AutoPartsArchive argsAutoPartsArchive = new MDLBS_AutoPartsArchive
            {
                WHERE_APA_OEMNo = paramAutoPartsArchive.APA_OEMNo,
                WHERE_APA_ThirdNo = paramAutoPartsArchive.APA_ThirdNo,
                WHERE_APA_Name = paramAutoPartsArchive.APA_Name,
                WHERE_APA_Brand = paramAutoPartsArchive.APA_Brand,
                WHERE_APA_Specification = paramAutoPartsArchive.APA_Specification,
                WHERE_APA_UOM = paramAutoPartsArchive.APA_UOM,
                WHERE_APA_Level = paramAutoPartsArchive.APA_Level,
                WHERE_APA_VehicleBrand = paramAutoPartsArchive.APA_VehicleBrand,
                WHERE_APA_VehicleInspire = paramAutoPartsArchive.APA_VehicleInspire,
                WHERE_APA_VehicleYearModel = paramAutoPartsArchive.APA_VehicleYearModel,
                WHERE_APA_VehicleCapacity = paramAutoPartsArchive.APA_VehicleCapacity,
                WHERE_APA_VehicleGearboxTypeName = paramAutoPartsArchive.APA_VehicleGearboxTypeName,
                WHERE_APA_IsValid = true
            };
            List<MDLBS_AutoPartsArchive> resultAutoPartsArchiveList = new List<MDLBS_AutoPartsArchive>();
            _bllBase.QueryForList(SQLID.COMM_SQL0701, argsAutoPartsArchive, resultAutoPartsArchiveList);

            if (resultAutoPartsArchiveList.Count == 1)
            {
                paramAutoPartsBarCode = resultAutoPartsArchiveList[0].APA_Barcode;
                archiveExist = true;
            }
            return archiveExist;
        }

        /// <summary>
        /// 检查配件档案是否已存在
        /// </summary>
        /// <param name="paramAutoPartsArchive">配件档案TBModel</param>
        /// <returns>true：配件存在，false：配件不存在</returns>
        public static MDLBS_AutoPartsArchive GetAutoPartsArchive(MDLBS_AutoPartsArchive paramAutoPartsArchive)
        {
            MDLBS_AutoPartsArchive resultAutoPartsArchive = new MDLBS_AutoPartsArchive();
            MDLBS_AutoPartsArchive argsAutoPartsArchive = new MDLBS_AutoPartsArchive
            {
                WHERE_APA_OEMNo = paramAutoPartsArchive.APA_OEMNo,
                WHERE_APA_ThirdNo = paramAutoPartsArchive.APA_ThirdNo,
                WHERE_APA_Name = paramAutoPartsArchive.APA_Name,
                WHERE_APA_Brand = paramAutoPartsArchive.APA_Brand,
                WHERE_APA_Specification = paramAutoPartsArchive.APA_Specification,
                WHERE_APA_UOM = paramAutoPartsArchive.APA_UOM,
                WHERE_APA_Level = paramAutoPartsArchive.APA_Level,
                WHERE_APA_VehicleBrand = paramAutoPartsArchive.APA_VehicleBrand,
                WHERE_APA_VehicleInspire = paramAutoPartsArchive.APA_VehicleInspire,
                WHERE_APA_VehicleYearModel = paramAutoPartsArchive.APA_VehicleYearModel,
                WHERE_APA_VehicleCapacity = paramAutoPartsArchive.APA_VehicleCapacity,
                WHERE_APA_VehicleGearboxTypeName = paramAutoPartsArchive.APA_VehicleGearboxTypeName,
                WHERE_APA_IsValid = true
            };
            List<MDLBS_AutoPartsArchive> resultAutoPartsArchiveList = new List<MDLBS_AutoPartsArchive>();
            _bllBase.QueryForList(SQLID.COMM_SQL0701, argsAutoPartsArchive, resultAutoPartsArchiveList);

            if (resultAutoPartsArchiveList.Count == 1)
            {
                resultAutoPartsArchive = resultAutoPartsArchiveList[0];
            }
            return resultAutoPartsArchive;
        }

        /// <summary>
        /// 验证原厂编码和第三方编码是否被其他配件名称使用
        /// </summary>
        /// <param name="paramAutoPartsName">配件名称</param>
        /// <param name="paramAutoPartsOEMNo">原厂编码</param>
        /// <param name="paramAutoPartsThirdNo">第三方编码</param>
        /// <returns></returns>
        public static bool ValidateOEMNoAndThirdNo(string paramAutoPartsName, string paramAutoPartsOEMNo, string paramAutoPartsThirdNo)
        {
            if (string.IsNullOrEmpty(paramAutoPartsName))
            {
                //MessageBoxs.Show("配件名称接收变量不能为空");
                return false;
            }
            //原厂编码和第三方编码是否被其他配件名称使用的数量
            int duplicateCount = (int)_bllBase.QueryForObject(SQLID.COMM_SQL0703, new MDLBS_AutoPartsArchive
            {
                WHERE_APA_Name = paramAutoPartsName,
                WHERE_APA_OEMNo = paramAutoPartsOEMNo,
                WHERE_APA_ThirdNo = paramAutoPartsThirdNo
            });
            if (duplicateCount > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据配件条形码获取配件档案
        /// </summary>
        /// <param name="paramBarcode">配件条形码</param>
        /// <returns></returns>
        public static MDLAPM_AutoPartsArchive GetAutoPartsArchiveByBarcode(string paramBarcode)
        {
            List<MDLAPM_AutoPartsArchive> resultAutoPartsArchiveList = new List<MDLAPM_AutoPartsArchive>();
            _bllBase.QueryForList(SQLID.COMM_SQL0704, paramBarcode, resultAutoPartsArchiveList);

            return resultAutoPartsArchiveList.Count == 1 ? resultAutoPartsArchiveList[0] : new MDLAPM_AutoPartsArchive();
        }

        #endregion

        /// <summary>
        /// 是否有进销存：根据系统参数[是否有库存]获取
        /// </summary>
        /// <returns>true:表示有进销存  false:表示无进销存 </returns>
        public static bool IsHasInventory()
        {
            MDLSM_Parameter resultParameter = new MDLSM_Parameter();
            _bllBase.QueryForObject<MDLSM_Parameter, MDLSM_Parameter>(new MDLSM_Parameter
            {
                WHERE_Para_Code1 = "1001",
                WHERE_Para_Name1 = "是否启用进销存模块",
                WHERE_Para_IsValid = true
            }, resultParameter);
            if (!string.IsNullOrEmpty(resultParameter.Para_ID)
                && resultParameter.Para_Value1 == "1")
            {
                return true;
            }
            return false;
        }

        #region 配件图片相关

        /// <summary>
        /// 删除配件图片
        /// </summary>
        /// <param name="paramAutoPartsPictureList">配件图片TBModel列表</param>
        /// <param name="paramResultMsg">输出消息</param>
        /// <returns></returns>
        public static bool DeleteAutoPartsPicture(List<MDLPIS_InventoryPicture> paramAutoPartsPictureList, ref string paramResultMsg)
        {
            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                var deleteAutoPartsPictureResult = _bllBase.DeleteByList<MDLPIS_InventoryPicture, MDLPIS_InventoryPicture>(paramAutoPartsPictureList);
                if (!deleteAutoPartsPictureResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    paramResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE, MsgParam.AUTOPARTS_PICTURE });
                    return false;
                }
                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                paramResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.DELETE, ex.Message });
                return false;
            }
            return true;
        }
        #endregion

    }
}
