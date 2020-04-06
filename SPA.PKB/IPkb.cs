using System.Collections.Generic;
using SPA.Models;

namespace SPA.Pkb
{
    public interface IPkb
    {
        void PrintAst();
        void CreateControlFlowGraph();

        /// <summary>
        /// AST tree root
        /// </summary>
        AbstractSyntaxTree AbstractSyntaxTree { get; set; }

        ControlFlowGraph ControlFlowGraph { get; set; }
        VarTable VarTable { get; set; }
        List<ProcTableCell> ProcTable { get; set; }
        List<StatementColumn> StatementModifiesTable { get; set; }
        Dictionary<string, List<string>> ProceduresModifiesTable { get; set; }
        List<StatementColumn> StatementUsesTable { get; set; }
        Dictionary<string, List<string>> ProceduresUsesTable { get; set; }
        List<string> UsedVariables { get; set; }
        List<ParrentTableCell> ParentTable { get; set; }
        Dictionary<int, int> FollowsTable { get; set; }
        List<string> UsedProcedures { get; set; }
        List<UsedStatementElement> UsedStatements { get; set; }
        bool Follows(AstNode s1, AstNode s2);
        //bool Parent(int stmt1, int stmt2);
        bool Calls(string s1, string s2);

        /// <summary>
        /// Returns all procedures which are called inside s1
        /// </summary>
        /// <param name="s1"></param>
        /// <returns></returns>
        List<string> CalledByProcedure(string s1);

        /// <summary>
        /// Returns all procedures that call procedure s1
        /// </summary>
        /// <param name="s1"></param>
        /// <returns></returns>
        List<string> ProceduresThatCall(string s1);

        /// <summary>
        /// Check if stetement modifies variable
        /// </summary>
        bool ModifiesStatement(int statementNumber, string variable);

        /// <summary>
        /// Returns  variables modified in statement that have passed number. 
        /// statementType is additional filter parameter, if this parameter is passsed only var in statement of given type will be return
        /// </summary>
        List<string> ModifiesVarModInStmt(int statementNumber,NodeType statementType = NodeType.Statement);

        /// <summary>
        /// Returns all statement numbers that modifies passed variable
        ///  statementType is additional filter parameter, if this parameter is passsed only statement of given type will be return
        /// </summary>
        List<int> ModifiesStmtThatModVar(string variable, NodeType statementType = NodeType.Statement);

        /// <summary>
        /// Check if procedure modifies variable
        /// </summary>
        bool ModifiesProcedure(string procedureName,string variable);

        /// <summary>
        /// Returns  variables modified in procedure 
        /// </summary>
        List<string> ModifiesAllProcModVar(string procedureName);

        /// <summary>
        /// Returns  procedure names that modified passed var
        /// </summary>
        List<string> ModifiesAllVarModProc(string variable);

        /// <summary>
        /// Check if stetement uses variable
        /// </summary>
        bool UsesStatement(int statementNumber, string variable);

        /// <summary>
        /// Returns  variables used in statement that have passed number. 
        /// statementType is additional filter parameter, if this parameter is passsed only var in statement of given type will be return
        /// </summary>
        List<string> UsesVarModInStmt(int statementNumber, NodeType statementType = NodeType.Statement);

        /// <summary>
        /// Returns all statement numbers that uses passed variable
        ///  statementType is additional filter parameter, if this parameter is passsed only statement of given type will be return
        /// </summary>
        List<int> UsesStmtThatModVar(string variable, NodeType statementType = NodeType.Statement);

        /// <summary>
        /// Check if procedure uses variable
        /// </summary>
        bool UsesProcedure(string procedureName, string variable);

        /// <summary>
        /// Returns  variables used in procedure 
        /// </summary>
        List<string> UsesAllProcModVar(string procedureName);

        /// <summary>
        /// Returns  procedure names that uses passed var
        /// </summary>
        List<string> UsesAllVarModProc(string variable);


        /// <summary>
        /// Returns  all used variables in program
        /// </summary>
        List<string> GetAllUsedVariables();

        /// <summary>
        /// Returns  if container statement given in statementNumber1 is parent for statement given in statementNumber2
        /// </summary>
        bool Parent(int statementNumber1, int statementNumber2);

        bool ParentStar(int statementNumber1, int statementNumber2);

        /// <summary>
        /// Returns  all used procedures in program
        /// </summary>
        List<string> GetAllUsedProcedures();


        /// <summary>
        /// Returns  all used statement numbers and their type in program
        /// </summary>
        List<UsedStatementElement> GetAllUsedStatements();

        /// <summary>
        /// Returns  if container statement given in statementNumber1 is followed by statement given in statementNumber2
        /// </summary>
        bool Follows(int statementNumber1, int statementNumber2);

        bool FollowsStar(int statementNumber1, int statementNumber2);

    }
}