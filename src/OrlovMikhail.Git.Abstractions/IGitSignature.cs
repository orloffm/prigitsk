using System;

namespace OrlovMikhail.Git
{
    public interface IGitSignature
    {
        string Email { get; }

        string Name { get; }

        DateTimeOffset When { get; }
    }
}