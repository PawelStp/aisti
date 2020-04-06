using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.LexicalRules
{
    public class Ident : ILexicalRules
    {
        public bool validate(string text)
        {
            Match synonim = Regex.Match(text, "^[A-Za-z][A-Za-z0-9#]*$");
            return synonim.Success;
        }
    }
}
