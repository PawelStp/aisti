using SPA.QueryProcessor.LexicalRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SPA.QueryProcessor.DesignEntity;

namespace SPA.QueryProcessor.AuxiliaryGrammar
{
    public class StmtRef : AbstractAuxiliaryGrammar
    {
        public StmtRef(string text) : base(text) { }
        public Statement Type { get; set; }

        public override void InitLexicalRules()
        {
            lexicalRules.Add(new Ident());
            lexicalRules.Add(new Placeholder());
            lexicalRules.Add(new IntegerRule());
        }
    }
}
