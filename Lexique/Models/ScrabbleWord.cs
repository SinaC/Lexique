using System;

namespace Lexique.WpfApp.Models
{
    public class ScrabbleWord : WordBase
    {
        //                                             A,  B,  C,  D, F,  F,  G,  H,  I,  J,  K,  L,  M,  N,  O,  P,  Q,  R, S,  T,  U,  V,  W,  W,  Y,  Z
        private static readonly int[] LetterValues = { 5, 30, 20, 20, 5, 30, 30, 30, 10, 50, 50, 20, 20, 10, 20, 30, 40, 10, 5, 20, 20, 30, 50, 40, 40, 30 };

        public char AdditionalLetter { get; }

        public ScrabbleWord(string letters, char additionalLetter) : base(letters)
        {
            AdditionalLetter = additionalLetter;
        }

        public override string ToString()
        {
            return $"{ReplaceFirst(Word, AdditionalLetter, $"({AdditionalLetter})")} => {Value}";
        }

        protected override int ComputeValue(string s)
        {
            int value = 0;
            foreach (var letter in s)
            {
                int c = Convert.ToInt32(letter) - Convert.ToInt32('a');
                value += c >= 0 && c <= 25
                    ? LetterValues[c]
                    : 0;
            }
            return value;
        }

        private string ReplaceFirst(string text, char search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
                return text;
            return text.Substring(0, pos) + replace + text.Substring(pos + 1);
        }
    }
}
