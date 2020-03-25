using System;

namespace Lexique.Models
{
    public class BuzzWord : WordBase
    {
        //                                             A,  B,  C,  D, F,  F,  G,  H,  I,  J,  K,  L,  M,  N,  O,  P,  Q,  R, S,  T,  U,  V,  W,  W,  Y,  Z
        private static readonly int[] LetterValues = { 5, 30, 20, 20, 5, 30, 30, 30, 10, 50, 50, 20, 20, 10, 20, 30, 40, 10, 5, 20, 20, 30, 50, 40, 40, 30 };
        //                                                  1   2   3   4   5    6    7    8    9   10   11   12   13   14   15   16
        private static readonly int[] WordLengthValues = { 15, 20, 30, 50, 75, 105, 140, 180, 225, 275, 330, 390, 455, 525, 600, 680 };

        public BuzzWord(string word) : base(word)
        {
        }

        public override string ToString()
        {
            return $"{Word} => {Value}";
        }

        protected override int ComputeValue(string s)
        {
            int value = s.Length > WordLengthValues.Length ? 1000 : WordLengthValues[s.Length - 1];
            for (int i = 0; i < s.Length; i++)
            {
                int c = Convert.ToInt32(s[i]) - Convert.ToInt32('a');
                value += (c >= 0 && c <= 25) ? LetterValues[c] : 0;
            }
            return value;
        }
    }
}
