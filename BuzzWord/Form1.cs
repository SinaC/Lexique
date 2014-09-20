using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BuzzWord
{
    public partial class Form1 : Form
    {
        private CLexique lexique;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnReadLexique_Click(object sender, EventArgs e) {
            lexique = new CLexique();
            lexique.ReadLexique3(@"D:\Projects\Personal\Lexique\WordList\Lexique3.txt");
            lexique.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\liste.de.mots.francais.frgut.txt");
            lexique.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\liste_francais.txt");
            lexique.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\ods4.txt");
            lexique.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\ODS5.txt");
            lexique.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\pli07.txt");
            lexique.ReadCSV(@"D:\Projects\Personal\Lexique\WordList\DicFra.csv");
            lexique.ReadTxt(@"D:\Projects\Personal\Lexique\WordList\dict.xmatiere.com.16.csvtxt");
            lexique.ReadWordList(@"D:\Projects\Personal\Lexique\WordList\liste16.txt");
            lexique.Distinct();
            lblWordCount.Text = lexique.WordCount.ToString();
        }

        private void btnGetBuzzWordList_Click(object sender, EventArgs e) {
            //lbResults.Items.Clear();
            if (lexique != null) {
                List<CBuzzWord> results = null;
                results = lexique.GetBuzzWordList(txtBuzzWord.Text.ToLower(), Int32.Parse(txtWordMinLength.Text));
                if (results == null) {
                    results.Add(new CBuzzWord("No results"));
                    lbResults.DataSource = results;
                    lblBuzzWordCount.Text = "0";
                    lblBuzzWordTop5.Text = "0";
                }
                else {
                    lblBuzzWordCount.Text = results.Count.ToString();
                    lbResults.DataSource = results;
                    int top5 = 0;
                    for (int i = 0; i < ( results.Count > 5 ? 5 : results.Count ); i++)
                        top5 += results[i].Value;
                    lblBuzzWordTop5.Text = top5.ToString();
                }
            }
            else {
                lbResults.Items.Add("No lexique loaded");
                lblBuzzWordCount.Text = "";
                lblBuzzWordTop5.Text = "";
            }
        }

        private void btnTest_Click(object sender, EventArgs e) {
            //lbResults.Items.Clear();
            if (lexique != null) {
                List<CSortedBuzzWord> results = null;
                results = lexique.GetBestBuzzWords(13, 16);
                if (results == null) {
                    results.Add(new CSortedBuzzWord("No results"));
                    lbResults.DataSource = results;
                    lblBuzzWordCount.Text = "0";
                    lblBuzzWordTop5.Text = "0";
                }
                else {
                    lblBuzzWordCount.Text = results.Count.ToString();
                    lbResults.DataSource = results;

                    int top5 = 0;
                    for (int i = 0; i < ( results.Count > 5 ? 5 : results.Count ); i++)
                        top5 += results[i].Value;
                    lblBuzzWordTop5.Text = top5.ToString();
                }
            }
            else {
                lbResults.Items.Add("No lexique loaded");
                lblBuzzWordCount.Text = "";
                lblBuzzWordTop5.Text = "";
            }
        }

        private void btnCount_Click(object sender, EventArgs e) {
            //lbResults.Items.Clear();
            if (lexique != null) {
                IOrderedEnumerable<KeyValuePair<int, int>> results = lexique.GetWordCountByLetters();
                if (results == null) {
                    lbResults.Items.Add("No results");
                }
                else {
                    lbResults.DataSource = results.ToList();
                    //foreach (KeyValuePair<int, int> kv in results)
                    //    lbResults.Items.Add(kv.Key.ToString() + " => " + kv.Value.ToString());
                    //lbResults.DataSource = results;
                }
            }
            else {
                lbResults.Items.Add("No lexique loaded");
            }
        }

        private void btnCrossword_Click(object sender, EventArgs e) {
            if (lexique != null) {
                List<string> results = lexique.CrossWords( txtBuzzWord.Text.ToLower() );
                if (results == null) {
                    lbResults.Items.Add("No results");
                }
                else {
                    lbResults.DataSource = results.ToList();
                    //foreach (KeyValuePair<int, int> kv in results)
                    //    lbResults.Items.Add(kv.Key.ToString() + " => " + kv.Value.ToString());
                    //lbResults.DataSource = results;
                }
            }
            else {
                lbResults.Items.Add("No lexique loaded");
            }
        }

        private void btnScrabble_Click(object sender, EventArgs e)
        {
            if (lexique != null)
            {
                List<string> results = lexique.GetScrabble(txtBuzzWord.Text.ToLower());
                if (results == null)
                {
                    lbResults.Items.Add("No results");
                }
                else
                {
                    lbResults.DataSource = results.ToList();
                    //foreach (KeyValuePair<int, int> kv in results)
                    //    lbResults.Items.Add(kv.Key.ToString() + " => " + kv.Value.ToString());
                    //lbResults.DataSource = results;
                }
            }
            else
            {
                lbResults.Items.Add("No lexique loaded");
            }
        }
    }
}
