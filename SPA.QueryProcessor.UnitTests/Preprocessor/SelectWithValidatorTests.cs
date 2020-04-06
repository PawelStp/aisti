using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SPA.QueryProcessor.Preprocessor;

namespace SPA.QueryProcessor.UnitTests.Preprocessor
{
    public class SelectWithValidatorTests
    {
        [Theory]
        [InlineData("q.procName =\"Third\"", "q", "PROCEDURE")]
        [InlineData("q = 3", "q", "PROG_LINE")]
        [InlineData("q=3", "q", "PROG_LINE")]
        [InlineData("c.value = 2", "c", "CONSTANT")]
        [InlineData("c.value   =   2", "c", "CONSTANT")]
        public void SelectWithValidator_ShouldReturnTrue(string queryToValidate, string declarationName, string declarationType)
        {
            DeclarationsArray declarationsArray = new DeclarationsArray();
            declarationsArray.AddDeclaration(declarationType, declarationName);

            SelectWithValidator validator = new SelectWithValidator(queryToValidate, declarationsArray);
            bool result = validator.isGrammarCorrect();
            Assert.True(result);
        }

        [Theory]
        [InlineData("s.stmt# = n", "s", "STMT", "n", "PROG_LINE")]
        [InlineData("s.stmt#=n", "s", "STMT", "n", "PROG_LINE")]
        [InlineData("s.stmt# = c.value", "s", "STMT", "c", "CONSTANT")]
        public void SelectWithValidator_ShouldReturnTrue_Advanced(string queryToValidate, string declarationName1, string declarationType1,
            string declarationName2, string declarationType2)
        {
            DeclarationsArray declarationsArray = new DeclarationsArray();
            declarationsArray.AddDeclaration(declarationType1, declarationName1);
            declarationsArray.AddDeclaration(declarationType2, declarationName2);

            SelectWithValidator validator = new SelectWithValidator(queryToValidate, declarationsArray);
            bool result = validator.isGrammarCorrect();
            Assert.True(result);
        }

        [Theory]
        [InlineData("q.procName =\"Third\"")]
        public void SelectWithValidator_ShouldReturnFalse_WhenDeclarationMissing(string queryToValidate)
        {
            DeclarationsArray declarationsArray = new DeclarationsArray();

            SelectWithValidator validator = new SelectWithValidator(queryToValidate, declarationsArray);
            bool result = validator.isGrammarCorrect();
            Assert.False(result);
        }

        [Theory]
        [InlineData("q.procName =3", "q", "PROCEDURE")]
        [InlineData("q = \"Third\"", "q", "PROG_LINE")]
        [InlineData("c.value = \"var\"", "c", "CONSTANT")]
        public void SelectWithValidator_ShouldReturnFalse_WhenWrongRefTypes(string queryToValidate, string declarationName, string declarationType)
        {
            DeclarationsArray declarationsArray = new DeclarationsArray();
            declarationsArray.AddDeclaration(declarationType, declarationName);

            SelectWithValidator validator = new SelectWithValidator(queryToValidate, declarationsArray);
            bool result = validator.isGrammarCorrect();
            Assert.False(result);
        }

        [Theory]
        [InlineData("q=3", "q", "WHILE")]
        [InlineData("q=3", "q", "IF")]
        [InlineData("q=3", "q", "ASSIGN")]
        [InlineData("q=3", "q", "STMT")]
        public void SelectWithValidator_ShouldReturnFalse_WhenSynonymTypeOtherThanProgLine(string queryToValidate, string declarationName, string declarationType)
        {
            DeclarationsArray declarationsArray = new DeclarationsArray();
            declarationsArray.AddDeclaration(declarationType, declarationName);

            SelectWithValidator validator = new SelectWithValidator(queryToValidate, declarationsArray);
            bool result = validator.isGrammarCorrect();
            Assert.False(result);
        }
    }
}
