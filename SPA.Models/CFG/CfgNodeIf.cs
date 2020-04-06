using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Models.CFG
{
    public class CfgNodeIf : CfgNodeBase
    {
        public CfgNodeBase NextNodeForTrue { get; set; }
        public CfgNodeBase NextNodeForFalse { get; set; }
    }
}
