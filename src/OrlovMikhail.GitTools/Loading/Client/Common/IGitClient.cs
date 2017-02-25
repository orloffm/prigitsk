using System;
using OrlovMikhail.GitTools.Loading.Client.Repository;

namespace OrlovMikhail.GitTools.Loading.Client.Common
{
    public interface IGitClient : IDisposable
    {
        void Init();
        IRepositoryData Load(GitClientLoadingOptions? options = null);
    }
}