using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Autofac;
using SPA.Models;
using SPA.Pkb.Exceptions;
using SPA.Pkb.Helpers;

namespace SPA.Pkb
{
    /// <summary>
    /// Class that is parsing source code into entities used in PKB
    /// NOTICE: Adopted convention is to use Syntax tokens in lower case!
    /// </summary>
    public class Parser
    {
        private IPkb PkbReference;

        public Parser()
        {
            PkbReference = Initialization.PkbReference;
        }

        #region SharedLogic

        #region Global Variables
        /// <summary>
        /// Variable that tells parser what should be next correct token
        /// </summary>
        private string nextToken;

        /// <summary>
        /// List of all tokens in source code excluding whitespaces
        /// </summary>
        private List<string> tokenList;

        /// <summary>
        /// Used to indicate position in tokenList
        /// </summary>
        private int tokenIterator = 0;

        /// <summary>
        /// Used to indicate actual line number
        /// </summary>
        private int currentLineNumber = 0;

        private List<NextNode> currentProcedure;

        private Stack<int> lineNumberStack = new Stack<int>();

        private int lastLineNumber = 1;

        private int statementCounter = 1;

        private int lastIfLineNumber = -1;


        /// <summary>
        /// Stack of operants used during expression parse
        /// </summary>
        private Stack<string> expresionOperandsStack = new Stack<string>();

        /// <summary>
        /// Stack of operators used during expression parse
        /// </summary>
        private Stack<string> expresionOperatorsStack = new Stack<string>();

        /// <summary>
        /// List of tokens used to create tree for expressions
        /// </summary>
        private List<string> expressionTreeTokens;

        /// <summary>
        /// Used to indicate position in expressionTreeTokens
        /// </summary>
        private int expressionTreeIterator = 0;

        #endregion Global Variables

        public bool FinishedParsing;

        public void StartParsing()
        {
            tokenList = PopulateTokenList();
            ProgramParse();
            FinishedParsing = true;
            Console.WriteLine("Ready");
            VarTable.Print();
            PkbReference.PrintAst();
            //Pkb.CreateControlFlowGraph();
        }

        public List<string> PopulateTokenList()
        {
            if (string.IsNullOrEmpty(SourceRepository.SourceCode))
                throw new ParsingException("Source code is empty");
            var arr = SourceRepository.SourceCode.Split(null);
            var list = new List<string>(arr);
            CleanEmptyTokens(ref list);
            SplitAllCharacter(ref list);
            return list;
        }

        /// <summary>
        /// Get rid of any "" items
        /// </summary>
        /// <param name="tokens"></param>
        private void CleanEmptyTokens(ref List<string> tokens)
        {
            tokens.RemoveAll(x => x.Equals(""));
        }

        /// <summary>
        /// In SIMPLE expressions there is no whitespace between this character: + / - * ( ) ;
        /// This method splits "1+1" to "1" , "+" , "1"
        /// </summary>
        /// <param name="tokens"></param>
        private void SplitAllCharacter(ref List<string> tokens)
        {
            var newList = new List<string>();

            foreach (string token in tokens)
            {
                if (token.Contains(";") || token.Contains("*") || token.Contains("(") || token.Contains(")") || token.Contains("+") || token.Contains("-") || token.Contains("/") || token.Contains("="))
                {
                    if (token.Length != 1)
                    {
                        string[] newSeparateList = Regex.Split(token, @"(?=[(*;+=/)])");

                        foreach (string item in newSeparateList)
                        {
                            string tempItem = item;
                            if (tempItem.Length > 1)
                            {
                                if (tempItem[0] == '*' || tempItem[0] == '+' || tempItem[0] == '-' || tempItem[0] == '/' || tempItem[0] == ')' || tempItem[0] == '(' || tempItem[0] == ';' || tempItem[0] == '=')
                                {

                                    newList.Add(tempItem[0].ToString());
                                    newList.Add(tempItem.Substring(1));
                                    continue;

                                }
                                else
                                {
                                    newList.Add(item);
                                }
                            }
                            else
                            {
                                newList.Add(item);
                            }
                        }

                    }
                    else
                    {
                        newList.Add(token);
                    }
                }
                else
                {
                    newList.Add(token);
                }
            }

            newList.RemoveAll(x => x.Equals(""));
            tokens.Clear();
            foreach (var item in newList)
            {
                tokens.Add(item);
            }
        }


