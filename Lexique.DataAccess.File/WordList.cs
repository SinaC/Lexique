using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Lexique.IDataAccess;

namespace Lexique.DataAccess.File
{
    public class WordList : IWordList
    {
        private Regex RegexAlphaNum { get; } = new Regex("[^a-zA-Z]");

        public List<string> Words { get; private set; }

        public WordList()
        {
            Words = new List<string>();
        }

        public WordList(IEnumerable<string> words)
        {
            Words = words.Select(x => RemoveAccent(x).Trim().ToLower()).Where(IsWordValid).Distinct().ToList();
        }

        #region IWordList

        public IEnumerable<string> GetWords(Language language)
        {
            switch (language)
            {
                case Language.French:
                    ReadFrench();
                    return Words.ToArray();
                case Language.English:
                    ReadEnglish();
                    return Words.ToArray();
            }

            return Enumerable.Empty<string>();
        }

        public async Task<IEnumerable<string>> GetWordsAsync(Language language, CancellationToken cancellationToken)
        {
            var words = GetWords(language);
            return await Task.FromResult(words);
        }

        #endregion

        private void ReadFrench()
        {
            Words.Clear();

            string path = ConfigurationManager.AppSettings["path"];
            ReadLexique3(Path.Combine(path, "Lexique3.txt"));
            ReadWordList(Path.Combine(path, "liste.de.mots.francais.frgut.txt"));
            ReadWordList(Path.Combine(path, "liste_francais.txt"));
            ReadWordList(Path.Combine(path, "ods4.txt"));
            ReadWordList(Path.Combine(path, "ODS5.txt"));
            ReadWordList(Path.Combine(path, "pli07.txt"));
            ReadCSV(Path.Combine(path, "DicFra.csv"));
            ReadTxt(Path.Combine(path, "dict.xmatiere.com.16.csvtxt"));
            ReadWordList(Path.Combine(path, "liste16.txt"));
            Distinct();
        }

        private void ReadEnglish()
        {
            Words.Clear();

            string path = ConfigurationManager.AppSettings["path"];
            ReadWordList(Path.Combine(path, "sowpods.txt"));
            Distinct();
        }

        public void Distinct()
        {
            Words = Words.Distinct().ToList();
        }

        public void ReadLexique3(string filename)
        {
            using (StreamReader reader = new StreamReader(filename, Encoding.UTF7))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line != null)
                    {
                        //line: 1_ortho	2_phon	3_lemme	4_cgram	5_genre	6_nombre	7_freqlemfilms2	8_freqlemlivres	9_freqfilms2	10_freqlivres	11_infover	12_nbhomogr	13_nbhomoph	14_islem	15_nblettres	16_nbphons	17_cvcv	18_p_cvcv	19_voisorth	20_voisphon	21_puorth	22_puphon	23_syll	24_nbsyll	25_cv-cv	26_orthrenv	27_phonrenv	28_orthosyll	29_cgramortho	30_deflem	31_defobs
                        //token       0      1 ....
                        string[] tokens = line.Split('\t');
                        if (tokens.Length > 0)
                        {
                            string word = RemoveAccent(tokens[0]).Trim().ToLower();
                            if (IsWordValid(word))
                                Words.Add(word);
                        }
                    }
                }
            }
        }

        public void ReadWordList(string filename)
        {
            using (StreamReader reader = new StreamReader(filename, Encoding.UTF7))
            {
                while (!reader.EndOfStream)
                {
                    string word = RemoveAccent(reader.ReadLine()).Trim().ToLower();
                    if (IsWordValid(word))
                        Words.Add(word);
                }
            }
        }

        public void ReadCSV(string filename)
        {
            using (StreamReader reader = new StreamReader(filename, Encoding.UTF7))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line != null)
                    {
                        //line:  "word", "definition"
                        //token: "" word "" definition
                        //        0    1  2          3
                        string[] tokens = line.Split('"');
                        if (tokens.Length > 0)
                        {
                            string word = RemoveAccent(tokens[1]).Trim().ToLower();
                            if (IsWordValid(word))
                                Words.Add(word);
                        }
                    }
                }
            }
        }

        public void ReadTxt(string filename)
        {
            using (StreamReader reader = new StreamReader(filename, Encoding.UTF7))
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    string[] tokens = line.Split(',');
                    foreach (string iter in tokens)
                    {
                        string word = RemoveAccent(iter).Trim().ToLower();
                        if (IsWordValid(word))
                            Words.Add(word);
                    }
                }
            }
        }

        private static string RemoveAccent(string text)
        {
            byte[] aOctets = Encoding.GetEncoding(1251).GetBytes(text);
            string sEnleverAccents = Encoding.ASCII.GetString(aOctets);
            return sEnleverAccents;
        }

        private bool IsWordValid(string word)
        {
            return !RegexAlphaNum.IsMatch(word);
        }
    }
}