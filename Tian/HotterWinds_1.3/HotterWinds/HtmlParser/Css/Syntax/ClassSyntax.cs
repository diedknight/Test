using Pricealyser.Crawler.HtmlParser.Css.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Syntax
{
    public class ClassSyntax : AbsSyntax
    {
        public string ClassName { get; private set; }

        public override AbsSyntax Init(Tokens<Token> tokens)
        {
            //去掉开始的空格
            SyntaxUtil.ReadTokenUntilNotIgnore(tokens);

            if (!tokens.HaveMoreToken()) return null;
            if (tokens.Read(0).Text != ".") return null;

            while (true)
            {
                if (!tokens.HaveMoreToken()) break;

                var token = tokens.ReadAndMoveNext();
                if (token.Id.Type == WordType.Ignore) break;
                if (token.Text == ":") break;
                if (token.Text == "[") break;

                if (token.Id.Name == ".")
                {
                    this.ClassName += " ";
                }
                else
                {
                    this.ClassName += token.Text;
                }
            }

            if (!string.IsNullOrEmpty(this.ClassName))
            {
                this.ClassName = this.ClassName.Trim();

                if (tokens.HaveMoreToken()) tokens.IndexMove(-1);

                tokens.Commit();

                return this;
            }
            
            tokens.Rollback();
            return null;
        }

        public override string ToString()
        {
            return "." + ClassName.Replace(" ", ".");
        }
    }
}
