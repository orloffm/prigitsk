using System;

namespace Prigitsk.Core.Git
{
    public interface ISignature
    {
        string Email { get; }

        string Name { get; }

        DateTimeOffset When { get; }
    }
}