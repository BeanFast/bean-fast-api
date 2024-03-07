using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Settings
{
    
    public class FirebaseSettings
    {
        public string StorageBucket { get; set; } = string.Empty;

        public string ProjectId { get; set; } = string.Empty;

        public string BaseUrl { get; set; } = string.Empty;

        public FolderNames FolderNames { get; set; } = default!;

        
    }
    public class FolderNames
    {
        public string Food { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string School { get; set; } = string.Empty;
        public string Kitchen { get; set; } = string.Empty;
        public string CardType { get; set; }
    }

}
