using Pricealyser.Crawler.HtmlParser.Css.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Syntax
{
    public class RelationSyntax : AbsSyntax
    {
        public string Relation { get; private set; }

        public override AbsSyntax Init(Tokens<Token> tokens)
        {
            SyntaxUtil.ReadTokenUntilNotIgnore(tokens);

            if (!tokens.HaveMoreToken()) return null;

            var token = tokens.ReadAndMoveNext();

            switch (token.Text)
            {
                case ">": this.Relation = ">"; tokens.Commit(); return this;
                case "+": this.Relation = "+"; tokens.Commit(); return this;
                case "~": this.Relation = "~"; tokens.Commit(); return this;
                case ",": this.Relation = ","; tokens.Commit(); return this;
            }

            tokens.Rollback();
            return null;
        }

        public override string ToString()
        {
            return Relation;
        }

    }
}
