using System.Collections.Generic;

namespace SPA.Pkb
{
    public class VarTable
    {
        public VarTable()
        {
            NextTable = new Dictionary<int, List<NextNode>>();
        }
        public static Dictionary<int, List<NextNode>> NextTable { get; set; }

        public static void Print()
        {
            //foreach (var item in NextTable)
            //{
            //    System.Console.WriteLine("Proc:");
            //    foreach (var lines in item.Value)
            //    {
            //        System.Console.WriteLine(lines.FirstLine + " -> "+ lines.SecondLine);
            //    }
            //}
        }
    }
}
