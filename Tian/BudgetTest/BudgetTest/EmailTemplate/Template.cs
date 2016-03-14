using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTest.EmailTemplate
{
    public class Template
    {
        public static string Load(string fileName)
        {
            string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplate", fileName);

            if (!File.Exists(filename)) return "";

            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                using (StreamReader read = new StreamReader(fs))
                {
                   return read.ReadToEnd();   
                }
            }
        }
    }
}
