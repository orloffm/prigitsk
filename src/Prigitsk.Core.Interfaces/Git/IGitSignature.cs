using System;

namespace Prigitsk.Core.Git
{
    public interface IGitSignature
    {
        string Email { get; }

        string Name { get; }

        DateTimeOffset When { get; }
    }
}