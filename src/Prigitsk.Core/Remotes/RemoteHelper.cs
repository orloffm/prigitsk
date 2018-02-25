using System;
using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.RepoData;
using Prigitsk.Core.Tree;

namespace Prigitsk.Core.Remotes
{
    public class RemoteHelper
        : IRemoteHelper
    {
      

        public IRemote PickRemote(IRepositoryData repository, string remoteToUse)
        {
            // We need to return something, so this check is mandatory.
            if (repository.Remotes.Count == 0)
            {
                throw new InvalidOperationException("The repository does not contain remotes.");
            }

            if (string.IsNullOrWhiteSpace(remoteToUse))
            {
                // Use default.
                IRemote originRemote = repository.Remotes.GetRemoteByName("origin");

                if (originRemote != null)
                {
                    return originRemote;
                }

                // Do we have multiple?
                if (repository.Remotes.Count > 1)
                {
                    throw new InvalidOperationException(
                        "The repository contains multiple remotes, cannot pick default.");
                }

                return repository.Remotes.Single();
            }

            // Find matching remote by name.
            IRemote matching = repository.Remotes.GetRemoteByName(remoteToUse);
            if (matching == null)
            {
                throw new InvalidOperationException(
                    $"The repository does not contain a remote named \"{remoteToUse}\".");
            }

            return matching;
        }
    }
}