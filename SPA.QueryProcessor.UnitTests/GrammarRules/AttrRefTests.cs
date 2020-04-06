using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SPA.QueryProcessor.GrammarRules;
using SPA.QueryProcessor.Preprocessor;

namespace SPA.QueryProcessor.UnitTests.GrammarRules
{
    public class AttrRefTests
    {
        [Theory]
        [InlineData("test.procName", "PROCEDURE", "test")]
        [InlineData("test#.procName", "PROCEDURE", "test#")]
        [InlineData("abc.varName", "VARIABLE", "abc")]
        [InlineData("value.value", "CONSTANT", "value")]
        [InlineData("a123.stmt#", "ASSIGN", "a123")]
        [InlineData("q.stmt#", "STMT", "q")]
        [InlineData("qwe.stmt#", "WHILE", "qwe")]
        [InlineData("t#.stmt#", "IF", "t#")]
        public void ValidateAttrRef_ShouldReturnTrue(string input, string designEntity, string declarationName)
        {
            DeclarationsArray declarationsArray = new DeclarationsArray();
            declarationsArray.AddDeclaration(designEntity, declarationName);

            AttrRef attrRef = new AttrRef(input, declarationsArray);
            bool result = attrRef.IsGrammarCorrect();
            Assert.True(result);
        }

        [Theory]
        [InlineData("test..procName", "PROCEDURE", "test")]
        [InlineData("abc-varName", "VARIABLE", "abc")]
        [InlineData("value", "CONSTANT", "value")]
        [InlineData("abc.#stmt#", "STMT", "abc")]
        [InlineData("a123.stmt##", "ASSIGN", "a123")]
        [InlineData("abc123.vvarName", "VARIABLE", "abc123")]
        public void ValidateAttrRef_ShouldReturnFalse_WhenWrongFormatting(string input, string designEntity, string declarationName)
        {
            DeclarationsArray declarationsArray = new DeclarationsArray();
            declarationsArray.AddDeclaration(designEntity, declarationName);

            AttrRef attrRef = new AttrRef(input, declarationsArray);
            bool result = attrRef.IsGrammarCorrect();
            Assert.False(result);
        }

        [Theory]
        [InlineData("test.procName", "VARIABLE", "test")]
        [InlineData("test#.procName", "CONSTANT", "test#")]
        [InlineData("abc.varName", "ASSIGN", "abc")]
        [InlineData("value.value", "STMT", "value")]
        [InlineData("a123.stmt#", "PROCEDURE", "a123")]
        [InlineData("q.stmt#", "VARIABLE", "q")]
        [InlineData("qwe.stmt#", "CONSTANT", "qwe")]
        public void ValidateAttrRef_ShouldReturnFalse_WhenWrongDesignEntity(string input, string designEntity, string declarationName)
        {
            DeclarationsArray declarationsArray = new DeclarationsArray();
            declarationsArray.AddDeclaration(designEntity, declarationName);

            AttrRef attrRef = new AttrRef(input, declarationsArray);
            bool result = attrRef.IsGrammarCorrect();
            Assert.False(result);
        }
    }
}
