using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public interface ICrawlDataInfo<T>
    {
        string ToTextLine();
        string WritePhones();
        T SetInfoFromTextLine(string textLine);
    }
}
