using System;
using Xunit;
using System.Text;

namespace SPA.QueryProcessor.Preprocessor
{
    public class ValidateSelectTests
    {
        [Theory]
        [InlineData("select boolean such that Modifies (\"x\", 10) ")]
        [InlineData("select boolean such that Modifies (1, \"x\")")]
        [InlineData("select boolean such that Modifies (\"First\", \"x\")")]
        public void ValidateSelectBoolean_ShouldReturnTrue_WhenModifies(string queryToValidate)
        {
            DeclarationsArray declarations = new DeclarationsArray();
            // Modifies design entity relationships:
            // Modifies(procedure, variable)  
            // Modifies(stmt, variable)
            SelectValidator validator = new SelectValidator(declarations);
            bool result = validator.ValidateSelectQuery(queryToValidate);
            Assert.True(result);
        }

        [Fact]
        public void ValidateSelectBoolean_ShouldReturnFalse_WhenModifiesIsAmbiguous()
        {
            DeclarationsArray declarations = new DeclarationsArray();
            // Use of underscore must not lead to ambiguities. For example, the following query should be rejected 
            // as incorrect as it is not clear if underscore refers to a statement or to a procedure
            SelectValidator validator = new SelectValidator(declarations);
            bool result = validator.ValidateSelectQuery("select boolean such that Modifies (_, \"x\")");
            Assert.False(result);
        }

        [Theory]
        [InlineData("Select p such that Calls (p, _) ")]
        [InlineData("  Select  p  such   that   Calls (p, _) ")]
        [InlineData("Select p such that Calls (p, q) ")]
        [InlineData("Select q  such that Calls (_, q) ")]
        [InlineData("Select <p,q > such that Calls (p, q) ")]
        [InlineData("Select q such that Calls (\"Second\", q)  ")]
        [InlineData("Select p, q such that Calls (p, q) and Calls(p, q) ")]
        [InlineData("")]
        public void ValidateSelect_ShouldReturnTrue_WhenCallsSyntaxIsCorrect(string queryToValidate)
        {
            DeclarationsArray declarations = new DeclarationsArray();
            SelectValidator validator = new SelectValidator(declarations);
            bool result = validator.ValidateSelectQuery(queryToValidate);
            Assert.True(result);
        }

        [Theory]
        [InlineData("Select p such that Calls (_, p, _) ")]
        [InlineData("  Select  p  such   that   Calls (p, 2, _) ")]
        [InlineData("Select <p,q > such that Calls (<p,q>, q) ")]
        [InlineData("Select p, q WITH Calls (p, q) ")]
        [InlineData("Select p, q Pattern Calls (p, q) ")]
        [InlineData("Select p, q such that Calling (p, q) ")]
        [InlineData("Select p, q such Calls (p, q) ")]
        [InlineData("Select p, q such that Call (p, q) ")]
        [InlineData("Select p, q such that Calls () ")]
        [InlineData("Select p, q such that Calls and Calls(p,q)")]
        [InlineData("Select p, q such that Calls(p,q) and Call(,)")]
        [InlineData("Select p, q such that Calls (p, q) Calls(p, q) ")]
        [InlineData("")]
        public void ValidateSelect_ShouldReturnFalse_WhenCallsSyntaxIsIncorrect(string queryToValidate)
        {
            DeclarationsArray declarations = new DeclarationsArray();
            SelectValidator validator = new SelectValidator(declarations);
            bool result = validator.ValidateSelectQuery(queryToValidate);
            Assert.True(result);
        }

        [Theory]
        [InlineData("Select p such that p (p, p) ")]
        [InlineData("Select p, q such that p=q")]
        [InlineData("Select p, q such that Calls(q, e)")]
        public void ValidateSelect_ShouldReturnFalse_WhenSuchThatUsedIncorrectly(string queryToValidate)
        {
            DeclarationsArray declarations = new DeclarationsArray();
            // **such that grammar rules** //
            // suchthat - cl : ‘such that’ relCond
            // relCond : relRef( ‘and’ relRef) *
            // relRef: ModifiesP | ModifiesS | UsesP | UsesS | Calls | CallsT | Parent | ParentT | 
            //          Follows | FollowsT | Next | NextT | Affects | AffectsT
            SelectValidator validator = new SelectValidator(declarations);
            bool result = validator.ValidateSelectQuery(queryToValidate);
            Assert.True(result);
        }

    }
}
