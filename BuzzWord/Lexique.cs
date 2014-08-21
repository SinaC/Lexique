using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace BuzzWord {
    public class CSortedBuzzWord : CBuzzWord {
        protected string m_SortedWord;

        public CSortedBuzzWord(string word)
            : base(word) {
            m_SortedWord = SortWord(word);
        }

        public string SortedWord { get { return m_SortedWord; } }

        public override string ToString() {
            return base.ToString() + "  [" + m_SortedWord + "]";
        }

        public static int CompareSortedWord(CSortedBuzzWord a, CSortedBuzzWord b) {
            return String.Compare(a.m_SortedWord, b.m_SortedWord);
        }

        protected static string SortWord(string word) {
            List<char> sortedWordLetter = word.ToCharArray().ToList();
            sortedWordLetter.Sort();
            return new string(sortedWordLetter.ToArray());
        }
    };

    public class CBuzzWord {
        protected string m_Word;
        protected int m_Value;

        public CBuzzWord(string word) {
            m_Word = word;
            m_Value = ComputeValue(word);
        }

        public string Word { get { return m_Word; } }
        public int Value { get { return m_Value; } }

        public override string ToString() {
            return m_Word + " => " + m_Value;
        }

        public static int Compare(CBuzzWord a, CBuzzWord b) {
            if (a.Value < b.Value)
                return +1;
            if (a.Value > b.Value)
                return -1;
            return 0;
        }

        protected static int ComputeValue(string s) {
            //                     A,  B,  C,  D, F,  F,  G,  H,  I,  J,  K,  L,  M,  N,  O,  P,  Q,  R, S,  T,  U,  V,  W,  W,  Y,  Z
            int[] letterValues = { 5, 30, 20, 20, 5, 30, 30, 30, 10, 50, 50, 20, 20, 10, 20, 30, 40, 10, 5, 20, 20, 30, 50, 40, 40, 30 };
            //                          1   2   3   4   5    6    7    8    9   10   11   12   13   14   15   16
            int[] wordLengthValues = { 15, 20, 30, 50, 75, 105, 140, 180, 225, 275, 330, 390, 455, 525, 600, 680 };

            int value = s.Length > wordLengthValues.Length ? 1000 : wordLengthValues[s.Length - 1];
            for (int i = 0; i < s.Length; i++) {
                int c = Convert.ToInt32(s[i]) - Convert.ToInt32('a');
                value += (c >= 0 && c <= 25) ? letterValues[c] : 0;
            }
            return value;
        }
    };

    public class CLexique {
        private List<string> m_WordList = null;
        private Regex m_regexAlphaNum;

        public CLexique() {
            m_WordList = new List<string>();
            m_regexAlphaNum = new Regex("[^a-zA-Z]");
        }

        public int WordCount { get { return m_WordList == null ? 0 : m_WordList.Count; } }

        private static string RemoveAccent(string text) {
            byte[] aOctets = System.Text.Encoding.GetEncoding(1251).GetBytes(text);
            string sEnleverAccents = System.Text.Encoding.ASCII.GetString(aOctets);
            return sEnleverAccents;
        }

        public bool IsWordValid(string word) {
            return !m_regexAlphaNum.IsMatch(word);
        }

        public void Distinct() {
            m_WordList = m_WordList.Distinct().ToList();
        }

        public void ReadLexique3(string filename) {
            using (StreamReader reader = new StreamReader(filename, System.Text.Encoding.UTF7)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    //line: 1_ortho	2_phon	3_lemme	4_cgram	5_genre	6_nombre	7_freqlemfilms2	8_freqlemlivres	9_freqfilms2	10_freqlivres	11_infover	12_nbhomogr	13_nbhomoph	14_islem	15_nblettres	16_nbphons	17_cvcv	18_p_cvcv	19_voisorth	20_voisphon	21_puorth	22_puphon	23_syll	24_nbsyll	25_cv-cv	26_orthrenv	27_phonrenv	28_orthosyll	29_cgramortho	30_deflem	31_defobs
                    //token       0      1 ....
                    string[] tokens = line.Split('\t');
                    if (null != tokens && tokens.Length > 0) {
                        string word = RemoveAccent(tokens[0]).Trim().ToLower();
                        if (IsWordValid(word))
                            m_WordList.Add(word);
                    }
                }
            }
        }

        public void ReadWordList(string filename) {
            using (StreamReader reader = new StreamReader(filename, System.Text.Encoding.UTF7)) {
                while (!reader.EndOfStream) {
                    string word = RemoveAccent(reader.ReadLine()).Trim().ToLower();
                    if (IsWordValid(word))
                        m_WordList.Add(word);
                }
            }
        }

        public void ReadCSV(string filename) {
            using (StreamReader reader = new StreamReader(filename, System.Text.Encoding.UTF7)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    //line:  "word", "definition"
                    //token: "" word "" definition
                    //        0    1  2          3
                    string[] tokens = line.Split('"');
                    if (null != tokens && tokens.Length > 0) {
                        string word = RemoveAccent(tokens[1]).Trim().ToLower();
                        if (IsWordValid(word))
                            m_WordList.Add(word);
                    }
                }
            }
        }

        public void ReadTxt(string filename) {
            using (StreamReader reader = new StreamReader(filename, System.Text.Encoding.UTF7)) {
                string line = reader.ReadLine();
                string[] tokens = line.Split(',');
                foreach (string iter in tokens) {
                    string word = RemoveAccent(iter).Trim().ToLower();
                    if (IsWordValid(word))
                        m_WordList.Add(word);
                }
            }
        }

        public void Clear() {
            m_WordList.Clear();
        }

        public List<CSortedBuzzWord> GetBestBuzzWords(int minWordLength, int maxWordLength) {
            List<CSortedBuzzWord> result = new List<CSortedBuzzWord>();

            foreach (string word in m_WordList)
                if (word.Length >= minWordLength && word.Length <= maxWordLength)
                    result.Add(new CSortedBuzzWord(word));

            result.Sort(CBuzzWord.Compare);

            return result;
        }

        public List<CBuzzWord> GetBuzzWordList(string buzzString, int minWordLength) {
            if (m_WordList == null)
                return null;

            List<CBuzzWord> result = new List<CBuzzWord>();

            bool[] matches = new bool[buzzString.Length];

            foreach (string word in m_WordList)
                if (word.Length >= minWordLength && word.Length <= buzzString.Length) { // Word long enough and not too long
                    for (int i = 0; i < matches.Length; i++)
                        matches[i] = false;
                    // For each letter i in word
                    //      For each letter j in buzzword
                    //          If j not marked and i == j
                    //              mark j and leave loop
                    bool fMatch = true;
                    for (int i = 0; i < word.Length; i++) {
                        bool fFound = false;
                        for (int j = 0; j < buzzString.Length; j++) {
                            if (!matches[j] && word[i] == buzzString[j]) {
                                matches[j] = true;
                                fFound = true;
                                break;
                            }
                        }
                        if (!fFound) {
                            fMatch = false;
                            break;
                        }
                    }
                    if (fMatch)
                        result.Add(new CBuzzWord(word));
                }

            result.Sort(CBuzzWord.Compare);

            return result;
        }

        public IOrderedEnumerable<KeyValuePair<int, int>> GetWordCountByLetters() {
            Dictionary<int, int> result = new Dictionary<int, int>();
            foreach (string word in m_WordList) {
                if (result.ContainsKey(word.Length))
                    result[word.Length]++;
                else
                    result[word.Length] = 1;
            }
            return result.OrderBy(kv => kv.Key);
        }

        public List<string> CrossWords(string pattern) {
            List<string> result = new List<string>();
            Regex regex = new Regex("^" + pattern.Replace("?", @"\w").Replace("*",@"\w*") + "$" );
            foreach (string word in m_WordList) {
                if (regex.IsMatch(word))
                    result.Add(word);
                //bool fMatch = false;
                //if (word.Length == pattern.Length) {
                //    fMatch = true;
                //    for (int i = 0; i < word.Length; i++)
                //        if (pattern[i] != '?'
                //            && pattern[i] != word[i]) {
                //            fMatch = false;
                //            break;
                //        }
                //}
                //if (fMatch)
                //    result.Add(word);
            }
            result.Sort();
            return result;
        }
    }
}
