using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Lexique.Models;
using Lexique.MVVM;

namespace Lexique.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private WordList _wordList;

        public int WordCount => _wordList.Words.Count;
        public bool AreWordsLoaded => _wordList != null && WordCount > 0;

        private ICommand _loadWordsCommand;
        public ICommand LoadWordsCommand => _loadWordsCommand = _loadWordsCommand ?? new RelayCommand(LoadWords);

        private void LoadWords()
        {
            string path = ConfigurationManager.AppSettings["path"];
            _wordList = new WordList();
            _wordList.ReadLexique3(Path.Combine(path, "Lexique3.txt"));
            _wordList.ReadWordList(Path.Combine(path, "liste.de.mots.francais.frgut.txt"));
            _wordList.ReadWordList(Path.Combine(path, "liste_francais.txt"));
            _wordList.ReadWordList(Path.Combine(path, "ods4.txt"));
            _wordList.ReadWordList(Path.Combine(path, "ODS5.txt"));
            _wordList.ReadWordList(Path.Combine(path, "pli07.txt"));
            _wordList.ReadCSV(Path.Combine(path, "DicFra.csv"));
            _wordList.ReadTxt(Path.Combine(path, "dict.xmatiere.com.16.csvtxt"));
            _wordList.ReadWordList(Path.Combine(path, "liste16.txt"));
            _wordList.Distinct();

            OnPropertyChanged(nameof(WordCount));
            OnPropertyChanged(nameof(AreWordsLoaded));
        }

        private string _inputText;

        public string InputText
        {
            get => _inputText;
            set => Set(() => InputText, ref _inputText, value);
        }

        private IEnumerable<string> _results;

        public IEnumerable<string> Results
        {
            get => _results;
            protected set => Set(() => Results, ref _results, value);
        }

        private ICommand _searchBuzzWordCommand;
        public ICommand SearchBuzzWordCommand => _searchBuzzWordCommand = _searchBuzzWordCommand ?? new RelayCommand(SearchBuzzWord);

        private void SearchBuzzWord()
        {
            if (_wordList == null || string.IsNullOrWhiteSpace(InputText))
                return;

            var words = GetWords(InputText, 3, s => new BuzzWord(s));
            Results = words.OrderBy(x => x).Select(x => x.ToString()).ToArray();
        }

        private ICommand _searchCrosswordsCommand;
        public ICommand SearchCrosswordsCommand => _searchCrosswordsCommand = _searchCrosswordsCommand ?? new RelayCommand(SearchCrosswords);

        private void SearchCrosswords()
        {
            if (_wordList == null || string.IsNullOrWhiteSpace(InputText))
                return;

            Regex regex = new Regex("^" + InputText.Replace("?", @"\w").Replace("*", @"\w*") + "$");
            Results = _wordList.Words.Where(word => regex.IsMatch(word)).OrderBy(x => x).ToList();
        }

        private ICommand _searchScrabbleCommand;
        public ICommand SearchScrabbleCommand => _searchScrabbleCommand = _searchScrabbleCommand ?? new RelayCommand(SearchScrabble);

        private void SearchScrabble()
        {
            if (_wordList == null || string.IsNullOrWhiteSpace(InputText))
                return;

            List<ScrabbleWord> words = new List<ScrabbleWord>();
            for (char c = 'a'; c <= 'z'; c++)
                words.AddRange(GetWords($"{InputText}{c}", InputText.Length - 1, s => new ScrabbleWord(s, c)));
            Results = words.OrderBy(x => x).Select(x => x.ToString()).Distinct().ToList();
        }

        //
        private List<T> GetWords<T>(string input, int minWordLength, Func<string, T> createWordFunc)
            where T : WordBase
        {
            List<T> results = new List<T>();

            bool[] matches = new bool[input.Length];

            foreach (string word in _wordList.Words.Where(word => word.Length >= minWordLength && word.Length <= input.Length))
            {
                // Word long enough and not too long
                for (int i = 0; i < matches.Length; i++)
                    matches[i] = false;
                // For each letter i in word
                //      For each letter j in buzzword
                //          If j not marked and i == j
                //              mark j and leave loop
                bool fMatch = true;
                foreach (char c in word)
                {
                    bool fFound = false;
                    for (int j = 0; j < input.Length; j++)
                    {
                        if (!matches[j] && c == input[j])
                        {
                            matches[j] = true;
                            fFound = true;
                            break;
                        }
                    }

                    if (!fFound)
                    {
                        fMatch = false;
                        break;
                    }
                }

                if (fMatch)
                    results.Add(createWordFunc(word));
            }

            return results;
        }
    }
}
