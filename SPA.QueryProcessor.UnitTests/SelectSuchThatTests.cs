using SPA.QueryProcessor.Preprocessor;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

// Waiting for Declarations to work properly - do not attach

namespace SPA.QueryProcessor.Preprocessor
{
    public class SelectSuchThatTests
    {
        [Theory]
        [InlineData("Procedure p; While w; Select w")]
        [InlineData("Procedure p; While w Select w")]
        [InlineData("Select p")]
        [InlineData("Procedure p; Select p")]
        [InlineData("PROCEDURE p; SELECT p")]
        [InlineData("PROCEDURE P; SELECT P")]
        [InlineData("SELECT boolean")]
        public void ValidateQuery_SelectSuchThat_Initialtest(string queryToValidate)
        {
            QueryValidator validator = new QueryValidator();
            var result = validator.ValidateQuery(queryToValidate);

            Assert.True(result);
        }

        [Theory]
        [InlineData("PROCEDURE P; STMT S; VARIABLE V; SELECT V SUCH THAT MODIFIES (P, V)")]
        [InlineData("PROCEDURE P; STMT S; VARIABLE V; SELECT V SUCH THAT MODIFIES (P, \"x\")")]
        [InlineData("PROCEDURE P; STMT S; VARIABLE V; SELECT V SUCH THAT MODIFIES (P, \"_\")")]
        [InlineData("PROCEDURE P; STMT S; VARIABLE V; SELECT V SUCH THAT MODIFIES (S, V)")]
        [InlineData("PROCEDURE P; STMT S; VARIABLE V; SELECT V SUCH THAT MODIFIES (S, \"x\")")]
        [InlineData("PROCEDURE P; STMT S; VARIABLE V; SELECT V SUCH THAT MODIFIES (S, \"_\")")]
        [InlineData("PROCEDURE P; STMT S; VARIABLE V; SELECT V SUCH THAT MODIFIES (2, \"_\")")]
        public void ValidateQuery_SelectSuchThat(string queryToValidate)
        {
            QueryValidator validator = new QueryValidator();
            var result = validator.ValidateQuery(queryToValidate);

            Assert.True(result);
        }

        [Theory]
        [InlineData("PROCEDURE P; STMT S; VARIABLE V; SELECT V SUCH THAT MODIFIES (S, P)")]
        [InlineData("PROCEDURE P; STMT S; VARIABLE V; SELECT V SUCH THAT MODIFIES (P, S)")]
        [InlineData("PROCEDURE P; STMT S; VARIABLE V; SELECT V SUCH THAT MODIFIES (V, S)")]
        [InlineData("PROCEDURE P; STMT S; VARIABLE V; SELECT V SUCH THAT MODIFIES (V, 2)")]
        [InlineData("PROCEDURE P; STMT S; VARIABLE V; SELECT V SUCH THAT MODIFIES (V, P)")]
        public void ValidateQuery_SelectSuchThat_InvalidQueries(string queryToValidate)
        {
            QueryValidator validator = new QueryValidator();
            var result = validator.ValidateQuery(queryToValidate);

            Assert.True(result);
        }
    }
}
