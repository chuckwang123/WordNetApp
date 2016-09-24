using System.Collections.Generic;
using System.Web.Http;
using WordNetApp.Models;

namespace WordNetApp.Controllers
{
    [RoutePrefix("Api/word")]
    public class WordController : ApiController
    {
        [Route("")]
        public IEnumerable<IndexEntry> GetSynonym()
        {
            return Searcher.Search("heart");
        }
    }
}
