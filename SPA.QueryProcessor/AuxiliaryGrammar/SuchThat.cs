using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.QueryProcessor.LexicalRules;
using SPA.Pkb;
using SPA.QueryProcessor.Preprocessor;
using SPA.QueryProcessor.DesignEntity;
using SPA.Models;

namespace SPA.QueryProcessor.AuxiliaryGrammar
{
    /// <summary>
    /// Class with methods called after keyword 'such that' in PQL query
    /// </summary>
    public class SuchThat
    {
        private DeclarationsArray Declarations;
        private IPkb Pkb;

        public SuchThat(DeclarationsArray Declarations)
        {
            this.Declarations = Declarations;
            Pkb = Initialization.PkbReference;
        }

        /// <summary>
        /// Executes correct method
        /// </summary>
        /// <param name="methodName">method which should be executed</param>
        /// <param name="param1">first param from query</param>
        /// <param name="param2">second param from query</param>
        /// <returns></returns>
        /// <returns></returns>
        public MethodResultList DoSuchThatMethod(string methodName, string param1, string param2)
        {
            switch (methodName)
            {
                case "MODIFIES":
                    {
                        //helper objects needed to check the type, param1 can be either PROCEDURE or STATEMENT
                        VarRef varRef = new VarRef(param2);
                        varRef.IsGrammarCorrect();
                        ProcRef procRef = new ProcRef(param1);
                        procRef.IsGrammarCorrect();
                        StmtRef stmtRef = new StmtRef(param1);
                        stmtRef.IsGrammarCorrect();

                        //present on DeclarationsArray and has required type
                        if (!IsTypeCorrect(varRef))
                        {
                            throw new SuchThatException($"#{varRef.entry} isn't declared as VARIABLE type");
                        }

                        if (IsTypeCorrect(procRef)) return Modifies(procRef, varRef);
                        else
                        if (IsTypeCorrect(stmtRef)) return Modifies(stmtRef, varRef);
                        else
                            throw new SuchThatException($"#{methodName} first param is neither 'PROCEDURE' or 'STATEMENT'");
                    }

                case "USES":
                    {
                        //helper objects needed to check the type, param1 can be either PROCEDURE or STATEMENT
                        VarRef varRef = new VarRef(param2);
                        varRef.IsGrammarCorrect();
                        ProcRef procRef = new ProcRef(param1);
                        procRef.IsGrammarCorrect();
                        StmtRef stmtRef = new StmtRef(param1);
                        stmtRef.IsGrammarCorrect();

                        //present on DeclarationsArray and has required type
                        if (!IsTypeCorrect(varRef))
                        {
                            throw new SuchThatException($"#{varRef.entry} isn't declared as VARIABLE type");
                        }

                        bool stmtFlag = IsTypeCorrect(stmtRef);
                        bool procFlag = IsTypeCorrect(procRef);

                        if (procFlag) return Uses(procRef, varRef);
                        else
                        if (stmtFlag) return Uses(stmtRef, varRef);
                        else
                            throw new SuchThatException($"#{methodName} first param is neither 'PROCEDURE' or 'STATEMENT'");
                    }

                case "CALLS":
                    {
                        ProcRef procRef1 = new ProcRef(param1);
                        procRef1.IsGrammarCorrect();
                        ProcRef procRef2 = new ProcRef(param2);
                        procRef2.IsGrammarCorrect();

                        if (IsTypeCorrect(procRef1) && IsTypeCorrect(procRef2))
                            return Calls(procRef1, procRef2);
                        else
                            throw new SuchThatException($"#{methodName} first or second param isn't 'PROCEDURE'");
                    }

                //case "CALLS*":
                //    {
                //        ProcRef procRef1 = new ProcRef(param1);
                //        procRef1.IsGrammarCorrect();
                //        ProcRef procRef2 = new ProcRef(param2);
                //        procRef2.IsGrammarCorrect();

                //        if (IsTypeCorrect(procRef1) && IsTypeCorrect(procRef2))
                //            return CallsAsterisk(procRef1, procRef2);
                //        else
                //            throw new SuchThatException($"#{methodName} first or second param isn't 'PROCEDURE'");
                //    }

                case "PARENT":
                    {
                        StmtRef stmtRef1 = new StmtRef(param1);
                        stmtRef1.IsGrammarCorrect();
                        StmtRef stmtRef2 = new StmtRef(param2);
                        stmtRef2.IsGrammarCorrect();

                        if (IsTypeCorrect(stmtRef1) && IsTypeCorrect(stmtRef2))
                            return Parent(stmtRef1, stmtRef2);
                        else
                            throw new SuchThatException($"#{methodName} first or second param isn't 'STATEMENT'");
                    }

                case "PARENT*":
                    {
                        StmtRef stmtRef1 = new StmtRef(param1);
                        stmtRef1.IsGrammarCorrect();
                        StmtRef stmtRef2 = new StmtRef(param2);
                        stmtRef2.IsGrammarCorrect();

                        if (IsTypeCorrect(stmtRef1) && IsTypeCorrect(stmtRef2))
                            return ParentAsterisk(stmtRef1, stmtRef2);
                        else
                            throw new SuchThatException($"#{methodName} first or second param isn't 'STATEMENT'");
                    }

                case "FOLLOWS":
                    {
                        StmtRef stmtRef1 = new StmtRef(param1);
                        stmtRef1.IsGrammarCorrect();
                        StmtRef stmtRef2 = new StmtRef(param2);
                        stmtRef2.IsGrammarCorrect();

                        if (IsTypeCorrect(stmtRef1) && IsTypeCorrect(stmtRef2))
                            return Follows(stmtRef1, stmtRef2);
                        else
                            throw new SuchThatException($"#{methodName} first or second param isn't 'STATEMENT'");
                    }

                case "FOLLOWS*":
                    {
                        StmtRef stmtRef1 = new StmtRef(param1);
                        stmtRef1.IsGrammarCorrect();
                        StmtRef stmtRef2 = new StmtRef(param2);
                        stmtRef2.IsGrammarCorrect();

                        if (IsTypeCorrect(stmtRef1) && IsTypeCorrect(stmtRef2))
                            return FollowsAsterisk(stmtRef1, stmtRef2);
                        else
                            throw new SuchThatException($"#{methodName} first or second param isn't 'STATEMENT'");
                    }

                case "NEXT":
                    return NotImplemented();

                case "NEXT*":
                    return NotImplemented();

                case "AFFECTS":
                    return NotImplemented();

                case "AFFECTS*":
                    return NotImplemented();

                default:
                    throw new NotImplementedException($"#Invalid methodName for Such That. Got: {methodName}");
            }
        }

