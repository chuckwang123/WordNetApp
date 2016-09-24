namespace WordNetApp.Models
{
    public class IndexEntry
    {
        public string file { get; set; }
        public string fragment { get; set; }

        public IndexEntry(string newfile, string newfragment)
        {
            file = newfile;
            fragment = newfragment;
        }
    }
}
