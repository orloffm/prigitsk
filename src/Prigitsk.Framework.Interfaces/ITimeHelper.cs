using System;

namespace Prigitsk.Framework
{
    public interface ITimeHelper
    {
        DateTime UnixTimeStampToDateTime(double unixTimeStamp);
    }
}