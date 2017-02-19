using System.Collections.Generic;

namespace OrlovMikhail.GitTools.Loading.Client.Common
{
    public class Commit
    {
        public string Description { get; set; }
        public string Hash { get; set; }
        public IEnumerable<string> ParentHashes { get; set; }
    }
}