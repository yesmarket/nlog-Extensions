using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using nlog.Extensions.Tests.Helpers;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using Shouldly;
using Xunit;

namespace nlog.Extensions.Tests
{
    public class RequestResponseLoggingInterceptorTests
    {
        private readonly MemoryTarget _memoryTarget;
        private readonly WindsorContainer _container;

        public RequestResponseLoggingInterceptorTests()
        {
            var config = new LoggingConfiguration();
            _memoryTarget = new MemoryTarget
            {
                Layout = new SimpleLayout("${message} ${mdlc:item=context}")
            };
            config.AddTarget("memory", _memoryTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, _memoryTarget));
            LogManager.Configuration = config;
            
            _container = new WindsorContainer();
            _container.Register(Component.For<ILogContextResolver>().ImplementedBy<TestLogContextResolver>());
            _container.Register(Component.For<RequestResponseLoggingInterceptor>());
            _container.Register(Component.For<A>().Interceptors<RequestResponseLoggingInterceptor>());
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(_container));
        }

        [Fact]
        public void Test()
        {
            var a = _container.Resolve<A>();
            a.C(new D {E = "e"});
            _memoryTarget.Logs.Count.ShouldBe(3);
        }
    }
}
