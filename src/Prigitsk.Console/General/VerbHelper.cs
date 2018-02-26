using System;

namespace Prigitsk.Console.General
{
    public static class VerbHelper
    {
        public static Verb FromName(string verbName)
        {
            switch (verbName)
            {
                case VerbConstants.Configure:
                    return Verb.Configure;
                case VerbConstants.Fetch:
                    return Verb.Fetch;
                case VerbConstants.Draw:
                    return Verb.Draw;
                default:
                    throw new NotSupportedException(verbName);
            }
        }
    }
}