using System;
using LibGit2Sharp;

namespace Prigitsk.Core.Git.LibGit2Sharp
{
    public class SignatureWrapped
        : ISignature
    {
        private readonly Signature _signature;

        private SignatureWrapped(Signature signature)
        {
            _signature = signature;
        }

        public string Email => _signature.Email;

        public string Name => _signature.Name;

        public DateTimeOffset When => _signature.When;

        public static ISignature Create(Signature signature)
        {
            return new SignatureWrapped(signature);
        }
    }
}