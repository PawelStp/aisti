using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.AuxiliaryGrammar
{
    public class GrammarException : Exception
    {
        public GrammarException()
        {
        }

        public GrammarException(string message)
            : base(message)
        {
        }

        public GrammarException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
