using OrlovMikhail.GitTools.Helpers;

namespace ConGitWriter
{
    public class ConGitWriterSettingsWrapper : SettingsWrapperBase<Settings>, IConGitWriterSettingsWrapper
    {
        public ConGitWriterSettingsWrapper(Settings instance)
            : base(instance)
        {
        }

        public string RepositoryDirectory
        {
            get { return Instance.RepositoryDirectory; }
            set { RepositoryDirectory = value; }
        }

        public string DotExePath
        {
            get { return Instance.DotExePath; }
            set { DotExePath = value; }
        }

        public string GitExePath
        {
            get { return Instance.GitExePath; }
            set { GitExePath = value; }
        }

        public string TargetFilePath
        {
            get { return Instance.TargetFilePath; }
            set { TargetFilePath = value; }
        }

        public string TargetFormat
        {
            get { return Instance.TargetFormat; }
            set { TargetFormat = value; }
        }
    }
}