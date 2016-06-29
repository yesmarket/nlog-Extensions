using System.Collections.Generic;

namespace nlog.Extensions
{
    public interface ILogContextResolver
    {
        KeyValuePair<string, string>[] GetContexts();
    }
}