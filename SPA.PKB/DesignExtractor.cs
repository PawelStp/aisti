using System.Collections.Generic;
using System.Linq;
using SPA.Models;
using System.Text.RegularExpressions;
using Autofac;

namespace SPA.Pkb
{
    public class DesignExtractor
    {
        private IPkb PkbReference;
        private List<string> UsedVariables;
        public DesignExtractor()
        {
            PkbReference = Initialization.PkbReference;
        }

        #region ProcTable

        public void CreateProcTable()
        {
            var resultList = new List<ProcTableCell>();
            var modifiesList = new List<StatementColumn>();
            var usesList = new List<StatementColumn>();
            var ast = PkbReference.AbstractSyntaxTree;
            var modifiesProc = new Dictionary<string, List<string>>();
            var usesProc = new Dictionary<string, List<string>>();
            var parentList = new List<ParrentTableCell>();
            var followsDicionary = new  Dictionary < int,int>();
            UsedVariables = new List<string>();

            //For each procedure
            foreach (var procedure in ast.GetAllChildren(ast.AstRoot))
            {
                StatementTableModel result = FindStatements(procedure, ref resultList, ref modifiesList, ref usesList, ref parentList, ref followsDicionary);
                modifiesProc.Add(procedure.Value, result.ModifiesElements);
                usesProc.Add(procedure.Value, result.UsesElements);
            }
            PkbReference.ProcTable = resultList;
            PkbReference.StatementModifiesTable = modifiesList.OrderBy(x=>x.StatementNumber).ToList();
            PkbReference.ProceduresModifiesTable = modifiesProc;
            PkbReference.StatementUsesTable = usesList.OrderBy(x => x.StatementNumber).ToList();
            PkbReference.ProceduresUsesTable = usesProc;
            PkbReference.UsedVariables = UsedVariables;
            PkbReference.ParentTable = parentList.OrderBy(x=>x.ContainerStatementNumber).ToList();
            PkbReference.FollowsTable = followsDicionary;
        }

        //private static void FindCallInStatements(AstNode procedure, ref List<ProcTableCell> resultList)
        //{
        //    //For each statement
        //    foreach (var node in ExtractStatements(procedure))
        //    {
        //        //Find call
        //        if (node.NodeType == NodeType.Call)
        //        {
        //            var parentValue = GetParentProcedureName(node);
        //            var cell = resultList.FirstOrDefault(c => c.ProcedureName == parentValue);
        //            if (cell != null)
        //            {
        //                cell.CalledProcedures.Add(GetCalledProcedureName(node));
        //            }
        //            else
        //            {
        //                resultList.Add(new ProcTableCell()
        //                {
        //                    ProcedureName = parentValue,
        //                    CalledProcedures = new List<string> { GetCalledProcedureName(node) }
        //                });
        //            }
        //        }
        //        else if (node.NodeType == NodeType.If || node.NodeType == NodeType.While)
        //        {
        //            FindCallInStatements(node, ref resultList);
        //        }
        //    }
        //}

