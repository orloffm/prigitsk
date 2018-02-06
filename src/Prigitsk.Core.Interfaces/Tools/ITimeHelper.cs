using System;

namespace Prigitsk.Core.Tools
{
    public interface ITimeHelper
    {
        DateTime UnixTimeStampToDateTime(double unixTimeStamp);
    }
}