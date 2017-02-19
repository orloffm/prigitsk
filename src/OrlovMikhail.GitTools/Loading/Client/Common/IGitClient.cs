using System;
using System.Collections.Generic;

namespace OrlovMikhail.GitTools.Loading.Client.Common
{
    public interface IGitClient : IDisposable
    {
        void Initialise();
        IEnumerable<Commit> Commits { get; }
        IEnumerable<Branch> Branches { get; }
        IEnumerable<Tag> Tags { get; }
    }
}