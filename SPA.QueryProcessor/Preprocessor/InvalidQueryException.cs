using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.Preprocessor
{
    public class InvalidQueryException : Exception
    {
        public InvalidQueryException(string message) : base(message)
        {
        }
    }
}
