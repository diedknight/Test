using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.Request
{
    public interface IWebRequest
    {
        void AddPara(string name, string value);
        void AddPara(string name, byte[] value);
        bool IsExistPara(string name);
        void RemovePara(string name);
        void ClearPara();
        string GetTextPara(string name);
        byte[] GetDataPara(string name);
        string Upload();
        string Post();
        string Get();
        byte[] GetFile();

        string GetCookie(string name);
        void SetCookie(string name, string value);

    }
}
