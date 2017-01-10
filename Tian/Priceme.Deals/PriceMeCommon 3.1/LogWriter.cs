using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PriceMeCommon
{
    public static class LogWriter
    {
        public static void WriteExceptionToDB(string ExceptionInfo, string ExceptionType, string errorPagePath, string ExceptionAppName, int level)
        {
            PriceMeDBA.CSK_Store_ExceptionCollect csk_Store_ExceptionCollect = new PriceMeDBA.CSK_Store_ExceptionCollect();

            csk_Store_ExceptionCollect.ExceptionInfo = ExceptionInfo;
            csk_Store_ExceptionCollect.ExceptionType = ExceptionType;
            csk_Store_ExceptionCollect.errorPagePath = errorPagePath;
            csk_Store_ExceptionCollect.ExceptionAppName = ExceptionAppName;
            csk_Store_ExceptionCollect.Level = level;

            csk_Store_ExceptionCollect.Save();
        }

        public static bool WriteLineToFile(string filePath, string info)
        {
            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
                if (!System.IO.Directory.Exists(fileInfo.DirectoryName))
                {
                    System.IO.Directory.CreateDirectory(fileInfo.DirectoryName);
                }
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(filePath))
                {
                    sw.WriteLine(info);
                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteExceptionToDB(ex.Message, "", "WriteLineToFile", "WriteLineToFile", 1);
            }
            return false;
        }
    }
}