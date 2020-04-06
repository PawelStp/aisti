using System;
using Xunit;
using System.Text;

namespace SPA.QueryProcessor.UnitTests
{
    public class ValidateQueryParser
    {
        [Fact]
        public void ProcessQuery_ShouldReturnExceptionMessage_WhenQueryIsInvalid()
        {
            string result = QueryParser.ProcessQuery("Procedure p; While P; Select P");
            Assert.Equal("#Invalid Query",result);
        }

        [Fact]
        public void ProcessQuery_ShouldReturnValidResult_WhenQueryIsValid()
        {
            string result = QueryParser.ProcessQuery("Procedure p; Select p");
            Assert.Equal("OK", result);
        }
    }
}