        public MethodResultList NotImplemented()
        {
            ProcRef pr = new ProcRef("komarch");
            pr.IsGrammarCorrect();
            VarRef vr = new VarRef("_");
            vr.IsGrammarCorrect();
            MethodResultList mrl = new MethodResultList(typeof(ProcRef), typeof(VarRef));
            mrl.List1.Add(pr);
            mrl.List2.Add(vr);
            mrl.ListType1 = typeof(Ident);
            mrl.ListType2 = typeof(Placeholder);
            mrl.QueryParam1 = "komarch";
            mrl.QueryParam2 = "_";
            return mrl;
        }

        public MethodResultList Modifies(ProcRef procRef, VarRef varRef)
        {
            Type procRefType = procRef.EntryTypeReference.GetType();
            Type varRefType = varRef.EntryTypeReference.GetType();
            MethodResultList modifiesReturn = new MethodResultList(procRef, varRef);
            List<string> allProcedures = Pkb.GetAllUsedProcedures();

            if (procRefType == typeof(Ident))
            {
                if (varRefType == typeof(Ident) || varRefType == typeof(Placeholder))
                {
                    foreach (string ptc in allProcedures)
                    {
                        List<string> variables = Pkb.ModifiesAllProcModVar(ptc);
                        foreach (string v in variables)
                        {
                            modifiesReturn.List1.Add(new ProcRef(ptc));
                            modifiesReturn.List2.Add(new VarRef(v));
                        }
                    }
                }
                else if (varRefType == typeof(BracesIdent))
                {
                    string varNameWithoutBraces = varRef.entry.Substring(1, varRef.entry.Length - 2);

                    List<string> procedures = Pkb.ModifiesAllVarModProc(varNameWithoutBraces);
                    foreach (string p in procedures)
                    {
                        modifiesReturn.List1.Add(new ProcRef(p));
                        modifiesReturn.List2.Add(new VarRef(varNameWithoutBraces));
                    }
                }
                else
                    throw new SuchThatException($"#Invalid VarRef type: expected 'Ident', 'Placeholder' or 'BracesIdent' for {varRef.entry}");
            }
            else if (procRefType == typeof(BracesIdent))
            {
                if (varRefType == typeof(Ident) || varRefType == typeof(Placeholder))
                {
                    string procNameWithoutBraces = procRef.entry.Substring(1, procRef.entry.Length - 2);

                    List<string> variables = Pkb.ModifiesAllProcModVar(procNameWithoutBraces);
                    foreach (string v in variables)
                    {
                        modifiesReturn.List1.Add(new ProcRef(procNameWithoutBraces));
                        modifiesReturn.List2.Add(new VarRef(v));
                    }
                }
                else if (varRefType == typeof(BracesIdent))
                {
                    string procNameWithoutBraces = procRef.entry.Substring(1, procRef.entry.Length - 2);
                    string varNameWithoutBraces = varRef.entry.Substring(1, varRef.entry.Length - 2);

                    List<string> variables = Pkb.ModifiesAllProcModVar(procNameWithoutBraces);
                    foreach (string v in variables)
                    {
                        if (v == varNameWithoutBraces)
                        {
                            modifiesReturn.List1.Add(new ProcRef(procNameWithoutBraces));
                            modifiesReturn.List2.Add(new VarRef(v));
                        }
                    }
                }
                else
                    throw new SuchThatException($"#Invalid VarRef type: expected 'Ident', 'Placeholder' or 'BracesIdent' for {varRef.entry}");
            }
            else if (procRefType == typeof(Placeholder))
            {
                throw new SuchThatException($"#Invalid ProcRef type: expected 'Ident' or 'BracesIdent' for {procRef.entry}");
            }
            else
                throw new SuchThatException($"#Invalid ProcRef type: expected 'Ident' or 'BracesIdent' for {procRef.entry}");

            return modifiesReturn;
        }

