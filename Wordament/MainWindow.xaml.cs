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
        private readonly WordList.WordList _wordList;
        private readonly Trie.Trie _trie;

        public MainWindow()
        {
            InitializeComponent();

            _wordList = new WordList.WordList();
            //wordList.ReadLexique3(@"D:\Projects\Personal\Lexique\WordList\Lexique3.txt");
            //wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\liste.de.mots.francais.frgut.txt");
            //wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\liste_francais.txt");
            //wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\ods4.txt");
            _wordList.ReadWordList(@"D:\GitHub\Lexique\WordList\ODS5.txt");
            //wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\pli07.txt");
            //wordList.ReadCSV(@"D:\Projects\Personal\Lexique\WordList\DicFra.csv");
            //wordList.ReadTxt(@"D:\Projects\Personal\Lexique\WordList\dict.xmatiere.com.16.csvtxt");
            //wordList.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\liste16.txt");
            _wordList.Distinct();

            _trie = new Trie.Trie();
            foreach (string word in _wordList.Words)
                _trie.Add(word);

            //
            string[,] table = new string[4, 4];
            table[0, 0] = "E";
            table[0, 1] = "E";
            table[0, 2] = "S";
            table[0, 3] = "T";
            table[1, 0] = "U";
            table[1, 1] = "I";
            table[1, 2] = "L";
            table[1, 3] = "M";
            table[2, 0] = "A";
            table[2, 1] = "E";
            table[2, 2] = "S";
            table[2, 3] = "T";
            table[3, 0] = "G";
            table[3, 1] = "E";
            table[3, 2] = "A";
            table[3, 3] = "N";

            for (int column = 0; column < gridInput.ColumnDefinitions.Count; column++)
                for (int row = 0; row < gridInput.RowDefinitions.Count; row++)
                {
                    TextBox input = gridInput.Children.Cast<TextBox>().First(x => Grid.GetRow(x) == row && Grid.GetColumn(x) == column);
                    if (input != null)
                        input.Text = table[row, column];
                }
        }

        private static void DepthFirstSearch(Graph.Graph graph, int vertexIndex, Trie.Trie trie, string content, bool[] visited, ISet<string> results)
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

        private static List<string> GetWords(Graph.Graph graph, Trie.Trie trie)
        {
            ISet<string> results = new HashSet<string>();
            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                string content = graph.Vertices[i].Content;
                bool[] visited = new bool[graph.Vertices.Count];
                visited[i] = true;
                DepthFirstSearch(graph, i, trie, content, visited, results);
            }

            return results.ToList();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            string[,] table = new string[4, 4];
            //table[0, 0] = "A";
            //table[0, 1] = "B";
            //table[0, 2] = "C";
            //table[0, 3] = "D";
            //table[1, 0] = "E";
            //table[1, 1] = "F";
            //table[1, 2] = "G";
            //table[1, 3] = "H";
            //table[2, 0] = "I";
            //table[2, 1] = "J";
            //table[2, 2] = "K";
            //table[2, 3] = "L";
            //table[3, 0] = "M";
            //table[3, 1] = "N";
            //table[3, 2] = "O";
            //table[3, 3] = "P";

            for(int column = 0; column < gridInput.ColumnDefinitions.Count; column++)
                for(int row = 0; row < gridInput.RowDefinitions.Count; row++)
                {
                    TextBox input = gridInput.Children.Cast<TextBox>().First(x => Grid.GetRow(x) == row && Grid.GetColumn(x) == column);
                    if (input != null)
                        table[row, column] = input.Text;
                }

            Graph.Graph graph = new Graph.Graph(table);

            List<string> words = GetWords(graph, _trie);

            lstResults.Items.Clear();
            foreach (string word in words.OrderByDescending(x => x.Length).ThenBy(x => x))
                lstResults.Items.Add(word);
        }
    }
}
