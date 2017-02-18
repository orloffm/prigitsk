using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using log4net;

namespace ConGitWriter
{
    public static class SettingsTools
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SettingsTools));

        public static bool LoadValue<T>(string key, Dictionary<string, string> argsDic, T settings,
            Expression<Func<T, string>> propertyExpression)
        {
            MemberExpression memberExpression = (MemberExpression) propertyExpression.Body;
            PropertyInfo propertyInfo = (PropertyInfo) memberExpression.Member;

            string value;
            argsDic.TryGetValue(key, out value);
            if (string.IsNullOrEmpty(value))
            {
                value = propertyInfo.GetValue(settings) as string;
            }
            else
            {
                propertyInfo.SetValue(settings, value);
            }

            if (string.IsNullOrEmpty(value))
            {
                log.ErrorFormat("No {0} known. Please specify it as /{0}=\"abc\" in the command line.", key);
                return false;
            }

            log.DebugFormat("Using {0}={1}.", key, value);
            return true;
        }
    }
}