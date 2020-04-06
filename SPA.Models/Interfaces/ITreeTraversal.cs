using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.Models.Interfaces
{
    public interface ITreeTraversal
    {
        AstNode GetRoot();
        AstNode GetFirstChild(AstNode node);
        AstNode GetRightSibling(AstNode node);
        IList<AstNode> GetAllChildren(AstNode node);

    }
}
