using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Lexique.DataAccess.Mongo
{
    public class WordListByFirstLetter
    {
        [BsonId]
        public char FirstLetter { get; set; }

        public List<string> Words { get; set; }
    }
}
