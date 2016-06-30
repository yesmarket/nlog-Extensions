using System.Collections.Generic;

namespace nlog.Extensions.Tests.Helpers
{
    public class TestLogContextResolver : ILogContextResolver
    {
        public KeyValuePair<string, string>[] GetContexts()
        {
            return new[] {new KeyValuePair<string, string>("context", "context")};
        }
    }
}