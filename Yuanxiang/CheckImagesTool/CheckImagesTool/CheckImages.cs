using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckImagesTool
{
    public class CheckImages
    {
        StreamWriter _sw;
        public StreamWriter SW
        {
            get { return _sw; }
            set { _sw = value; }
        }

        private string imagePathDriver;
        private string imagePathByKeyword;
        private string largeImagePatch;

        string targetPath;
        string targetIP;
        string userID;
        string password;
        private string targetLargePath;

        private DateTime imageCreateon;
        private Dictionary<string, string> dicTable;
        private bool isDebug;

        public void Check()
        {
            Writer("Begin......" + DateTime.Now);
            imagePathDriver = ConfigurationManager.AppSettings["CheckImagePathDriver"].ToString();
            string imagePath = ConfigurationManager.AppSettings["CheckImagePath"].ToString();
            largeImagePatch = ConfigurationManager.AppSettings["LargeImagePatch"].ToString();

            targetPath = System.Configuration.ConfigurationManager.AppSettings["targetOriginalPath"];
            targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIPAddress"];
            userID = System.Configuration.ConfigurationManager.AppSettings["targetuserid"];
            password = System.Configuration.ConfigurationManager.AppSettings["targetpassword"];
            targetLargePath = ConfigurationManager.AppSettings["targetLargePath"].ToString();

            imagePathByKeyword = ConfigurationManager.AppSettings["CheckImagePathByKeyword"].ToString().ToLower();
            DateTime.TryParse(ConfigurationManager.AppSettings["CreatedTimeOfImage"].ToString(), out imageCreateon);
            bool.TryParse(ConfigurationManager.AppSettings["debug"].ToString(), out isDebug);

            string tableAndColumn = ConfigurationManager.AppSettings["TableAndColumn"].ToString();
            dicTable = new Dictionary<string, string>();
            string[] temps = tableAndColumn.Split(';');
            foreach (string temp in temps)
            {
                if (string.IsNullOrEmpty(temp)) continue;

                string[] tbls = temp.Split(',');
                if (!dicTable.ContainsKey(tbls[0]))
                    dicTable.Add(tbls[0], tbls[1]);
            }

            string path = imagePathDriver + imagePath;

            DirectoryInfo info = new DirectoryInfo(path);
            FileSystemInfo[] infos = info.GetFileSystemInfos();
            foreach (FileSystemInfo fs in infos)
            {
                if (Directory.Exists(fs.FullName))
                {
                    if (string.IsNullOrEmpty(imagePathByKeyword) || fs.FullName.ToLower().Contains(imagePathByKeyword))
                    {
                        string dirPath = fs.FullName.Substring(fs.FullName.LastIndexOf('\\')).Replace("\\", "");
                        int index = dirPath.ToLower().LastIndexOf(imagePathByKeyword);
                        if (index == 0)
                        {
                            Writer("check " + path);
                            GetDirectorys(fs.FullName);
                        }
                    }
                }
            }
            
            Writer("End......." + DateTime.Now);
        }

        private void GetDirectorys(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            FileSystemInfo[] infos = info.GetFileSystemInfos();
            foreach (FileSystemInfo fs in infos)
            {
                if (File.Exists(fs.FullName))
                {
                    if (fs.FullName.Contains("_ms") || fs.FullName.Contains("_m") || fs.FullName.Contains("_s")) continue;

                    if (fs.CreationTime < imageCreateon)
                        CheckDatabase(fs.FullName);
                }
                else if (Directory.Exists(fs.FullName))
                {
                    GetDirectorys(fs.FullName);
                }
            }
        }

        private void CheckDatabase(string path)
        {
            string imagename = path.Substring(path.LastIndexOf('\\')).Replace("\\", "");
            bool isCheck = false;
            foreach (KeyValuePair<string, string> pair in dicTable)
            {
                string sql = "select count(" + pair.Value + ") from " + pair.Key + " where " + pair.Value + " like '%" + imagename + "%'";
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandSql = sql;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandType = CommandType.Text;
                IDataReader dr = sp.ExecuteReader();
                while (dr.Read())
                {
                    int count = 0;
                    int.TryParse(dr[0].ToString(), out count);
                    if (count > 0)
                        isCheck = true;
                }
                dr.Close();

                if (isCheck)
                    break;
            }

            if (!isCheck)
            {
                Writer("iamge: " + path);
                if (!isDebug)
                {
                    string imagepath = path.Substring(0, path.LastIndexOf('\\'));
                    DeleteImages(imagepath, imagename);
                }
            }
        }

        private void DeleteImages(string path, string fileName)
        {
            CopyFile.NetWorkCopy.CopyFile(targetIP, targetPath, userID, password, path, fileName);
            File.Delete(path + "\\" + fileName);
            string shortFileName = fileName.Substring(0, fileName.LastIndexOf('.'));
            string ex = fileName.Substring(fileName.LastIndexOf('.'));

            string sfileName = shortFileName + "_s" + ex;
            CopyFile.NetWorkCopy.CopyFile(targetIP, targetPath, userID, password, path, sfileName);
            File.Delete(path + "\\" + sfileName);

            string mfileName = shortFileName + "_m" + ex;
            CopyFile.NetWorkCopy.CopyFile(targetIP, targetPath, userID, password, path, mfileName);
            File.Delete(path + "\\" + mfileName);

            string msfileName = shortFileName + "_ms" + ex;
            CopyFile.NetWorkCopy.CopyFile(targetIP, targetPath, userID, password, path, msfileName);
            File.Delete(path + "\\" + msfileName);

            string lpath = path.Replace(imagePathDriver, largeImagePatch);
            string lfileName = shortFileName + "_l" + ex;

            if (File.Exists(lpath + "\\" + lfileName))
            {
                CopyFile.NetWorkCopy.CopyFile(targetIP, targetLargePath, userID, password, path, lfileName);
                File.Delete(lpath + "\\" + lfileName);
            }
        }

        private void Writer(string info)
        {
            System.Console.WriteLine(info);

            _sw.WriteLine(info);
            _sw.WriteLine(_sw.NewLine);
            _sw.Flush();
        }
    }
}