        public MethodResultList Modifies(StmtRef stmtRef, VarRef varRef)
        {
            Type stmtRefType = stmtRef.EntryTypeReference.GetType();
            Type varRefType = varRef.EntryTypeReference.GetType();
            MethodResultList modifiesReturn = new MethodResultList(stmtRef, varRef);

            if (stmtRefType == typeof(Ident))
            {
                NodeType statementNodeType = ConvertStatementToPkbType(stmtRef.Type);

                if (varRefType == typeof(Ident) || varRefType == typeof(Placeholder))
                {
                    List<string> allVariables = Pkb.UsedVariables;

                    foreach (string v in allVariables)
                    {
                        List<int> statementNumbers = Pkb.ModifiesStmtThatModVar(v, statementNodeType);

                        foreach (int sn in statementNumbers)
                        {
                            modifiesReturn.List1.Add(new StmtRef("" + sn));
                            modifiesReturn.List2.Add(new VarRef(v));
                        }
                    }
                }
                else if (varRefType == typeof(BracesIdent))
                {
                    string varNameWithoutBraces = varRef.entry.Substring(1, varRef.entry.Length - 2);
                    List<int> statementNumbers = Pkb.ModifiesStmtThatModVar(varNameWithoutBraces, statementNodeType);

                    foreach (int sn in statementNumbers)
                    {
                        modifiesReturn.List1.Add(new StmtRef("" + sn));
                        modifiesReturn.List2.Add(new VarRef(varNameWithoutBraces));
                    }
                }
                else
                    throw new SuchThatException($"#Invalid VarRef type: expected 'Ident', 'Placeholder' or 'BracesIdent' for {varRef.entry}");

            }
            else if (stmtRefType == typeof(IntegerRule))
            {
                if (varRefType == typeof(Ident) || varRefType == typeof(Placeholder))
                {
                    List<string> variables = Pkb.ModifiesVarModInStmt(int.Parse(stmtRef.entry));

                    foreach (string v in variables)
                    {
                        modifiesReturn.List1.Add(new StmtRef(stmtRef.entry));
                        modifiesReturn.List2.Add(new VarRef(v));
                    }
                }
                else if (varRefType == typeof(BracesIdent))
                {
                    List<string> variables = Pkb.ModifiesVarModInStmt(int.Parse(stmtRef.entry));
                    string varNameWithoutBraces = varRef.entry.Substring(1, varRef.entry.Length - 2);

                    foreach (string v in variables)
                    {
                        if (varNameWithoutBraces == v)
                        {
                            modifiesReturn.List1.Add(new StmtRef(stmtRef.entry));
                            modifiesReturn.List2.Add(new VarRef(v));
                        }
                    }
                }
                else
                    throw new SuchThatException($"#Invalid VarRef type: expected 'Ident', 'Placeholder' or 'BracesIdent' for {varRef.entry}");

            }
            else if (stmtRefType == typeof(Placeholder))
            {
                throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident' or 'Integer' for {stmtRef.entry}");
            }
            else
                throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident' or 'BracesIdent' for {stmtRef.entry}");

            return modifiesReturn;
        }

        public MethodResultList Uses(ProcRef procRef, VarRef varRef)
        {
            Type procRefType = procRef.EntryTypeReference.GetType();
            Type varRefType = varRef.EntryTypeReference.GetType();
            MethodResultList usesReturn = new MethodResultList(procRef, varRef);
            List<string> allProcedures = Pkb.UsedProcedures;

            if (procRefType == typeof(Ident))
            {
                if (varRefType == typeof(Ident) || varRefType == typeof(Placeholder))
                {
                    foreach (string ptc in allProcedures)
                    {
                        List<string> variables = Pkb.UsesAllProcModVar(ptc);
                        foreach (string v in variables)
                        {
                            usesReturn.List1.Add(new ProcRef(ptc));
                            usesReturn.List2.Add(new VarRef(v));
                        }
                    }
                }
                else if (varRefType == typeof(BracesIdent))
                {
                    string varNameWithoutBraces = varRef.entry.Substring(1, varRef.entry.Length - 2);

                    List<string> procedures = Pkb.UsesAllVarModProc(varNameWithoutBraces);
                    foreach (string p in procedures)
                    {
                        usesReturn.List1.Add(new ProcRef(p));
                        usesReturn.List2.Add(new VarRef(varNameWithoutBraces));
                    }
                }
                else
                    throw new SuchThatException($"#Invalid VarRef type: expected 'Ident', 'Placeholder' or 'BracesIdent' for {varRef.entry}");
            }
            else if (procRefType == typeof(BracesIdent))
            {
                if (varRefType == typeof(Ident) || varRefType == typeof(Placeholder))
                {
                    string procNameWithoutBraces = procRef.entry.Substring(1, procRef.entry.Length - 2);

                    List<string> variables = Pkb.UsesAllProcModVar(procNameWithoutBraces);
                    foreach (string v in variables)
                    {
                        usesReturn.List1.Add(new ProcRef(procNameWithoutBraces));
                        usesReturn.List2.Add(new VarRef(v));
                    }
                }
                else if (varRefType == typeof(BracesIdent))
                {
                    string procNameWithoutBraces = procRef.entry.Substring(1, procRef.entry.Length - 2);
                    string varNameWithoutBraces = varRef.entry.Substring(1, varRef.entry.Length - 2);

                    if (Pkb.UsesProcedure(procNameWithoutBraces, varNameWithoutBraces))
                    {
                        usesReturn.List1.Add(new ProcRef(procNameWithoutBraces));
                        usesReturn.List2.Add(new VarRef(varNameWithoutBraces));
                    }
                }
                else
                    throw new SuchThatException($"#Invalid VarRef type: expected 'Ident', 'Placeholder' or 'BracesIdent' for {varRef.entry}");
            }
            else if (procRefType == typeof(Placeholder))
            {
                throw new SuchThatException($"#Invalid ProcRef type: expected 'Ident' or 'BracesIdent' for {procRef.entry}");
            }
            else
                throw new SuchThatException($"#Invalid ProcRef type: expected 'Ident' or 'BracesIdent' for {procRef.entry}");

            return usesReturn;
        }

