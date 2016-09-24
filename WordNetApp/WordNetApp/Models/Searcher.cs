using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Highlight;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using WordNetApp.Interface;

namespace WordNetApp.Models
{
    public class Searcher
    {
        public static List<IndexEntry> Search(string queryString)
        {
            ISynonymEngine synonymEngine = new WordNetSynonymEngine(@"C:\Users\jwang\Source\Repos\WordNetApp\WordNetApp\WordNetApp\App_Data\syn_index");
            var analyzer = new SynonymAnalyzer(synonymEngine);

            var indexDir = FSDirectory.Open(new DirectoryInfo(@"C:\Users\jwang\Source\Repos\WordNetApp\WordNetApp\WordNetApp\App_Data\syn_index"));
            var searcher = new IndexSearcher(indexDir, true);

            var parser = new QueryParser(Version.LUCENE_29, "content", analyzer);

            var query = parser.Parse(queryString);

            var hits = searcher.Search(query,Sort.RELEVANCE);
            var entries = new List<IndexEntry>();

            var simpleHtmlFormatter = new SimpleHTMLFormatter("<span style='background-color:#23dc23;color:white'>", "</span>");
            var highlighter = new Highlighter(simpleHtmlFormatter, new QueryScorer(query));

            highlighter.SetTextFragmenter(new SimpleFragmenter(256));
            highlighter.SetMaxDocBytesToAnalyze(int.MaxValue);

            var standAnalyzer = new StandardAnalyzer();

            for (var i = 0; i < hits.Length(); i++)
            {
                var doc = hits.Doc(i);
                var fragment = highlighter.GetBestFragment(standAnalyzer, "content", doc.Get("content"));

                var entry = new IndexEntry(doc.Get("file"), fragment);
                entries.Add(entry);
            }

            return entries;
        }
    }
}
