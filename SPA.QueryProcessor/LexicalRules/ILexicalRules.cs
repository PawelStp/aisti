using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.LexicalRules
{
    public interface ILexicalRules
    {
        bool validate(string text);
    }
}
