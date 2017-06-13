using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css
{
    public class Tokens<T>
    {
        private List<T> _list = new List<T>();
        private int _startIndex = 0;
        private int _curIndex = 0;
        private int _endIndex = 0;

        public int CurIndex
        {
            get { return this._curIndex; }
        }

        public int StartIndex
        {
            get { return this._startIndex; }
        }
        public int EndIndex
        {
            get { return this._endIndex; }
        }

        public int Count
        {
            get { return this._list.Count; }
        }

        public void Add(T token)
        {
            if (token == null) return;

            this._list.Add(token);
            this._endIndex = this._list.Count - 1;
        }

        public bool HaveMoreToken()
        {
            return this._curIndex < this._list.Count;
        }

        public T ReadAndMoveNext()
        {
            T t = this._list[this._curIndex];
            this._curIndex++;

            return t;
        }

        public T Read(int distance)
        {
            return this._list[this._curIndex + distance];
        }

        public void Rollback()
        {
            this._curIndex = this._startIndex;
        }

        public void Commit()
        {
            this._startIndex = this._curIndex;
        }

        public void IndexMove(int distance)
        {
            if (this._curIndex + distance < this._startIndex) throw new IndexOutOfRangeException();
            if (this._curIndex + distance > this._endIndex) throw new IndexOutOfRangeException();

            this._curIndex = this._curIndex + distance;
        }

        public T FirstOrDefault(Func<T, bool> Func)
        {
            return this._list.FirstOrDefault(item => Func(item));
        }

        public void Insert(T token)
        {
            if (this.CurIndex > this.EndIndex) return;

            this._list.Insert(this.CurIndex, token);

            this._curIndex++;
            this._endIndex++;            
        }

    }
}
