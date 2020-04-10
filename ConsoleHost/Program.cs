using System;
using Lexique.IDataAccess;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
           try
           {
               Lexique.DataAccess.File.WordList frenchFile = new Lexique.DataAccess.File.WordList();
               var frenchWords = frenchFile.GetWords(Lexique.IDataAccess.Language.French);
               Lexique.DataAccess.Mongo.WordList frenchMongo = new Lexique.DataAccess.Mongo.WordList();
               frenchMongo.Fetch(Language.French, frenchWords);

               Lexique.DataAccess.File.WordList englishFile = new Lexique.DataAccess.File.WordList();
               var englishWords = frenchFile.GetWords(Lexique.IDataAccess.Language.English);
               Lexique.DataAccess.Mongo.WordList englishMongo = new Lexique.DataAccess.Mongo.WordList();
               englishMongo.Fetch(Language.English, englishWords);
            }
            catch (Exception ex)
            {
            }
        }
    }

   
}
