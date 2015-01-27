using System;

namespace Wordament.Trie
{
    public class TrieNode
    {
        private readonly TrieNode[] _descendants;

        public bool IsTerminal { get; set; }

        public TrieNode()
        {
            IsTerminal = false;
            _descendants = new TrieNode[26];
        }

        public TrieNode AddLetter(char letter)
        {
            int index = GetLetterIndex(letter);
            if (index < 0 || index >= 26)
                return this;
            TrieNode node = FindLetter(letter);
            if (node == null)
            {
                node = new TrieNode();
                _descendants[index] = node;
            }
            return node;
        }

        public TrieNode FindLetter(char letter)
        {
            int index = GetLetterIndex(letter);
            if (index < 0 || index >= 26)
                return null;
            return _descendants[index];
        }

        private static int GetLetterIndex(char letter)
        {
            if (!Char.IsLetter(letter))
                return -1;
            return Char.ToUpper(letter) - 'A';
        }
    }
}
