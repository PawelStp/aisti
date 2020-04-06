using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Models.Interfaces
{
    public interface ITreeCreation
    {
        AstNode CreateNode(NodeType nodeType, bool isRoot = false);
        void SetSibling(AstNode sibling);
        void SetChild(AstNode child);
        void SetParent(AstNode parent);
    }
}
