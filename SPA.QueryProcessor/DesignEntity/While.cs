using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.DesignEntity {
    public class While : Statement
    {
        public While(string name) : base(name) {}

        public While(string name, int number) : base(name, number) {}
    }
}
