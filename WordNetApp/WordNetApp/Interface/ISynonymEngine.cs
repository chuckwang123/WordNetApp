using System.Collections.Generic;

namespace WordNetApp.Interface
{
    /// <summary>  
    /// Summary description for ISynonymEngine  
    /// </summary>  
    public interface ISynonymEngine
    {
        IEnumerable<string> GetSynonyms(string word);
    }
}