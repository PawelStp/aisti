using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.DesignEntity {
    public class StatementList : AbstractDesignEntity
    {
		public List<Statement> statements;

        public StatementList(string name) : base(name) {}

        public StatementList(string name, Statement statement) : base(name) {
			      this.statements = new List<Statement>();
            this.statements.Add(statement);
        }

        public StatementList(string name, List<Statement> statements) : base(name) {
			      this.statements = statements;
        }
    }
}
