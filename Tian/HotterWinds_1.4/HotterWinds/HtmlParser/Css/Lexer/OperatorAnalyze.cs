using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Lexer
{
    public class OperatorAnalyze : TokenAnalyze
    {
        public override Token Analyze(Reader.CodeReader reader)
        {
            string resStr = "";
            Word resWord = null;

            if (!reader.HaveMoreCode()) return null;

            string singleStr = reader.ReadAndMoveNext().ToString();
            Word singleWord = Word.GetWord(WordType.Operator,singleStr);

            string doubleStr = "";
            Word doubleWord = null;

            if (reader.HaveMoreCode())
            {
                doubleStr += singleStr + reader.ReadAndMoveNext().ToString();
                doubleWord = Word.GetWord(WordType.Operator,doubleStr);

                //双字符运算符为空时，reader退一格
                if (doubleWord == null) reader.IndexMove(-1);
            }

            resStr = doubleWord == null ? singleStr : doubleStr;
            resWord = doubleWord == null ? singleWord : doubleWord;

            if (resWord == null)
            {
                reader.Rollback();
                return null;
            }

            var token = new Token(resWord, resStr, reader.StartIndex, resStr.Length);

            reader.Commit();
            return token;
        }
    }
}
