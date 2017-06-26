using Pricealyser.Crawler.HtmlParser.Css.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Syntax
{
    public class IdSyntax : AbsSyntax
    {
        public string IdName { get; private set; }

        public override AbsSyntax Init(Tokens<Token> tokens)
        {
            //去掉开始的空格
            SyntaxUtil.ReadTokenUntilNotIgnore(tokens);

            if (!tokens.HaveMoreToken()) return null;
            if (tokens.Read(0).Text != "#") return null;

            tokens.ReadAndMoveNext();

            while (tokens.HaveMoreToken())
            {
                var token = tokens.ReadAndMoveNext();
                
                if (token.Id.Type == WordType.Ignore) break;                            

                this.IdName += token.Text;                
            }

            if (!string.IsNullOrEmpty(this.IdName.Trim()))
            {
                tokens.Commit();
                return this;
            }

            tokens.Rollback();
            return null;
        }

        public override string ToString()
        {
            return "#" + IdName;
        }

    }
}