        public MethodResultList Uses(StmtRef stmtRef, VarRef varRef)
        {
            Type stmtRefType = stmtRef.EntryTypeReference.GetType();
            Type varRefType = varRef.EntryTypeReference.GetType();
            MethodResultList usesReturn = new MethodResultList(stmtRef, varRef);

            if (stmtRefType == typeof(Ident))
            {
                NodeType statementNodeType = ConvertStatementToPkbType(stmtRef.Type);

                if (varRefType == typeof(Ident) || varRefType == typeof(Placeholder))
                {
                    List<string> allVariables = Pkb.UsedVariables;

                    foreach (string v in allVariables)
                    {
                        List<int> statementNumbers = Pkb.UsesStmtThatModVar(v, statementNodeType);

                        foreach (int sn in statementNumbers)
                        {
                            usesReturn.List1.Add(new StmtRef("" + sn));
                            usesReturn.List2.Add(new VarRef(v));
                        }
                    }
                }
                else if (varRefType == typeof(BracesIdent))
                {
                    string varNameWithoutBraces = varRef.entry.Substring(1, varRef.entry.Length - 2);
                    List<int> statementNumbers = Pkb.UsesStmtThatModVar(varNameWithoutBraces, statementNodeType);

                    foreach (int sn in statementNumbers)
                    {
                        usesReturn.List1.Add(new StmtRef("" + sn));
                        usesReturn.List2.Add(new VarRef(varNameWithoutBraces));
                    }
                }
                else
                    throw new SuchThatException($"#Invalid VarRef type: expected 'Ident', 'Placeholder' or 'BracesIdent' for {varRef.entry}");

            }
            else if (stmtRefType == typeof(IntegerRule))
            {
                if (varRefType == typeof(Ident) || varRefType == typeof(Placeholder))
                {
                    List<string> variables = Pkb.UsesVarModInStmt(int.Parse(stmtRef.entry));

                    foreach (string v in variables)
                    {
                        usesReturn.List1.Add(new StmtRef(stmtRef.entry));
                        usesReturn.List2.Add(new VarRef(v));
                    }
                }
                else if (varRefType == typeof(BracesIdent))
                {
                    List<string> variables = Pkb.UsesVarModInStmt(int.Parse(stmtRef.entry));
                    string varNameWithoutBraces = varRef.entry.Substring(1, varRef.entry.Length - 2);

                    foreach (string v in variables)
                    {
                        if (varNameWithoutBraces == v)
                        {
                            usesReturn.List1.Add(new StmtRef(stmtRef.entry));
                            usesReturn.List2.Add(new VarRef(v));
                        }
                    }
                }
                else
                    throw new SuchThatException($"#Invalid VarRef type: expected 'Ident', 'Placeholder' or 'BracesIdent' for {varRef.entry}");

            }
            else if (stmtRefType == typeof(Placeholder))
            {
                throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident' or 'Integer' for {stmtRef.entry}");
            }
            else
                throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident' or 'BracesIdent' for {stmtRef.entry}");

            return usesReturn;
        }

