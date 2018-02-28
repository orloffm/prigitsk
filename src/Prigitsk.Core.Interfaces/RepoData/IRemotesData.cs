using Prigitsk.Core.Entities;

namespace Prigitsk.Core.RepoData
{
    public interface IRemotesData : IEntityData<IRemote>
    {
        IRemote GetRemoteByName(string name);
    }
}