        public string Match(TokenType type, string token = "")
        {
            //name or value
            string validatedToken = string.Empty;
            switch (type)
            {
                case TokenType.Syntax:

                    if (String.Equals(nextToken.ToLower(), token))
                    {
                        nextToken = GetToken();
                    }
                    else
                    {
                        throw new ParsingException("Error in parsing");
                    }

                    break;
                case TokenType.Name:

                    //nextToken is value of currently parsed expression
                    ValidateName(nextToken);
                    validatedToken = nextToken;
                    nextToken = GetToken();

                    break;
                case TokenType.Value:

                    if (ValidateValue(nextToken))
                    {
                        //Constant value
                        validatedToken = nextToken;

                    }
                    else
                    {
                        //Another variable value
                        validatedToken = nextToken;

                    }
                    nextToken = GetToken();

                    break;
                case TokenType.Operator:

                    ValidateOperator(nextToken);
                    validatedToken = nextToken;
                    nextToken = GetToken();

                    break;
                default:
                    throw new ParsingException("Unknown token type");

            }
            return validatedToken;
        }

        private void ValidateName(string name)
        {
            if (Regex.IsMatch(name, @"^[a-zA-Z]{1}[a-zA-Z0-9]*$") == false)
                throw new ParsingException("Invalid variable/procedure name");
        }

        private void ValidateOperator(string oper)
        {
            if (CheckIfOperator(oper))
                return;
            throw new ParsingException("Invalid operator");
        }

        private bool ValidateValue(string value)
        {
            //Value is constant
            if (Regex.IsMatch(value, @"^[0-9]+$"))
                return true;
            //Value is another variable
            else if (Regex.IsMatch(value, @"^[a-zA-Z0-9]+$"))
                return false;
            else
                throw new ParsingException("Invalid variable value");
        }

        private string GetToken()
        {
            if (tokenIterator < tokenList.Count)
                return tokenList[tokenIterator++];
            return null;
        }

        #endregion SharedLogic

        #region EntitiesToParse

        private void ProgramParse()
        {
            AstNode currentNode = PkbReference.AbstractSyntaxTree.AddNode(NodeType.Program, 0, tokenValue: "Main", isRoot: true);
            nextToken = GetToken();
            int i = 1;
            while (!string.IsNullOrEmpty(nextToken))
            {
                currentProcedure = new List<NextNode>();
                currentLineNumber = 0;
                lastLineNumber = 1;
                ProcedureParse();
                VarTable.NextTable.Add(i, currentProcedure.OrderBy(x => x.FirstLine).ToList());
                i++;
            }
            currentNode.IsAstEndScope = true;
        }

        private void ProcedureParse()
        {
            Match(TokenType.Syntax, "procedure");
            string nameOfNode = Match(TokenType.Name);
            if (PkbReference.UsedProcedures == null)
                PkbReference.UsedProcedures = new List<string>();
            PkbReference.UsedProcedures.Add(nameOfNode);
            AstNode currentProc = PkbReference.AbstractSyntaxTree.AddNode(NodeType.Procedure, 0, tokenValue: nameOfNode);
            Match(TokenType.Syntax, "{");
            StmtLstParse();
            currentProc.IsAstEndScope = true;
            Match(TokenType.Syntax, "}");
        }

        private void StmtLstParse()
        {
            var currentStatementList = PkbReference.AbstractSyntaxTree.AddNode(NodeType.StatementList, 0);
            StmtLstParseInternal();
            currentStatementList.IsAstEndScope = true;
        }

        private void StmtLstParseInternal()
        {
            StmtParse();
            if (nextToken == "}")
                return;
            else
                StmtLstParseInternal();
        }

        private void StmtParse()
        {
            if (nextToken.Equals("call"))
                CallParse();
            else if (nextToken.Equals("while"))
                WhileParse();
            else if (nextToken.Equals("if"))
                IfParse();
            else
                AssignmentParse();
        }

        private void AssignmentParse()
        {
            currentLineNumber++;
            int assignLine = currentLineNumber;
            CompleteNextTable(assignLine);

            expresionOperandsStack.Clear();
            expresionOperatorsStack.Clear();
            string varName = Match(TokenType.Name);
            Match(TokenType.Syntax, "=");
            AstNode currentAssign = PkbReference.AbstractSyntaxTree.AddNode(NodeType.Assign, lineNumber: currentLineNumber);
            currentAssign.StatementNumber = statementCounter++;
            if (PkbReference.UsedStatements == null)
                PkbReference.UsedStatements = new List<UsedStatementElement>();
            PkbReference.UsedStatements.Add(new UsedStatementElement() { statementNumber= currentAssign.StatementNumber, stementType = currentAssign.NodeType });
            PkbReference.AbstractSyntaxTree.AddNode(NodeType.Variable, 0, tokenValue: varName, isEndedScope: true);
            AstNode expressionNode = ExpressionParse();
            AstNode currentNode = PkbReference.AbstractSyntaxTree.AddNode(expressionNode);
            currentAssign.IsAstEndScope = true;

        }

