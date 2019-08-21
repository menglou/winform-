using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common.APModel;

namespace SkyCar.Coeus.BLL.PIS
{
    /// <summary>
    /// 库存查询BLL
    /// </summary>
    public class InventoryQueryBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.PIS);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 库存查询BLL
        /// </summary>
        public InventoryQueryBLL() : base(Trans.PIS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存库存图片
        /// </summary>
        /// <param name="paramAutoPartsPictureList">库存图片UIModel列表</param>
        /// <returns></returns>
        public bool SaveAutoPartsPicture(List<AutoPartsPictureUIModel> paramAutoPartsPictureList)
        {
            var funcName = "SaveAutoPartsPicture";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 准备数据

            //待保存的库存图片列表
            List<MDLPIS_InventoryPicture> savePictureList = new List<MDLPIS_InventoryPicture>();

            foreach (var loopPicture in paramAutoPartsPictureList)
            {
                if (string.IsNullOrEmpty(loopPicture.SourceFilePath))
                {
                    continue;
                }
                #region 将图片保存到本地以及上传文件服务器

                string fileNetUrl = string.Empty;
                bool savePictureResult = BLLCom.SaveFileByFileName(loopPicture.SourceFilePath, loopPicture.INVP_PictureName, ref fileNetUrl);
                if (!savePictureResult)
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + MsgParam.AUTOPARTS_PICTURE });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 保存库存图片数据

                //截取上传图片返回值中的文件名称
                int fileNameStartIndex = fileNetUrl.IndexOf("FileName=", StringComparison.Ordinal) + 1;
                int fileNameEndIndex = fileNameStartIndex + "FileName=".Length;
                int length = fileNetUrl.Length;
                //文件名称
                string tempFileName = fileNetUrl.Substring(fileNameEndIndex - 1, length - (fileNameEndIndex - 1));

                MDLPIS_InventoryPicture newAutoPartsPicture = new MDLPIS_InventoryPicture();

                _bll.CopyModel(loopPicture, newAutoPartsPicture);
                newAutoPartsPicture.INVP_PictureName = tempFileName;
                newAutoPartsPicture.INVP_IsValid = true;
                newAutoPartsPicture.INVP_CreatedBy = LoginInfoDAX.UserName;
                newAutoPartsPicture.INVP_CreatedTime = BLLCom.GetCurStdDatetime();
                newAutoPartsPicture.INVP_UpdatedBy = LoginInfoDAX.UserName;
                newAutoPartsPicture.INVP_UpdatedTime = BLLCom.GetCurStdDatetime();

                newAutoPartsPicture.WHERE_INVP_ID = newAutoPartsPicture.INVP_ID;
                newAutoPartsPicture.WHERE_INVP_VersionNo = newAutoPartsPicture.INVP_VersionNo;

                savePictureList.Add(newAutoPartsPicture);

                #endregion
            }

            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存单头

                foreach (var loopPicture in savePictureList)
                {
                    //执行保存
                    bool saveInvPictureResult = _bll.Save(loopPicture);
                    if (!saveInvPictureResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);

                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopPicture.INVP_PictureName, ref outMsg);

                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + MsgParam.AUTOPARTS_PICTURE });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);

                foreach (var loopPicture in savePictureList)
                {
                    //保存失败，删除本地以及文件服务器上的图片
                    var outMsg = string.Empty;
                    BLLCom.DeleteFileByFileName(loopPicture.INVP_PictureName, ref outMsg);
                }

                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                return false;
            }

            #endregion

            #region 更新配件图片版本号
            foreach (var loopPicture in paramAutoPartsPictureList)
            {
                //保存成功，删除临时保存的图片
                if (File.Exists(loopPicture.SourceFilePath))
                {
                    File.Delete(loopPicture.SourceFilePath);
                }
                //本次保存的图片
                var thisSavePicture = savePictureList.FirstOrDefault(x => x.INVP_PictureName == loopPicture.INVP_PictureName);
                if (thisSavePicture != null)
                {
                    _bll.CopyModel(thisSavePicture, loopPicture);
                }
                //设置版本号
                if (loopPicture.INVP_VersionNo == null)
                {
                    loopPicture.INVP_VersionNo = 1;
                }
                else
                {
                    loopPicture.INVP_VersionNo += 1;
                }
            }
            #endregion

            return true;
        }
        
        #endregion

        #region 私有方法

        #endregion
    }
}
