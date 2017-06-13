using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Reader
{
    public abstract class CodeReader
    {
        public abstract int CurIndex { get; }
        public abstract int StartIndex { get; }
        public abstract int StrLength { get; }
        public abstract int EndIndex { get; }

        public abstract bool HaveMoreCode();
        public abstract char ReadAndMoveNext();
        public abstract char Read(int distance);

        public abstract void IndexMove(int distance);

        public abstract void Rollback();
        public abstract void Commit();
    }
}
