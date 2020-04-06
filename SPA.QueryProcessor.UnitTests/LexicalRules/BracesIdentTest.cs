using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SPA.QueryProcessor.LexicalRules;

namespace SPA.QueryProcessor.UnitTests.LexicalRules
{
    public class BracesIdentTest
    {
        [Theory]
        [InlineData("\"a\"")]
        [InlineData("\"abc\"")]
        [InlineData("\"Abc\"")]
        [InlineData("\"a#123\"")]
        [InlineData("\"Z123#\"")]
        [InlineData("\"zXC3#33##\"")]
        public void ValidateIdent_ShouldReturnTrue(string input)
        {
            ILexicalRules identValidator = new BracesIdent();
            bool result = identValidator.validate(input);
            Assert.True(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1abc")]
        [InlineData("#Abc")]
        [InlineData("##")]
        [InlineData("123")]
        [InlineData("a_2")]
        [InlineData("a$")]
        [InlineData("\"good")]
        [InlineData("good\"")]
        [InlineData("\"\"")]
        [InlineData("\" \"")]
        public void ValidateIdent_ShouldReturnFalse(string input)
        {
            ILexicalRules identValidator = new BracesIdent();
            bool result = identValidator.validate(input);
            Assert.False(result);
        }
    }
}