        private AstNode ExpressionParse()
        {
            if (nextToken.Equals("("))
            {
                Match(TokenType.Syntax, "(");
                expresionOperatorsStack.Push("(");
            }
            if (nextToken.Equals(")"))
            {
                Match(TokenType.Syntax, ")");
                while (!expresionOperatorsStack.Peek().Trim().Equals("("))
                {
                    CreateTreeNodeBasedOnOperandStack();
                }
                expresionOperatorsStack.Pop();
            }
            string operandName = Match(TokenType.Value);
            expresionOperandsStack.Push(operandName);
            if (IsNextTokenSemicolon())
            {
                Match(TokenType.Syntax, ";");
                if (expresionOperatorsStack.Count() == 0)
                    return new AstNode()
                    {
                        NodeType = NodeType.Variable,
                        Value = expresionOperandsStack.Pop(),
                    };
                while (expresionOperatorsStack.Count() > 0)
                    CreateTreeNodeBasedOnOperandStack();
                AstNode tmp = CreateNodeFromOperandStack();
                return tmp;
            }
            if (nextToken.Equals("("))
            {
                Match(TokenType.Syntax, "(");
                expresionOperatorsStack.Push("(");
            }
            if (nextToken.Equals(")"))
            {
                Match(TokenType.Syntax, ")");
                while (!expresionOperatorsStack.Peek().Trim().Equals("("))
                {
                    CreateTreeNodeBasedOnOperandStack();
                }
                expresionOperatorsStack.Pop();
            }
            if (IsNextTokenSemicolon())
            {
                Match(TokenType.Syntax, ";");
                if (expresionOperatorsStack.Count() == 0)
                    return new AstNode()
                    {
                        NodeType = NodeType.Variable,
                        Value = expresionOperandsStack.Pop(),
                    };
                while (expresionOperatorsStack.Count() > 0)
                    CreateTreeNodeBasedOnOperandStack();
                AstNode tmp = CreateNodeFromOperandStack();
                return tmp;
            }
            string operatorName = Match(TokenType.Operator);
            if (IsPriorityHigherThatOperatorsStackTop(operatorName))
                expresionOperatorsStack.Push(operatorName);
            else
            {
                while (!IsPriorityHigherThatOperatorsStackTop(operatorName))
                    CreateTreeNodeBasedOnOperandStack();
                expresionOperatorsStack.Push(operatorName);
            }
            return ExpressionParse();

        }

        private bool IsNextTokenSemicolon()
        {
            if (nextToken.Equals(";"))
                return true;
            return false;
        }

        private void CallParse()
        {
            currentLineNumber++;
            int callLine = currentLineNumber;
            CompleteNextTable(callLine);

            Match(TokenType.Syntax, "call");
            string varName = Match(TokenType.Name);
            AstNode currentCall = PkbReference.AbstractSyntaxTree.AddNode(NodeType.Call, lineNumber: currentLineNumber);
            currentCall.StatementNumber = statementCounter++;
            if (PkbReference.UsedStatements == null)
                PkbReference.UsedStatements = new List<UsedStatementElement>();
            PkbReference.UsedStatements.Add(new UsedStatementElement() { statementNumber = currentCall.StatementNumber, stementType = currentCall.NodeType });
            PkbReference.AbstractSyntaxTree.AddNode(NodeType.Variable, 0, tokenValue: varName, isEndedScope: true);
            currentCall.IsAstEndScope = true;
            Match(TokenType.Syntax, ";");
        }

        private void WhileParse()
        {
            currentLineNumber++;
            int whileLine = currentLineNumber;
            CompleteNextTable(whileLine);
            lineNumberStack.Push(whileLine);

            Match(TokenType.Syntax, "while");
            string varName = Match(TokenType.Value);
            AstNode currentWhile = PkbReference.AbstractSyntaxTree.AddNode(NodeType.While, lineNumber: currentLineNumber);
            currentWhile.StatementNumber = statementCounter++;
            if (PkbReference.UsedStatements == null)
                PkbReference.UsedStatements = new List<UsedStatementElement>();
            PkbReference.UsedStatements.Add(new UsedStatementElement() { statementNumber = currentWhile.StatementNumber, stementType = currentWhile.NodeType });
            PkbReference.AbstractSyntaxTree.AddNode(NodeType.Variable, 0, tokenValue: varName, isEndedScope: true);
            Match(TokenType.Syntax, "{");
            StmtLstParse();
            currentWhile.IsAstEndScope = true;
            Match(TokenType.Syntax, "}");

            CompleteNextTable(whileLine);
            //lastLineNumber = whileLine;
        }

