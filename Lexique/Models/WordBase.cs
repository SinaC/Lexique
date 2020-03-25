using System;

namespace Lexique.Models
{
    public abstract class WordBase : IComparable<WordBase>
    {
        public string Word { get; }
        public int Value { get; }

        protected WordBase(string word)
        {
            Word = word;
            Value = ComputeValue(Word);
        }

        protected abstract int ComputeValue(string s);

        public override string ToString()
        {
            return Word + " => " + Value;
        }

        public int CompareTo(WordBase other)
        {
            if (Value < other.Value)
                return +1;
            if (Value > other.Value)
                return -1;
            return 0;
        }
    }
}
