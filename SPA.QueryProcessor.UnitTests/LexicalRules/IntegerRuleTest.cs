using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SPA.QueryProcessor.LexicalRules;

namespace SPA.QueryProcessor.UnitTests.LexicalRules
{
    public class IntegerRuleTest
    {
        [Theory]
        [InlineData("1")]
        [InlineData("123")]
        [InlineData("99999999")]
        [InlineData("997")]
        [InlineData("0")]
        public void ValidateIntegerRule_ShouldReturnTrue(string input)
        {
            ILexicalRules IntegerValidator = new IntegerRule();
            bool result = IntegerValidator.validate(input);
            Assert.True(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1abc")]
        [InlineData("#Abc")]
        [InlineData("##")]
        [InlineData("a_2")]
        [InlineData("a$")]
        [InlineData("\"good")]
        [InlineData("good\"")]
        [InlineData("\"\"")]
        [InlineData("\" \"")]
        [InlineData("125121.")]
        public void ValidateIntegerRule_ShouldReturnFalse(string input)
        {
            ILexicalRules IntegerValidator = new IntegerRule();
            bool result = IntegerValidator.validate(input);
            Assert.False(result);
        }
    }
}
