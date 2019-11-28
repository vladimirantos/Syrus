using Newtonsoft.Json.Linq;
using Syrus.Plugin;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Syrus.Plugins.Files
{
    internal enum ResultSorting
    {
        DirectoriesFirst = 1,
        FilesFirst = 2,
        AlphabetAsc = 3,
        AlphabetDesc = 4
    }

    public class Main : BasePlugin
    {
        private PluginContext _context;
        public override void OnInitialize(PluginContext context)
        {
            _context = context;
            //var  a = ((JArray)context.Metadata.Constants["indexedFolders"]).Children();
            //foreach(var x in a)
            //{
            //    var folder = x["folder"].ToString();
            //    var excluded = x["exclude"].ToString();
                
            //}
            //var xa = FileSearcher.GetFiles(@"C:\Users\vladi\OneDrive\Plocha\test\", new List<string>() {
            //    @"^*\.xlsx$", @"C:\Users\vladi\OneDrive\Plocha\test\abc.xlsx"
            //}) ;
            //var f = new FileInfo(@"C:\Users\vladi\OneDrive\Plocha\test\New Text Document.txt");
            
        }

        public async override Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            if(_context.Metadata.FromKeyword.Id == 2)
            {
                string path = query.Original;
                bool exists = FilesManager.Exists(path);
                if (!exists)
                    path = FilesManager.GetDir(query.Original);
                
                IEnumerable<Directory> directories = FilesManager.GetDirectories(path);
                IEnumerable<File> files = FilesManager.GetFiles(path);

                if (!exists)
                {
                    directories = directories.Where(d => d.FullPath.ToLower().StartsWith(query.Original.ToLower()));
                    files = files.Where(f => f.FullPath.ToLower().StartsWith(query.Original.ToLower()));
                }

                var result = directories.Select(d => (Result)d).ToList();
                result.AddRange(files.Select(f => (Result)f));
                return result;
            }
            return Empty;
        }
    }
}
