using System;

namespace OrlovMikhail.GitTools.Loading.Client.Common
{
    [Flags]
    public enum GitClientLoadingOptions
    {
        Default = 0,

        /// <summary>
        ///     When specified, loads all commits,
        ///     even orphaned in the non-merged branches.
        /// </summary>
        IncludeAllCommits = 1 << 0
    }
}