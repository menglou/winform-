using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace SkyCar.Coeus.UpdateClient
{
    /// <summary>
    /// 自动升级
    /// </summary>
    public partial class FrmAutoUpdate : Form
    {
        #region 全局变量
        public delegate void updateData(string value); //设置委托用来更新主界面
        //设置一个定时器
        private DispatcherTimer _updateTimer = new DispatcherTimer();
        private DispatcherTimer _downLoadFileTimer = new DispatcherTimer();
        private int _totalSeconds = 0; //总用时
        private int _downLoadTotalSeconds = 0;
        private readonly string appPath = Environment.CurrentDirectory;

        Thread _thUpdate = null;

        /// <summary>
        /// 已经下载的大小
        /// </summary>
        long _hasDownloadSize = 0;
        /// <summary>
        /// 需要下载的大小
        /// </summary>
        long _downloadTotalSize = 0;

        long totalSize = 0;
        bool _isDownloading = false;

        private List<FTPFileInfo> downLoadFileNameList = new List<FTPFileInfo>();
        #endregion

        #region 构造方法

        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmAutoUpdate()
        {
            InitializeComponent();

            string configFilePath = Environment.CurrentDirectory + "\\" + AutoUpdateCoeus.LocalConfigFileName;
            if (!File.Exists(configFilePath))
            {
                MessageBox.Show("不存在配置文件：" + configFilePath + ",升级失败");
                Environment.Exit(0);
                return;
            }
            AppSettingsSection configSection = null;
            var fileMap = new ExeConfigurationFileMap() { ExeConfigFilename = configFilePath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            foreach (var loopSectionKey in config.Sections.Keys)
            {
                if (loopSectionKey.ToString() == "appSettings")
                {
                    configSection = (AppSettingsSection)config.GetSection("appSettings");
                    break;
                }
            }
            if (configSection != null)
            {
                if (configSection.Settings.AllKeys.Contains("UpgradeFtpUrl"))
                {
                    AutoUpdateCoeus.UrlOfUpgrade = configSection.Settings["UpgradeFtpUrl"].Value;
                }
                if (configSection.Settings.AllKeys.Contains("UpgradeFtpUserName"))
                {
                    AutoUpdateCoeus.UserNameOfUpgrade = configSection.Settings["UpgradeFtpUserName"].Value;
                }
                if (configSection.Settings.AllKeys.Contains("UpgradeFtpPwd"))
                {
                    AutoUpdateCoeus.PasswordOfUpgrade = configSection.Settings["UpgradeFtpPwd"].Value;
                }
            }
            if (string.IsNullOrEmpty(AutoUpdateCoeus.UrlOfUpgrade))
            {
                MessageBox.Show("未配置 FTP升级地址：appSettings->UpgradeFtpUrl,升级失败");
                Environment.Exit(0);
                return;
            }
            if (string.IsNullOrEmpty(AutoUpdateCoeus.UserNameOfUpgrade))
            {
                MessageBox.Show("未配置 FTP升级用户名：appSettings->UpgradeFtpUserName,升级失败");
                Environment.Exit(0);
                return;
            }
            if (string.IsNullOrEmpty(AutoUpdateCoeus.PasswordOfUpgrade))
            {
                MessageBox.Show("未配置 FTP升级密码：appSettings->UpgradeFtpPwd,升级失败");
                Environment.Exit(0);
                return;
            }
        }

        #endregion

        #region 系统事件

        /// <summary>
        /// 自动升级Load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAutoUpdate_Load(object sender, EventArgs e)
        {
            InitViewAndUpdateSoft();
        }

        void InitViewAndUpdateSoft()
        {
            this.Invoke(new Action(() =>
            {
                _thUpdate = new Thread(BeginUpdate);
                _thUpdate.Start();
                _updateTimer.Tick += timer1_Tick;
                _updateTimer.Interval = new TimeSpan(0, 0, 1);
                _updateTimer.IsEnabled = true;
                _updateTimer.Start();
                _downLoadFileTimer.Tick += DownLoadFileTimer_Tick;
                _downLoadFileTimer.Interval = new TimeSpan(0, 0, 1);
            }));
        }

        /// <summary>
        /// 更新日志MouseDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblUpdateLog_MouseDown(object sender, MouseEventArgs e)
        {
            if (File.Exists(AutoUpdateCoeus.LogFileName))
            {
                Process.Start("notepad.exe", Environment.CurrentDirectory + "\\" + AutoUpdateCoeus.LogFileName);
            }
            else
            {
                if (!AutoUpdateCoeus.IsServer)
                {
                    MessageBox.Show("未找到日志文件!");
                }
            }
        }

        #endregion

        #region 自定义方法

        private void DownLoadFileTimer_Tick(object sender, EventArgs e)
        {
            _downLoadTotalSeconds += 1;
            if (_isDownloading)
            {
                //速度
                lblSpeedValue.Text = _hasDownloadSize / (1024 * _downLoadTotalSeconds) / 1024 >= 1
                    ? Math.Round((decimal)_hasDownloadSize / (1024 * _downLoadTotalSeconds) / 1024, 2) + "M/s"
                    : _hasDownloadSize / (1024 * _downLoadTotalSeconds) + "KB/s";
            }
            else
            {
                //速度
                lblSpeedValue.Text = "0KB/s";
            }
            //进度
            lblProgressVaule.Text = Math.Round((decimal)_hasDownloadSize / 1024 / 1024, 2) + "/" +
                                         Math.Round((decimal)_downloadTotalSize / 1024 / 1024, 2) + "M";

            double percentValue = (_downloadTotalSize > 0 ? (_hasDownloadSize * 100.00 / _downloadTotalSize) : 100.00);
            //百分比
            lblPercentageValue.Text = percentValue.ToString("0.00") + "%";
            //进度条
            this.progressBarValue.Value = (int) percentValue;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _totalSeconds += 1;

            //窗体标题Text
            this.Text = "正在为您更新,可能需要几分钟的时间,请耐心等待......,已用时" + (_totalSeconds / 3600).ToString("00") + "时" + ((_totalSeconds % 3600) / 60).ToString("00") + "分" + (_totalSeconds % 3600 % 60).ToString("00") + "秒";
        }

        /// <summary>
        /// 尝试次数
        /// </summary>
        private int tryTimes = 0;

        /// <summary>
        /// 开始更新
        /// </summary>
        private void BeginUpdate()
        {
            string tmpDir = appPath + "\\" + AutoUpdateCoeus.TempUpgradeDirectory;
            bool upgradeResult = false;
            try
            {
                if (!Directory.Exists(tmpDir))
                {
                    Directory.CreateDirectory(tmpDir);
                }
                _hasDownloadSize = 0;
                _downloadTotalSize = 0;
                ShowInfo("获取升级包大小...");
                downLoadFileNameList.Clear();

                FTPFileInfo serverAllFileInfo = new FTPFileInfo();
                FTPFileInfo serverDirectoryAndFileInfo = GetServerDirectoryAndFileInfo(serverAllFileInfo);
                if (serverDirectoryAndFileInfo == null)
                {
                    MessageBox.Show("升级服务器上不存在文件：UpgradeBaseFileList.xml，升级失败。");
                    //等待两秒钟之后关闭当前窗口，跳转登录窗口
                    AutoUpdateCoeus.CloseWindowDelay(1000);
                    Process.Start(Environment.CurrentDirectory + "\\" + AutoUpdateCoeus.LocalExeFileName, null);
                    return;
                }
                Dictionary<string, string> localSoftFileHashInfo = GetFileHashInfo(Environment.CurrentDirectory, Environment.CurrentDirectory + @"\" + AutoUpdateCoeus.TempUpgradeDirectory);
                Dictionary<string, string> localTempFileHashInfo = GetFileHashInfo(Environment.CurrentDirectory + @"\" + AutoUpdateCoeus.TempUpgradeDirectory, string.Empty);
                long tempCopyTotalSize = 0;
                foreach (var loopChild in serverDirectoryAndFileInfo.ChildList)
                {
                    loopChild.NeedDownload = true;
                }
                foreach (var loopServerFile in serverAllFileInfo.ChildList)
                {
                    loopServerFile.NeedDownload = !AutoUpdateCoeus.SelfFiles.Contains(loopServerFile.Name);

                    if (localSoftFileHashInfo.ContainsKey(loopServerFile.Key))
                    {
                        if (localSoftFileHashInfo[loopServerFile.Key] == loopServerFile.Hash)
                        {
                            loopServerFile.NeedDownload = false;
                        }
                    }
                    if (loopServerFile.NeedDownload)
                    {
                        if (localTempFileHashInfo.ContainsKey(loopServerFile.Key))
                        {
                            if (localTempFileHashInfo[loopServerFile.Key] == loopServerFile.Hash)
                            {
                                loopServerFile.NeedDownload = false;
                                tempCopyTotalSize += loopServerFile.Size;
                            }
                        }
                    }
                    if (loopServerFile.NeedDownload)
                    {
                        _downloadTotalSize += loopServerFile.Size;
                        tempCopyTotalSize += loopServerFile.Size;
                    }
                }

                if (_downloadTotalSize > 0 || tempCopyTotalSize > 0)
                {
                    if (_downloadTotalSize > 0)
                    {
                        this.Invoke(new Action(() =>
                        {
                            this.lblProgressVaule.Text = Math.Round((decimal)_hasDownloadSize / 1024 / 1024, 2) + "/" +
                                                         Math.Round((decimal)_downloadTotalSize / 1024 / 1024, 2) + "M";
                        }));

                        ShowInfo("开始升级...");
                        _isDownloading = true;
                        _downLoadFileTimer.Start();
                        DownLoadFile(appPath, tmpDir, AutoUpdateCoeus.UserNameOfUpgrade, AutoUpdateCoeus.PasswordOfUpgrade,
                            AutoUpdateCoeus.UrlOfUpgrade, serverDirectoryAndFileInfo.ChildList);
                        _isDownloading = false;
                    }

                    ShowInfo("复制文件到软件目录...");
                    CopyDir(tmpDir, appPath);
                    ShowInfo("复制文件到软件目录完成！");
                    if (Directory.Exists(tmpDir))
                    {
                        try
                        {
                            Directory.Delete(tmpDir, true);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                    ShowInfo("升级成功!");
                }
                else
                {
                    ShowInfo("没有需要更新的文件。");
                }

                //升级成功之后修改本地的版本号
                string localConfigFilePath = LocalConfigFile.ConfigFilePath.SoftVersionNo.Replace(PH_SOFTVERSIONNO, null);
                if (!File.Exists(localConfigFilePath))
                {
                    string localConfigDirectory = appPath + "\\LocalConfig";
                    if (!Directory.Exists(localConfigDirectory))
                    {
                        Directory.CreateDirectory(localConfigDirectory);
                    }
                    string sysConDataDirectory = appPath + "\\LocalConfig\\SysConData";
                    if (!Directory.Exists(sysConDataDirectory))
                    {
                        Directory.CreateDirectory(sysConDataDirectory);
                    }
                    File.Create(localConfigFilePath);
                }
                if (!string.IsNullOrEmpty(AutoUpdateCoeus.LatestSoftVersionNo))
                {
                    WritePrivateProfileString(LocalConfigFile.SelectionName.SoftInfo,
                        LocalConfigFile.KeyName.SoftVersionNo, AutoUpdateCoeus.LatestSoftVersionNo, localConfigFilePath);
                }
                //UpdateLocalVersionNo();

                upgradeResult = true;

                Process.Start(Environment.CurrentDirectory + "\\" + AutoUpdateCoeus.LocalExeFileName, null);

                //等待两秒钟之后关闭当前窗口，跳转登录窗口
                AutoUpdateCoeus.CloseWindowDelay(1000);
            }
            catch (Exception ex)
            {
                if (upgradeResult)
                {
                    Environment.Exit(0);
                }
                WriteLogFileAndEmail(ex);
                _isDownloading = false;
                if (tryTimes < 3)
                {
                    tryTimes++;
                    InitViewAndUpdateSoft();
                }
                return;
            }
            //Application.Exit();















            //string tmpDir = appPath + "\\" + AutoUpdateCoeus.TempUpgradeDirectory;
            //bool upgradeResult = false;
            //try
            //{
            //    if (!Directory.Exists(tmpDir))
            //    {
            //        Directory.CreateDirectory(tmpDir);
            //    }

            //    _hasDownloadSize = 0;
            //    _downloadTotalSize = 0;
            //    //totalSize = 0;
            //    ShowInfo("获取升级包大小...");
            //    downLoadFileNameList.Clear();

            //    GetDownloadFileSize(appPath, AutoUpdateCoeus.UserNameOfUpgrade, AutoUpdateCoeus.PasswordOfUpgrade, AutoUpdateCoeus.UrlOfUpgrade, downLoadFileNameList);
            //    ShowInfo("开始升级...");
            //    _isDownloading = true;
            //    _downLoadFileTimer.Start();
            //    DownLoadFile(appPath, tmpDir, AutoUpdateCoeus.UserNameOfUpgrade, AutoUpdateCoeus.PasswordOfUpgrade, AutoUpdateCoeus.UrlOfUpgrade, downLoadFileNameList);
            //    _isDownloading = false;
            //    ShowInfo("复制文件到工作目录...");
            //    CopyDirection(tmpDir, appPath);
            //    ShowInfo("复制文件到工作目录完成！！！");
            //    Directory.Delete(tmpDir, true);
            //    ShowInfo("升级成功!!!");

            //    //升级成功之后修改本地的版本号
            //    string localConfigFilePath = LocalConfigFile.ConfigFilePath.SoftVersionNo.Replace(PH_SOFTVERSIONNO, null);
            //    if (!string.IsNullOrEmpty(AutoUpdateCoeus.LatestSoftVersionNo))
            //    {
            //        WritePrivateProfileString(LocalConfigFile.SelectionName.SoftInfo, LocalConfigFile.KeyName.SoftVersionNo, AutoUpdateCoeus.LatestSoftVersionNo, localConfigFilePath);
            //    }
            //    //UpdateLocalVersionNo();
            //    AutoUpdateCoeus.CloseWindowDelay(2000);
            //    //等待两秒钟之后关闭当前窗口，跳转登录窗口
            //    Process.Start(Environment.CurrentDirectory + "\\SkyCar.Coeus.Ult.Entrance.exe", null);
            //}
            //catch (Exception ex)
            //{
            //    WriteLogFileAndEmail(ex);
            //    _isDownloading = false;
            //    Directory.Delete(tmpDir, true);
            //    MessageBox.Show(ex.Message);
            //    return;
            //}
            ////Application.Exit();
        }

        private static FTPFileInfo GetAllLocalFileInfo()
        {
            try
            {
                string tempBaseFilePath = Environment.CurrentDirectory + @"\" + "UpgradeBaseFileList.xml";
                FTPFileInfo resultAllFileInfo = new FTPFileInfo();
                if (File.Exists(tempBaseFilePath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(tempBaseFilePath);

                    XmlNode rootNode = doc.SelectSingleNode("descendant::dir");
                    if (rootNode == null)
                    {
                        return null;
                    }
                    else
                    {
                        AnalyzeXmlNode(rootNode, null, resultAllFileInfo);
                    }
                }
                return resultAllFileInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static Dictionary<string, string> GetFileHashInfo(string directoryPath, string excludeDirPath)
        {
            try
            {
                Dictionary<string, string> resultFileHashDic = new Dictionary<string, string>();

                List<string> resultFileList = new List<string>();
                ListFiles(new DirectoryInfo(directoryPath), resultFileList, excludeDirPath);

                foreach (var loopFilePath in resultFileList)
                {
                    if (loopFilePath.Contains(".png"))
                    {

                    }
                    string fileHashValue = GetFileHash(loopFilePath);
                    if (fileHashValue == "")
                    {

                    }
                    if (!string.IsNullOrEmpty(fileHashValue))
                    {
                        resultFileHashDic.Add(loopFilePath.Replace(directoryPath + "\\", ""), fileHashValue);
                    }
                }
                return resultFileHashDic;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void ListFiles(FileSystemInfo info, List<string> resultFileList, string excludeDirPath)
        {
            if (!info.Exists)
            {
                return;
            }
            DirectoryInfo dir = info as DirectoryInfo;
            //不是目录 
            if (dir == null)
            {
                return;
            }
            FileSystemInfo[] files = dir.GetFileSystemInfos();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].FullName == excludeDirPath)
                {

                }
                FileInfo file = files[i] as FileInfo;
                //是文件 
                if (file != null)
                {
                    resultFileList.Add(file.FullName);
                }
                //对于子目录，进行递归调用 
                else if (string.IsNullOrEmpty(excludeDirPath) || files[i].FullName != excludeDirPath)
                {
                    ListFiles(files[i], resultFileList, excludeDirPath);
                }
            }
        }

        private static string GetFileHash(string filePath)
        {
            if (!File.Exists(filePath))
                return string.Empty;

            string md5;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer;
                using (HashAlgorithm hash = HashAlgorithm.Create())
                {
                    buffer = hash.ComputeHash(fs);
                    hash.Clear();
                }
                md5 = Convert.ToBase64String(buffer);
                fs.Close();
            }
            return md5;
        }

        private static FTPFileInfo GetServerDirectoryAndFileInfo(FTPFileInfo allFileInfo)
        {
            try
            {
                string tempUpgradeDirectory = Environment.CurrentDirectory + "\\" + AutoUpdateCoeus.TempUpgradeDirectory;
                if (!Directory.Exists(tempUpgradeDirectory))
                {
                    Directory.CreateDirectory(tempUpgradeDirectory);
                }
                FTPClientHelper.Instance.Download(AutoUpdateCoeus.UserNameOfUpgrade, AutoUpdateCoeus.PasswordOfUpgrade, AutoUpdateCoeus.UrlOfUpgrade, tempUpgradeDirectory,
                                    "UpgradeBaseFileList.xml");
                string tempBaseFilePath = Environment.CurrentDirectory + "\\" + AutoUpdateCoeus.TempUpgradeDirectory +
                                          @"\" +
                                          "UpgradeBaseFileList.xml";
                FTPFileInfo resultFileInfo = new FTPFileInfo();
                if (File.Exists(tempBaseFilePath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(tempBaseFilePath);

                    XmlNode rootNode = doc.SelectSingleNode("descendant::dir");
                    if (rootNode == null)
                    {
                        return null;
                    }
                    else
                    {
                        resultFileInfo = AnalyzeXmlNode(rootNode, null, allFileInfo);
                    }
                }
                return resultFileInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static FTPFileInfo AnalyzeXmlNode(XmlNode rootNode, FTPFileInfo pntDir, FTPFileInfo allFileInfo)
        {
            FTPFileInfo dir = new FTPFileInfo();
            if (rootNode.Attributes != null)
            {
                dir.Name = rootNode.Attributes["name"].InnerText;
                dir.IsDirectory = true;
                dir.Size = 0;

                XmlNodeList nodes = rootNode.ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    if (node == null)
                    {
                        continue;
                    }
                    switch (node.Name)
                    {
                        case "dir":
                            AnalyzeXmlNode(node, dir, allFileInfo);
                            break;
                        case "file":
                            if (node.Attributes != null)
                            {
                                FTPFileInfo file = new FTPFileInfo()
                                {
                                    Name = node.Attributes["name"].InnerText,
                                    Hash = node.Attributes["hash"].InnerText,
                                    Key = node.Attributes["filekey"].InnerText,
                                    IsDirectory = false
                                };
                                file.Size = 0;
                                long tempSize = 0;
                                long.TryParse(node.Attributes["size"].InnerText, out tempSize);
                                file.Size = tempSize;

                                dir.ChildList.Add(file);

                                allFileInfo.ChildList.Add(file);
                            }
                            break;
                    }
                }
            }

            if (pntDir != null)
            {
                pntDir.ChildList.Add(dir);
            }

            return dir;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="ex"></param>
        void WriteLogFileAndEmail(Exception ex)
        {
            string exceptionContent = null;
            if (ex != null)
            {
                if (ex.Message.Contains("信息已过期"))
                {
                    return;
                }
                StackTrace st = new StackTrace(ex, true);
                exceptionContent = string.Format("错误的信息：{0}\n出错的方法名:{1}\n出错的类名:{2}\n出错行号:{3}\n文件名:{4}\n错误的堆栈{5}",
                   ex.Message, ex.TargetSite.Name, ex.GetType().Name, st.GetFrame(0).GetFileLineNumber(), st.GetFrame(0).GetFileName(), ex.StackTrace);
            }

            if (!Directory.Exists(Application.StartupPath + @"\ErrorLog"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\ErrorLog");
            }
            string errLogFile = Application.StartupPath + @"\ErrorLog\升级异常" + DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString().ToUpper() + ".txt";
            using (StreamWriter sw = new StreamWriter(errLogFile, true))
            {
                sw.WriteLine(exceptionContent);
                sw.WriteLine("---------------------------------------------------------");
                sw.Close();
            }
        }

        #region 修改本地版本号

        #region win32非托管方法
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string settingIniPath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string settingIniPath);
        #endregion

        /// <summary>
        /// SoftVersionNo占位符(Placeholder)
        /// </summary>
        public const string PH_SOFTVERSIONNO = "{SoftVersionNo}";

        /// <summary>
        /// 修改本地本地版本号
        /// </summary>
        public void UpdateLocalVersionNo()
        {
            string localConfigFilePath = LocalConfigFile.ConfigFilePath.SoftVersionNo.Replace(PH_SOFTVERSIONNO, null);
            if (File.Exists(localConfigFilePath))
            {
                //本地版本号
                StringBuilder tempSoftVersionNo = new StringBuilder(255);
                GetPrivateProfileString(LocalConfigFile.SelectionName.SoftInfo,
                    LocalConfigFile.KeyName.SoftVersionNo, string.Empty, tempSoftVersionNo, 255,
                    localConfigFilePath);
                string configFilePath = Environment.CurrentDirectory + "\\SkyCar.Coeus.Ult.Entrance.exe.config";
                AppSettingsSection configSection;
                if (!string.IsNullOrEmpty(configFilePath))
                {
                    var fileMap = new ExeConfigurationFileMap() { ExeConfigFilename = configFilePath };
                    var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                    configSection = (AppSettingsSection)config.GetSection("appSettings");
                }
                else
                {
                    configSection = (AppSettingsSection)ConfigurationManager.GetSection("appSettings");
                }
                string argsUrl = configSection.Settings["APIURL"].Value + "BF0006";
                string argsPostData = string.Format("MCT_Code={0}&SP_Code={1}", configSection.Settings["MCT_Code"].Value, "CoeusUlt");
                string strApiData = GetAPIData(argsUrl, argsPostData);
                //获取平台版本号
                var jsonResult = (JObject)JsonConvert.DeserializeObject(strApiData);
                if (jsonResult["ResultCode"] != null && jsonResult["ResultCode"].ToString().Equals("I0001"))
                {
                    if (jsonResult["ML_LastVersionNo"] != null && !string.IsNullOrEmpty(jsonResult["ML_LastVersionNo"].ToString()))
                    {
                        WritePrivateProfileString(LocalConfigFile.SelectionName.SoftInfo,
                            LocalConfigFile.KeyName.SoftVersionNo, jsonResult["ML_LastVersionNo"].ToString(),
                            localConfigFilePath);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("本地配置文件：" + localConfigFilePath + "不存在，无法完成更新", null, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

        /// <summary>
        /// 获取接口信息
        /// </summary>
        /// <param name="paramUrl">接口地址</param>
        /// <param name="paramPostData">参数</param>
        /// <returns></returns>
        public String GetAPIData(string paramUrl, string paramPostData)
        {
            Encoding myEncoding = Encoding.UTF8;
            string sMode = "POST";
            string sUrl = paramUrl;
            string sPostData = paramPostData;
            string sContentType = "application/x-www-form-urlencoded";
            HttpWebRequest req;
            string strResultData = string.Empty;

            try
            {
                // init
                req = HttpWebRequest.Create(sUrl) as HttpWebRequest;
                req.Method = sMode;
                req.Accept = "*/*";
                req.KeepAlive = false;
                req.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                if (0 == String.CompareOrdinal("POST", sMode))
                {
                    byte[] bufPost = myEncoding.GetBytes(sPostData);
                    req.ContentType = sContentType;
                    req.ContentLength = bufPost.Length;
                    using (Stream newStream = req.GetRequestStream())
                    {
                        newStream.Write(bufPost, 0, bufPost.Length);
                    }
                }

                // 找到合适的编码
                Encoding encoding = null;
                //encoding = Encoding_FromBodyName(res.CharacterSet);	// 后来发现主体部分的字符集与Response.CharacterSet不同.
                //if (null == encoding) encoding = myEncoding;
                encoding = Encoding.UTF8;
                Debug.WriteLine(encoding);
                using (HttpWebResponse res = req.GetResponse() as HttpWebResponse)
                {
                    if (res != null)
                    {
                        using (Stream resStream = res.GetResponseStream())
                        {
                            if (resStream != null)
                            {
                                using (StreamReader resStreamReader = new StreamReader(resStream, encoding))
                                {
                                    strResultData = resStreamReader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
                return strResultData;
            }
            catch (Exception ex)
            {
                return strResultData;
            }
        }

        #region 提示信息

        private delegate void OutPutMsgDelegate(string msg);

        /// <summary>
        /// 输出升级信息
        /// </summary>
        /// <param name="msg"></param>
        private void OutPutMsg(string msg)
        {
            this.txtProgressContent.AppendText(msg);
            this.txtProgressContent.AppendText("\r\n");
            //this.txtProgressContent.ScrollToEnd();
            this.txtProgressContent.Focus();
        }

        /// <summary>
        /// 显示升级信息
        /// </summary>
        /// <param name="msg"></param>
        public void ShowInfo(string msg)
        {
            try
            {
                this.Invoke(new OutPutMsgDelegate(this.OutPutMsg), msg);
                File.AppendAllText("updlog.txt", msg.Replace("\r\n", string.Empty), Encoding.GetEncoding("gb2312"));
                File.AppendAllText("updlog.txt", DateTime.Now.ToString(" yyyy-MM-dd HH:mm:ss"));
                File.AppendAllText("updlog.txt", "\r\n");
            }
            catch (Exception ex)
            {
                File.AppendAllText("updlog.txt", ex.Message.Replace("\r\n", string.Empty), Encoding.GetEncoding("gb2312"));
                File.AppendAllText("updlog.txt", DateTime.Now.ToString(" yyyy-MM-dd HH:mm:ss"));
                File.AppendAllText("updlog.txt", "\r\n");
            }
        }

        #endregion 提示信息

        /// <summary>
        /// 复制文件到工作目录
        /// </summary>
        /// <param name="paramSrcPath">原文件目录</param>
        /// <param name="paramAimPath">目的文件目录</param>
        private void CopyDirection(string paramSrcPath, string paramAimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加
                if (paramAimPath[paramAimPath.Length - 1] != Path.DirectorySeparatorChar)
                {
                    paramAimPath += Path.DirectorySeparatorChar;
                }

                // 判断目标目录是否存在如果不存在则新建
                if (!Directory.Exists(paramAimPath))
                {
                    Directory.CreateDirectory(paramAimPath);
                }

                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles（paramSrcPath）；
                string[] fileList = Directory.GetFileSystemEntries(paramSrcPath);

                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (Directory.Exists(file))
                    {
                        CopyDirection(file, paramAimPath + Path.GetFileName(file));
                    }
                    // 否则直接Copy文件
                    else
                    {
                        File.Copy(file, paramAimPath + Path.GetFileName(file), true);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void CopyDir(string srcPath, string destPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加

                if (destPath[destPath.Length - 1] != Path.DirectorySeparatorChar)
                {
                    destPath += Path.DirectorySeparatorChar;
                }

                // 判断目标目录是否存在如果不存在则新建

                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }

                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组

                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法

                // string[] fileList = Directory.GetFiles（srcPath）；

                string[] fileList = Directory.GetFileSystemEntries(srcPath);

                // 遍历所有的文件和目录

                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件

                    if (Directory.Exists(file))
                    {
                        CopyDir(file, destPath + Path.GetFileName(file));
                    }

                    // 否则直接Copy文件
                    else if (!AutoUpdateCoeus.SelfFiles.Contains(file.Replace(srcPath + "\\", "")))
                    {

                        bool overwrite = !File.Exists(destPath + Path.GetFileName(file)) ||
                            file == "UpgradeBaseFileList.xml" ||
                            !AutoUpdateCoeus.SelfFiles.Contains(file);
                        if (overwrite)
                        {
                            File.Copy(file, destPath + Path.GetFileName(file), true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="currentWorkPath"></param>
        /// <param name="localPath"></param>
        /// <param name="ftpUser"></param>
        /// <param name="ftpPasswd"></param>
        /// <param name="ftpPath"></param>
        private void DownLoadFile(string currentWorkPath, string localPath, string ftpUser, string ftpPasswd, string ftpPath, List<FTPFileInfo> fileList)
        {
            string curFileName = string.Empty;
            try
            {
                foreach (var fileInfo in fileList)
                {
                    curFileName = fileInfo.Name;
                    if (fileInfo.IsDirectory)
                    {
                        if (fileInfo.ChildList.Count > 0)
                        {
                            string newFtpPath = (ftpPath.EndsWith("/")
                                ? ftpPath + fileInfo.Name + "/"
                                : ftpPath + "/" + fileInfo.Name + "/");
                            string newLocalPath = localPath + "\\" + fileInfo.Name;
                            string newCurrentWorkPath = currentWorkPath + "\\" + fileInfo.Name;
                            if (!Directory.Exists(newLocalPath))
                            {
                                Directory.CreateDirectory(newLocalPath);
                            }
                            DownLoadFile(newCurrentWorkPath, newLocalPath, ftpUser, ftpPasswd, newFtpPath,
                                fileInfo.ChildList);
                        }

                    }
                    else if (fileInfo.NeedDownload)
                    {
                        if (fileInfo.Name == "Newtonsoft.Json.dll")
                        {

                        }
                        ShowInfo(string.Format("正在下载文件{0}...", fileInfo.Name));
                        int count = 0;
                        while (true)
                        {
                            try
                            {
                                FTPClientHelper.Instance.Download(ftpUser, ftpPasswd, ftpPath, localPath, fileInfo.Name);
                                _hasDownloadSize += fileInfo.Size;
                                break;
                            }
                            catch (Exception ex)
                            {
                                AutoUpdateCoeus.WriteLogFileAndEmail(ex, "下载" + fileInfo.Name + "时发现异常");
                                count++;
                                if (count <= 3)
                                {
                                    ShowInfo(string.Format("下载文件{0}失败！！！失败原因：{1},正在进行第{2}次重试...", fileInfo.Name,
                                        ex.Message, count));
                                    continue;
                                }
                                else
                                {
                                    ShowInfo(string.Format("下载文件{0}失败！！！失败原因：{1}", fileInfo.Name, ex.Message));
                                    throw ex;
                                }
                            }
                        }
                        ShowInfo(string.Format("下载文件{0}完成", fileInfo.Name));
                    }
                }
            }
            catch (Exception ex)
            {
                AutoUpdateCoeus.WriteLogFileAndEmail(ex, "下载" + curFileName + "时发现异常");
                throw;
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="currentWorkPath">工作路径</param>
        /// <param name="ftpUser">FTP用户名</param>
        /// <param name="ftpPasswd">FTP密码</param>
        /// <param name="ftpPath">FTP的路径</param>
        private void GetDownloadFileSize(string currentWorkPath, string ftpUser, string ftpPasswd, string ftpPath,
            List<FTPFileInfo> fileList, string folderName = "")
        {
            string tempUpgradeDicectory = appPath + "\\" + AutoUpdateCoeus.TempUpgradeDirectory;
            string[] fileFullPathList = FTPClientHelper.Instance.GetDetailFilePath(ftpUser, ftpPasswd, ftpPath);
            List<string> fileNameList =
                new List<string>(FTPClientHelper.Instance.GetFilePath(ftpUser, ftpPasswd, ftpPath));

            foreach (var fileFullPath in fileFullPathList)
            {
                string onlyFileName = string.Empty;
                foreach (var fileName in fileNameList)
                {
                    if (fileFullPath.Contains(fileName))
                    {
                        onlyFileName = fileName;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(onlyFileName) && !AutoUpdateCoeus.SelfFiles.Contains(onlyFileName))
                {
                    if (fileFullPath.TrimStart().StartsWith("d"))
                    {
                        string newFtpPath = (ftpPath.EndsWith("/")
                            ? ftpPath + onlyFileName + "/"
                            : ftpPath + "/" + onlyFileName + "/");
                        string newCurrentWorkPath = currentWorkPath + "\\" + onlyFileName;
                        FTPFileInfo fileInfo = new FTPFileInfo
                        {
                            Name = onlyFileName,
                            IsDirectory = true,
                            ChildList = new List<FTPFileInfo>()
                        };
                        fileList.Add(fileInfo);
                        GetDownloadFileSize(newCurrentWorkPath, ftpUser, ftpPasswd, newFtpPath, fileInfo.ChildList,
                            (!string.IsNullOrEmpty(folderName) ? folderName + "\\" : string.Empty) + onlyFileName);
                    }
                    else
                    {
                        DateTime ftpFileModifyTime = FTPClientHelper.Instance.GetFileModifyDateTime(ftpUser, ftpPasswd,
                            ftpPath, string.Empty, onlyFileName);
                        DateTime localFileModifyTime = ftpFileModifyTime.AddYears(-1);
                        string localFullFileName = currentWorkPath + "\\" + onlyFileName;
                        if (File.Exists(localFullFileName))
                        {
                            localFileModifyTime = File.GetLastWriteTime(localFullFileName);
                        }
                        //程序路径不存在文件，判断是否存在升级临时文件
                        if (localFileModifyTime < ftpFileModifyTime)
                        {
                            string tempFullFileName = tempUpgradeDicectory +
                                                      (!string.IsNullOrEmpty(folderName)
                                                          ? "\\" + folderName
                                                          : string.Empty) + "\\" + onlyFileName;
                            if (File.Exists(tempFullFileName))
                            {
                                localFileModifyTime = File.GetLastWriteTime(tempFullFileName);
                            }
                        }
                        if (onlyFileName.Contains("programConfig") || localFileModifyTime < ftpFileModifyTime)
                        {
                            int count = 0;
                            while (true)
                            {
                                try
                                {
                                    var tempList = !string.IsNullOrEmpty(fileFullPath)
                                        ? fileFullPath.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToList()
                                        : null;
                                    //totalSize += FTPClientHelper.Instance.GetFileSize(ftpUser, ftpPasswd, ftpPath, onlyFileName);
                                    if (tempList != null)
                                    {
                                        _downloadTotalSize += long.Parse(tempList[4]);
                                    }
                                    this.Invoke(new Action(() =>
                                    {
                                        this.lblProgressVaule.Text =
                                            Math.Round((decimal)_hasDownloadSize / 1024 / 1024, 2) + "/" +
                                            Math.Round((decimal)_downloadTotalSize / 1024 / 1024, 2) + "M";
                                    }));
                                    fileList.Add(new FTPFileInfo
                                    {
                                        IsDirectory = false,
                                        Name = onlyFileName,
                                        Size = long.Parse(tempList[4])
                                    });
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    AutoUpdateCoeus.WriteLogFileAndEmail(ex, "获取" + onlyFileName + "大小时发现异常");
                                    count++;
                                    if (count <= 3)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        throw ex;
                                    }
                                }
                            }
                        }
                    }
                    fileNameList.Remove(onlyFileName);
                }
            }
        }
        #endregion

    }

    internal class FTPFileInfo
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public long Size { get; set; }
        public bool IsDirectory { get; set; }
        public bool NeedDownload { get; set; }
        public List<FTPFileInfo> ChildList { get; set; }

        public FTPFileInfo()
        {
            ChildList = new List<FTPFileInfo>();
        }
    }
}
