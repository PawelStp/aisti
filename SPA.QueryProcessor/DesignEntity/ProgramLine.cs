using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.DesignEntity {
    public class ProgramLine : AbstractDesignEntity
    {
        public int number;

        public ProgramLine(string name) : base(name) {}

        public ProgramLine(string name, int number) : base(name)
        {
            this.number = number;
        }
    }
}