        private StatementTableModel FindStatements(AstNode parentNode, ref List<ProcTableCell> resultList, ref List<StatementColumn> modifiesList, ref List<StatementColumn> usesList, ref List<ParrentTableCell> parentList, ref Dictionary<int,int> followsDicionary)
        {
            List<string> tmpElementsModifies = new List<string>();
            List<string> tmpElementsUses = new List<string>();
            var statements = parentNode?.Children;
            if (!(parentNode.NodeType == NodeType.StatementList))
                statements = ExtractStatements(parentNode);
            int previousStatementNumber = 0;
            foreach (var node in statements)
            {
                if (node.NodeType == NodeType.Variable && Regex.IsMatch(node.Value, @"^[a-zA-Z_$][a-zA-Z_$0-9]*$") && !UsedVariables.Contains(node.Value))
                    UsedVariables.Add(node.Value);
                if (previousStatementNumber == 0)
                    previousStatementNumber = node.StatementNumber;
                else
                {
                    if(!followsDicionary.ContainsKey(previousStatementNumber))
                        followsDicionary.Add(previousStatementNumber, node.StatementNumber);
                    previousStatementNumber = node.StatementNumber;
                }
                    
                //Find call
                if (node.NodeType == NodeType.Assign)
                {
                    tmpElementsModifies.Add(AddAssignModifiesColumn(node, ref modifiesList));
                    tmpElementsUses.AddRange(AddUsesAssignData(node,ref usesList).Distinct());
                }
                else if (node.NodeType == NodeType.If)
                {
                    //Parent
                    ParrentTableCell cell = GetParentCell(node, ref parentList);
                    foreach (var childNode in node.Children[1].Children)
                    {
                        if (!cell.ChildsStatementNumber.Contains(childNode.StatementNumber))
                            cell.ChildsStatementNumber.Add(childNode.StatementNumber);
                    }
                    foreach (var childNode in node.Children[2].Children)
                    {
                        if (!cell.ChildsStatementNumber.Contains(childNode.StatementNumber))
                            cell.ChildsStatementNumber.Add(childNode.StatementNumber);
                    }

                    List<string> tmpModifiesArray = new List<string>();
                    List<string> tmpUsesArray = new List<string>();
                    StatementTableModel thenResult = FindStatements(node.Children[1], ref resultList, ref modifiesList, ref usesList, ref parentList, ref followsDicionary);
                    StatementTableModel elseResult = FindStatements(node.Children[2], ref resultList, ref modifiesList, ref usesList, ref parentList, ref followsDicionary);

                    //Modifies
                    tmpModifiesArray.AddRange(thenResult.ModifiesElements.Distinct());
                    tmpModifiesArray.AddRange(elseResult.ModifiesElements.Distinct());
                    CheckTableRow(node, ref tmpModifiesArray, ref modifiesList);
                    tmpElementsModifies.AddRange(tmpModifiesArray.Distinct());

                    //Uses
                    if (node.Children[0].NodeType == NodeType.Variable && Regex.IsMatch(node.Children[0].Value, @"^[a-zA-Z_$][a-zA-Z_$0-9]*$"))
                    {
                        if (!UsedVariables.Contains(node.Children[0].Value))
                            UsedVariables.Add(node.Children[0].Value);
                        tmpUsesArray.Add(node.Children[0].Value);
                    }
                    tmpUsesArray.AddRange(thenResult.UsesElements.Distinct());
                    tmpUsesArray.AddRange(elseResult.UsesElements.Distinct());
                    CheckTableRow(node, ref tmpUsesArray, ref usesList);
                    tmpElementsUses.AddRange(tmpUsesArray.Distinct());
                }
                else if (node.NodeType == NodeType.While)
                {
                    //Parent
                    ParrentTableCell cell=GetParentCell(node, ref parentList);
                    foreach (var childNode in node.Children[1].Children)
                    {
                        if (!cell.ChildsStatementNumber.Contains(childNode.StatementNumber))
                            cell.ChildsStatementNumber.Add(childNode.StatementNumber);
                    }

                    List<string> tmpModifiesArray = new List<string>();
                    List<string> tmpUsesArray = new List<string>();
                    StatementTableModel result = FindStatements(node.Children[1], ref resultList, ref modifiesList, ref usesList, ref parentList, ref followsDicionary);

                    //Modifies
                    tmpModifiesArray.AddRange(result.ModifiesElements.Distinct());
                    CheckTableRow(node, ref tmpModifiesArray, ref modifiesList);
                    tmpElementsModifies.AddRange(tmpModifiesArray.Distinct());

                    //Uses
                    if (node.Children[0].NodeType == NodeType.Variable && Regex.IsMatch(node.Children[0].Value, @"^[a-zA-Z_$][a-zA-Z_$0-9]*$"))
                    {
                        if (!UsedVariables.Contains(node.Children[0].Value))
                            UsedVariables.Add(node.Children[0].Value);
                        tmpUsesArray.Add(node.Children[0].Value);
                    }
                    tmpUsesArray.AddRange(result.UsesElements.Distinct());
                    CheckTableRow(node, ref tmpUsesArray, ref usesList);
                    tmpElementsUses.AddRange(tmpUsesArray.Distinct());
                }
                else if (node.NodeType == NodeType.Call)
                {
                    var parentValue = GetParentProcedureName(node);
                    var cell = resultList.FirstOrDefault(c => c.ProcedureName == parentValue);
                    string calledProcedureName = GetCalledProcedureName(node);
                    if (cell != null)
                    {
                        if(!cell.CalledProcedures.Contains(calledProcedureName))
                            cell.CalledProcedures.Add(calledProcedureName);
                    }
                    else
                    {
                        resultList.Add(new ProcTableCell()
                        {
                            ProcedureName = parentValue,
                            CalledProcedures = new List<string> { GetCalledProcedureName(node) }
                        });
                    }
                    List<string> tmpModifiesArray = new List<string>();
                    List<string> tmpUsesArray = new List<string>();
                    AstNode calledProcedureNode = PkbReference.AbstractSyntaxTree.GetAllChildren(PkbReference.AbstractSyntaxTree.AstRoot).Where(x => x.Value == calledProcedureName).FirstOrDefault();
                    StatementTableModel result = FindStatements(calledProcedureNode, ref resultList, ref modifiesList, ref usesList, ref parentList, ref followsDicionary);

                    //Modifies
                    tmpModifiesArray.AddRange(result.ModifiesElements.Distinct());
                    CheckTableRow(node, ref tmpModifiesArray, ref modifiesList);
                    tmpElementsModifies.AddRange(tmpModifiesArray.Distinct());

                    //Uses
                    tmpUsesArray.AddRange(result.UsesElements.Distinct());
                    CheckTableRow(node, ref tmpUsesArray, ref usesList);
                    tmpElementsUses.AddRange(tmpUsesArray.Distinct());
                }
            }
            return new StatementTableModel()
            {
                ModifiesElements = tmpElementsModifies.Distinct().ToList(),
                UsesElements = tmpElementsUses.Distinct().ToList(),
            };
        }


