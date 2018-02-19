using System.Diagnostics;

namespace Prigitsk.Core.Entities
{
    [DebuggerDisplay("{RemoteName} => {Url}")]
    public class Remote : IRemote
    {
        public Remote(string remoteName, string url)
        {
            RemoteName = remoteName;
            Url = url;
        }

        public string RemoteName { get; }
        public string Url { get; }
    }
}