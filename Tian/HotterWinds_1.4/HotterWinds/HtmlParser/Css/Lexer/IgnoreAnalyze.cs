using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Lexer
{
    public class IgnoreAnalyze:TokenAnalyze
    {
        public override Token Analyze(Reader.CodeReader reader)
        {
            if (!reader.HaveMoreCode()) return null;

            var token = new Token(Word.GetWord(WordType.Ignore,"Ignore"), reader.ReadAndMoveNext().ToString(), reader.StartIndex, 1);

            reader.Commit();

            return token;
        }
    }
}
