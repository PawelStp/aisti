using System;
using System.Diagnostics;
using SPA.Pkb.Helpers;
using Xunit;

namespace SPA.Pkb.UnitTests
{
    public class PkbTests
    {
        private Parser parser;
        private DesignExtractor designExtractor;

        public PkbTests()
        {
            Initialization.PkbReference = new Pkb();
            parser = new Parser();
            designExtractor = new DesignExtractor();
        }

        [Fact]
        public void EnsureSourceCodeWasStored()
        {
            SourceRepository.StoreSourceCode("");
            Assert.NotEmpty(SourceRepository.SourceCode);
        }

        [Fact]
        public void CheckPopulateOperationResultNotEmpty()
        {
            SourceRepository.StoreSourceCode("");
            var result = parser.PopulateTokenList();
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData("procedure Third {\nz = 5;\nv = z; }")]
        public void CheckIfSimpleProgramParsesWithoutException(string source)
        {
            SourceRepository.StoreSourceCodeFromString(source);
            parser.StartParsing();
            Assert.True(parser.FinishedParsing);
        }

        [Theory]
        [InlineData(@"
                        procedure p {
                            x = 1;
                            y = 2;
                            z = y;
                            call q;
                            z = x + y + z; }
                            procedure q {
                            x = 5;
                            if z then {
                            t = x + 1; }
                            else {
                            y = z + x; } }")]
        public void CheckIfTwoProceduresProgramParsesWithoutException(string source)
        {
            SourceRepository.StoreSourceCodeFromString(source);
            parser.StartParsing();
            Assert.True(parser.FinishedParsing);
        }

        [Theory]
        [InlineData(@"
                            procedure    p    {
                            if   x       then    {
                            x    =    10  ; }
                                    else           {
                            y  =   20;   }  }
                        ")]
        public void CheckIfWhitespaceProgramParsesWithoutException(string source)
        {
            SourceRepository.StoreSourceCodeFromString(source);
            parser.StartParsing();
            Assert.True(parser.FinishedParsing);
        }

        [Fact]
        public void EnsureParserCreatedAbstractSyntaxTree()
        {
            SourceRepository.StoreSourceCode("");
            parser.StartParsing();
            designExtractor.CreateProcTable();
            Assert.NotNull(Initialization.PkbReference.AbstractSyntaxTree);
        }

        [Fact]
        public void EnsureDesignExtractorCreatedProcTable()
        {
            SourceRepository.StoreSourceCode("");
            parser.StartParsing();
            designExtractor.CreateProcTable();
            Assert.NotEmpty(Initialization.PkbReference.ProcTable);
        }

        [Fact]
        public void EnsureDesignExtractorCreatedProceduresModifiesTable()
        {
            SourceRepository.StoreSourceCode("");
            parser.StartParsing();
            designExtractor.CreateProcTable();
            Assert.NotEmpty(Initialization.PkbReference.ProceduresModifiesTable);
        }

        [Fact]
        public void EnsureDesignExtractorCreatedStatementModifiesTable()
        {
            SourceRepository.StoreSourceCode("");
            parser.StartParsing();
            designExtractor.CreateProcTable();
            Assert.NotEmpty(Initialization.PkbReference.StatementModifiesTable);
        }

        [Fact]
        public void CallTwoParameterTest()
        {
            SourceRepository.StoreSourceCode("");
            parser.StartParsing();
            designExtractor.CreateProcTable();
            var result = Initialization.PkbReference.Calls("Second", "Third");
            Assert.True(result);
        }

        [Fact]
        public void CallTwoParameterFailTest()
        {
            SourceRepository.StoreSourceCode("");
            parser.StartParsing();
            designExtractor.CreateProcTable();
            var result = Initialization.PkbReference.Calls("123\n", "][][][");
            Assert.False(result);
        }

        [Fact]
        public void CallTwoParameterEmptyTest()
        {
            SourceRepository.StoreSourceCode("");
            parser.StartParsing();
            designExtractor.CreateProcTable();
            var result = Initialization.PkbReference.Calls("", "");
            Assert.False(result);
        }

        [Fact]
        public void CalledProceduresTest()
        {
            SourceRepository.StoreSourceCode("");
            parser.StartParsing();
            designExtractor.CreateProcTable();
            var result = Initialization.PkbReference.CalledByProcedure("Second");
            Assert.Contains("Third", result);
        }

        [Fact]
        public void CalledProceduresFailTest()
        {
            SourceRepository.StoreSourceCode("");
            parser.StartParsing();
            designExtractor.CreateProcTable();
            var result = Initialization.PkbReference.CalledByProcedure("[][2");
            Assert.Empty(result);
        }

        [Fact]
        public void ProceduresThatCallTest()
        {
            SourceRepository.StoreSourceCode("");
            parser.StartParsing();
            designExtractor.CreateProcTable();
            var result = Initialization.PkbReference.ProceduresThatCall("Second");
            Assert.Contains("First", result);
        }

        [Fact]
        public void ProceduresThatCallFailTest()
        {
            SourceRepository.StoreSourceCode("");
            parser.StartParsing();
            designExtractor.CreateProcTable();
            var result = Initialization.PkbReference.ProceduresThatCall("ipo[");
            Assert.Empty(result);
        }
    }
}