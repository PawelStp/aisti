using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SPA.QueryProcessor.AuxiliaryGrammar;

namespace SPA.QueryProcessor.UnitTests.AuxiliaryGrammar
{
    public class SynonymTests
    {
        [Theory]
        [InlineData("a")]
        [InlineData("abc")]
        [InlineData("Abc")]
        [InlineData("a#123")]
        [InlineData("Z123#")]
        [InlineData("zXC3#33##")]
        public void ValidateSynonym_ShouldReturnTrue(string input)
        {
            Synonym synonym = new Synonym(input);
            bool result = synonym.IsGrammarCorrect();
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
        public void ValidateSynonym_ShouldReturnFalse(string input)
        {
            Synonym synonym = new Synonym(input);
            bool result = synonym.IsGrammarCorrect();
            Assert.False(result);
        }
    }
}
