namespace Wordament.Trie
{
    public class Trie
    {
        private readonly TrieNode _root;

        public Trie()
        {
            _root = new TrieNode();
        }

        public TrieNode GetNode(string s)
        {
            TrieNode node = _root;
            foreach (char c in s)
            {
                node = node.FindLetter(c);
                if (node == null)
                    break;
            }
            return node;
        }

        public TrieNode Add(string s)
        {
            TrieNode node = _root;
            foreach (char c in s)
                node = node.AddLetter(c);
            node.IsTerminal = true;
            return node;
        }

        public bool Contains(string s)
        {
            TrieNode node = GetNode(s);
            return node != null && node.IsTerminal;
        }
    }
}
