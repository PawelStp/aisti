using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.QueryProcessor.Preprocessor;

namespace SPA.QueryProcessor.GrammarRules
{
    public class With
    {
        private string[] queryWords;
        private int currentPosition;
        public int Offset { get; set; }

        public With(string[] queryWords, int currentPosition)
        {
            this.queryWords = queryWords;
            this.currentPosition = currentPosition;
        }

        internal string getWithEntry()
        {
            currentPosition += 1;
            Offset = 2;
            string withEntry = "";

            for (int j = currentPosition; j < queryWords.Length && j < currentPosition + 3; j++)
            {
                if (queryWords[j].Length != 1 && (queryWords[j].EndsWith("=") || queryWords[j].StartsWith("=")))
                {
                    Offset = 3;
                }
                else if (queryWords[j].Equals("="))
                {
                    Offset = 4;
                }

                withEntry += queryWords[j] + " ";
            }

            string[] selectWith = withEntry.Split('=');

            if (selectWith.Length < 2)
            {
                throw new InvalidQueryException("Wrong arguments in Select with");
            }

            withEntry = selectWith[0].Trim() + "=" + selectWith[1].Trim().Split(' ').FirstOrDefault();
            return withEntry;
        }
    }
}
