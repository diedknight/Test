using Pricealyser.Crawler.HtmlParser.Css.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Syntax
{
    public class AllElementSyntax : AbsSyntax
    {
        public override AbsSyntax Init(Tokens<Lexer.Token> tokens)
        {
            var token = SyntaxUtil.ReadSkipIgnoreToken(tokens);

            if (token == null) return null;

            if (token.Text == "*")
            {
                tokens.Commit();
                return this;
            }            

            tokens.Rollback();
            return null;
        }

        public override string ToString()
        {
            return "*";
        }

    }
}
