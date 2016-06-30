using System;
using NLog;

namespace nlog.Extensions
{
    public class DefaultLoggerFactory : ILoggerFactory
    {
        public ILogger Create(Type type)
        {
            return LogManager.GetLogger("", type);
        }
    }
}