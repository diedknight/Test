using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Pricealyser.SiteMap
{
    public class CopySiteMapXml
    {
        string xmlPath = System.Configuration.ConfigurationManager.AppSettings["XmlPath"].ToString();
        string webPath = System.Configuration.ConfigurationManager.AppSettings["WebPath"].ToString();

        public void SiteMapXml()
        {
            DirectoryInfo[] infos = (new DirectoryInfo(xmlPath)).GetDirectories();

            DirectoryInfo[] webinfos = (new DirectoryInfo(webPath)).GetDirectories();

            foreach (DirectoryInfo di in infos)
            {
                string diname = di.Name.ToLower();
                string xmlfile = di.FullName + @"\SiteMap.xml";

                foreach (DirectoryInfo webdi in webinfos)
                {
                    if (diname == webdi.Name.ToLower())
                    {
                        DirectoryInfo[] dirs = (new DirectoryInfo(webdi.FullName)).GetDirectories();
                        foreach (DirectoryInfo dir in dirs)
                        {
                            string path = dir.FullName + @"\SiteMap";
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);

                            path = dir.FullName + @"\SiteMap\SiteMap.xml";
                            if (File.Exists(path))
                                File.Delete(path);

                            File.Copy(xmlfile, path);
                        }

                        break;
                    }
                }
            }
        }
    }
}