        private void IfParse()
        {
            currentLineNumber++;
            int ifLine = currentLineNumber;
            CompleteNextTable(ifLine);
            lineNumberStack.Push(ifLine);

            Match(TokenType.Syntax, "if");
            string varName = Match(TokenType.Value);
            AstNode currentIf = PkbReference.AbstractSyntaxTree.AddNode(NodeType.If, lineNumber: currentLineNumber);
            currentIf.StatementNumber = statementCounter++;
            if (PkbReference.UsedStatements == null)
                PkbReference.UsedStatements = new List<UsedStatementElement>();
            PkbReference.UsedStatements.Add(new UsedStatementElement() { statementNumber = currentIf.StatementNumber, stementType = currentIf.NodeType });
            PkbReference.AbstractSyntaxTree.AddNode(NodeType.Variable, 0, tokenValue: varName, isEndedScope: true);
            Match(TokenType.Syntax, "then");
            Match(TokenType.Syntax, "{");
            StmtLstParse();
            lastLineNumber = ifLine;
            Match(TokenType.Syntax, "}");
            Match(TokenType.Syntax, "else");
            Match(TokenType.Syntax, "{");
            StmtLstParse();
            lastIfLineNumber = ifLine + 1;
            currentIf.IsAstEndScope = true;
            Match(TokenType.Syntax, "}");

        }

        private void CompleteNextTable(int currentLine)
        {
            if (lastLineNumber != currentLine)
            {
                if (!lineNumberStack.Any())
                {
                    currentProcedure.Add(new NextNode(lastLineNumber, currentLine));
                    lastLineNumber = currentLine;
                }
                else
                {
                    currentProcedure.Add(new NextNode(lineNumberStack.Pop(), currentLine));
                    lastLineNumber = currentLine;
                }
                if (lastIfLineNumber != -1)
                {
                    currentProcedure.Add(new NextNode(lastIfLineNumber, currentLine));
                    lastIfLineNumber = -1;
                }
            }
        }


        public bool IsPriorityHigherThatOperatorsStackTop(string newOperator)
        {
            if (expresionOperatorsStack.Count == 0)
                return true;
            string lastOperator = expresionOperatorsStack.Peek().Trim();
            if (lastOperator.Equals("(") || lastOperator.Equals(")"))
                return true;
            if (lastOperator.Equals("*") || lastOperator.Equals("/"))
                return false;
            else if (lastOperator.Equals("+") || lastOperator.Equals("-"))
            {
                if (newOperator.Equals("*") || newOperator.Equals("/"))
                    return true;
                return false;
            }
            return false;

        }

        private void CreateTreeNodeBasedOnOperandStack()
        {
            string rightNodeString = expresionOperandsStack.Pop().Trim();
            string leftNodeString = expresionOperandsStack.Pop().Trim();
            string operatorNodeString = expresionOperatorsStack.Pop().Trim();
            expresionOperandsStack.Push($"{operatorNodeString};(;{leftNodeString};{rightNodeString};)");
        }

        private AstNode CreateNodeFromOperandStack()
        {
            expressionTreeIterator = 0;
            if (expresionOperandsStack.Count() > 1)
                throw new ParsingException("Invalid expression");
            string nodeTreeString = expresionOperandsStack.Pop();
            expressionTreeTokens = nodeTreeString.Split(';').ToList();
            return CreateNodeForExpression();
        }

        private AstNode CreateNodeForExpression()
        {
            AstNode treeNode = new AstNode() { NodeType = NodeType.Operator };
            if (!CheckIfOperator(expressionTreeTokens[expressionTreeIterator]))
                throw new ParsingException("Invalid expression");
            string operatorValue = expressionTreeTokens[expressionTreeIterator++];
            treeNode.Value = operatorValue;
            AstNode leftNode;
            AstNode rightNode;
            if (CheckIfOperator(expressionTreeTokens[++expressionTreeIterator]))
            {
                leftNode = CreateNodeForExpression();
                treeNode.Children.Add(leftNode);
            }
            else
            {
                leftNode = new AstNode() { NodeType = NodeType.Variable, Value = expressionTreeTokens[expressionTreeIterator++], ParentNode = treeNode };
                treeNode.Children.Add(leftNode);
            }
            if (CheckIfOperator(expressionTreeTokens[expressionTreeIterator]))
            {
                rightNode = CreateNodeForExpression();
                treeNode.Children.Add(rightNode);
            }
            else
            {
                rightNode = new AstNode() { NodeType = NodeType.Variable, Value = expressionTreeTokens[expressionTreeIterator++], ParentNode = treeNode };
                treeNode.Children.Add(rightNode);
            }
            leftNode.RightSiblingNode = rightNode;
            leftNode.ParentNode = treeNode;
            rightNode.ParentNode = treeNode;
            expressionTreeIterator++;
            return treeNode;
        }

        private bool CheckIfOperator(string operatorValue)
        {
            if (operatorValue.Equals("*") || operatorValue.Equals("/") || operatorValue.Equals("+") || operatorValue.Equals("-"))
                return true;
            return false;
        }

        #endregion
    }

    public enum TokenType
    {
        Syntax,
        Name,
        Value,
        Operator
    }
}