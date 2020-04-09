using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false";

            try
            {
                string path = @"F:\GitHub\Lexique\wordlist";

                WordList frenchWordList = new WordList();
                frenchWordList.ReadLexique3(Path.Combine(path, "Lexique3.txt"));
                frenchWordList.ReadWordList(Path.Combine(path, "liste.de.mots.francais.frgut.txt"));
                frenchWordList.ReadWordList(Path.Combine(path, "liste_francais.txt"));
                frenchWordList.ReadWordList(Path.Combine(path, "ods4.txt"));
                frenchWordList.ReadWordList(Path.Combine(path, "ODS5.txt"));
                frenchWordList.ReadWordList(Path.Combine(path, "pli07.txt"));
                frenchWordList.ReadCSV(Path.Combine(path, "DicFra.csv"));
                frenchWordList.ReadTxt(Path.Combine(path, "dict.xmatiere.com.16.csvtxt"));
                frenchWordList.ReadWordList(Path.Combine(path, "liste16.txt"));
                frenchWordList.Distinct();

                WordList englishWordList = new WordList();
                englishWordList.ReadWordList(Path.Combine(path, "sowpods.txt"));
                englishWordList.Distinct();

                MongoCappedRepository<WordListByFirstLetter> repoFrench = new MongoCappedRepository<WordListByFirstLetter>(connectionString, "Lexique", "FrenchByFirstLetter", 26);
                var frenchWordsByFirstLetter = frenchWordList.Words.Distinct().GroupBy(x => x[0]).Select(x => new WordListByFirstLetter 
                {
                    FirstLetter = x.Key,
                    Words = x.ToList()
                }).ToList();
                repoFrench.Fetch(frenchWordsByFirstLetter);


                MongoCappedRepository<WordListByFirstLetter> repoEnglish = new MongoCappedRepository<WordListByFirstLetter>(connectionString, "Lexique", "EnglishByFirstLetter", 26);
                var englishWordsByFirstLetter = englishWordList.Words.Distinct().GroupBy(x => x[0]).Select(x => new WordListByFirstLetter
                {
                    FirstLetter = x.Key,
                    Words = x.ToList()
                }).ToList();
                repoEnglish.Fetch(englishWordsByFirstLetter);
            }
            catch (Exception ex)
            {
            }
        }
    }
    public class WordListByFirstLetter
    {
        [BsonId]
        public char FirstLetter { get; set; }

        public List<string> Words { get; set; }
    }
}
