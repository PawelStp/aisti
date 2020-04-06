using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SPA.QueryProcessor.AuxiliaryGrammar;
using SPA.QueryProcessor.DesignEntity;
using SPA.QueryProcessor.LexicalRules;

namespace SPA.QueryProcessor.Preprocessor
{
    public class QueryValidator
    {
        private DeclarationValidator DeclarationValidator;
        // TODO: SelectValidator class
        private SelectValidator SelectValidator;

        public QueryValidator()
        {
            DeclarationValidator = new DeclarationValidator();
        }

        public DeclarationsArray GetDeclarationsArray()
        {
            return DeclarationValidator.GetDeclarationsArray();
        }

        public bool ValidateQuery(string query)
        {
            query = query.ToUpper();

            // It removes all multiple spaces in whole query
            // e.g. procedure  p   ;  select     p
            // will be replaced to: procedure p ; select p
            query = Regex.Replace(query, @"\s+", " ");

            string[] statements = query.Split(';');

            foreach (string statement in statements)
            {
                string firstWord = statement.Trim().Split(' ')[0];
                if (firstWord == "SELECT")
                {
                    SelectValidator = new SelectValidator(DeclarationValidator.GetDeclarationsArray());

                    SuchThatValidator.SetList(new List<SuchThatValidator.MethodToDo>());

                    // TODO: ValidateSelectQuery method
                    if (!SelectValidator.ValidateSelectQuery(statement.Trim()))
                    {
                        throw new InvalidQueryException("#Invalid Select Query");
                    }

                    //tu dodać
                    SelectOutput selectOutput = new SelectOutput(SelectValidator.SelectParam, DeclarationValidator.GetDeclarationsArray());

                    Console.WriteLine(selectOutput.GenerateResult());

                    //Console.ReadKey();
                }
                else
                {
                    if (!DeclarationValidator.ValidateDeclarationQuery(statement.Trim()))
                    {
                        throw new InvalidQueryException("#Invalid Declaration Query");
                    }
                }
            }

            return true;
        }
    }
}
