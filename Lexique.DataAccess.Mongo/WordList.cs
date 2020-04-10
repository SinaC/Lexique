using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lexique.IDataAccess;
using Lexique.Mongo;

namespace Lexique.DataAccess.Mongo
{
    public class WordList : IWordList
    {
        private const string DatabaseName = "Lexique";

        public IEnumerable<string> GetWords(Language language)
        {
            string connectionString = ConfigurationManager.AppSettings["connectionstring"];

            string collectionName = MapLanguageToCollectionName(language);
            if (string.IsNullOrWhiteSpace(collectionName))
                return Enumerable.Empty<string>();

            MongoCappedRepository<WordListByFirstLetter> repo = new MongoCappedRepository<WordListByFirstLetter>(connectionString, DatabaseName, collectionName, 26);
            var wordsByFirstLetter = repo.GetAll();
            var words = wordsByFirstLetter.SelectMany(x => x.Words).Distinct().ToArray();

            return words;
        }

        public async Task<IEnumerable<string>> GetWordsAsync(Language language, CancellationToken cancellationToken)
        {
            string connectionString = ConfigurationManager.AppSettings["connectionstring"];

            string collectionName = MapLanguageToCollectionName(language);
            if (string.IsNullOrWhiteSpace(collectionName))
                return Enumerable.Empty<string>();

            MongoCappedRepository<WordListByFirstLetter> repo = new MongoCappedRepository<WordListByFirstLetter>(connectionString, DatabaseName, collectionName, 26);
            var wordsByFirstLetter = await repo.GetAllAsync(cancellationToken);
            var words = wordsByFirstLetter.SelectMany(x => x.Words).Distinct().ToArray();

            return words;
        }

        public void Fetch(Language language, IEnumerable<string> words)
        {
            string connectionString = ConfigurationManager.AppSettings["connectionstring"];

            string collectionName = MapLanguageToCollectionName(language);
            if (string.IsNullOrWhiteSpace(collectionName))
                return;

            MongoCappedRepository<WordListByFirstLetter> repo = new MongoCappedRepository<WordListByFirstLetter>(connectionString, DatabaseName, collectionName, 26);

            var frenchWordsByFirstLetter = words.Distinct().GroupBy(x => x[0]).Select(x => new WordListByFirstLetter
            {
                FirstLetter = x.Key,
                Words = x.ToList()
            }).ToList();
            repo.Fetch(frenchWordsByFirstLetter);
        }

        private string MapLanguageToCollectionName(Language language)
        {
            switch (language)
            {
                case Language.French:
                    return "FrenchByFirstLetter";
                case Language.English:
                    return "EnglishByFirstLetter";
            }

            return string.Empty;
        }
    }
}