        private ParrentTableCell GetParentCell(AstNode node, ref List<ParrentTableCell> parentList)
        {
            ParrentTableCell cell = parentList.Where(x => x.ContainerStatementNumber == node.StatementNumber).FirstOrDefault();
            if (cell == null)
            {
                cell = new ParrentTableCell()
                {
                    ContainerStatementNumber = node.StatementNumber,
                    ChildsStatementNumber = new List<int>()
                };
                parentList.Add(cell);
            }
            return cell;
        }


        private string AddAssignModifiesColumn(AstNode node, ref List<StatementColumn>  resultList)
        {
            var cell = resultList.FirstOrDefault(c => c.StatementNumber == node.StatementNumber);
            AstNode chldrenNode = node.Children.FirstOrDefault();
            if (chldrenNode.NodeType == NodeType.Variable && Regex.IsMatch(chldrenNode.Value, @"^[a-zA-Z_$][a-zA-Z_$0-9]*$") && !UsedVariables.Contains(chldrenNode.Value))
                UsedVariables.Add(chldrenNode.Value);
            string value = node.Children.FirstOrDefault().Value;
            if (cell != null)
            {
                if(!cell.Variables.Contains(value))
                    cell.Variables.Add(value);
            }
            else
            {
                resultList.Add(new StatementColumn()
                {
                    StatementNumber = node.StatementNumber,
                    StatementType = node.NodeType,
                    Variables = new List<string> { value }
                });
            }
            return value;
        }

        private void CheckTableRow(AstNode node, ref List<string> tmpArray, ref List<StatementColumn> table)
        {
            var cell = table.FirstOrDefault(c => c.StatementNumber == node.StatementNumber);
            if (cell != null)
            {
                cell.Variables.AddRange(tmpArray.Distinct());
                cell.Variables = cell.Variables.Distinct().ToList();
            }
            else
            {
                StatementColumn column = new StatementColumn()
                {
                    StatementNumber = node.StatementNumber,
                    StatementType = node.NodeType,
                    Variables = new List<string>()
                };
                column.Variables.AddRange(tmpArray.Distinct());
                table.Add(column);
            }
        }
        
        private List<string> AddUsesAssignData(AstNode node, ref List<StatementColumn> resultList)
        {
            if (node.NodeType == NodeType.Variable && Regex.IsMatch(node.Value, @"^[a-zA-Z_$][a-zA-Z_$0-9]*$") && !UsedVariables.Contains(node.Value))
                UsedVariables.Add(node.Value);
            var cell = resultList.FirstOrDefault(c => c.StatementNumber == node.StatementNumber);
            List<string> value = value = AddAssignUsesColumn(node.Children[1]);
            value=value.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            CheckTableRow(node, ref value, ref resultList);
            return value;
        }

        private List<string> AddAssignUsesColumn(AstNode node)
        {
            List<string> returnList = new List<string>();
            AstNode currentNode = node;
            while(currentNode!=null)
            {
                AstNode tmpNode = currentNode;
                while(tmpNode!=null)
                {
                    if (tmpNode.NodeType == NodeType.Variable && Regex.IsMatch(tmpNode.Value, @"^[a-zA-Z_$][a-zA-Z_$0-9]*$"))
                    {
                        if (!UsedVariables.Contains(tmpNode.Value))
                            UsedVariables.Add(tmpNode.Value);
                        returnList.Add(tmpNode.Value);
                    }  
                    tmpNode = tmpNode.RightSiblingNode;
                }
                currentNode = (currentNode.Children.Count>0) ? currentNode.Children[0]:null;
            }
            return returnList;
        }

        private IList<AstNode> ExtractStatements(AstNode containerNode)
        {
            var stmtListNode = containerNode.Children.FirstOrDefault(n => n.NodeType == NodeType.StatementList);
            return stmtListNode?.Children;
        }

        private string GetParentProcedureName(AstNode node)
        {
            if (node.ParentNode.NodeType == NodeType.Procedure)
                return node.ParentNode.Value;
            else
                return GetParentProcedureName(node.ParentNode); ;
        }

        private string GetCalledProcedureName(AstNode node)
        {
            return node.Children.FirstOrDefault(n => n.NodeType == NodeType.Variable)?.Value;
        }

        #endregion
    }

    public class ProcTableCell
    {
        public string ProcedureName { get; set; }
        public List<string> CalledProcedures { get; set; }
    }

    public class StatementColumn
    {
        public int StatementNumber { get; set; }
        public NodeType StatementType { get; set; }
        public List<string> Variables { get; set; }
    }

    public class StatementTableModel
    {
        public List<string> ModifiesElements { get; set; }
        public List<string> UsesElements { get; set; }
    }

    public class ParrentTableCell
    {
        public int ContainerStatementNumber { get; set; }
        public List<int> ChildsStatementNumber { get; set; }
    }
}