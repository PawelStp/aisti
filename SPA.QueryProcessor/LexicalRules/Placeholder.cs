using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.LexicalRules
{
    public class Placeholder : ILexicalRules
    {
        public bool validate(string text)
        {
            Match placeholder = Regex.Match(text, "^_$");
            return placeholder.Success;
        }
    }
}
