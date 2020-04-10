using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lexique.IDataAccess
{
    public interface IWordList
    {
        IEnumerable<string> GetWords(Language language);
        Task<IEnumerable<string>> GetWordsAsync(Language language, CancellationToken cancellationToken);
    }
}
