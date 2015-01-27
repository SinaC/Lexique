using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wordament
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            WordList.WordList wordList = new WordList.WordList();
            //wordList.ReadLexique3(@"D:\Projects\Personal\Lexique\WordList\Lexique3.txt");
            //wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\liste.de.mots.francais.frgut.txt");
            //wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\liste_francais.txt");
            //wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\ods4.txt");
            wordList.ReadWordList(@"D:\GitHub\Lexique\WordList\ODS5.txt");
            //wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\pli07.txt");
            //wordList.ReadCSV(@"D:\Projects\Personal\Lexique\WordList\DicFra.csv");
            //wordList.ReadTxt(@"D:\Projects\Personal\Lexique\WordList\dict.xmatiere.com.16.csvtxt");
            //wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\liste16.txt");
            wordList.Distinct();

            Trie.Trie trie = new Trie.Trie();
            foreach (string word in wordList.Words)
                trie.Add(word);

            string[,] table = new string[4,4];
            table[0, 0] = "A";
            table[0, 1] = "B";
            table[0, 2] = "C";
            table[0, 3] = "D";
            table[1, 0] = "E";
            table[1, 1] = "F";
            table[1, 2] = "G";
            table[1, 3] = "H";
            table[2, 0] = "I";
            table[2, 1] = "J";
            table[2, 2] = "K";
            table[2, 3] = "L";
            table[3, 0] = "M";
            table[3, 1] = "N";
            table[3, 2] = "O";
            table[3, 3] = "P";

            Graph.Graph graph = new Graph.Graph(table);

            GetWords(graph, trie);
        }

        private void DepthFirstSearch(Graph.Graph graph, int vertexIndex, Trie.Trie trie, string content, bool[] visited, ISet<string> results)
        {
            if (trie.Contains(content) && content.Length >= 3)
                results.Add(content);
            foreach (int edgeIndex in graph.Vertices[vertexIndex].Edges)
                if (!visited[edgeIndex])
                {
                    string newContent = content + graph.Vertices[edgeIndex].Content;
                    Trie.TrieNode node = trie.GetNode(newContent);
                    if (node != null)
                    {
                        visited[edgeIndex] = true;
                        DepthFirstSearch(graph, edgeIndex, trie, newContent, visited, results);
                        visited[edgeIndex] = false;
                    }
                }
        }

        private void GetWords(Graph.Graph graph, Trie.Trie trie)
        {
            ISet<string> results = new HashSet<string>();
            for(int i = 0; i < graph.Vertices.Count; i++)
            {
                string content = graph.Vertices[i].Content;
                bool[] visited = new bool[graph.Vertices.Count];
                visited[i] = true;
                DepthFirstSearch(graph, i, trie, content, visited, results);
            }
        }
    }
}
