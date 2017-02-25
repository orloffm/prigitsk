using OrlovMikhail.GitTools.Helpers;

namespace ConGitWriter.Helpers
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
            set { Instance.RepositoryDirectory = value; }
        }

        public string DotExePath
        {
            get { return Instance.DotExePath; }
            set { Instance.DotExePath = value; }
        }

        public string GitExePath
        {
            get { return Instance.GitExePath; }
            set { Instance.GitExePath = value; }
        }

        public string TargetFilePath
        {
            get { return Instance.TargetFilePath; }
            set { Instance.TargetFilePath = value; }
        }

        public string TargetFormat
        {
            get { return Instance.TargetFormat; }
            set { Instance.TargetFormat = value; }
        }
    }
}