using System;
using Xunit;
using SPA.QueryProcessor;

namespace SPA.QueryProcessor.Preprocessor
{
    public class ValidateQueryTests
    {
        [Theory]
        [InlineData("Procedure p;While w;Select w")]
        [InlineData("Procedure p, q;Select q")]
        [InlineData("Procedure  p;      While w;     SELECT p")]
        [InlineData("            Procedure p;While w;  SELECT p")]
        [InlineData("procedure p123, q#123heyqwejquqweqhu;  SELECT q#123heyqwejquqweqhu")]
        [InlineData("procedure p,q; if if; SELECT if")]
        [InlineData("stmt s, s1; assign a, a1, a2; while w; if ifstat; procedure p; variable v; constant c; prog_line n, n1, n2; SELECT w")]
        public void ValidateQuery_ShouldReturnTrue(string queryToValidate)
        {
            QueryValidator validator = new QueryValidator();
            bool result = validator.ValidateQuery(queryToValidate);
            Assert.True(result, $"{queryToValidate} should return true");
        }

        [Fact]
        public void ValidateQuery_ShouldReturnTrue_WhenTheSameVariablesDefinedWithLowerAndUpperCase()
        {
            // Can we declare lower and uppercase? (Compare with how PipeTester works)
            QueryValidator validator = new QueryValidator();
            bool result = validator.ValidateQuery("Procedure p; While P; Select P");
            Assert.True(result);
        }

        [Theory]
        [InlineData("Procedure p;While w; Select x")]
        [InlineData("Select p")]
        public void ValidateQuery_ShouldReturnFalse_WhenNotExistingDeclarationIsUsed(string queryToValidate)
        {
            // arrange
            QueryValidator validator = new QueryValidator();
            // act
            Action act = () => validator.ValidateQuery(queryToValidate);
            // assert
            Assert.Throws<InvalidQueryException>(act);
        }

        [Fact]
        public void ValidateQuery_ShouldReturnFalse_WhenTheSameVariablesAreDefined()
        {
            // arrange
            QueryValidator validator = new QueryValidator();
            // act
            Action act = () => validator.ValidateQuery("Procedure p; While p; Select p");
            // assert
            Assert.Throws<InvalidQueryException>(act);
        }

    }
}
