using System;

namespace Prigitsk.Core.Entities
{
    public interface ISignature
    {
        string EMail { get; }

        string Name { get; }

        DateTimeOffset When { get; }
    }
}