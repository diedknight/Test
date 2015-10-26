using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Lexer
{
    public class Token
    {
        public Word Id { get; private set; }
        public string Text { get; private set; }

        public int IndexStart { get; private set; }
        public int Length { get; private set; }

        public Token(Word word, string text, int indexStart, int length)
        {
            this.Id = word;
            this.Text = text;
            this.IndexStart = indexStart;
            this.Length = length;
        }

    }
}
