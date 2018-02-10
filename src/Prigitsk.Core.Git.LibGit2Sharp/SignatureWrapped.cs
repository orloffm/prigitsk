using System;
using LibGit2Sharp;

namespace Prigitsk.Core.Git.LibGit2Sharp
{
    public class SignatureWrapped
        : ISignature
    {
        private readonly Signature _signature;

        public SignatureWrapped(Signature signature)
        {
            _signature = signature;
        }

        public string Name => _signature.Name;
        public string Email => _signature.Email;
        public DateTimeOffset When => _signature.When;
    }
}