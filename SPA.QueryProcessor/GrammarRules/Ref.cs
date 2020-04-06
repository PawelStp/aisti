using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.QueryProcessor.Preprocessor;
using SPA.QueryProcessor.AuxiliaryGrammar;
using SPA.QueryProcessor.LexicalRules;

namespace SPA.QueryProcessor.GrammarRules
{
    public class Ref
    {
        private string text;
        private DeclarationsArray declarations;
        public RefType refType { get; set; }
        public Ref(string text, DeclarationsArray declarations)
        {
            this.text = text.Trim();
            this.declarations = declarations;
        }

        public bool IsGrammarCorrect()
        {
            AttrRef attrRef = new AttrRef(text, declarations);

            if (attrRef.IsGrammarCorrect())
            {
                refType = attrRef.refType;
                return true;
            }

            ProglineSynonym proglineSynonym = new ProglineSynonym(text, declarations);
            if (proglineSynonym.IsGrammarCorrect())
            {
                refType = RefType.Integer;
                return true;
            }

            BracesIdent bracesIdent = new BracesIdent();
            if (bracesIdent.validate(text))
            {
                refType = RefType.String;
                return true;
            }

            IntegerRule integerRule = new IntegerRule();
            if (integerRule.validate(text))
            {
                refType = RefType.Integer;
                return true;
            }


            return false;
        }
    }
}
