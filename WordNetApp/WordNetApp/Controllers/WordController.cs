using System.Collections.Generic;
using System.Web.Http;
using WordNetApp.Models;

namespace WordNetApp.Controllers
{
    [RoutePrefix("Api/word")]
    public class WordController : ApiController
    {
        [Route("")]
        public IEnumerable<string> GetSynonym(string word)
        {
            return Searcher.Search(word);
        }
    }
}
