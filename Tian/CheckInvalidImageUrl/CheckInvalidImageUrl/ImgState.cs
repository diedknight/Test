using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInvalidImageUrl
{
    public class ImgState
    {
        public bool State { get; set; }
        public string StateStr { get; set; }

        public int Height { get; set; }
        public int BorderHeight { get; set; }

        public int Width { get; set; }
        public int BorderWidth { get; set; }
    }
}
