using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricealyser.Crawler.HtmlParser.Css.Lexer
{
    public enum WordType
    {
        /// <summary>
        /// 关键字
        /// </summary>
        Keyword,
        
        /// <summary>
        /// 标识符
        /// </summary>
        Id,

        /// <summary>
        /// 常量
        /// </summary>
        Const,

        /// <summary>
        /// 操作符
        /// </summary>
        Operator,

        /// <summary>
        /// 界符
        /// </summary>
        Delimiter,

        Ignore
    }
}
