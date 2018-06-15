using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEPREmail.Data
{
    public class WeeklyDealsUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public bool IsAcceptGDPR { get; set; }
    }
}
