using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Reader
{
    public class CssReader : CodeReader
    {
        private string _cssSelector = "";
        private int _startIndex = 0;
        private int _curIndex = 0;
        private int _endIndex = 0;

        public override int CurIndex
        {
            get { return this._curIndex; }
        }

        public override int StartIndex
        {
            get { return this._startIndex; }
        }
        public override int EndIndex
        {
            get { return this._endIndex; }
        }

        public override int StrLength
        {
            get { return this._cssSelector.Length; }
        }

        public CssReader(string cssSelector)
        {
            if (cssSelector == null) throw new ArgumentNullException();

            this._cssSelector = cssSelector;
            this._endIndex = cssSelector.Length - 1;
        }

        public override bool HaveMoreCode()
        {
            return this._curIndex < this._cssSelector.Length;
        }

        public override char ReadAndMoveNext()
        {
            char c = this._cssSelector[this._curIndex];
            this._curIndex++;

            return c;
        }

        public override char Read(int distance)
        {
            return this._cssSelector[this._curIndex + distance];
        }

        public override void Rollback()
        {
            this._curIndex = this._startIndex;
        }

        public override void Commit()
        {
            this._startIndex = this._curIndex;
        }

        public override void IndexMove(int distance)
        {
            if (this._curIndex + distance < this._startIndex) throw new IndexOutOfRangeException();
            if (this._curIndex + distance > this._endIndex) throw new IndexOutOfRangeException();

            this._curIndex = this._curIndex + distance;
        }
    }
}
