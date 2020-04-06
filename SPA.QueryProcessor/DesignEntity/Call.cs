using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.DesignEntity {
    public class Call : Statement
    {
        public Call(string name) : base(name) {}

        public Call(string name, int number) : base(name, number) {}
    }
}
