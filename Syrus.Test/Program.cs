using Syrus.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Syrus.Test
{
    class Program
    {
        static List<KeyValuePair<string[], IEnumerable<string>>> _items = new List<KeyValuePair<string[], IEnumerable<string>>>();
        static Random random = new Random();
        static void Main(string[] args)
        {
            //string instalationFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Syrus", "plugins");
            //string cacheLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Syrus", "cache");
            //Core.Syrus factory = new Core.Syrus(instalationFolder, cacheLocation);
            //factory.LoadPlugins().Initialize();


            //string x1 = "Whats weather today in Prague?";
            //string x2 = "Whats weather in Moscow?";
            ////string x2 = "What is weather yesterday in Washington";
            //var x1n = x1.Select(x => (int)x).ToArray();
            //var x2n = x2.Select(x => (int)x).ToArray();
            //var treshold = x1.Length / 3;
            //Console.WriteLine(DamerauLevenshteinDistance(x1n, x2n, treshold).ToString());


            //Aho-Corasick algorith
            //IStringSearchAlgorithm searchAlg = new StringSearch();
            //searchAlg.Keywords = new string[5] { "weather", "sunny", "rain", "forecast", "snow"};

            //StringSearchResult[] results = searchAlg.FindAll("Whats weather today?");

            //// Write all results  
            //foreach (StringSearchResult r in results)
            //{
            //    Console.WriteLine("Keyword='{0}', Index={1}", r.Keyword, r.Index);
            //}

            //Console.WriteLine("Write: ");
            //Regex x = new Regex("[whats weather in ]([A - Za - z\\s] *)");
            //string ans;
            //do
            //{
            //    ans = Console.ReadLine();
            //    Console.WriteLine(x.Match(ans).ToString());
            //} while (ans != "yes");
            A a = new A();
            B b = (B)a;
            b.AA = "A";
            b.BB = "B";
            Console.WriteLine(b.AA);
            Console.WriteLine(b.BB);
            Console.WriteLine(a.AA);

            Console.ReadKey();
        }
        public static int DamerauLevenshteinDistance(int[] source, int[] target, int threshold)
        {
            void Swap<T>(ref T arg1, ref T arg2)
            {
                T temp = arg1;
                arg1 = arg2;
                arg2 = temp;
            }


            int length1 = source.Length;
            int length2 = target.Length;

            // Return trivial case - difference in string lengths exceeds threshhold
            if (Math.Abs(length1 - length2) > threshold) { return int.MaxValue; }

            // Ensure arrays [i] / length1 use shorter length 
            if (length1 > length2)
            {
                Swap(ref target, ref source);
                Swap(ref length1, ref length2);
            }

            int maxi = length1;
            int maxj = length2;

            int[] dCurrent = new int[maxi + 1];
            int[] dMinus1 = new int[maxi + 1];
            int[] dMinus2 = new int[maxi + 1];
            int[] dSwap;

            for (int i = 0; i <= maxi; i++) { dCurrent[i] = i; }

            int jm1 = 0, im1 = 0, im2 = -1;

            for (int j = 1; j <= maxj; j++)
            {

                // Rotate
                dSwap = dMinus2;
                dMinus2 = dMinus1;
                dMinus1 = dCurrent;
                dCurrent = dSwap;

                // Initialize
                int minDistance = int.MaxValue;
                dCurrent[0] = j;
                im1 = 0;
                im2 = -1;

                for (int i = 1; i <= maxi; i++)
                {

                    int cost = source[im1] == target[jm1] ? 0 : 1;

                    int del = dCurrent[im1] + 1;
                    int ins = dMinus1[i] + 1;
                    int sub = dMinus1[im1] + cost;

                    //Fastest execution for min value of 3 integers
                    int min = (del > ins) ? (ins > sub ? sub : ins) : (del > sub ? sub : del);

                    if (i > 1 && j > 1 && source[im2] == target[jm1] && source[im1] == target[j - 2])
                        min = Math.Min(min, dMinus2[im2] + cost);

                    dCurrent[i] = min;
                    if (min < minDistance) { minDistance = min; }
                    im1++;
                    im2++;
                }
                jm1++;
                if (minDistance > threshold)
                {
                    return int.MaxValue;
                }
            }

            int result = dCurrent[maxi];
            return (result > threshold) ? int.MaxValue : result;
        }

    }

    class A
    {
        public string AA { get; set; }
    }

    class B : A
    {
        public string BB { get; set; }
    }


    /// <summary>
	/// Interface containing all methods to be implemented
	/// by string search algorithm
	/// </summary>
	public interface IStringSearchAlgorithm
    {
        #region Methods & Properties

        /// <summary>
        /// List of keywords to search for
        /// </summary>
        string[] Keywords { get; set; }


        /// <summary>
        /// Searches passed text and returns all occurrences of any keyword
        /// </summary>
        /// <param name="text">Text to search</param>
        /// <returns>Array of occurrences</returns>
        StringSearchResult[] FindAll(string text);

        /// <summary>
        /// Searches passed text and returns first occurrence of any keyword
        /// </summary>
        /// <param name="text">Text to search</param>
        /// <returns>First occurrence of any keyword (or StringSearchResult.Empty if text doesn't contain any keyword)</returns>
        StringSearchResult FindFirst(string text);

        /// <summary>
        /// Searches passed text and returns true if text contains any keyword
        /// </summary>
        /// <param name="text">Text to search</param>
        /// <returns>True when text contains any keyword</returns>
        bool ContainsAny(string text);

        #endregion
    }

    /// <summary>
    /// Structure containing results of search 
    /// (keyword and position in original text)
    /// </summary>
    public struct StringSearchResult
    {
        #region Members

        private int _index;
        private string _keyword;

        /// <summary>
        /// Initialize string search result
        /// </summary>
        /// <param name="index">Index in text</param>
        /// <param name="keyword">Found keyword</param>
        public StringSearchResult(int index, string keyword)
        {
            _index = index; _keyword = keyword;
        }


        /// <summary>
        /// Returns index of found keyword in original text
        /// </summary>
        public int Index {
            get { return _index; }
        }


        /// <summary>
        /// Returns keyword found by this result
        /// </summary>
        public string Keyword {
            get { return _keyword; }
        }


        /// <summary>
        /// Returns empty search result
        /// </summary>
        public static StringSearchResult Empty {
            get { return new StringSearchResult(-1, ""); }
        }

        #endregion
    }


    /// <summary>
    /// Class for searching string for one or multiple 
    /// keywords using efficient Aho-Corasick search algorithm
    /// </summary>
    public class StringSearch : IStringSearchAlgorithm
    {
        #region Objects

        /// <summary>
        /// Tree node representing character and its 
        /// transition and failure function
        /// </summary>
        class TreeNode
        {
            #region Constructor & Methods

            /// <summary>
            /// Initialize tree node with specified character
            /// </summary>
            /// <param name="parent">Parent node</param>
            /// <param name="c">Character</param>
            public TreeNode(TreeNode parent, char c)
            {
                _char = c; _parent = parent;
                _results = new ArrayList();
                _resultsAr = new string[] { };

                _transitionsAr = new TreeNode[] { };
                _transHash = new Hashtable();
            }


            /// <summary>
            /// Adds pattern ending in this node
            /// </summary>
            /// <param name="result">Pattern</param>
            public void AddResult(string result)
            {
                if (_results.Contains(result)) return;
                _results.Add(result);
                _resultsAr = (string[])_results.ToArray(typeof(string));
            }

            /// <summary>
            /// Adds trabsition node
            /// </summary>
            /// <param name="node">Node</param>
            public void AddTransition(TreeNode node)
            {
                _transHash.Add(node.Char, node);
                TreeNode[] ar = new TreeNode[_transHash.Values.Count];
                _transHash.Values.CopyTo(ar, 0);
                _transitionsAr = ar;
            }


            /// <summary>
            /// Returns transition to specified character (if exists)
            /// </summary>
            /// <param name="c">Character</param>
            /// <returns>Returns TreeNode or null</returns>
            public TreeNode GetTransition(char c)
            {
                return (TreeNode)_transHash[c];
            }


            /// <summary>
            /// Returns true if node contains transition to specified character
            /// </summary>
            /// <param name="c">Character</param>
            /// <returns>True if transition exists</returns>
            public bool ContainsTransition(char c)
            {
                return GetTransition(c) != null;
            }

            #endregion
            #region Properties

            private char _char;
            private TreeNode _parent;
            private TreeNode _failure;
            private ArrayList _results;
            private TreeNode[] _transitionsAr;
            private string[] _resultsAr;
            private Hashtable _transHash;

            /// <summary>
            /// Character
            /// </summary>
            public char Char {
                get { return _char; }
            }


            /// <summary>
            /// Parent tree node
            /// </summary>
            public TreeNode Parent {
                get { return _parent; }
            }


            /// <summary>
            /// Failure function - descendant node
            /// </summary>
            public TreeNode Failure {
                get { return _failure; }
                set { _failure = value; }
            }


            /// <summary>
            /// Transition function - list of descendant nodes
            /// </summary>
            public TreeNode[] Transitions {
                get { return _transitionsAr; }
            }


            /// <summary>
            /// Returns list of patterns ending by this letter
            /// </summary>
            public string[] Results {
                get { return _resultsAr; }
            }

            #endregion
        }

        #endregion
        #region Local fields

        /// <summary>
        /// Root of keyword tree
        /// </summary>
        private TreeNode _root;

        /// <summary>
        /// Keywords to search for
        /// </summary>
        private string[] _keywords;

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize search algorithm (Build keyword tree)
        /// </summary>
        /// <param name="keywords">Keywords to search for</param>
        public StringSearch(string[] keywords)
        {
            Keywords = keywords;
        }


        /// <summary>
        /// Initialize search algorithm with no keywords
        /// (Use Keywords property)
        /// </summary>
        public StringSearch()
        { }

        #endregion
        #region Implementation

        /// <summary>
        /// Build tree from specified keywords
        /// </summary>
        void BuildTree()
        {
            // Build keyword tree and transition function
            _root = new TreeNode(null, ' ');
            foreach (string p in _keywords)
            {
                // add pattern to tree
                TreeNode nd = _root;
                foreach (char c in p)
                {
                    TreeNode ndNew = null;
                    foreach (TreeNode trans in nd.Transitions)
                        if (trans.Char == c) { ndNew = trans; break; }

                    if (ndNew == null)
                    {
                        ndNew = new TreeNode(nd, c);
                        nd.AddTransition(ndNew);
                    }
                    nd = ndNew;
                }
                nd.AddResult(p);
            }

            // Find failure functions
            ArrayList nodes = new ArrayList();
            // level 1 nodes - fail to root node
            foreach (TreeNode nd in _root.Transitions)
            {
                nd.Failure = _root;
                foreach (TreeNode trans in nd.Transitions) nodes.Add(trans);
            }
            // other nodes - using BFS
            while (nodes.Count != 0)
            {
                ArrayList newNodes = new ArrayList();
                foreach (TreeNode nd in nodes)
                {
                    TreeNode r = nd.Parent.Failure;
                    char c = nd.Char;

                    while (r != null && !r.ContainsTransition(c)) r = r.Failure;
                    if (r == null)
                        nd.Failure = _root;
                    else
                    {
                        nd.Failure = r.GetTransition(c);
                        foreach (string result in nd.Failure.Results)
                            nd.AddResult(result);
                    }

                    // add child nodes to BFS list 
                    foreach (TreeNode child in nd.Transitions)
                        newNodes.Add(child);
                }
                nodes = newNodes;
            }
            _root.Failure = _root;
        }


        #endregion
        #region Methods & Properties

        /// <summary>
        /// Keywords to search for (setting this property is slow, because
        /// it requieres rebuilding of keyword tree)
        /// </summary>
        public string[] Keywords {
            get { return _keywords; }
            set {
                _keywords = value;
                BuildTree();
            }
        }


        /// <summary>
        /// Searches passed text and returns all occurrences of any keyword
        /// </summary>
        /// <param name="text">Text to search</param>
        /// <returns>Array of occurrences</returns>
        public StringSearchResult[] FindAll(string text)
        {
            ArrayList ret = new ArrayList();
            TreeNode ptr = _root;
            int index = 0;

            while (index < text.Length)
            {
                TreeNode trans = null;
                while (trans == null)
                {
                    trans = ptr.GetTransition(text[index]);
                    if (ptr == _root) break;
                    if (trans == null) ptr = ptr.Failure;
                }
                if (trans != null) ptr = trans;

                foreach (string found in ptr.Results)
                    ret.Add(new StringSearchResult(index - found.Length + 1, found));
                index++;
            }
            return (StringSearchResult[])ret.ToArray(typeof(StringSearchResult));
        }


        /// <summary>
        /// Searches passed text and returns first occurrence of any keyword
        /// </summary>
        /// <param name="text">Text to search</param>
        /// <returns>First occurrence of any keyword (or StringSearchResult.Empty if text doesn't contain any keyword)</returns>
        public StringSearchResult FindFirst(string text)
        {
            ArrayList ret = new ArrayList();
            TreeNode ptr = _root;
            int index = 0;

            while (index < text.Length)
            {
                TreeNode trans = null;
                while (trans == null)
                {
                    trans = ptr.GetTransition(text[index]);
                    if (ptr == _root) break;
                    if (trans == null) ptr = ptr.Failure;
                }
                if (trans != null) ptr = trans;

                foreach (string found in ptr.Results)
                    return new StringSearchResult(index - found.Length + 1, found);
                index++;
            }
            return StringSearchResult.Empty;
        }


        /// <summary>
        /// Searches passed text and returns true if text contains any keyword
        /// </summary>
        /// <param name="text">Text to search</param>
        /// <returns>True when text contains any keyword</returns>
        public bool ContainsAny(string text)
        {
            TreeNode ptr = _root;
            int index = 0;

            while (index < text.Length)
            {
                TreeNode trans = null;
                while (trans == null)
                {
                    trans = ptr.GetTransition(text[index]);
                    if (ptr == _root) break;
                    if (trans == null) ptr = ptr.Failure;
                }
                if (trans != null) ptr = trans;

                if (ptr.Results.Length > 0) return true;
                index++;
            }
            return false;
        }

        #endregion
    }


    //enum Config { C1, C2, C3, C4 }
    //enum Config2 { C21, C22, C23, C24 }

    //class Configuration
    //{
    //    public Config Const1 { get; set; }
    //    public Config2 Const2 { get; set; }
    //}

    //class A
    //{
    //    public Configuration Config 
    //    {
    //        get => CombineConfigs();
    //    }
    //    public B B { get; set; }

    //    private Configuration CombineConfigs()
    //    {
    //        Config c1 = B?.Config?.Const1
    //     ?? B?.C?.Config?.Const1
    //     ?? B?.C?.D?.Config?.Const1
    //     ?? Test.Config.C1;
    //        Config2 c2 = B.Config != null && B.Config.Const2 != null ? B.Config.Const2 :
    //            B.C.Config != null && B.C.Config.Const2 != null ? B.C.Config.Const2 :
    //            B.C.D.Config != null && B.C.D.Config.Const2 != null ? B.C.D.Config.Const2 :
    //            Test.Config2.C21;
    //        return new Configuration()
    //        {
    //            Const1 = c1,
    //            Const2 = c2
    //        };
    //    }
    //}

    //class B
    //{
    //    public Configuration Config { get; set; }
    //    public C C { get; set; }
    //}

    //class C
    //{
    //    public Configuration Config { get; set; }
    //    public D D { get; set; }
    //}

    //class D
    //{
    //    public Configuration Config { get; set; }
    //}
}
