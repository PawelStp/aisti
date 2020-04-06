using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Models.CFG
{
    public class CfgNode
    {
        public CfgNode()
        {
            this.LineNumbers = new List<int>();
            this.NextNodes = new List<CfgNode>();
        }
        public List<int> LineNumbers { get; set; }
        public List<CfgNode> NextNodes { get; set; }
        public bool IsEndScope { get; set; }
    }
}
