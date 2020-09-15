using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Lexique.IDataAccess;
using Lexique.WpfApp.Models;
using Lexique.WpfApp.MVVM;

namespace Lexique.WpfApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IWordList WordList => new DataAccess.File.WordList(); // TODO: test with mongo

        public int WordCount => _words?.Length ?? 0;
        public bool AreWordsLoaded => _words != null && WordCount > 0;

        private string[] _words;
        [LinkedProperty(nameof(WordCount))]
        [LinkedProperty(nameof(AreWordsLoaded))]
        public string[] Words
        {
            get => _words;
            protected set => Set(() => Words, ref _words, value);
        }

        private ICommand _loadFrWordsCommand;
        public ICommand LoadFrWordsCommand => _loadFrWordsCommand = _loadFrWordsCommand ?? new RelayCommand(LoadFrWords);

        private void LoadFrWords()
        {
            Words = WordList.GetWords(Language.French).ToArray();
        }

        private ICommand _loadEnWordsCommand;
        public ICommand LoadEnWordsCommand => _loadEnWordsCommand = _loadEnWordsCommand ?? new RelayCommand(LoadEnWords);

        private void LoadEnWords()
        {
            Words = WordList.GetWords(Language.English).ToArray();
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

        private ICommand _searchBookwormCommand;
        public ICommand SearchBookwormCommand => _searchBookwormCommand = _searchBookwormCommand ?? new RelayCommand(SearchBookworm);

        private void SearchBookworm()
        {
            if (_words == null || string.IsNullOrWhiteSpace(InputText))
                return;
            Regex r = new Regex(@"\((\w+)\)");
            MatchCollection matches = r.Matches(InputText);
            var valuableLetters = matches.Cast<Match>().Select(x => x.Value[1]).ToArray();
            var words = GetWords(InputText, 3, s => new BookwormWord(s, valuableLetters));
            Results = words.OrderBy(x => x).Select(x => x.ToString()).ToArray();
        }

        private ICommand _searchBuzzWordCommand;
        public ICommand SearchBuzzWordCommand => _searchBuzzWordCommand = _searchBuzzWordCommand ?? new RelayCommand(SearchBuzzWord);

        private void SearchBuzzWord()
        {
            if (_words == null || string.IsNullOrWhiteSpace(InputText))
                return;

            var words = GetWords(InputText, 3, s => new BuzzWord(s));
            Results = words.OrderBy(x => x).Select(x => x.ToString()).ToArray();
        }

        private ICommand _searchCrosswordsCommand;
        public ICommand SearchCrosswordsCommand => _searchCrosswordsCommand = _searchCrosswordsCommand ?? new RelayCommand(SearchCrosswords);

        private void SearchCrosswords()
        {
            if (_words == null || string.IsNullOrWhiteSpace(InputText))
                return;

            string pattern = InputText.ToLowerInvariant().Replace("?", @"\w").Replace("*", @"\w*");
            Regex regex = new Regex("^" + pattern  + "$");
            Results = _words.Where(word => regex.IsMatch(word)).OrderBy(x => x).ToList();
        }

        private ICommand _searchScrabbleCommand;
        public ICommand SearchScrabbleCommand => _searchScrabbleCommand = _searchScrabbleCommand ?? new RelayCommand(SearchScrabble);

        private void SearchScrabble()
        {
            if (_words == null || string.IsNullOrWhiteSpace(InputText))
                return;

            List<ScrabbleWord> words = new List<ScrabbleWord>();
            for (char c = 'a'; c <= 'z'; c++)
                words.AddRange(GetWords($"{InputText}{c}", InputText.Length - 1, s => new ScrabbleWord(s, c)));
            Results = words.OrderBy(x => x).Select(x => x.ToString()).Distinct().ToList();
        }

        private ICommand _searchCodedWordsCommand;
        public ICommand SearchCodedWordsCommand => _searchCodedWordsCommand = _searchCodedWordsCommand ?? new RelayCommand(SearchCodedWords);

        private void SearchCodedWords()
        {
            if (_words == null || string.IsNullOrWhiteSpace(InputText))
                return;

            int length = InputText.Length;
            string pattern = InputText.ToLowerInvariant();
            int[][] positionMatching = new int[10][];
            for (int i = 0; i < 10; i++)
            {
                var iString = i.ToString();
                if (InputText.Contains(iString))
                {
                    positionMatching[i] = AllIndexOf(InputText, iString, StringComparison.InvariantCultureIgnoreCase).ToArray();
                    pattern = pattern.Replace(iString, @"\w");
                }
            }
            pattern = pattern.Replace("?", @"\w");

            Regex regex = new Regex("^" + pattern + "$");
            var firstMatchResults = _words.Where(word => word.Length == length).Where(word => regex.IsMatch(word)).OrderBy(x => x).ToList();
            List<string> results = new List<string>();
            foreach (string result in firstMatchResults)
            {
                bool match = true;
                foreach (var positions in Enumerable.Range(0, 10).Where(x => positionMatching[x] != null && positionMatching[x].Length > 1).Select(digit => positionMatching[digit]))
                {
                    for (int i = 1; i < positions.Length; i++)
                        if (result[positions[i - 1]] != result[positions[i]])
                        {
                            match = false;
                            break;
                        }

                    if (!match)
                        break;
                }
                if (match)
                    results.Add(result);
            }

            Results = results;
        }

        //
        private List<T> GetWords<T>(string input, int minWordLength, Func<string, T> createWordFunc)
            where T : WordBase
        {
            List<T> results = new List<T>();

            bool[] matches = new bool[input.Length];

            foreach (string word in _words.Where(word => word.Length >= minWordLength && word.Length <= input.Length))
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

        public static List<int> AllIndexOf(string text, string str, StringComparison comparisonType)
        {
            List<int> allIndexOf = new List<int>();
            int index = text.IndexOf(str, comparisonType);
            while (index != -1)
            {
                allIndexOf.Add(index);
                index = text.IndexOf(str, index + 1, comparisonType);
            }
            return allIndexOf;
        }
    }

    internal class MainViewModelDesignData : MainViewModel
    {
    }
}
