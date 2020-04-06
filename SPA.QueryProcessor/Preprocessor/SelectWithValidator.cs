using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.QueryProcessor.GrammarRules;

namespace SPA.QueryProcessor.Preprocessor
{
    public class SelectWithValidator
    {
        private string entry;
        private DeclarationsArray declarations;

        public SelectWithValidator(string entry, DeclarationsArray declarations)
        {
            this.entry = entry;
            this.declarations = declarations;
        }

        public bool isGrammarCorrect()
        {
            AttrCompare attrCompare = new AttrCompare(entry, declarations);
            return attrCompare.IsGrammarCorrect();
        }
    }
}
