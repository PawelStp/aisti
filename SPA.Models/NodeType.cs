using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Models
{
    public enum NodeType
    {
        If,
        While,
        Assign,
        Call,
        StatementList,
        Statement,
        Procedure,
        Program,
        Variable,
        Operator,
        Value,
        
    }
}
