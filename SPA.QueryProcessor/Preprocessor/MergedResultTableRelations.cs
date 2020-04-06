using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.Preprocessor
{
    public class MergedResultTableRelations
    {
        private Queue<DictionaryNode> OrderQueue { get; set; }
        private HashSet<int> VisitedNode { get; set; }
        private HashSet<string> VisitedName { get; set; }
        private ResultTableRelations ResultTableRelations { get; set; }

        //inner class
        public class StackNode
        {
            public int CurrentIndex { get; set; }
            public int ParentIndex { get; set; }

            public StackNode(int currentIndex, int parentIndex)
            {
                CurrentIndex = currentIndex;
                ParentIndex = parentIndex;
            }
        }

        public MergedResultTableRelations(ResultTableRelations ResultTableRelations)
        {
            OrderQueue = new Queue<DictionaryNode>();
            VisitedNode = new HashSet<int>();
            VisitedName = new HashSet<string>();
            this.ResultTableRelations = ResultTableRelations;
        }

        public Stack<StackNode> Merge(string identName)
        {
            Stack<StackNode>  MergeOrder = new Stack<StackNode>();

            List<DictionaryNode> list = ResultTableRelations.dict[identName];

            int dictionaryNodeReference = list[0].Value;

            MergeOrder.Push(new StackNode(dictionaryNodeReference, -1));
            VisitedNode.Add(dictionaryNodeReference);
            VisitedName.Add(identName);

            if (list[0].KeyInRelatedNode != "$")
            {
                OrderQueue.Enqueue(list[0]);
                VisitedName.Add(list[0].KeyInRelatedNode);
            }

            for (int i = 1; i < list.Count; i++)
            {
                MergeOrder.Push(new StackNode(list[i].Value, dictionaryNodeReference));
                VisitedNode.Add(list[i].Value);
                VisitedName.Add(list[i].KeyInRelatedNode);

                if (list[i].KeyInRelatedNode != "$")
                {
                    OrderQueue.Enqueue(list[i]);
                    VisitedName.Add(list[i].KeyInRelatedNode);
                }
            }

            while (OrderQueue.Count > 0)
            {
                DictionaryNode top = OrderQueue.Dequeue();
                VisitedNode.Add(top.Value);

                list = ResultTableRelations.dict[top.KeyInRelatedNode];

                for (int i = 0; i < list.Count; i++)
                {
                    if (!VisitedNode.Contains(list[i].Value))
                    {
                        MergeOrder.Push(new StackNode(list[i].Value, top.Value));
                        VisitedNode.Add(list[i].Value);
                        VisitedName.Add(list[i].KeyInRelatedNode);

                        if (list[i].KeyInRelatedNode != "$")
                        {
                            OrderQueue.Enqueue(list[i]);
                        }
                    }
                }
            }

            return MergeOrder;
        }

        public List<Stack<StackNode>> ExecuteAllMerges(string selectParameter)
        {
            List<Stack<StackNode>> AllConstrainedPossibilites = new List<Stack<StackNode>>();

            if (selectParameter != "BOOLEAN")
                AllConstrainedPossibilites.Add(Merge(selectParameter));

            while (VisitedName.Count != ResultTableRelations.dict.Count)
            {
                string key = ResultTableRelations.dict.Keys
                    .Where(k => !VisitedName.Contains(k))
                    .Select(k => k)
                    .First();

                AllConstrainedPossibilites.Add(Merge(key));
            }

            return AllConstrainedPossibilites;
        }
    }
}
