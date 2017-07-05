using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Lexer
{
    public class IdAnalyze:TokenAnalyze
    {
        public override Token Analyze(Reader.CodeReader reader)
        {
            string str = this.Reads(reader);

            if (str == "")
            {
                reader.Rollback();
                return null;
            }

            Token token = new Token(Word.GetWord(WordType.Id,"Id"), str, reader.StartIndex, str.Length);

            reader.Commit();

            return token;
        }

        private string Reads(Reader.CodeReader reader)
        {
            string str = "";

            while (reader.HaveMoreCode())
            {
                char c = reader.ReadAndMoveNext();

                if (Char.IsLetterOrDigit(c)) { str += c; continue; }
                if (c=='_') { str += c; continue; }

                reader.IndexMove(-1);
                break;
            }

            return str;
        }

    }
}
