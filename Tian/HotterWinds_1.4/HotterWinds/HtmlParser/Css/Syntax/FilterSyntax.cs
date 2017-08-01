using Pricealyser.Crawler.HtmlParser.Css.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Syntax
{
    public class FilterSyntax : AbsSyntax
    {
        public override AbsSyntax Init(Tokens<Lexer.Token> tokens)
        {
            var token = SyntaxUtil.ReadSkipIgnoreToken(tokens);
            if (token == null || token.Text != ":")
            {
                tokens.Rollback();
                return null;
            }

            if (tokens.StartIndex == 0 || tokens.Read(-2).Id.Type == WordType.Ignore)
            {
                tokens.IndexMove(-1);
                this.InsertLexerToken(tokens);
                tokens.Commit();

                return new AllElementSyntax();
            }

            string filterName = this.ReadFilterName(tokens);
            if (string.IsNullOrEmpty(filterName.Trim()))
            {
                tokens.Rollback();
                return null;
            }

            token = SyntaxUtil.ReadSkipIgnoreToken(tokens);
            if (token == null || token.Text != "(")
            {
                if (token != null) tokens.IndexMove(-1);

                tokens.Commit();
                return new NotValueFilterSyntax(filterName);
            }

            tokens.IndexMove(-1);

            Tokens<Token> tempTokens = this.ReadAndCreateValuePart(tokens);

            if (tempTokens == null || tempTokens.Count == 0)
            {
                tokens.Rollback();
                return null;
            }

            if (tempTokens.Read(0).Text != "(" || tempTokens.Read(tempTokens.EndIndex).Text != ")")
            {
                tokens.Rollback();
                return null;
            }

            if (tempTokens.Count < 3)
            {
                tokens.Rollback();
                return null;
            }

            //if (tempTokens.Count == 3 && tempTokens.Read(1).Id.Name == "Number")
            //{
            //    tokens.Commit();
            //    return new ValueFilterSyntax(filterName, tempTokens.Read(1).Text);
            //}

            string selector = "";

            for (int i = 1; i < tempTokens.Count - 1; i++)
            {
                selector += tempTokens.Read(i).Text;
            }

            //valueFilter
            int tempValue = 0;
            if (int.TryParse(selector, out tempValue))
            {
                tokens.Commit();
                return new ValueFilterSyntax(filterName, tempValue.ToString());
            }

            //selectorFilter
            if (!string.IsNullOrEmpty(selector.Trim()))
            {
                tokens.Commit();
                return new SelectorFilterSyntax(filterName, selector);
            }

            tokens.Rollback();
            return null;
        }

        private string ReadFilterName(Tokens<Token> tokens)
        {
            SyntaxUtil.ReadTokenUntilNotIgnore(tokens);
            if (!tokens.HaveMoreToken()) return "";

            string filterName = "";

            while (tokens.HaveMoreToken())
            {
                var token = tokens.ReadAndMoveNext();

                if (token.Id.Type != WordType.Id && token.Id.Name != "-") break;

                filterName += token.Text;
            }

            //while (token.Id.Type != WordType.Ignore && token.Text != "(")
            //{
            //    filterName += token.Text;

            //    if (!tokens.HaveMoreToken()) break;

            //    token = tokens.ReadAndMoveNext();
            //}

            if (tokens.HaveMoreToken()) tokens.IndexMove(-1);
            
            return filterName;
        }

        private Tokens<Token> ReadAndCreateValuePart(Tokens<Token> tokens)
        {
            Tokens<Token> tempTokens = new Tokens<Token>();
            var token = SyntaxUtil.ReadSkipIgnoreToken(tokens);

            if (token == null || token.Text != "(") return null;

            int tempLeft = 1;
            int tempRight = 0;

            tempTokens.Add(token);

            while (true)
            {
                if (!tokens.HaveMoreToken()) break;

                token = tokens.ReadAndMoveNext();                
                tempTokens.Add(token);

                if (token.Text == "(") tempLeft++;
                if (token.Text == ")") tempRight++;

                if (tempLeft == tempRight) break;
            }

            return tempTokens;
        }

        public void InsertLexerToken(Tokens<Lexer.Token> tokens)
        {
            Token token = new Token(Word.GetWord(WordType.Operator, "*"), "*", tokens.CurIndex, 1);
            tokens.Insert(token);
        }

    }
}
