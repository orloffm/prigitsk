using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OrlovMikhail.GitTools.Helpers
{
    public interface ISettingsHelper
    {
        bool UpdateFrom<T>(Dictionary<string, string> argsDic, string repositoryPathArgumentName, T settings, Expression<Func<T, string>> propertyExpression)
            where T : ISettingsWrapper;
    }
}