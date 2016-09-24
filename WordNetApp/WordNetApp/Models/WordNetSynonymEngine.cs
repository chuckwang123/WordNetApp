using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using WordNetApp.Interface;
using LuceneDirectory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace WordNetApp.Models
{
    public class WordNetSynonymEngine : ISynonymEngine
    {

        private IndexSearcher searcher;
        private IndexReader _Reader;
        private Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_29);

        public WordNetSynonymEngine(string syn_index_directory)//syn_index_directory
        {

            LuceneDirectory indexDir = FSDirectory.Open(new DirectoryInfo(syn_index_directory));
            searcher = new IndexSearcher(indexDir, true);
            
        }

        public IEnumerable<string> GetSynonyms(string word)
        {
            var parser = new QueryParser(Version.LUCENE_29, "word", analyzer);
            var query = parser.Parse(word);

            var hitsFound = searcher.Search(query);

            //this will contain a list, of lists of words that go together
            var synonyms = new List<string>();

            for (var i = 0; i < hitsFound.Length(); i++)
            {
                Field[] fields = hitsFound.Doc(i).GetFields("syn");
                synonyms.AddRange(fields.Select(field => field.StringValue()));
            }
            return synonyms;
        }
    }
}