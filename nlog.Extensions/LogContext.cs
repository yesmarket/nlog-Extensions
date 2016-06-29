using System.Collections.Generic;
using NLog;

namespace nlog.Extensions
{
    public class LogContext : ILogContext
    {
        private readonly List<string> _keys = new List<string>();

        public ILogContext With(string key, string value)
        {
            _keys.Add(key);
            MappedDiagnosticsLogicalContext.Set(key, value);
            return this;
        }

        public ILogContext With(params KeyValuePair<string, string>[] contexts)
        {
            if (contexts == null) return this;
            foreach (var context in contexts)
            {
                With(context.Key, context.Value);
            }
            return this;
        }

        public void Dispose()
        {
            _keys.ForEach(MappedDiagnosticsLogicalContext.Remove);
        }
    }
}