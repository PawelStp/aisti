using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.LexicalRules
{
    public class IntegerRule : ILexicalRules
    {
        public bool validate(string text)
        {
            Match placeholder = Regex.Match(text, "^[0-9]+$");
            return placeholder.Success;
        }
    }
}
