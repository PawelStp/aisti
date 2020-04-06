using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SPA.QueryProcessor.Preprocessor;

namespace SPA.QueryProcessor.UnitTests.FullQuery
{
    public class QueryParser_SelectWithTests
    {
        [Theory]
        [InlineData("procedure p;select p WITH p.procName = \"Third\"")]
        [InlineData("procedure p; select p WITH p.procName =\"Third\"")]
        [InlineData("procedure p; select p WITH p.procName=\"Third\"")]
        [InlineData("prog_line q; select q with q = 3")]
        [InlineData("constant c; select c with c.value = 2")]
        [InlineData("stmt s; prog_line n; select s with s.stmt# = n")]
        [InlineData("stmt s; constant c; select s with s.stmt# = c.value")]
        [InlineData("stmt s; constant c; select s with s.stmt# = c.value with c.value = 3")]
        [InlineData("stmt s; constant c; select s with s.stmt# = c.value with c.value = 3   with  s.stmt#  = 12")]
        public void SelectValidator_ShouldReturnTrue_WhenValidSelectWith(string queryToValidate)
        {
            QueryValidator queryValidator = new QueryValidator();
            bool result = queryValidator.ValidateQuery(queryToValidate);
            Assert.True(result);
        }

        [Theory]
        [InlineData("procedure p;stmt s; prog_line n; select p WITH p.procName=\"Third\" AND s.stmt# = n with n=20")]
        [InlineData("stmt s; prog_line n; select s with s.stmt# = n and n =20")]
        [InlineData("stmt s; constant c; select s with s.stmt# = c.value and c.value = 20")]
        [InlineData("stmt s; constant c; select s with s.stmt# = c.value with c.value = 3   and   s.stmt#  = 12")]
        public void SelectValidator_ShouldReturnTrue_WhenValidSelectWith_PlusAnd(string queryToValidate)
        {
            QueryValidator queryValidator = new QueryValidator();
            bool result = queryValidator.ValidateQuery(queryToValidate);
            Assert.True(result);
        }
    }
}
