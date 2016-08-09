using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentToosCVS.Email
{
    public interface IEmail
    {
        void Send(string subject, string body, params string[] to);

        void addAttachment(Stream s, string name);
    }
}
