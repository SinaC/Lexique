using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexique.Models
{
    public class BookwormWord : WordBase
    {
        public char[] ValuableLetters { get; }

        public BookwormWord(string word, char[] valuableLetters) : base(word)
        {
            ValuableLetters = valuableLetters;
        }

        protected override int ComputeValue(string s)
        {
            // Longer word means higher value and letter between parenthese rewards more point
            int value = s.Length;
            foreach (char valuableLetter in ValuableLetters)
                if (s.Contains(valuableLetter))
                    value += 10;
            return value;
        }
    }
}
