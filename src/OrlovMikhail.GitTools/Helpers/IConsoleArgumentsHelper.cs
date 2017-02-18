using System.Collections.Generic;

namespace OrlovMikhail.GitTools.Helpers
{
    public interface IConsoleArgumentsHelper
    {
        Dictionary<string, string> ArgumentsToDictionary(string[] args);
    }
}