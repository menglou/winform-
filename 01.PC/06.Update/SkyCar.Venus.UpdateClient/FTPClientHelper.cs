using System;
using System.IO;
using System.Net;
using System.Text;

namespace SkyCar.Coeus.UpdateClient
{
    /// <summary>
    /// FTP帮助类
    /// </summary>
    public class FTPClientHelper
    {
        public static readonly FTPClientHelper Instance = new FTPClientHelper();
        public static readonly bool _userpassive = true;

        /// <summary>
        /// 取得文件名
        /// </summary>
        /// <param name="ftpPath">ftp路径</param>
        /// <returns></returns>
        public string[] GetFilePath(string userId, string pwd, string ftpPath)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                var reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath));
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(userId, pwd);
                reqFtp.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFtp.UsePassive = _userpassive;
                reqFtp.KeepAlive = false;
                using (WebResponse response = reqFtp.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            result.Append(line);
                            result.Append("\n");
                            line = reader.ReadLine();
                        }
                        result.Remove(result.ToString().LastIndexOf('\n'), 1);

                        return result.ToString().Split('\n');
                    }
                }
            }
            catch
            {
                var downloadFiles = new string[0];
                return downloadFiles;
            }
        }

        //ftp的上传功能
        public void Upload(string userId, string pwd, string filename, string ftpPath, bool useBinary)
        {
            FileInfo fileInf = new FileInfo(filename);
            FtpWebRequest reqFTP;
            // 根据uri创建FtpWebRequest对象 
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath + fileInf.Name));
            // ftp用户名和密码
            reqFTP.Credentials = new NetworkCredential(userId, pwd);

            reqFTP.UsePassive = _userpassive;
            // 默认为true，连接不会被关闭
            // 在一个命令之后被执行
            reqFTP.KeepAlive = true;
            // 指定执行什么命令
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            // 指定数据传输类型
            reqFTP.UseBinary = true;
            // 上传文件时通知服务器文件的大小
            reqFTP.ContentLength = fileInf.Length;
            // 缓冲大小设置为2kb
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;

            try
            {
                // 打开一个文件流 (System.IO.FileStream) 去读上传的文件
                using (FileStream fs = fileInf.OpenRead())
                {
                    // 把上传的文件写入流
                    using (Stream strm = reqFTP.GetRequestStream())
                    {
                        // 每次读文件流的2kb
                        contentLen = fs.Read(buff, 0, buffLength);
                        // 流内容没有结束
                        while (contentLen != 0)
                        {
                            // 把内容从file stream 写入 upload stream
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(string userId, string pwd, string ftpPath, string fileName)
        {
            var reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath + fileName));
            reqFtp.Method = WebRequestMethods.Ftp.DeleteFile;
            reqFtp.UseBinary = true;
            reqFtp.Credentials = new NetworkCredential(userId, pwd);
            reqFtp.UsePassive = _userpassive;
            using (FtpWebResponse listResponse = (FtpWebResponse)reqFtp.GetResponse())
            {
                string sStatus = listResponse.StatusDescription;
            }
        }

        //从ftp服务器上下载文件的功能
        public void Download(string userId, string pwd, string ftpPath, string filePath, string fileName)
        {
            try
            {
                using (FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    var reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath + fileName));
                    reqFtp.Method = WebRequestMethods.Ftp.DownloadFile;
                    reqFtp.UseBinary = true;
                    reqFtp.Credentials = new NetworkCredential(userId, pwd);
                    reqFtp.UsePassive = _userpassive;
                    reqFtp.KeepAlive = false;
                    using (FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse())
                    {
                        using (Stream ftpStream = response.GetResponseStream())
                        {
                            int bufferSize = 2048;
                            byte[] buffer = new byte[bufferSize];
                            if (ftpStream != null)
                            {
                                var readCount = ftpStream.Read(buffer, 0, bufferSize);
                                while (readCount > 0)
                                {
                                    outputStream.Write(buffer, 0, readCount);
                                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(filePath + "\\" + fileName))
                {
                    File.Delete(filePath + "\\" + fileName);
                }
                throw ex;
            }
        }
        /// <summary>
        /// 获取文件夹中详细的文件列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <param name="ftpPath"></param>
        /// <returns></returns>
        public string[] GetDetailFilePath(string userId, string pwd, string ftpPath)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                var reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath));
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(userId, pwd);
                reqFtp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                reqFtp.UsePassive = _userpassive;
                reqFtp.KeepAlive = false;

                using (WebResponse response = reqFtp.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            result.Append(line);
                            result.Append("\n");
                            line = reader.ReadLine();
                        }
                        result.Remove(result.ToString().LastIndexOf('\n'), 1);
                    }
                }

                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                var downloadFiles = new string[0]; ;
                return downloadFiles;
            }
        }


        public DateTime GetFileModifyDateTime(string userId, string pwd, string ftpPath, string filePath, string fileName)
        {
            try
            {
                var reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath + fileName));

                reqFtp.UseBinary = true;
                //reqFTP.UsePassive = false;
                reqFtp.Credentials = new NetworkCredential(userId, pwd);

                reqFtp.Method = WebRequestMethods.Ftp.GetDateTimestamp;

                using (FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse())
                {
                    DateTime dt = response.LastModified;

                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool FtpFileExist(string userId, string pwd, string ftpPath, string fileName)
        {
            var request = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath + fileName));
            request.Credentials = new NetworkCredential(userId, pwd);
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {

            }
            return true;
        }

        public long GetFileSize(string userId, string pwd, string ftpPath, string fileName)
        {
            var reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath + fileName));

            reqFtp.UseBinary = true;
            reqFtp.UsePassive = _userpassive;
            reqFtp.Credentials = new NetworkCredential(userId, pwd);

            reqFtp.Method = WebRequestMethods.Ftp.GetFileSize;

            using (FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse())
            {
                long length = response.ContentLength;

                return length;
            }
        }
    }
}
