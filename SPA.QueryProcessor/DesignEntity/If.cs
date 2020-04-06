using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.DesignEntity {
    public class If : Statement
    {
        public If(string name) : base(name) {}

        public If(string name, int number) : base(name, number) {}
    }
}
