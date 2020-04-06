using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPA.QueryProcessor.Preprocessor;

namespace SPA.QueryProcessor
{
    /*
     * This class represents PQL Controller.
     * It returns formatted query results as 'String'.
     *
     * Refer to Handbook Figure 18 for reference.
     */
    public class QueryParser
    {
        public static string ProcessQuery(string query)
        {
            try
            {
                QueryValidator queryValidator = new QueryValidator();
                bool isQueryValid = queryValidator.ValidateQuery(query);
            }
            catch (Exception e)
            {
                //return $"#{e.Message} - {e.GetType()}";
                Console.WriteLine("none");
                //return "none";
            }

            return "OK";
        }
    }
}
