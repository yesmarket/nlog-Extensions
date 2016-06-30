using System;
using System.Linq;
using Castle.DynamicProxy;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using NLog;

namespace nlog.Extensions
{
    public class RequestResponseLoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var logger = LogManager.GetLogger("", invocation.TargetType);

            var args = invocation.Arguments.Select(SerializeObject).ToList();
            if (args.Any() && logger.IsDebugEnabled)
                logger.Debug($"{Environment.NewLine}{invocation.Method.Name} called with:{Environment.NewLine}{string.Join(@",\n", args)}");

            invocation.Proceed();

            var returnValue = SerializeObject(invocation.ReturnValue);
            if (!string.IsNullOrEmpty(returnValue) && logger.IsDebugEnabled)
                logger.Debug($"{Environment.NewLine}{invocation.Method.Name} returned:{Environment.NewLine}{returnValue}");
        }

        private static string SerializeObject(object value)
        {
            if (value == null) return null;
            var type = typeof(IJsonSerializer<>).MakeGenericType(value.GetType());
            var jsonSerializer = ServiceLocator.Current?.GetAllInstances(type).FirstOrDefault() as IJsonSerializer;
            return jsonSerializer == null
                ? DefaultJsonSerializer.SerializeObject(value, Formatting.Indented)
                : jsonSerializer.SerializeObject(value, Formatting.Indented);
        }
    }
}