using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Models.CFG
{
    public class CfgNodeWhile : CfgNodeBase
    {
        public CfgNodeBase NextNode { get; set; }
        public CfgNodeBase Children { get; set; }
    }
}
