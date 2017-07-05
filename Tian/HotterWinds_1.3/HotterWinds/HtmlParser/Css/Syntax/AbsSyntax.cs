using Pricealyser.Crawler.HtmlParser.Css.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Syntax
{
    public abstract class AbsSyntax
    {
        private bool _isFilter = false;

        public bool IsFilter
        {
            get { return this._isFilter; }
            set { this._isFilter = value; }
        }

        public abstract AbsSyntax Init(Tokens<Token> tokens);
    }
}
