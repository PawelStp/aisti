using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.AuxiliaryGrammar
{
    /// <summary>
    /// Important: Before Validation clear MethodsToDoList
    /// </summary>
    public static class SuchThatValidator
    {
        private static readonly List<string> MethodNames = new List<string> {
            "MODIFIES", "USES", "CALLS", "CALLS*", "PARENT", "PARENT*", "FOLLOWS", "FOLLOWS*", "NEXT", "NEXT*", "AFFECTS", "AFFECTS*"
        };

        #region inner class

        private static IList<MethodToDo> MethodsToDoList = new List<MethodToDo>();

        public static IList<MethodToDo> GetMethodsToDo()
        {
            return MethodsToDoList;
        }

        public static void AddMethod(string methodName, string param1, string param2)
        {
            MethodsToDoList.Add(new MethodToDo(methodName, param1, param2));
        }

        public static void SetList(IList<MethodToDo> methodsToDo)
        {
            MethodsToDoList = methodsToDo;
        }

        public class MethodToDo
        {
            public string MethodName;
            public string Param1;
            public string Param2;

            public MethodToDo(string methodName, string param1, string param2)
            {
                MethodName = methodName;
                Param1 = param1;
                Param2 = param2;
            }
        }

        #endregion

        public static bool ContainsMethodName(string methodName)
        {
            return MethodNames.Contains(methodName);
        }

        public static bool ValidateMethodAndParams(string methodName, string param1, string param2)
        {
            AddMethod(methodName, param1, param2);

            switch (methodName)
            {
                case "MODIFIES":
                    return ValidateModifies(param1, param2);

                case "USES":
                    return ValidateUses(param1, param2);

                case "CALLS":
                    return ValidateCalls(param1, param2);

                case "CALLS*":
                    return ValidateCallsAsterisk(param1, param2);

                case "PARENT":
                    return ValidateParent(param1, param2);

                case "PARENT*":
                    return ValidateParentAsterisk(param1, param2);

                case "FOLLOWS":
                    return ValidateFollows(param1, param2);

                case "FOLLOWS*":
                    return ValidateFollowsAsterisk(param1, param2);

                case "NEXT":
                    return ValidateNext(param1, param2);

                case "NEXT*":
                    return ValidateNextAsterisk(param1, param2);

                case "AFFECTS":
                    return ValidateAffects(param1, param2);

                case "AFFECTS*":
                    return ValidateAffectsAsterisk(param1, param2);

                default:
                    return false;
            }
        }

        public static bool ExtractParams(ref string param1, ref string param2)
        {
            if (CheckRegex(param1, param2))
            {
                param1 = param1.Substring(1, param1.Length - 2); //cut ( and ,
                param2 = param2.Substring(0, param2.Length - 1); //cut )
                return true;
            }
            //throw new GrammarException($"Invalid parameters: {param1} or/and {param2}\n" +
            //    $"Or method ExtractParams was used for the second time");
            return false;
        }

        private static bool CheckRegex(string param1, string param2)
        {
            if (param1.ElementAt(0) == '(' &&
                param1.ElementAt(param1.Length - 1) == ',' &&
                param2.ElementAt(param2.Length - 1) == ')')
                return true;

            return false;
        }

        private static bool ValidateModifies(string param1, string param2)
        {
            VarRef varRef = new VarRef(param2);

            if (varRef.IsGrammarCorrect())
            {
                ProcRef procRef = new ProcRef(param1);
                StmtRef stmtRef = new StmtRef(param1);

                return procRef.IsGrammarCorrect() || stmtRef.IsGrammarCorrect();
            }
            return false;
        }

        private static bool ValidateUses(string param1, string param2)
        {
            VarRef varRef = new VarRef(param2);

            if (varRef.IsGrammarCorrect())
            {
                ProcRef procRef = new ProcRef(param1);
                StmtRef stmtRef = new StmtRef(param1);

                return procRef.IsGrammarCorrect() || stmtRef.IsGrammarCorrect();
            }
            return false;
        }
        private static bool ValidateCalls(string param1, string param2)
        {
            ProcRef procRef1 = new ProcRef(param1);
            ProcRef procRef2 = new ProcRef(param2);

            return procRef1.IsGrammarCorrect() && procRef2.IsGrammarCorrect();
        }
        private static bool ValidateCallsAsterisk(string param1, string param2)
        {
            ProcRef procRef1 = new ProcRef(param1);
            ProcRef procRef2 = new ProcRef(param2);

            return procRef1.IsGrammarCorrect() && procRef2.IsGrammarCorrect();
        }
        private static bool ValidateParent(string param1, string param2)
        {
            StmtRef stmtRef1 = new StmtRef(param1);
            StmtRef stmtRef2 = new StmtRef(param2);

            return stmtRef1.IsGrammarCorrect() && stmtRef2.IsGrammarCorrect();
        }
        private static bool ValidateParentAsterisk(string param1, string param2)
        {
            StmtRef stmtRef1 = new StmtRef(param1);
            StmtRef stmtRef2 = new StmtRef(param2);

            return stmtRef1.IsGrammarCorrect() && stmtRef2.IsGrammarCorrect();
        }
        private static bool ValidateFollows(string param1, string param2)
        {
            StmtRef stmtRef1 = new StmtRef(param1);
            StmtRef stmtRef2 = new StmtRef(param2);

            return stmtRef1.IsGrammarCorrect() && stmtRef2.IsGrammarCorrect();
        }
        private static bool ValidateFollowsAsterisk(string param1, string param2)
        {
            StmtRef stmtRef1 = new StmtRef(param1);
            StmtRef stmtRef2 = new StmtRef(param2);

            return stmtRef1.IsGrammarCorrect() && stmtRef2.IsGrammarCorrect();
        }
        private static bool ValidateNext(string param1, string param2)
        {
            LineRef lineRef1 = new LineRef(param1);
            LineRef lineRef2 = new LineRef(param2);

            return lineRef1.IsGrammarCorrect() && lineRef2.IsGrammarCorrect();
        }
        private static bool ValidateNextAsterisk(string param1, string param2)
        {
            LineRef lineRef1 = new LineRef(param1);
            LineRef lineRef2 = new LineRef(param2);

            return lineRef1.IsGrammarCorrect() && lineRef2.IsGrammarCorrect();
        }
        private static bool ValidateAffects(string param1, string param2)
        {
            StmtRef stmtRef1 = new StmtRef(param1);
            StmtRef stmtRef2 = new StmtRef(param2);

            return stmtRef1.IsGrammarCorrect() && stmtRef2.IsGrammarCorrect();
        }
        private static bool ValidateAffectsAsterisk(string param1, string param2)
        {
            StmtRef stmtRef1 = new StmtRef(param1);
            StmtRef stmtRef2 = new StmtRef(param2);

            return stmtRef1.IsGrammarCorrect() && stmtRef2.IsGrammarCorrect();
        }
    }
}
