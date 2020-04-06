using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.QueryProcessor.LexicalRules;
using SPA.QueryProcessor.Preprocessor;
using SPA.QueryProcessor.DesignEntity;

namespace SPA.QueryProcessor.AuxiliaryGrammar
{
    public class ProglineSynonym : AbstractAuxiliaryGrammar
    {
        private DeclarationsArray declarations;
        public ProglineSynonym(string text, DeclarationsArray declarations) : base(text)
        {
            this.declarations = declarations;
        }

        public override void InitLexicalRules()
        {
            // IT OVERRIDES IS GRAMMAR CORRECT SO LEXICAL RULES INIT IS NOT NEEDED
        }

        public override bool IsGrammarCorrect()
        {
            var declaration = declarations.GetDeclarationByName(entry);

            if (declaration == null)
            {
                return false;
            }

            if (!(declaration is ProgramLine))
            {
                return false;
            }

            return new Ident().validate(entry);
        }
    }
}
