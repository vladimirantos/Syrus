using Newtonsoft.Json.Linq;
using Syrus.Plugin;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Syrus.Plugins.Files
{
    public class Main : BasePlugin
    {
        private PluginContext _context;
        public override void OnInitialize(PluginContext context)
        {
            _context = context;
            var  a = ((JArray)context.Metadata.Constants["indexedFolders"]).Children();
            foreach(var x in a)
            {
                var folder = x["folder"].ToString();
                var excluded = x["exclude"].ToString();
            }
            var xa = FileSearcher.GetFiles(@"C:\Users\vladi\OneDrive\Plocha\test\", new List<string>() {
                @"^*\.xlsx$", @"C:\Users\vladi\OneDrive\Plocha\test\abc.xlsx"
            }) ;
            //var f = new FileInfo(@"C:\Users\vladi\OneDrive\Plocha\test\New Text Document.txt");
            
        }

        public async override Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            if(_context.Metadata.FromKeyword.Id == 2)
            {

            }
            return Empty;
        }
    }
}
