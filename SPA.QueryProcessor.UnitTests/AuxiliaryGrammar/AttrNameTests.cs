using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SPA.QueryProcessor.AuxiliaryGrammar;
using SPA.QueryProcessor.Preprocessor;

namespace SPA.QueryProcessor.UnitTests.AuxiliaryGrammar
{
    public class AttrNameTests
    {
        [Theory]
        [InlineData("procName", "q", "PROCEDURE")]
        [InlineData("varName", "q", "VARIABLE")]
        [InlineData("value", "q", "CONSTANT")]
        [InlineData("stmt#", "q", "STMT")]
        public void ValidateAttrName_ShouldReturnTrue(string attrName, string declarationName, string declarationType)
        {
            DeclarationsArray declarations = new DeclarationsArray();
            declarations.AddDeclaration(declarationType, declarationName);

            AbstractAuxiliaryGrammar attrNameValidator = new AttrName(attrName, declarationName, declarations);
            bool result = attrNameValidator.IsGrammarCorrect();
            Assert.True(result);
        }

        [Theory]
        [InlineData("", "q", "PROCEDURE")]
        [InlineData(" ", "q", "PROCEDURE")]
        [InlineData("proc Name", "q", "PROCEDURE")]
        [InlineData("varName#", "q", "VARIABLE")]
        [InlineData("vvalue", "q", "CONSTANT")]
        [InlineData("#stmt", "q", "ASSIGN")]
        [InlineData("procNamee", "q", "PROCEDURE")]
        [InlineData("bleevalueblee", "q", "CONSTANT")]
        public void ValidateAttrName_ShouldReturnFalse_WhenIncorrectAttributeName(string attrName, string declarationName, string declarationType)
        {
            DeclarationsArray declarations = new DeclarationsArray();
            declarations.AddDeclaration(declarationType, declarationName);

            AbstractAuxiliaryGrammar attrNameValidator = new AttrName(attrName, declarationName, declarations);
            bool result = attrNameValidator.IsGrammarCorrect();
            Assert.False(result);
        }

        [Theory]
        [InlineData("procName", "q", "WHILE")]
        [InlineData("varName", "q", "PROCEDURE")]
        [InlineData("value", "q", "STMT")]
        [InlineData("stmt#", "q", "VARIABLE")]
        public void ValidateAttrName_ShouldReturnFalse_WhenIncorrectDeclarationType(string attrName, string declarationName, string declarationType)
        {
            DeclarationsArray declarations = new DeclarationsArray();
            declarations.AddDeclaration(declarationType, declarationName);

            AbstractAuxiliaryGrammar attrNameValidator = new AttrName(attrName, declarationName, declarations);
            bool result = attrNameValidator.IsGrammarCorrect();
            Assert.False(result);
        }
    }
}
