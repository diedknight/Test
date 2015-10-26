using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Lexer
{
    public class DelimiterAnalyze : TokenAnalyze
    {
        public override Token Analyze(Reader.CodeReader reader)
        {
            if (!reader.HaveMoreCode()) return null;

            string c = reader.ReadAndMoveNext().ToString();

            var word = Word.GetWord(WordType.Delimiter,c.ToString());

            if (word == null)
            {
                reader.Rollback();
                return null;
            }

            var token = new Token(word, c, reader.StartIndex, 1);

            reader.Commit();

            return token;
        }
    }
}
