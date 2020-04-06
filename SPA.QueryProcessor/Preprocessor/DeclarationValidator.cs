using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.Preprocessor
{
    public class DeclarationValidator
    {
        private static readonly string[] keywords = { "PROCEDURE", "STMTLIST", "STMT", "ASSIGN", "CALL", "WHILE", "IF", "VARIABLE", "CONSTANT", "PROG_LINE" };

        private DeclarationsArray declarationsArray;
        private string currentDeclarationType;

        public DeclarationValidator() {
          this.declarationsArray = new DeclarationsArray();
        }

        public DeclarationsArray GetDeclarationsArray()
        {
          return this.declarationsArray;
        }

        //nie przechodzi testów
        //Asignment1 -> stmt s, s1; assign a, a1, a2; while w; if ifstat; procedure p; variable v; constant c; prog_line n, n1, n2;
        public bool ValidateDeclarationQuery(string declarationQuery)
        {
            declarationQuery = declarationQuery.ToUpper();
            string[] declarations = declarationQuery.Split(',');

            for (int i = 0; i < declarations.Length; i++)
            {
                string declaration = declarations[i].Trim();

                if (IsDeclarationEmpty(declaration))
                {
                    return false;
                }

                Regex regex = new Regex("[ ]{2,}", RegexOptions.None);
                declaration = regex.Replace(declaration, " ");

                string[] declarationParts = declaration.Split(' ');

                if (declarationParts.Length == 1)
                {
                    if (!ValidateName(declarationParts[0]))
                    {
                        return false;
                    }
                    this.declarationsArray.AddDeclaration(this.currentDeclarationType, declarationParts[0]);
                }
                else if (declarationParts.Length == 2)
                {
                    if (!ValidateKeyword(declarationParts[0]) || !ValidateName(declarationParts[1]))
                    {
                        return false;
                    }
                    this.currentDeclarationType = declarationParts[0];
                    this.declarationsArray.AddDeclaration(this.currentDeclarationType, declarationParts[1]);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsDeclarationEmpty(string declaration)
        {
            return string.IsNullOrEmpty(declaration);
        }

        private bool ValidateKeyword(string word)
        {
            foreach (string keyword in keywords)
            {
                if (word == keyword)
                {
                    return true;
                }
            }

            return false;
        }

        private bool ValidateName(string word)
        {
            foreach (string keyword in keywords)
            {
                if (word == keyword)
                {
                    return false;
                }
            }

            Match nameMatches = Regex.Match(word, "^[A-Z][A-Z_$0-9#]*$");

            if (!nameMatches.Success)
            {
                return false;
            }

            return true;
        }
    }
}
