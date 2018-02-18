namespace Prigitsk.Core.Entities
{
    public class Remote
        : IRemote
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