using System.Linq;
using System.Windows.Input;
using Lexique.Models;
using Lexique.MVVM;

namespace Lexique.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private WordList _wordList;

        private ICommand _loadWordsCommand;
        public ICommand LoadWordsCommand {
            get
            {
                _loadWordsCommand = _loadWordsCommand ?? new RelayCommand(LoadWords);
                return _loadWordsCommand;
            }
        }


        private void LoadWords()
        {
            _wordList = new WordList();
            _wordList.ReadLexique3(@"D:\Projects\Personal\Lexique\WordList\Lexique3.txt");
            _wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\liste.de.mots.francais.frgut.txt");
            _wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\liste_francais.txt");
            _wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\ods4.txt");
            _wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\ODS5.txt");
            _wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\pli07.txt");
            _wordList.ReadCSV(@"D:\Projects\Personal\Lexique\WordList\DicFra.csv");
            _wordList.ReadTxt(@"D:\Projects\Personal\Lexique\WordList\dict.xmatiere.com.16.csvtxt");
            _wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\liste16.txt");
            _wordList.Distinct();
        }
    }
}
