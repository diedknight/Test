using Pricealyser.Crawler.HtmlParser.Css.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Syntax
{
    public class ElementSyntax : AbsSyntax
    {
        public string TagName { get; private set; }

        public override AbsSyntax Init(Tokens<Token> tokens)
        {
            //去掉开始的空格
            SyntaxUtil.ReadTokenUntilNotIgnore(tokens);

            if (!tokens.HaveMoreToken()) return null;

            var prevToken = tokens.CurIndex <= 0 ? null : tokens.Read(-1);
            var token = tokens.ReadAndMoveNext();

            if ((prevToken == null || prevToken.Id.Type == WordType.Ignore) && token.Id.Type == WordType.Id)
            {
                this.TagName = token.Text;
                tokens.Commit();
                return this;
            }

            tokens.Rollback();
            return null;
        }

        public override string ToString()
        {
            return TagName;
        }
    }
}
