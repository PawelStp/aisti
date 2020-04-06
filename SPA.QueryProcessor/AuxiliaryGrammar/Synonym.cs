using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SPA.QueryProcessor.LexicalRules;

namespace SPA.QueryProcessor.AuxiliaryGrammar
{
    public class Synonym : AbstractAuxiliaryGrammar, ILexicalRules
    {
        public Synonym(string text) : base(text) { }

        public override void InitLexicalRules()
        {
            lexicalRules.Add(new Ident());
        }

        public bool validate(string text)
        {
            return new Ident().validate(text);
        }
    }
}
