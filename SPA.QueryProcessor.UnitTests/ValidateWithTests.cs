using System;
using Xunit;
using System.Text;

namespace SPA.QueryProcessor.Preprocessor
{
    public class ValidateWithTests
    {
        [Theory]
        [InlineData("procedure p; select p with p.procName = \"Third\"")]
        [InlineData("stmt s; constant c; select s with s.stmt# = c.value")]
        [InlineData("stmt s; select s with s.stmt# = 12")]
        [InlineData("assign a; select  a   with a.stmt# = 12")]
        [InlineData("assign a; select BOOLEAN   with a.stmt# = 12")]
        public void ValidateWith_ShouldReturnTrue(string queryToValidate)
        {
            QueryValidator validator = new QueryValidator();
            bool result = validator.ValidateQuery(queryToValidate);
            Assert.True(result);
        }

        [Fact]
        public void ValidateSelectBoolean_ShouldReturnFalse_WhenModifiesIsAmbiguous()
        {
            // Use of underscore must not lead to ambiguities. For example, the following query should be rejected 
            // as incorrect as it is not clear if underscore refers to a statement or to a procedure
            QueryValidator validator = new QueryValidator();
            bool result = validator.ValidateQuery("select boolean such that Modifies (_, \"x\")");
            Assert.False(result);
        }
    }
}
