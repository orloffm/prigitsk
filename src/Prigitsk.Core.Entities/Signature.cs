using System;

namespace Prigitsk.Core.Entities
{
    public sealed class Signature : ISignature
    {
        public Signature(string name, string email, DateTimeOffset? when)
        {
            Name = name;
            EMail = email;
            When = when;
        }

        public string EMail { get; }

        public string Name { get; }

        public DateTimeOffset? When { get; }
    }
}