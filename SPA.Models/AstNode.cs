using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Models
{
    public class AstNode
    {
        public AstNode()
        {
            this.Children = new List<AstNode>();
        }
        public NodeType NodeType { get; set; }
        public string Value { get; set; }
        public IList<AstNode> Children { get; set; }
        public bool IsRoot { get; set; }
        public AstNode RightSiblingNode { get; set; }
        public AstNode ParentNode { get; set; }
        public int LineNumber { get; set; }
        public int StatementNumber { get; set; }
        /// <summary>
        /// Define if scope of current node is already ended
        /// Important !!! 
        /// After adding all children of the node, set this prop to true
        /// </summary>
        public bool IsAstEndScope { get; set; }
        public bool IsCfgEndScope { get; set; }
    }
}
