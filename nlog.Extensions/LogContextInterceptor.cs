using System.Linq;
using Castle.DynamicProxy;

namespace nlog.Extensions
{
    public class LogContextInterceptor : IInterceptor
    {
        private readonly ILogContextResolver _logContextResolver;

        public LogContextInterceptor(ILogContextResolver logContextResolver)
        {
            _logContextResolver = logContextResolver;
        }

        public void Intercept(IInvocation invocation)
        {
            var contexts = _logContextResolver.GetContexts();
            if (contexts != null && contexts.Any())
            {
                using (new LogContext().With(contexts))
                {
                    invocation.Proceed();
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}