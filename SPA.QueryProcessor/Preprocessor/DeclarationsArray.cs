using System;
using System.Collections.Generic;
using System.Linq;

using SPA.QueryProcessor.DesignEntity;

namespace SPA.QueryProcessor.Preprocessor
{

    public class DeclarationsArray
    {
        public List<AbstractDesignEntity> allDeclarations { get; private set; }

        public DeclarationsArray()
        {
            allDeclarations = new List<AbstractDesignEntity>();
        }

        public void AddDeclaration(string declarationType, string name)
        {
            if (HasBeenDeclaredAlready(name))
            {
                throw new InvalidQueryException("#Duplicated declaration");
            }

            switch (declarationType)
            {
                case "PROCEDURE":
                    allDeclarations.Add(new Procedure(name));
                    break;

                case "STMTLIST":
                    allDeclarations.Add(new StatementList(name));
                    break;

                case "STMT":
                    allDeclarations.Add(new Statement(name));
                    break;

                case "ASSIGN":
                    allDeclarations.Add(new Assign(name));
                    break;

                case "CALL":
                    allDeclarations.Add(new Call(name));
                    break;

                case "WHILE":
                    allDeclarations.Add(new While(name));
                    break;

                case "IF":
                    allDeclarations.Add(new If(name));
                    break;

                case "VARIABLE":
                    allDeclarations.Add(new Variable(name));
                    break;

                case "CONSTANT":
                    allDeclarations.Add(new Constant(name));
                    break;

                case "PROG_LINE":
                    allDeclarations.Add(new ProgramLine(name));
                    break;

                default:
                    throw new InvalidQueryException("#Invalid Declaration Type");
            }
        }

        public AbstractDesignEntity GetDeclarationByName(string declarationName)
        {
            foreach (AbstractDesignEntity declaration in allDeclarations)
            {
                if (declarationName == declaration.name)
                {
                    return declaration;
                }
            }
            return null;
        }

        private bool HasBeenDeclaredAlready(string declarationName)
        {
            foreach (AbstractDesignEntity declaration in allDeclarations)
            {
                if (declarationName == declaration.name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
