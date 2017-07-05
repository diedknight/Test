using Pricealyser.Crawler.HtmlParser.Css.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Syntax
{
    public class SyntaxUtil
    {
        public static void ReadTokenUntilNotIgnore(Tokens<Token> tokens)
        {
            if (!tokens.HaveMoreToken()) return;

            //去掉开始的空格
            var token = tokens.ReadAndMoveNext();
            while (token.Id.Type == WordType.Ignore)
            {
                if (!tokens.HaveMoreToken()) return;

                token = tokens.ReadAndMoveNext();
            }

            tokens.IndexMove(-1);   //向前移一位
        }

        public static Token ReadSkipIgnoreToken(Tokens<Token> tokens)
        {
            if (!tokens.HaveMoreToken()) return null;

            ReadTokenUntilNotIgnore(tokens);

            if (!tokens.HaveMoreToken()) return null;

            return tokens.ReadAndMoveNext();
        }

        public static string ConvertToString(Tokens<Token> tokens)
        {
            string str = "";
            for (int i = tokens.CurIndex; i <= tokens.EndIndex; i++)
            {
                str += tokens.Read(i - tokens.CurIndex).Text;
            }

            return str;
        }

        public static bool HaveMoreTokenSkipIgnore(Tokens<Token> tokens)
        {
            if (!tokens.HaveMoreToken()) return false;

            for (int i = tokens.CurIndex; i <= tokens.EndIndex; i++)
            {
                if (tokens.Read(i - tokens.CurIndex).Id.Type != WordType.Ignore) return true;
            }

            return false;
        }
    }
}
