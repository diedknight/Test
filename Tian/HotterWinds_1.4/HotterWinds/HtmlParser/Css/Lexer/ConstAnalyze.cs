using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Lexer
{
    public class ConstAnalyze:TokenAnalyze
    {
        public override Token Analyze(Reader.CodeReader reader)
        {
            char prevChar = reader.CurIndex <= 0 ? '\0' : reader.Read(-1);
            string str = this.Reads(reader);
            char NextChar = reader.CurIndex >= reader.StrLength ? '\0' : reader.Read(0);

            if (str == "")
            {
                reader.Rollback();
                return null;
            }

            Token token = null;

            if (str == "true") token = new Token(Word.GetWord(WordType.Const, "Bool"), str, reader.StartIndex, 4);
            else if (str == "false") token = new Token(Word.GetWord(WordType.Const, "Bool"), str, reader.StartIndex, 5);
            else if (Regex.IsMatch(str.Trim(), @"^-?(([0-9]+\.[0-9]+)|([0-9]+))$")) token = new Token(Word.GetWord(WordType.Const, "Number"), str, reader.StartIndex, str.Length);
            else if (prevChar == '\'' && NextChar == '\'' || prevChar == '"' && NextChar == '"') token = new Token(Word.GetWord(WordType.Const, "String"), str, reader.StartIndex, str.Length);
            else
            {
                reader.Rollback();
                return null;
            }

            reader.Commit();

            return token;
        }

        private string Reads(Reader.CodeReader reader)
        {            
            string str = "";

            if (!reader.HaveMoreCode()) return str;

            char prevChar = reader.CurIndex <= 0 ? '\0' : reader.Read(-1);

            //判断字符串
            if (prevChar == '\'' || prevChar == '"')
            {
                char stopChar = prevChar;

                while (reader.HaveMoreCode())
                {
                    char c = reader.ReadAndMoveNext();

                    if (stopChar == c)
                    {
                        reader.IndexMove(-1);
                        break;
                    }

                    str += c;                    
                }
            }

            if (str != "") return str;


            //判断数字
            while (reader.HaveMoreCode())
            {
                char c = reader.ReadAndMoveNext();

                if (Char.IsLetterOrDigit(c)) { str += c; continue; }
                
                if (c == '_') { str += c; continue; }
                if (c == '.') { str += c; continue; }
                //if (c == ' ') { str += c; continue; }

                reader.IndexMove(-1);
                break;
            }

            return str;
        }
    }
}
