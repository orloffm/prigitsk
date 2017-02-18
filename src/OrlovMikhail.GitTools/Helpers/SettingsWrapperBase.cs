using System.Configuration;

namespace OrlovMikhail.GitTools.Helpers
{
    public abstract class SettingsWrapperBase<T>: ISettingsWrapper
        where T: ApplicationSettingsBase
    {
        protected readonly T Instance;

        protected SettingsWrapperBase(T instance)
        {
            Instance = instance;
        }

        public void Save()
        {
            Instance.Save();
        }
    }
}