        public MethodResultList Calls(ProcRef procRef1, ProcRef procRef2)
        {
            Type procRef1_Type = procRef1.EntryTypeReference.GetType();
            Type procRef2_Type = procRef2.EntryTypeReference.GetType();
            MethodResultList callsReturn = new MethodResultList(procRef1, procRef2);

            if (procRef1_Type == typeof(Ident) || procRef1_Type == typeof(Placeholder))
            {
                if (procRef2_Type == typeof(Ident) || procRef2_Type == typeof(Placeholder))
                {
                    List<string> allProcedures = Pkb.UsedProcedures;

                    foreach (string p in allProcedures)
                    {
                        List<string> calledProcedures = Pkb.CalledByProcedure(p);

                        foreach (string cp in calledProcedures)
                        {
                            callsReturn.List1.Add(new ProcRef(p));
                            callsReturn.List2.Add(new ProcRef(cp));
                        }
                    }
                }
                else if (procRef2_Type == typeof(BracesIdent))
                {
                    string procNameWithoutBraces = procRef2.entry.Substring(1, procRef2.entry.Length - 2);

                    List<string> proceduresThatCall = Pkb.ProceduresThatCall(procNameWithoutBraces);

                    foreach (string ptc in proceduresThatCall)
                    {
                        callsReturn.List1.Add(new ProcRef(ptc));
                        callsReturn.List2.Add(new ProcRef(procNameWithoutBraces));
                    }
                }
                else
                    throw new SuchThatException($"#Invalid procRef type: expected 'Ident', 'Placeholder' or 'BracesIdent' for {procRef2.entry}");

            }
            else if (procRef1_Type == typeof(BracesIdent))
            {
                string procName1_WithoutBraces = procRef1.entry.Substring(1, procRef1.entry.Length - 2);

                if (procRef2_Type == typeof(Ident) || procRef2_Type == typeof(Placeholder))
                {
                    List<string> calledProcedures = Pkb.CalledByProcedure(procName1_WithoutBraces);

                    foreach (string cp in calledProcedures)
                    {
                        callsReturn.List1.Add(new ProcRef(procName1_WithoutBraces));
                        callsReturn.List2.Add(new ProcRef(cp));
                    }
                }
                else if (procRef2_Type == typeof(BracesIdent))
                {
                    string procName2_WithoutBraces = procRef2.entry.Substring(1, procRef2.entry.Length - 2);

                    if (Pkb.Calls(procName1_WithoutBraces, procName2_WithoutBraces))
                    {
                        callsReturn.List1.Add(new ProcRef(procName1_WithoutBraces));
                        callsReturn.List2.Add(new ProcRef(procName2_WithoutBraces));
                    }
                }
                else
                    throw new SuchThatException($"#Invalid procRef type: expected 'Ident', 'Placeholder' or 'BracesIdent' for {procRef2.entry}");

            }
            else
                throw new SuchThatException($"#Invalid procRef type: expected 'Ident', 'Placeholder' or 'BracesIdent' for {procRef1.entry}");

            return callsReturn;
        }

        //public MethodResultList CallsAsterisk(ProcRef procRef1, ProcRef procRef2) { }

        public MethodResultList Parent(StmtRef stmtRef1, StmtRef stmtRef2)
        {
            Type stmtRef1_Type = stmtRef1.EntryTypeReference.GetType();
            Type stmtRef2_Type = stmtRef2.EntryTypeReference.GetType();
            MethodResultList parentReturn = new MethodResultList(stmtRef1, stmtRef2);

            if (stmtRef1_Type == typeof(Ident) || stmtRef1_Type == typeof(Placeholder))
            {
                NodeType statementNode1_Type = NodeType.Statement;
                if (stmtRef1_Type == typeof(Ident))
                    statementNode1_Type = ConvertStatementToPkbType(stmtRef1.Type);

                if (stmtRef2_Type == typeof(Ident) || stmtRef2_Type == typeof(Placeholder))
                {
                    NodeType statement2_NodeType = NodeType.Statement;
                    if (stmtRef2_Type == typeof(Ident))
                        statement2_NodeType = ConvertStatementToPkbType(stmtRef2.Type);

                    List<UsedStatementElement> usedStatements1 = GetAllStatements(statementNode1_Type);
                    List<UsedStatementElement> usedStatements2 = GetAllStatements(statement2_NodeType);

                    foreach (UsedStatementElement use1 in usedStatements1)
                    {
                        foreach (UsedStatementElement use2 in usedStatements2)
                        {
                            if (Pkb.Parent(use1.statementNumber, use2.statementNumber))
                            {
                                parentReturn.List1.Add(new StmtRef("" + use1.statementNumber));
                                parentReturn.List2.Add(new StmtRef("" + use2.statementNumber));
                            }
                        }
                    }
                }
                else if (stmtRef2_Type == typeof(IntegerRule))
                {
                    List<UsedStatementElement> usedStatements1 = GetAllStatements(statementNode1_Type);

                    foreach (UsedStatementElement use1 in usedStatements1)
                    {
                        if (Pkb.Parent(use1.statementNumber, int.Parse(stmtRef2.entry)))
                        {
                            parentReturn.List1.Add(new StmtRef("" + use1.statementNumber));
                            parentReturn.List2.Add(new StmtRef(stmtRef2.entry));
                        }
                    }
                }
                else
                    throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident', 'Placeholder' or 'Integer' for {stmtRef2.entry}");

            }
            else if (stmtRef1_Type == typeof(IntegerRule))
            {
                if (stmtRef2_Type == typeof(Ident) || stmtRef2_Type == typeof(Placeholder))
                {
                    NodeType statement2_NodeType = NodeType.Statement;
                    if (stmtRef2_Type == typeof(Ident))
                        statement2_NodeType = ConvertStatementToPkbType(stmtRef2.Type);

                    List<UsedStatementElement> usedStatements2 = GetAllStatements(statement2_NodeType);

                    foreach (UsedStatementElement use2 in usedStatements2)
                    {
                        if (Pkb.Parent(int.Parse(stmtRef1.entry), use2.statementNumber))
                        {
                            parentReturn.List1.Add(new StmtRef(stmtRef1.entry));
                            parentReturn.List2.Add(new StmtRef("" + use2.statementNumber));
                        }
                    }
                }
                else if (stmtRef2_Type == typeof(IntegerRule))
                {
                    if (Pkb.Parent(int.Parse(stmtRef1.entry), int.Parse(stmtRef2.entry)))
                    {
                        parentReturn.List1.Add(new StmtRef(stmtRef1.entry));
                        parentReturn.List2.Add(new StmtRef(stmtRef2.entry));
                    }
                }
                else
                    throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident', 'Placeholder' or 'Integer' for {stmtRef2.entry}");

            }
            else
                throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident', 'Placeholder' or 'Integer' for {stmtRef1.entry}");

            return parentReturn;
        }

