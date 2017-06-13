using Pricealyser.Crawler.HtmlParser.Css.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Lexer
{
    public class LexerBuilder 
    {
        private static TokenAnalyze _operatorAnalyze = new OperatorAnalyze();
        private static TokenAnalyze _constAnalyze = new ConstAnalyze();
        private static TokenAnalyze _delimiterAnalyze = new DelimiterAnalyze();
        private static TokenAnalyze _idAnalyze = new IdAnalyze();
        private static TokenAnalyze _ignoreAnalyze = new IgnoreAnalyze();

        public static Tokens<Token> Create(string code)
        {
            Tokens<Token> lex = new Tokens<Token>();

            CodeReader reader = new CssReader(code);

            while (reader.HaveMoreCode())
            {
                Token token = null;

                if (token == null) token = _delimiterAnalyze.Analyze(reader);
                if (token == null) token = _operatorAnalyze.Analyze(reader);
                if (token == null) token = _constAnalyze.Analyze(reader);
                if (token == null) token = _idAnalyze.Analyze(reader);
                if (token == null) token = _ignoreAnalyze.Analyze(reader);

                lex.Add(token);
            }

            return lex;
        }


    }
}
