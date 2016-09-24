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
        public static List<string> Search(string queryString)
        {
            ISynonymEngine synonymEngine = new WordNetSynonymEngine(@"C:\Users\jwang\Source\Repos\WordNetApp\WordNetApp\WordNetApp\App_Data\syn_index");
            var analyzer = new SynonymAnalyzer(synonymEngine);

            var indexDir = FSDirectory.Open(new DirectoryInfo(@"C:\Users\jwang\Source\Repos\WordNetApp\WordNetApp\WordNetApp\App_Data\syn_index"));
            var searcher = new IndexSearcher(indexDir, true);

            var parser = new QueryParser(Version.LUCENE_29, "word", analyzer);

            var query = parser.Parse(queryString);

            var hits = searcher.Search(query);
            var entries = new List<string>();

            for (var i = 0; i < hits.Length(); i++)
            {
                var doc = hits.Doc(i);

                var entry = doc.Get("word");
                entries.Add(entry);
            }

            return entries;
        }
    }
}
