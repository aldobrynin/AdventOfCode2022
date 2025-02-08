namespace Common;

public class Trie {
    private class TrieNode {
        public Dictionary<char, TrieNode> Children { get; } = new();
        public bool IsEndOfWord { get; set; }
    }

    private readonly TrieNode _root = new();

    private Trie() {
    }
    public static Trie FromWords(IEnumerable<string> words) {
        var trie = new Trie();
        foreach (var word in words) {
            trie.Insert(word);
        }

        return trie;
    }

    public void Insert(string word) {
        var current = _root;
        foreach (var c in word) {
            if (!current.Children.TryGetValue(c, out var node)) {
                node = new TrieNode();
                current.Children[c] = node;
            }

            current = node;
        }

        current.IsEndOfWord = true;
    }

    public bool Contains(ReadOnlySpan<char> word) {
        var current = _root;
        foreach (var c in word) {
            if (!current.Children.TryGetValue(c, out var node)) {
                return false;
            }

            current = node;
        }

        return current.IsEndOfWord;
    }

    public List<string> FindPrefixes(string word) {
        var result = new List<string>();

        var current = _root;
        for (var index = 0; index < word.Length; index++) {
            if (!current.Children.TryGetValue(word[index], out var node)) {
                 break;
            }

            current = node;
            if (current.IsEndOfWord) {
                result.Add(word[..(index + 1)]);
            }
        }

        return result;
    }
}
