using SPA.QueryProcessor.LexicalRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.AuxiliaryGrammar
{
    public class ProcRef : AbstractAuxiliaryGrammar
    {
        public ProcRef(string text) : base(text) { }

        public override void InitLexicalRules()
        {
            lexicalRules.Add(new Ident());
            lexicalRules.Add(new Placeholder());
            lexicalRules.Add(new BracesIdent());
        }
    }
}
