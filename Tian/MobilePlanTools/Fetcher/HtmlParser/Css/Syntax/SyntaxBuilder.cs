using Pricealyser.Crawler.HtmlParser.Css.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Syntax
{
    public class SyntaxBuilder
    {
        public static List<Tokens<AbsSyntax>> Create(Tokens<Token> tokens)
        {
            List<Tokens<AbsSyntax>> list = new List<Tokens<AbsSyntax>>();

            Tokens<AbsSyntax> syntaxs = new Tokens<AbsSyntax>();

            while (SyntaxUtil.HaveMoreTokenSkipIgnore(tokens))
            {
                bool isFilter = false;
                if (tokens.Read(0).Id.Type != WordType.Ignore)
                {
                    if (tokens.StartIndex == 0) isFilter = false;
                    else if (tokens.Read(-1).Id.Type == WordType.Ignore) isFilter = false;
                    else isFilter = true;
                }

                AbsSyntax syntax = null;

                if (syntax == null) syntax = new AttrSyntax().Init(tokens);
                if (syntax == null) syntax = new ClassSyntax().Init(tokens);
                if (syntax == null) syntax = new AllElementSyntax().Init(tokens);
                if (syntax == null) syntax = new ElementSyntax().Init(tokens);
                if (syntax == null) syntax = new IdSyntax().Init(tokens);
                if (syntax == null) syntax = new RelationSyntax().Init(tokens);
                if (syntax == null) syntax = new FilterSyntax().Init(tokens);

                if (syntax == null) throw new Exception("syntax error " + SyntaxUtil.ConvertToString(tokens));

                syntax.IsFilter = isFilter;

                syntaxs.Add(syntax);
            }

            //有逗号就分段
            Tokens<AbsSyntax> tempToken = new Tokens<AbsSyntax>();
            while (syntaxs.HaveMoreToken())
            {
                var token = syntaxs.ReadAndMoveNext();

                if ((token is RelationSyntax && ((RelationSyntax)token).Relation == ","))
                {
                    list.Add(tempToken);
                    tempToken = new Tokens<AbsSyntax>();
                    continue;
                }

                tempToken.Add(token);
            }

            list.Add(tempToken);

            return list;
        }
    }
}
