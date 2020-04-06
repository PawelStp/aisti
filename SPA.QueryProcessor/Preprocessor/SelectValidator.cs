using System;
using System.Collections.Generic;
using SPA.QueryProcessor.AuxiliaryGrammar;
using SPA.QueryProcessor.DesignEntity;
using SPA.QueryProcessor.GrammarRules;

namespace SPA.QueryProcessor.Preprocessor
{
    public class SelectValidator
    {
        public string SelectParam { get; set; }
        private DeclarationsArray Declarations;
        private enum Types { None, SuchThat, SuchThatAnd, Pattern, With };

        private Types CurrentType;

        public SelectValidator(DeclarationsArray Declarations)
        {
            this.Declarations = Declarations;
        }

        public bool ValidateSelectQuery(string queryToValidate)
        {
            string[] queryWords = queryToValidate.Split(' ');

            if (!IsThereSelectAndParam(queryWords[0], queryWords[1]))
                throw new SelectValidatorException("#Missing keywords 'Select' or 'argument'");

            SelectParam = queryWords[1];

            for (int i = 2; i < queryWords.Length - 1;) //note that i++ is missing for a reason
            {
                CurrentType = SuchThatPatternWithAnd(queryWords[i], queryWords[i + 1]);

                if (CurrentType == Types.None)
                    throw new SelectValidatorException($"#Wrong keywords. Expected 'SUCH THAT', 'AND', 'WITH', 'PATTERN'.\nGot: {queryWords[i]} and {queryWords[i + 1]}");

                if (CurrentType == Types.SuchThat)
                {
                    i += 2;
                    if (!IsGrammarCorrect(queryWords[i], ref queryWords[i+1], ref queryWords[i+2]))
                        throw new SelectValidatorException($"#Invalid SUCH THAT syntax. Got {queryWords[0]}, {queryWords[1]}, {queryWords[2]}");
                    else
                        SuchThatValidator.ValidateMethodAndParams(queryWords[i], queryWords[i + 1], queryWords[i + 2]);
                    i += 3;
                }
                else if (CurrentType == Types.SuchThatAnd)
                {
                    i++;
                    if (!IsGrammarCorrect(queryWords[i], ref queryWords[i+1], ref queryWords[i+2]))
                        throw new SelectValidatorException($"#Invalid SUCH THAT .. AND syntax. Got {queryWords[0]}, {queryWords[1]}, {queryWords[2]}");
                    else
                        SuchThatValidator.ValidateMethodAndParams(queryWords[i], queryWords[i + 1], queryWords[i + 2]);
                    i += 3;
                }
                else if (CurrentType == Types.Pattern)
                {
                    i += 1;

                    string name = queryWords[i].Contains("(") ? queryWords[i].Split('(')[0] : queryWords[i];

                    if (Declarations.GetDeclarationByName(name) == null)
                    {
                        throw new InvalidQueryException("#Invalid 'PATTERN' query - have you declared first argument earlier?");
                    }

                    if (!queryWords[i].Contains("("))
                    {
                        i += 1;
                    }

                    if (queryWords[i][queryWords[i].Length - 1] == ',')
                    {
                        // General grammar

                        if (!queryWords[i].Contains("(")
                            || queryWords[i].Split('(').Length != 2
                            || queryWords[i + 1].Length < 2
                            || !queryWords[i + 1].Contains(")")
                            || (!(queryWords[i + 1][queryWords[i + 1].Length - 1] == ')')
                                && !(queryWords[i + 1].Length < 3
                                    && queryWords[i + 1][queryWords[i + 1].Length - 2] == ')'
                                    && queryWords[i + 1][queryWords[i + 1].Length - 1] == ';'
                                    )
                                )
                            )
                        {
                            throw new InvalidQueryException("#Invalid 'PATTERN' query - lexical mistake, maybe missing brackets or something?");
                        }

                        // Verifying whether the first pattern argument has been declared (if not in quotes)

                        if (!(queryWords[i].Split('(')[1][0] == '\''
                                && queryWords[i].Split('(')[1][queryWords[i].Split('(')[1].Split(',')[0].Length - 1] == '\'')
                            && !(queryWords[i].Split('(')[1][0] == '"'
                                && queryWords[i].Split('(')[1][queryWords[i].Split('(')[1].Split(',')[0].Length - 1] == '"')
                            && !(int.TryParse(queryWords[i].Split('(')[1].Split(',')[0], out int n))
                            && queryWords[i].Split('(')[1].Split(',')[0] != "_")
                        {
                            if (Declarations.GetDeclarationByName(queryWords[i].Split('(')[1].Split(',')[0]) == null)
                            {
                                throw new InvalidQueryException("#Invalid 'PATTERN' query - have you declared first argument earlier?");
                            }
                        }

                        i += 1;

                        // Verifying whether the second pattern argument has been declared (if not in quotes)

                        if (!(queryWords[i][0] == '\''
                                && queryWords[i][queryWords[i].Split(')')[0].Length - 1] == '\'')
                            && !(queryWords[i][0] == '"'
                                && queryWords[i][queryWords[i].Split(')')[0].Length - 1] == '"')
                            && !(int.TryParse(queryWords[i].Split(')')[0], out int m))
                            && queryWords[i].Split(')')[0] != "_")
                        {
                            if (Declarations.GetDeclarationByName(queryWords[i].Split('(')[1].Split(',')[0]) == null)
                            {
                                throw new InvalidQueryException("#Invalid 'PATTERN' query - have you declared the second argument earlier?");
                            }
                        }

                        i += 1;
                    }

                    else if (queryWords[i][queryWords[i].Length - 1] == ')' || queryWords[i][queryWords[i].Length - 1] == ';')
                    {
                        // General grammar

                        if (!queryWords[i].Contains("(")
                            || !queryWords[i].Contains(")")
                            || (!(queryWords[i][queryWords[i].Length] == ')')
                                && !(queryWords[i + 1].Length < 3
                                    && queryWords[i][queryWords[i].Length - 2] == ')'
                                    && queryWords[i][queryWords[i].Length - 1] == ';'
                                    )
                                )
                            )
                        {
                            throw new InvalidQueryException("#Invalid 'PATTERN' query (missing brackets or something)");
                        }

                        // Verifying whether the first pattern argument has been declared (if not in quotes)

                        if (!(queryWords[i][0] == '\''
                            && queryWords[i].Split(',')[0][queryWords[i].Split(',')[0].Split(')')[0].Length - 1] == '\'')
                            && !(queryWords[i][0] == '"'
                            && queryWords[i].Split(',')[0][queryWords[i].Split(',')[0].Split(')')[0].Length - 1] == '"')
                            && !(int.TryParse(queryWords[i].Split(')')[0].Split(',')[0], out int n))
                            && queryWords[i].Split(')')[0].Split(',')[0] != "_")
                        {
                            if (Declarations.GetDeclarationByName(queryWords[i].Split('(')[1].Split(',')[0]) == null)
                            {
                                throw new InvalidQueryException("#Invalid 'PATTERN' query - have you declared the first argument earlier?");
                            }
                        }

                        // Verifying whether the second pattern argument has been declared (if not in quotes)

                        if (!(queryWords[i].Split(',')[1][0] == '\''
                            && queryWords[i].Split(',')[1][queryWords[i].Split(',')[1].Split(')')[0].Length - 1] == '\'')
                            && !(queryWords[i].Split(',')[1][0] == '"'
                            && queryWords[i].Split(',')[1][queryWords[i].Split(',')[1].Split(')')[0].Length - 1] == '"')
                            && !(int.TryParse(queryWords[i].Split(')')[0].Split(',')[1], out int m))
                            && queryWords[i].Split(')')[0].Split(',')[1] != "_")
                        {
                            if (Declarations.GetDeclarationByName(queryWords[i].Split('(')[1].Split(',')[0]) == null)
                            {
                                throw new InvalidQueryException("#Invalid 'PATTERN' query - have you declared the second argument earlier?");
                            }
                        }


                        i += 1;
                    }

                    else
                    {
                        throw new InvalidQueryException("#Invalid 'PATTERN' query (missing brackets or something)");
                    }

                }
                else if (CurrentType == Types.With)
                {
                    With with = new With(queryWords, i);
                    string withEntry = with.getWithEntry();
                    i += with.Offset;

                    SelectWithValidator selectWithValidator = new SelectWithValidator(withEntry, Declarations);
                    if (!selectWithValidator.isGrammarCorrect())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsThereSelectAndParam(string keyWord, string param)
        {
            if (keyWord == "SELECT")
            {
                if (param == "BOOLEAN")
                    return true;
                else
                {
                    AbstractDesignEntity declaration = Declarations.GetDeclarationByName(param);

                    if (declaration != null)
                        return true;

                    throw new SelectValidatorException("#Parameter after Select not declared");
                }
            }
            else
                throw new SelectValidatorException("#Missing SELECT keyword");
        }

        private Types SuchThatPatternWithAnd(string keyWord1, string keyWord2)
        {
            if (keyWord1 == "SUCH" && keyWord2 == "THAT")
                return Types.SuchThat;

            if (keyWord1 == "WITH")
                return Types.With;

            if (keyWord1 == "PATTERN")
                return Types.Pattern;

            if (keyWord1 == "AND")
            {
                if (CurrentType == Types.SuchThat)
                    return Types.SuchThatAnd;
                return CurrentType;
            }

            return Types.None;
        }

        private bool HasBeenDeclaredAlready(string declarationName)
        {
            if (Declarations.GetDeclarationByName(declarationName) == null)
                return false;
            return true;
        }

        private bool ShouldBeDeclared(string declarationName)
        // declarationName should be declared in the List of Declarations
        {
            LexicalRules.Ident ident = new LexicalRules.Ident();

            return ident.validate(declarationName) ? HasBeenDeclaredAlready(declarationName) : true;
        }

        private bool IsGrammarCorrect(string methodName, ref string param1, ref string param2)
        // ... Parent (n, s) -> methodName, param1, param2 --- cez2 Assignment1
        {
            if (SuchThatValidator.ContainsMethodName(methodName) &&
                        SuchThatValidator.ExtractParams(ref param1, ref param2) &&
                        ShouldBeDeclared(param1) && ShouldBeDeclared(param2))
            {
                return true;
            }
            else
                throw new SelectValidatorException($"#Invalid method name or params for such that - found: {methodName}, {param1}, {param2}");
        }
    }

    public class SelectValidatorException : Exception
    {
        public SelectValidatorException()
        {
        }

        public SelectValidatorException(string message)
            : base(message)
        {
        }

        public SelectValidatorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}