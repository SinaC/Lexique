using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        private List<string> _validWords;

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
            table[0, 0] = "G";
            table[0, 1] = "I";
            table[0, 2] = "R";
            table[0, 3] = "I";
            table[1, 0] = "I";
            table[1, 1] = "A";
            table[1, 2] = "A";
            table[1, 3] = "S";
            table[2, 0] = "R";
            table[2, 1] = "N";
            table[2, 2] = "E";
            table[2, 3] = "T";
            table[3, 0] = "N";
            table[3, 1] = "O";
            table[3, 2] = "O";
            table[3, 3] = "S";

            for (int column = 0; column < gridInput.ColumnDefinitions.Count; column++)
                for (int row = 0; row < gridInput.RowDefinitions.Count; row++)
                {
                    TextBox input = gridInput.Children.OfType<TextBox>().First(x => Grid.GetRow(x) == row && Grid.GetColumn(x) == column);
                    if (input != null)
                        input.Text = table[row, column];
                }

            //for (int y = 0; y < 4; y++)
            //    for (int x = 0; x < 4; x++)
            //    {
            //        Label lbl = new Label
            //        {
            //            Content = table[x, y],
            //            Width = 40,
            //            Height = 40,
            //            HorizontalContentAlignment = HorizontalAlignment.Center,
            //            VerticalContentAlignment = VerticalAlignment.Center,
            //            Background = new SolidColorBrush(Colors.CadetBlue),
            //            Tag = null
            //        };
            //        Canvas.SetLeft(lbl, x * (40 + 10) + 100);
            //        Canvas.SetTop(lbl, y * (40 + 10) + 100);
            //        Panel.SetZIndex(lbl, 10);
            //        canvasPaint.Children.Add(lbl);
            //    }

            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                {
                    Label lbl = new Label
                    {
                        Content = table[x, y],
                        Width = 40,
                        Height = 40,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Background = new SolidColorBrush(Colors.CadetBlue),
                        Tag = null
                    };
                    Canvas.SetLeft(lbl, x * 40 + 100);
                    Canvas.SetTop(lbl, y * 40 + 100);
                    Panel.SetZIndex(lbl, 10);
                    canvasPaint.Children.Add(lbl);
                }

            //for (int y = 0; y < 4; y++)
            //    for (int x = 0; x < 4; x++)
            //    {
            //        Label lbl = new Label
            //            {
            //                Content = table[x, y],
            //                Width = 40,
            //                Height = 40,
            //                HorizontalContentAlignment = HorizontalAlignment.Center,
            //                VerticalContentAlignment = VerticalAlignment.Center,
            //                Background = new SolidColorBrush(Colors.CadetBlue),
            //                Tag = null
            //            };
            //        Canvas.SetLeft(lbl, x * 40 + 100);
            //        Canvas.SetTop(lbl, y * 40 + 100);
            //        Panel.SetZIndex(lbl, 0);
            //        canvasPaint.Children.Add(lbl);

            //        Ellipse ellipse = new Ellipse
            //        {
            //            Width = 40,
            //            Height = 40,
            //            Tag = lbl,
            //            Fill = new SolidColorBrush(Color.FromArgb(128, 255, 0, 0))
            //        };
            //        Canvas.SetLeft(ellipse, x * 40 + 100);
            //        Canvas.SetTop(ellipse, y * 40 + 100);
            //        Panel.SetZIndex(ellipse, 10);
            //        canvasPaint.Children.Add(ellipse);
            //    }
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
                    TextBox input = gridInput.Children.OfType<TextBox>().First(x => Grid.GetRow(x) == row && Grid.GetColumn(x) == column);
                    if (input != null)
                        table[row, column] = input.Text;
                }

            Graph.Graph graph = new Graph.Graph(table);

            List<string> words = GetWords(graph, _trie);
            _validWords = words;

            lstResults.Items.Clear();
            foreach (string word in words.OrderByDescending(x => x.Length).ThenBy(x => x))
                lstResults.Items.Add(word);
        }

        private string _currentGuess;
        private Point _previousPoint;
        private List<Line> _currentPath;

        private void Canvas_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                _previousPoint = e.GetPosition(canvasPaint);
                _currentGuess = String.Empty;
                _currentPath = new List<Line>();
                txtGuess.Foreground = new SolidColorBrush(Colors.Black);
                SwitchCellOn(e);
            }
        }

        private void Canvas_MouseUp_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                txtGuess.Text = _currentGuess;
                if (_validWords != null)
                    txtGuess.Foreground = _validWords.Any(x => x.ToLower() == _currentGuess.ToLower()) ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                SwitchOffCells();
            }
        }

        private void Canvas_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                SwitchCellOn(e);
        }

        private void SwitchCellOn(MouseEventArgs e)
        {
            Point position = e.GetPosition(canvasPaint);
            SwitchCellOn(position);
        }

        private void SwitchCellOn(MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(canvasPaint);
            SwitchCellOn(position);
        }

        private void SwitchCellOn(Point position)
        {
            foreach (Label lbl in canvasPaint.Children.OfType<Label>().Where(x => x.Tag == null)) // unmarked cells
            {
                double lblX = Canvas.GetLeft(lbl);
                double lblY = Canvas.GetTop(lbl);

                double centerX = lblX + lbl.ActualWidth/2;
                double centerY = lblY + lbl.ActualHeight/2;

                double distance2 = (position.X - centerX)*(position.X - centerX) + (position.Y - centerY)*(position.Y - centerY);

                if (distance2 <= (lbl.ActualHeight * lbl.ActualHeight + 1)/4)
                //if (position.X >= lblX && position.X <= lblX + lbl.ActualWidth &&
                //    position.Y >= lblY && position.Y <= lblY + lbl.ActualHeight)
                {
                    lbl.Background = new SolidColorBrush(Colors.Green);
                    lbl.Tag = lbl.Background; // Mark cell
                    _currentGuess += (string) lbl.Content;
                    txtGuess.Text = _currentGuess;
                }
            }
            DrawPath(position);
        }

        private void DrawPath(Point position)
        {
            Line line = new Line
                {
                    Stroke = new SolidColorBrush(Colors.LightGreen),
                    X1 = _previousPoint.X,
                    Y1 = _previousPoint.Y,
                    X2 = position.X,
                    Y2 = position.Y
                };
            Panel.SetZIndex(line, 100);
            _previousPoint = position;
            _currentPath.Add(line);

            canvasPaint.Children.Add(line);
        }

        private void SwitchOffCells()
        {
            foreach (Label lbl in canvasPaint.Children.OfType<Label>())
            {
                lbl.Background = new SolidColorBrush(Colors.CadetBlue);
                lbl.Tag = null; // Unmark cell
            }
            HidePath();
        }

        private void HidePath()
        {
            foreach (Line line in _currentPath)
                canvasPaint.Children.Remove(line);
        }
    }
}