        public MethodResultList ParentAsterisk(StmtRef stmtRef1, StmtRef stmtRef2)
        {
            Type stmtRef1_Type = stmtRef1.EntryTypeReference.GetType();
            Type stmtRef2_Type = stmtRef2.EntryTypeReference.GetType();
            MethodResultList parentReturn = new MethodResultList(stmtRef1, stmtRef2);

            if (stmtRef1_Type == typeof(Ident) || stmtRef1_Type == typeof(Placeholder))
            {
                NodeType statementNode1_Type = NodeType.Statement;
                if (stmtRef1_Type == typeof(Ident))
                    statementNode1_Type = ConvertStatementToPkbType(stmtRef1.Type);

                if (stmtRef2_Type == typeof(Ident) || stmtRef2_Type == typeof(Placeholder))
                {
                    NodeType statement2_NodeType = NodeType.Statement;
                    if (stmtRef2_Type == typeof(Ident))
                        statement2_NodeType = ConvertStatementToPkbType(stmtRef2.Type);

                    List<UsedStatementElement> usedStatements1 = GetAllStatements(statementNode1_Type);
                    List<UsedStatementElement> usedStatements2 = GetAllStatements(statement2_NodeType);

                    foreach (UsedStatementElement use1 in usedStatements1)
                    {
                        foreach (UsedStatementElement use2 in usedStatements2)
                        {
                            if (Pkb.ParentStar(use1.statementNumber, use2.statementNumber))
                            {
                                parentReturn.List1.Add(new StmtRef("" + use1.statementNumber));
                                parentReturn.List2.Add(new StmtRef("" + use2.statementNumber));
                            }
                        }
                    }
                }
                else if (stmtRef2_Type == typeof(IntegerRule))
                {
                    List<UsedStatementElement> usedStatements1 = GetAllStatements(statementNode1_Type);

                    foreach (UsedStatementElement use1 in usedStatements1)
                    {
                        if (Pkb.ParentStar(use1.statementNumber, int.Parse(stmtRef2.entry)))
                        {
                            parentReturn.List1.Add(new StmtRef("" + use1.statementNumber));
                            parentReturn.List2.Add(new StmtRef(stmtRef2.entry));
                        }
                    }
                }
                else
                    throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident', 'Placeholder' or 'Integer' for {stmtRef2.entry}");

            }
            else if (stmtRef1_Type == typeof(IntegerRule))
            {
                if (stmtRef2_Type == typeof(Ident) || stmtRef2_Type == typeof(Placeholder))
                {
                    NodeType statement2_NodeType = NodeType.Statement;
                    if (stmtRef2_Type == typeof(Ident))
                        statement2_NodeType = ConvertStatementToPkbType(stmtRef2.Type);

                    List<UsedStatementElement> usedStatements2 = GetAllStatements(statement2_NodeType);

                    foreach (UsedStatementElement use2 in usedStatements2)
                    {
                        if (Pkb.ParentStar(int.Parse(stmtRef1.entry), use2.statementNumber))
                        {
                            parentReturn.List1.Add(new StmtRef(stmtRef1.entry));
                            parentReturn.List2.Add(new StmtRef("" + use2.statementNumber));
                        }
                    }
                }
                else if (stmtRef2_Type == typeof(IntegerRule))
                {
                    if (Pkb.ParentStar(int.Parse(stmtRef1.entry), int.Parse(stmtRef2.entry)))
                    {
                        parentReturn.List1.Add(new StmtRef(stmtRef1.entry));
                        parentReturn.List2.Add(new StmtRef(stmtRef2.entry));
                    }
                }
                else
                    throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident', 'Placeholder' or 'Integer' for {stmtRef2.entry}");

            }
            else
                throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident', 'Placeholder' or 'Integer' for {stmtRef1.entry}");

            return parentReturn;
        }

