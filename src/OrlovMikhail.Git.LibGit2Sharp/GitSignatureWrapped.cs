using System;
using LibGit2Sharp;

namespace OrlovMikhail.Git.LibGit2Sharp
{
    public class GitSignatureWrapped
        : IGitSignature
    {
        private readonly Signature _signature;

        private GitSignatureWrapped(Signature signature)
        {
            _signature = signature;
        }

        public string Email => _signature.Email;

        public string Name => _signature.Name;

        public DateTimeOffset When => _signature.When;

        public static IGitSignature Create(Signature signature)
        {
            return new GitSignatureWrapped(signature);
        }
    }
}