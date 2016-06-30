using Castle.MicroKernel.Registration;
using Castle.Windsor;
using nlog.Extensions.Tests.Helpers;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using Shouldly;
using Xunit;

namespace nlog.Extensions.Tests
{
    public class LogContextInterceptorTests
    {
        private readonly MemoryTarget _memoryTarget;
        private readonly WindsorContainer _container;

        public LogContextInterceptorTests()
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
            _container.Register(Component.For<LogContextInterceptor>());
            _container.Register(Component.For<A>().Interceptors<LogContextInterceptor>());
        }

        [Fact]
        public void Test()
        {
            var a = _container.Resolve<A>();
            a.B();
            var s = _memoryTarget.Logs[0];
            s.ShouldBe("test context");
        }
    }
}
