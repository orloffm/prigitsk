using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace OrlovMikhail.GitTools.Helpers
{
    public class SettingsHelper : ISettingsHelper
    {
        public bool UpdateFrom<T>(Dictionary<string, string> argsDic, string key, T settings,
            Expression<Func<T, string>> propertyExpression) where T : ISettingsWrapper
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

            bool hasValue = !string.IsNullOrWhiteSpace(value);
            return hasValue;
        }
    }
}