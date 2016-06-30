using System;
using NLog;

namespace nlog.Extensions
{
    public interface ILoggerFactory
    {
        ILogger Create(Type type);
    }
}