using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.QueryProcessor.Preprocessor;
using SPA.QueryProcessor.DesignEntity;

namespace SPA.QueryProcessor.AuxiliaryGrammar
{
    public class AttrName : AbstractAuxiliaryGrammar
    {
        public RefType refType { get; set; }
        private string declarationName;
        private DeclarationsArray declarations;

        public AttrName(string attrName, string declarationName, DeclarationsArray declarations) : base(attrName)
        {
            this.declarationName = declarationName;
            this.declarations = declarations;
        }

        public override void InitLexicalRules()
        {
            // IT DOESN'T USE LEXICAL RULES
        }

        public override bool IsGrammarCorrect()
        {
            var declarationType = declarations.GetDeclarationByName(declarationName);


            if (entry.Equals("procName", StringComparison.OrdinalIgnoreCase) && declarationType is Procedure)
            {
                refType = RefType.String;
                return true;
            }

            if (entry.Equals("varName", StringComparison.OrdinalIgnoreCase) && declarationType is Variable)
            {
                refType = RefType.String;
                return true;
            }

            if (entry.Equals("value", StringComparison.OrdinalIgnoreCase) && declarationType is Constant)
            {
                refType = RefType.Integer;
                return true;
            }

            if (entry.Equals("stmt#", StringComparison.OrdinalIgnoreCase) && declarationType is Statement)
            {
                refType = RefType.Integer;
                return true;
            }

            return false;
        }
    }
}
