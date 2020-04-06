using SPA.Pkb;
using SPA.Pkb.Exceptions;
using SPA.Pkb.Helpers;
using SPA.QueryProcessor;
using System;

namespace SPA.Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No args passed");
                SourceRepository.StoreSourceCode("");
            }
            else
            {
                SourceRepository.StoreSourceCode(args[0]);
            }
            
            Initialization.PkbReference = new Pkb.Pkb();

            try
            {
                Parser parser = new Parser();
                parser.StartParsing();
                DesignExtractor designExtractor = new DesignExtractor();
                designExtractor.CreateProcTable();

                //QueryParser.ProcessQuery("PROCEDURE p; VARIABLE v, v2; STMT s1, s2, s3, s4; WHILE w;" +
                //    "SELECT p SUCH THAT MODIFIES (p, v) and MODIFIES (s1, v) and USES (s2, v) and USES (s1, v) and PARENT (w, s1)");
                //QueryParser.ProcessQuery("stmt s; SELECT BOOLEAN such that follows (54, 51)");
                //QueryParser.ProcessQuery("stmt s; Select BOOLEAN such that Follows(5, 6)");
                //QueryParser.ProcessQuery("PROCEDURE p; VARIABLE v, v2; STMT s1, s2, s3, s4; WHILE w;" +
                //    "SELECT p SUCH THAT MODIFIES (p, v) and MODIFIES (s1, v) and USES (s2, v) and USES (s1, v) and PARENT (w, s1)");


                while (true)
                {
                    string s1 = Console.ReadLine();
                    string s2 = Console.ReadLine();
                    QueryParser.ProcessQuery("PROCEDURE p; SELECT p such that Uses (p, \"dos\")");
                }
            }
            catch (ParsingException e)
            {
                Console.WriteLine(e);
                //Console.ReadLine();
                Environment.Exit(1);
            }
            //string command = Console.ReadLine();
            //while(!(command.Equals("exit")))
            //{
            //    if(command.Equals("print-ast"))
            //        Pkb.Pkb.PrintAst();
            //    command = Console.ReadLine();
            //}
        }
    }
}
