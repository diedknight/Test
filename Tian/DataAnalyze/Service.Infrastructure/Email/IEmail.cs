using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.Email
{
    public interface IEmail
    {
        void Send(string subject, string body, params string[] to);
    }
}
