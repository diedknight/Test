using Pricealyser.Crawler.HtmlParser.Css.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Lexer
{
    public abstract class TokenAnalyze
    {
        public abstract Token Analyze(CodeReader reader);
    }
}
