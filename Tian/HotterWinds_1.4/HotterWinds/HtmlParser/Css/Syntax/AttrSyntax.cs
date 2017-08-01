using Pricealyser.Crawler.HtmlParser.Css.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Syntax
{
    public class AttrSyntax : AbsSyntax
    {
        public string AttrName { get; private set; }
        public string Operation { get; private set; }
        public string Value { get; private set; }

        public override AbsSyntax Init(Tokens<Lexer.Token> tokens)
        {
            Tokens<Token> tempTokens = new Tokens<Token>();

            var token = SyntaxUtil.ReadSkipIgnoreToken(tokens);

            if (token == null || token.Text != "[")
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


            tempTokens.Add(token);
            while (true)
            {
                token = SyntaxUtil.ReadSkipIgnoreToken(tokens);

                if (token == null) break;

                tempTokens.Add(token);

                if (token.Text == "]") break;
            }

            if (tempTokens.Read(0).Text != "[" || tempTokens.Read(tempTokens.EndIndex).Text != "]")
            {
                tokens.Rollback();
                return null;
            }

            //属性节点至少有5个
            if (tempTokens.Count < 5)
            {
                tokens.Rollback();
                return null;
            }

            List<string> operatorList = new List<string>();
            operatorList.Add("*=");
            operatorList.Add("|=");
            operatorList.Add("~=");
            operatorList.Add("$=");
            operatorList.Add("=");
            operatorList.Add("!=");
            operatorList.Add("^=");


            while (tempTokens.HaveMoreToken())
            {
                var tempToken = tempTokens.ReadAndMoveNext();

                if (tempToken.Text == "[" || tempToken.Text == "]") continue;

                if (!operatorList.Contains(tempToken.Text))
                {
                    if (string.IsNullOrEmpty(this.Operation))
                    {
                        this.AttrName += tempToken.Text;
                    }
                    else
                    {
                        this.Value += tempToken.Text;
                    }
                }
                else
                {
                    this.Operation = tempToken.Text;
                }
            }

            //清除单引号和双引号
            this.Value = this.ValueTrim(this.Value);


            //var tempToken = tempTokens.FirstOrDefault(item => item.Id.Type == WordType.Id);
            //this.AttrName = tempToken == null ? "" : tempToken.Text;

            //tempToken = tempTokens.FirstOrDefault(item => item.Id.Type == WordType.Operator);
            //this.Operation = tempToken == null ? "" : tempToken.Text;

            //tempToken = tempTokens.FirstOrDefault(item => item.Id.Type == WordType.Const);
            //this.Value = tempToken == null ? "" : tempToken.Text;

            tokens.Commit();            

            return this;
        }

        public void InsertLexerToken(Tokens<Lexer.Token> tokens)
        {
            Token token = new Token(Word.GetWord(WordType.Operator, "*"), "*", tokens.CurIndex, 1);
            tokens.Insert(token);
        }

        public override string ToString()
        {
            return "[" + AttrName + Operation + "'" + Value + "']";
        }

        private string ValueTrim(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            if (value.Length < 2) return value;

            string startChar = value.Substring(0, 1);
            string endChar = value.Substring(value.Length - 1);

            if (startChar == endChar && (startChar == "'" || startChar == "\"")) return value.Substring(1, value.Length - 2);

            return value;
        }

    }
}
