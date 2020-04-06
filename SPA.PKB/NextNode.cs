using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Pkb
{
    public class NextNode
    {
        public NextNode(int lastLineNumber, int currentLine)
        {
            this.FirstLine = lastLineNumber;
            this.SecondLine = currentLine;
        }

        public int FirstLine { get; set; }
        public int SecondLine { get; set; }
    }
}
