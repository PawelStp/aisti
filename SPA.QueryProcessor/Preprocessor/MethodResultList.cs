using SPA.QueryProcessor.DesignEntity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using SPA.QueryProcessor.AuxiliaryGrammar;
using SPA.QueryProcessor.LexicalRules;

namespace SPA.QueryProcessor.Preprocessor
{
    /// <summary>
    /// Class that contains 2 lists of dynamically assigned objects types
    /// </summary>
    public class MethodResultList
    {
        // feel free to change names for better
        public IList List1 { get; set; }
        public IList List2 { get; set; }

        //ListType and QueryParam required for combining sets

        public Type ListType1 { get; set; }
        public Type ListType2 { get; set; }

        public string QueryParam1 { get; set; }
        public string QueryParam2 { get; set; }

        /// <summary>
        /// Creates lists with dynamic types
        /// </summary>
        /// <param name="param1">List1 type</param>
        /// <param name="param2">List2 type</param>
        public MethodResultList(AbstractAuxiliaryGrammar param1, AbstractAuxiliaryGrammar param2)
        {
            Type t1 = param1.GetType();
            List1 = (IList)Activator.CreateInstance((typeof(List<>).MakeGenericType(t1)));
            Type t2 = param2.GetType();
            List2 = (IList)Activator.CreateInstance((typeof(List<>).MakeGenericType(t2)));

            ListType1 = param1.EntryTypeReference.GetType();
            ListType2 = param2.EntryTypeReference.GetType();

            QueryParam1 = param1.entry;
            QueryParam2 = param2.entry;
        }
        /// <summary>
        /// Method for not implemented queries
        /// </summary>
        public MethodResultList(Type t1, Type t2)
        {
            List1 = (IList) Activator.CreateInstance((typeof(List<>).MakeGenericType(t1)));
            List2 = (IList) Activator.CreateInstance((typeof(List<>).MakeGenericType(t2)));
        }
    }
}
