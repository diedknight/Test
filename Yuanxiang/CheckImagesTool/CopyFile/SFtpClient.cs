using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Renci.SshNet;
using System.IO;

namespace CopyFile
{
    public static class SFtpClient
    {
        static SftpClient sftp;

        public static void SetSFtpClient(string host, int port, string user, string passPhrase)
        {
            sftp = new SftpClient(host, port, user, passPhrase);

            if (sftp != null)
            {
                sftp.ConnectionInfo.RetryAttempts = 1500;
                sftp.ConnectionInfo.Timeout = new TimeSpan(0, 3, 0);
            }
        }

        public static bool Connect()
        {
            if (sftp == null)
                return false;

            if (sftp.IsConnected)
                return true;

            try
            {
                sftp.Connect();
                return true;
            }
            catch (Exception ex)
            {
                string server = string.Format("{0}:{1}", sftp.ConnectionInfo.Username, sftp.ConnectionInfo.Host);
                return false;
            }
        }

        public static void DisConnect()
        {
            if (sftp == null)
                return;
            if (!sftp.IsConnected)
                return;

            try
            {
                sftp.Disconnect();
                sftp.Dispose();
                sftp = null;
            }
            catch (Exception ex) { }
        }

        public static List<string> ListFiles(string path)
        {
            if (!Connect())
            {
                return null;
            }

            List<string> files = new List<string>();
            try
            {
                sftp.ChangeDirectory("/");
                sftp.ListDirectory(path).ToList().ForEach(f =>
                {

                    files.Add(f.FullName);
                });

                return files;
            }
            catch (Exception ex) { return null; }
        }

        public static bool Download(string remoteFileName, string localFileName)
        {
            if (!Connect())
                return false;

            try
            {
                sftp.ChangeDirectory("/");
                FileStream fs = File.OpenWrite(localFileName);
                sftp.DownloadFile(remoteFileName, fs);
                fs.Close();
                return true;
            }
            catch (Exception ex) { return false; }
        }
    }
}
