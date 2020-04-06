using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.DesignEntity
{
    public class Assign : Statement
    {
        public Assign(string name) : base(name) {}

        public Assign(string name, int number) : base(name, number) {}
    }
}
