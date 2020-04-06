using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.DesignEntity {
    public abstract class AbstractDesignEntity {
        public string name { get; protected set; }

        public AbstractDesignEntity(string name) {
            this.name = name;
        }
    }
}
