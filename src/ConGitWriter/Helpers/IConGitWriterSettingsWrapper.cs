using OrlovMikhail.GitTools.Helpers;

namespace ConGitWriter.Helpers
{
    public interface IConGitWriterSettingsWrapper : ISettingsWrapper
    {
        string RepositoryDirectory { get; set; }

        string DotExePath { get; set; }

        string GitExePath { get; set; }

        string TargetFilePath { get; set; }

        string TargetFormat { get; set; }
    }
}