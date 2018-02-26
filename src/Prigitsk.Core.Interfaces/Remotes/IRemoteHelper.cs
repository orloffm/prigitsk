using Prigitsk.Core.Entities;
using Prigitsk.Core.RepoData;

namespace Prigitsk.Core.Remotes
{
    public interface IRemoteHelper
    {
        IRemote PickRemote(IRepositoryData repository, string remoteToUse);
    }
}