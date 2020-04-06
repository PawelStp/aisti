using System;
using Xunit;
using System.Text;

namespace SPA.QueryProcessor.Preprocessor
{
    public class ValidateDeclarationsTests
    {
        [Theory]
        [InlineData("Procedure p, q, w, e, s")]
        [InlineData("   While    w")]
        [InlineData("procedure p123, q#123heyqwejquqweqhu")]
        [InlineData("assign   a  ")]
        [InlineData("  stmt  s  ,  s1  ")]
        [InlineData("assign a, a1, a2, a3, a4, a5, a6")]
        [InlineData("variable v")]
        [InlineData("variable v12, v321")]
        [InlineData("constant c")]
        [InlineData("constant c#123")]
        [InlineData("prog_line n, n1, n2")]
        public void ValidateDeclarations_ShouldReturnTrue(string queryToValidate)
        {
            DeclarationValidator validator = new DeclarationValidator();
            bool result = validator.ValidateDeclarationQuery(queryToValidate);
            Assert.True(result, $"{queryToValidate} should return true");
        }

        [Theory]
        [InlineData("Procedure 1procedure")]
        [InlineData("While 1")]
        [InlineData(" if #q")]
        [InlineData("constant 123TEST, q")]
        [InlineData("assign a, b, c, d, #, 1, 2, aa ")]
        [InlineData("assign _a")]
        public void ValidateDeclarations_ShouldReturnFalse_WhenIncorrectVariableName(string queryToValidate)
        {
            DeclarationValidator validator = new DeclarationValidator();
            bool result = validator.ValidateDeclarationQuery(queryToValidate);
            Assert.False(result, $"{queryToValidate} should be in format: LETTER ( LETTER | DIGIT | ‘#’ )* ");
        }

        [Theory]
        [InlineData("if if")]
        [InlineData("if if, procedure, while, variable, constant, prog_line, stmt")]
        public void ValidateDeclarations_ShouldReturnFalse_WhenKeywordsUsedAsVariable(string queryToValidate)
        {
            DeclarationValidator validator = new DeclarationValidator();
            bool result = validator.ValidateDeclarationQuery(queryToValidate);
            Assert.False(result, $"{queryToValidate} contains keyword as variable");
        }

        [Theory]
        [InlineData("program p, q")]
        [InlineData("Ifo o")]
        [InlineData("Konstant k")]
        [InlineData("assign_ a")]
        public void ValidateDeclarations_ShouldReturnFalse_WhenIncorrectDesignEntity(string queryToValidate)
        {
            DeclarationValidator validator = new DeclarationValidator();
            bool result = validator.ValidateDeclarationQuery(queryToValidate);
            Assert.False(result, $"{queryToValidate} have incorrect synonyms. " +
                $"Valid synonyms are ‘procedure’ | ‘stmtLst’ | ‘stmt’ | ‘assign’ | ‘call’ | ‘while’ " +
                $"| ‘if’ | ‘variable’ | ‘constant’| ‘prog_line’ ");
        }

        [Fact]
        public void ValidateDeclarations_ShouldReturnFalse_WhenVariableMissing()
        {
            DeclarationValidator validator = new DeclarationValidator();
            bool result = validator.ValidateDeclarationQuery("while    ");
            Assert.False(result);
        }

    }
}
