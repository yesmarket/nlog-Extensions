using System;
using System.Collections.Generic;

namespace nlog.Extensions
{
    public interface ILogContext : IDisposable
    {
        ILogContext With(string key, string value);
        ILogContext With(params KeyValuePair<string, string>[] contexts);
    }
}