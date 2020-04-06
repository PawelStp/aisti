using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.QueryProcessor.AuxiliaryGrammar;
using SPA.QueryProcessor.Preprocessor;

namespace SPA.QueryProcessor.GrammarRules
{
    public class AttrRef
    {

        public string entry { get; protected set; }
        public RefType refType { get; set; }
        private DeclarationsArray declarations;

        public AttrRef(string text, DeclarationsArray declarations)
        {
            entry = text;
            this.declarations = declarations;
        }
        public bool IsGrammarCorrect()
        {
            string[] splittedEntry = entry.Split('.');

            if (splittedEntry.Length != 2)
            {
                return false;
                //throw new Exception("#Incorrect length of attr ref");
            }

            string declarationName = splittedEntry[0];
            string attrNameEntry = splittedEntry[1];

            Synonym synonym = new Synonym(declarationName);
            AttrName attrName = new AttrName(attrNameEntry, declarationName, declarations);
            
            if (!synonym.IsGrammarCorrect())
            {
                return false;
                //throw new Exception("#Incorrect synonym in attr ref: " + synonym.entry);
            }

            if (!attrName.IsGrammarCorrect())
            {
                return false;
                //throw new Exception("#Incorrect attr name: " + attrName.entry);
            }

            refType = attrName.refType;
            return true;
        }
    }
}
