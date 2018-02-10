using System;

namespace Prigitsk.Core.Git
{
    public interface ISignature
    {
        string Name { get; }

        string Email{ get; }

        DateTimeOffset When { get; }
    }
}