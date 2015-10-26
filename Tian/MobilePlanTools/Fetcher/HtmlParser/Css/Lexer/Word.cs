using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Lexer
{
    public class Word
    {
        private static List<Word> _wordList = new List<Word>()
        {
            new Word("Id",WordType.Id),

            new Word("+",WordType.Operator),
            new Word("-",WordType.Operator),
            new Word("*",WordType.Operator),
            new Word("/",WordType.Operator),
            new Word("%",WordType.Operator),
            new Word(">",WordType.Operator),
            new Word(">=",WordType.Operator),
            new Word("<",WordType.Operator),
            new Word("<=",WordType.Operator),
            new Word("=",WordType.Operator),
            new Word("==",WordType.Operator),
            new Word("!",WordType.Operator),
            new Word("!=",WordType.Operator),
            new Word("^=",WordType.Operator),
            new Word("$=",WordType.Operator),
            new Word("~=",WordType.Operator),
            new Word("|=",WordType.Operator),
            new Word("*=",WordType.Operator),
            new Word("++",WordType.Operator),
            new Word("--",WordType.Operator),
            new Word("||",WordType.Operator),
            new Word("&&",WordType.Operator),
            new Word("&",WordType.Operator),
            new Word("|",WordType.Operator),
            new Word("~",WordType.Operator),
            //new Word("^",WordType.Operator),
            //new Word("$",WordType.Operator),
            

            new Word("(",WordType.Delimiter),
            new Word(")",WordType.Delimiter),
            new Word("[",WordType.Delimiter),
            new Word("]",WordType.Delimiter),
            new Word("{",WordType.Delimiter),
            new Word("}",WordType.Delimiter),
            new Word("\"",WordType.Delimiter),
            new Word("'",WordType.Delimiter),
            new Word(";",WordType.Delimiter),
            new Word(",",WordType.Delimiter),
            new Word(":",WordType.Delimiter),
            new Word(".",WordType.Delimiter),
            new Word("#",WordType.Delimiter),
            //new Word(" ",WordType.Delimiter),

            //new Word("true",WordType.Keyword),
            //new Word("false",WordType.Keyword),

            new Word("Number",WordType.Const),
            new Word("String",WordType.Const),
            new Word("Bool",WordType.Const),

            new Word("Ignore",WordType.Ignore),
        };

        public static List<Word> GetWords(WordType type)
        {
            return _wordList.Where(item => item.Type == type).ToList();
        }

        public static Word GetWord(WordType type, string name)
        {
            return _wordList.FirstOrDefault(item => item.Type == type && item.Name == name);
        }

        public static Word GetWord(int index)
        {
            return _wordList[index];
        }

        public string Name { get; private set; }
        public WordType Type { get; private set; }

        public Word(string name, WordType type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
