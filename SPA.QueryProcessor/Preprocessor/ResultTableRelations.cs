using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPA.QueryProcessor.Preprocessor
{
    public class ResultTableRelations
    {
        public Dictionary<string, List<DictionaryNode>> dict { get; set; }

        public ResultTableRelations()
        {
            dict = new Dictionary<string, List<DictionaryNode>>();
        }

        /// <summary>
        /// Use this method for only one Ident, $ - is a guardian, positionInRelated = -1 because there is no relation
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddRelation(string key, int value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key].Add(new DictionaryNode("$", value, -1));
            }
            else
            {
                dict.Add(key, new List<DictionaryNode> {
                            new DictionaryNode("$", value, -1)
                    });
            }
        }

        /// <summary>
        /// Use this method for double Ident
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="value"></param>
        public void AddRelation(string key1, string key2, int value)
        {
            if (dict.ContainsKey(key1))
            {
                int list1_count = dict[key1].Count;
                int list2_count = 0;

                if (dict.ContainsKey(key2))
                {
                    list2_count = dict[key2].Count;
                    //both lists exists so we just add
                    dict[key1].Add(new DictionaryNode(key2, value, list2_count));
                    dict[key2].Add(new DictionaryNode(key1, value, list1_count));
                }
                else
                {
                    dict[key1].Add(new DictionaryNode(key2, value, list2_count));

                    dict.Add(key2, new List<DictionaryNode> {
                            new DictionaryNode(key1, value, list1_count) }
                    );
                }
            }
            else
            {
                int list1_count = 0;
                int list2_count = 0;

                if (dict.ContainsKey(key2))
                {
                    list2_count = dict[key2].Count;

                    dict.Add(key1, new List<DictionaryNode> {
                            new DictionaryNode(key2, value, list2_count) }
                    );

                    dict[key2].Add(new DictionaryNode(key1, value, list1_count));
                }
                else
                {
                    dict.Add(key1, new List<DictionaryNode> {
                            new DictionaryNode(key2, value, list2_count) }
                    );

                    dict.Add(key2, new List<DictionaryNode> {
                            new DictionaryNode(key1, value, list1_count) }
                    );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentKey"></param>
        /// <param name="indexInResultArray"></param>
        /// <returns>Corresponding key or Empty string </returns>
        public string FindRelatedTableByIndex(string currentKey, int indexInResultArray)
        {
            if (currentKey == "$")
                return "$";

            if (!dict.ContainsKey(currentKey))
                throw new Exception($"#Passed key is invalid: {currentKey}");

            string relatedKey = dict
            .First(d => d.Key == currentKey).Value //Find correct key and get its values
            .Where(v => v.Value == indexInResultArray) //find correct value
            .Select(vk => vk.KeyInRelatedNode) //get key which has the same value
            .FirstOrDefault();

            return relatedKey;
        }
    }
}