        public MethodResultList Follows(StmtRef stmtRef1, StmtRef stmtRef2)
        {
            Type stmtRef1_Type = stmtRef1.EntryTypeReference.GetType();
            Type stmtRef2_Type = stmtRef2.EntryTypeReference.GetType();
            MethodResultList followsReturn = new MethodResultList(stmtRef1, stmtRef2);

            if (stmtRef1_Type == typeof(Ident) || stmtRef1_Type == typeof(Placeholder))
            {
                NodeType statementNode1_Type = NodeType.Statement;
                if (stmtRef1_Type == typeof(Ident))
                    statementNode1_Type = ConvertStatementToPkbType(stmtRef1.Type);

                if (stmtRef2_Type == typeof(Ident) || stmtRef2_Type == typeof(Placeholder))
                {
                    NodeType statement2_NodeType = NodeType.Statement;
                    if (stmtRef2_Type == typeof(Ident))
                        statement2_NodeType = ConvertStatementToPkbType(stmtRef2.Type);

                    List<UsedStatementElement> usedStatements1 = GetAllStatements(statementNode1_Type);
                    List<UsedStatementElement> usedStatements2 = GetAllStatements(statement2_NodeType);

                    foreach (UsedStatementElement use1 in usedStatements1)
                    {
                        foreach (UsedStatementElement use2 in usedStatements2)
                        {
                            if (Pkb.Follows(use1.statementNumber, use2.statementNumber))
                            {
                                followsReturn.List1.Add(new StmtRef("" + use1.statementNumber));
                                followsReturn.List2.Add(new StmtRef("" + use2.statementNumber));
                            }
                        }
                    }
                }
                else if (stmtRef2_Type == typeof(IntegerRule))
                {
                    List<UsedStatementElement> usedStatements1 = GetAllStatements(statementNode1_Type);

                    foreach (UsedStatementElement use1 in usedStatements1)
                    {
                        if (Pkb.Follows(use1.statementNumber, int.Parse(stmtRef2.entry)))
                        {
                            followsReturn.List1.Add(new StmtRef("" + use1.statementNumber));
                            followsReturn.List2.Add(new StmtRef(stmtRef2.entry));
                        }
                    }
                }
                else
                    throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident', 'Placeholder' or 'Integer' for {stmtRef2.entry}");

            }
            else if (stmtRef1_Type == typeof(IntegerRule))
            {
                if (stmtRef2_Type == typeof(Ident) || stmtRef2_Type == typeof(Placeholder))
                {
                    NodeType statement2_NodeType = NodeType.Statement;
                    if (stmtRef2_Type == typeof(Ident))
                        statement2_NodeType = ConvertStatementToPkbType(stmtRef2.Type);

                    List<UsedStatementElement> usedStatements2 = GetAllStatements(statement2_NodeType);

                    foreach (UsedStatementElement use2 in usedStatements2)
                    {
                        if (Pkb.Follows(int.Parse(stmtRef1.entry), use2.statementNumber))
                        {
                            followsReturn.List1.Add(new StmtRef(stmtRef1.entry));
                            followsReturn.List2.Add(new StmtRef("" + use2.statementNumber));
                        }
                    }
                }
                else if (stmtRef2_Type == typeof(IntegerRule))
                {
                    if (Pkb.Follows(int.Parse(stmtRef1.entry), int.Parse(stmtRef2.entry)))
                    {
                        followsReturn.List1.Add(new StmtRef(stmtRef1.entry));
                        followsReturn.List2.Add(new StmtRef(stmtRef2.entry));
                    }
                }
                else
                    throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident', 'Placeholder' or 'Integer' for {stmtRef2.entry}");

            }
            else
                throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident', 'Placeholder' or 'Integer' for {stmtRef1.entry}");

            return followsReturn;
        }

        public MethodResultList FollowsAsterisk(StmtRef stmtRef1, StmtRef stmtRef2)
        {
            Type stmtRef1_Type = stmtRef1.EntryTypeReference.GetType();
            Type stmtRef2_Type = stmtRef2.EntryTypeReference.GetType();
            MethodResultList followsReturn = new MethodResultList(stmtRef1, stmtRef2);

            if (stmtRef1_Type == typeof(Ident) || stmtRef1_Type == typeof(Placeholder))
            {
                NodeType statementNode1_Type = NodeType.Statement;
                if (stmtRef1_Type == typeof(Ident))
                    statementNode1_Type = ConvertStatementToPkbType(stmtRef1.Type);

                if (stmtRef2_Type == typeof(Ident) || stmtRef2_Type == typeof(Placeholder))
                {
                    NodeType statement2_NodeType = NodeType.Statement;
                    if (stmtRef2_Type == typeof(Ident))
                        statement2_NodeType = ConvertStatementToPkbType(stmtRef2.Type);

                    List<UsedStatementElement> usedStatements1 = GetAllStatements(statementNode1_Type);
                    List<UsedStatementElement> usedStatements2 = GetAllStatements(statement2_NodeType);

                    foreach (UsedStatementElement use1 in usedStatements1)
                    {
                        foreach (UsedStatementElement use2 in usedStatements2)
                        {
                            if (Pkb.FollowsStar(use1.statementNumber, use2.statementNumber))
                            {
                                followsReturn.List1.Add(new StmtRef("" + use1.statementNumber));
                                followsReturn.List2.Add(new StmtRef("" + use2.statementNumber));
                            }
                        }
                    }
                }
                else if (stmtRef2_Type == typeof(IntegerRule))
                {
                    List<UsedStatementElement> usedStatements1 = GetAllStatements(statementNode1_Type);

                    foreach (UsedStatementElement use1 in usedStatements1)
                    {
                        if (Pkb.FollowsStar(use1.statementNumber, int.Parse(stmtRef2.entry)))
                        {
                            followsReturn.List1.Add(new StmtRef("" + use1.statementNumber));
                            followsReturn.List2.Add(new StmtRef(stmtRef2.entry));
                        }
                    }
                }
                else
                    throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident', 'Placeholder' or 'Integer' for {stmtRef2.entry}");

            }
            else if (stmtRef1_Type == typeof(IntegerRule))
            {
                if (stmtRef2_Type == typeof(Ident) || stmtRef2_Type == typeof(Placeholder))
                {
                    NodeType statement2_NodeType = NodeType.Statement;
                    if (stmtRef2_Type == typeof(Ident))
                        statement2_NodeType = ConvertStatementToPkbType(stmtRef2.Type);

                    List<UsedStatementElement> usedStatements2 = GetAllStatements(statement2_NodeType);

                    foreach (UsedStatementElement use2 in usedStatements2)
                    {
                        if (Pkb.FollowsStar(int.Parse(stmtRef1.entry), use2.statementNumber))
                        {
                            followsReturn.List1.Add(new StmtRef(stmtRef1.entry));
                            followsReturn.List2.Add(new StmtRef("" + use2.statementNumber));
                        }
                    }
                }
                else if (stmtRef2_Type == typeof(IntegerRule))
                {
                    if (Pkb.FollowsStar(int.Parse(stmtRef1.entry), int.Parse(stmtRef2.entry)))
                    {
                        followsReturn.List1.Add(new StmtRef(stmtRef1.entry));
                        followsReturn.List2.Add(new StmtRef(stmtRef2.entry));
                    }
                }
                else
                    throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident', 'Placeholder' or 'Integer' for {stmtRef2.entry}");

            }
            else
                throw new SuchThatException($"#Invalid StmtRef type: expected 'Ident', 'Placeholder' or 'Integer' for {stmtRef1.entry}");

            return followsReturn;
        }

