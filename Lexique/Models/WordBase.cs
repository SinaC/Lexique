using System;

namespace Lexique.Models
{
    public abstract class WordBase : IComparable<WordBase>
    {
        public string Word { get; }

        private int? _value;
        public int Value
        {
            get
            {
                _value = _value ?? ComputeValue(Word);
                return _value.Value;
            }
        }

        protected WordBase(string word)
        {
            Word = word;
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
