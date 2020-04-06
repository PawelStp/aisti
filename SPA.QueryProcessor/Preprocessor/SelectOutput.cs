using SPA.QueryProcessor.AuxiliaryGrammar;
using SPA.QueryProcessor.LexicalRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.Preprocessor
{
    public class SelectOutput
    {
        private List<MethodResultList> queryResults;
        private List<Stack<MergedResultTableRelations.StackNode>> StackList;
        private string SelectParam;

        public SelectOutput(string selectParam, DeclarationsArray declarationsArray)
        {
            SuchThat suchThat = new SuchThat(declarationsArray);
            SelectParam = selectParam;

            queryResults = new List<MethodResultList>();
            //validation completed
            foreach (var methodToDo in SuchThatValidator.GetMethodsToDo())
            {
                queryResults.Add(suchThat.DoSuchThatMethod(methodToDo.MethodName, methodToDo.Param1, methodToDo.Param2));
            }

            //foreach (var qr in queryResults)
            //{
            //    Console.WriteLine($"\n{qr.ListType1}\t{qr.ListType2}");
            //    for (int i = 0; i < qr.List1.Count; i++)
            //    {
            //        Console.WriteLine($"{((AbstractAuxiliaryGrammar) qr.List1[i]).entry}\t{((AbstractAuxiliaryGrammar) qr.List2[i]).entry}");
            //    }
            //}


            ResultTableRelations resultTableRelations = new ResultTableRelations();
            for (int i = 0; i < queryResults.Count; i++)
            {
                MethodResultList qr = queryResults[i];

                if (qr.ListType1 == typeof(Ident))
                {
                    if (qr.ListType2 == typeof(Ident))
                    {
                        resultTableRelations.AddRelation(qr.QueryParam1, qr.QueryParam2, i);
                    }
                    else //qr.ListType2 == typeof(Ident)
                    {
                        resultTableRelations.AddRelation(qr.QueryParam1, i);
                    }
                }
                else //qr.ListType1 != typeof(Ident)
                {
                    if (qr.ListType2 == typeof(Ident))
                    {
                        resultTableRelations.AddRelation(qr.QueryParam2, i);
                    }
                    else //qr.ListType2 != typeof(Ident)
                    {
                        resultTableRelations.AddRelation(qr.QueryParam2, qr.QueryParam1, i);
                    }
                }
            }

            MergedResultTableRelations mergedResultTableRelations = new MergedResultTableRelations(resultTableRelations);
            StackList = mergedResultTableRelations.ExecuteAllMerges(selectParam);
        }

        public string GenerateResult()
        {
            foreach (MethodResultList mrl in queryResults)
            {
                if (mrl.List1.Count == 0)
                {
                    if (SelectParam == "BOOLEAN")
                    {
                        return "FALSE";
                    }
                    else
                    {
                        return "NONE";
                    }
                }
            }

            List<MethodResultList> ultimateResultList = new List<MethodResultList>();
            foreach (var stack in StackList)
            {
                ultimateResultList.Add(ConstrainedAllConnectedMethodResultList(stack));
            }

            if (SelectParam == "BOOLEAN")
            {
                foreach (MethodResultList mrl in queryResults)
                {
                    if (mrl.List1.Count == 0)
                    {
                        return "FALSE";
                    }
                }
                return "TRUE";
            }
            else
            {
                foreach (MethodResultList mrl in queryResults)
                {
                    if (mrl.List1.Count == 0)
                    {
                        return "NONE";
                    }
                }

                List<string> finalOutput = new List<string>();
                if (queryResults[0].QueryParam1 == SelectParam)
                {
                    foreach (AbstractAuxiliaryGrammar aag in queryResults[0].List1)
                    {
                        finalOutput.Add(aag.entry + ", ");
                    }
                }
                else
                {
                    foreach (AbstractAuxiliaryGrammar aag in queryResults[0].List2)
                    {
                        finalOutput.Add(aag.entry + ", ");
                    }
                }

                string weHopeItWorks = "";
                foreach (string s in finalOutput.Distinct().ToList())
                {
                    weHopeItWorks += s;
                }

                return weHopeItWorks.Remove(weHopeItWorks.Length - 2);
            }
        }

        public MethodResultList ConstrainedAllConnectedMethodResultList(Stack<MergedResultTableRelations.StackNode> stack)
        {
            while (stack.Count > 0)
            {
                MergedResultTableRelations.StackNode top = stack.Pop();

                if (top.ParentIndex != -1)
                {
                    ProductOfSets(queryResults[top.ParentIndex], queryResults[top.CurrentIndex]);
                }
                else
                {
                    return queryResults[top.CurrentIndex];
                }
            }

            throw new Exception("#BTH");
        }

        private void ProductOfSets(MethodResultList set, MethodResultList restrictingSet)
        {
            if (set.QueryParam1 == restrictingSet.QueryParam1)
            {
                for (int i = 0; i < set.List1.Count; i++)
                {
                    bool exist = false;
                    foreach (AbstractAuxiliaryGrammar tmp in restrictingSet.List1)
                    {
                        if (tmp.entry == ((AbstractAuxiliaryGrammar)set.List1[i]).entry)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        set.List1.RemoveAt(i);
                        set.List2.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (set.QueryParam1 == restrictingSet.QueryParam2)
            {
                for (int i = 0; i < set.List1.Count; i++)
                {
                    bool exist = false;
                    foreach (AbstractAuxiliaryGrammar tmp in restrictingSet.List2)
                    {
                        if (tmp.entry == ((AbstractAuxiliaryGrammar)set.List1[i]).entry)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        set.List1.RemoveAt(i);
                        set.List2.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (set.QueryParam2 == restrictingSet.QueryParam1)
            {
                for (int i = 0; i < set.List2.Count; i++)
                {
                    bool exist = false;
                    foreach (AbstractAuxiliaryGrammar tmp in restrictingSet.List1)
                    {
                        if (tmp.entry == ((AbstractAuxiliaryGrammar)set.List2[i]).entry)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        set.List1.RemoveAt(i);
                        set.List2.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (set.QueryParam2 == restrictingSet.QueryParam2)
            {
                for (int i = 0; i < set.List2.Count; i++)
                {
                    bool exist = false;
                    foreach (AbstractAuxiliaryGrammar tmp in restrictingSet.List2)
                    {
                        if (tmp.entry == ((AbstractAuxiliaryGrammar)set.List2[i]).entry)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        set.List1.RemoveAt(i);
                        set.List2.RemoveAt(i);
                        i--;
                    }
                }
            }


        }


    }
}