        public void Next(LineRef lineRef1, LineRef lineRef2)
        {

        }
        //void Next*(LineRef lineRef1, LineRef lineRef2);

        public void Affects(StmtRef stmtRef1, StmtRef stmtRef2) { }
        //void Affects*(StmtRef stmtRef1, StmtRef stmtRef2);

        private NodeType ConvertStatementToPkbType(Statement statement)
        {
            Type statementType = statement.GetType();

            if (statementType == typeof(Statement))
                return NodeType.Statement;
            if (statementType == typeof(If))
                return NodeType.If;
            if (statementType == typeof(While))
                return NodeType.While;
            if (statementType == typeof(Call))
                return NodeType.Call;
            if (statementType == typeof(Assign))
                return NodeType.Assign;

            throw new InvalidCastException($"#Invalid statement type {statementType}. Couldn't be converted to NodeType");
        }

        #region Is type correct
        private bool IsTypeCorrect(VarRef varRef)
        {
            Type varRefType = varRef.EntryTypeReference.GetType();

            if (varRefType == typeof(Ident))
            {
                if (Declarations.GetDeclarationByName(varRef.entry).GetType() != typeof(Variable))
                {
                    return false;
                }
                return true;
            }
            else if (varRefType == typeof(BracesIdent))
            {
                return true;
            }
            else if (varRefType == typeof(Placeholder))
            {
                return true;
            }
            else
                return false;
        }

        private bool IsTypeCorrect(ProcRef procRef)
        {
            Type procRefType = procRef.EntryTypeReference.GetType();

            if (procRefType == typeof(Ident))
            {
                if (Declarations.GetDeclarationByName(procRef.entry).GetType() != typeof(Procedure))
                {
                    return false;
                }
                return true;
            }

            else if (procRefType == typeof(BracesIdent))
                return true;

            else if (procRefType == typeof(Placeholder))
                return true;

            return false;
        }

        private bool IsTypeCorrect(StmtRef stmtRef)
        {
            Type stmtRefType = stmtRef.EntryTypeReference.GetType();

            if (stmtRefType == typeof(Ident))
            {
                Type type = Declarations.GetDeclarationByName(stmtRef.entry).GetType();
                if (type == typeof(Statement))
                {
                    stmtRef.Type = new Statement("");
                }
                else if (type == typeof(If))
                {
                    stmtRef.Type = new If("");
                }
                else if (type == typeof(While))
                {
                    stmtRef.Type = new While("");
                }
                else if (type == typeof(Assign))
                {
                    stmtRef.Type = new Assign("");
                }
                else if (type == typeof(Call))
                {
                    stmtRef.Type = new Call("");
                }
                else
                    return false;

                return true;
            }

            else if (stmtRefType == typeof(IntegerRule))
                return true;

            else if (stmtRefType == typeof(Placeholder))
                return true;

            return false;
        }
        #endregion

        private List<UsedStatementElement> GetAllStatements(NodeType statementType)
        {
            if (statementType == NodeType.Statement)
                return Pkb.UsedStatements.ToList();
            else
                return Pkb.UsedStatements.Where(x => x.stementType == statementType).ToList();
        }
    }


    #region Exceptions

    public class SuchThatException : Exception
    {
        public SuchThatException()
        {
        }

        public SuchThatException(string message)
            : base(message)
        {
        }

        public SuchThatException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    #endregion
}
