using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using Castle.Services.Logging.NLogIntegration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Shouldly;
using Xunit;

namespace nlog.Extensions.Tests
{
    public class RequestResponseLoggingInterceptorTests
    {
        private readonly WindsorContainer _container;

        public RequestResponseLoggingInterceptorTests()
        {
            _container = new WindsorContainer();
            _container.Register(Component.For<ILogContextResolver>().ImplementedBy<TestLogContextResolver>());
            _container.Register(Component.For<ILoggerFactory>().ImplementedBy<NLogFactory>());
            _container.Register(Component.For<RequestResponseLoggingInterceptor>());
            _container.Register(Component.For<A>().Interceptors<RequestResponseLoggingInterceptor>());
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(_container));
        }

        [Fact]
        public void Test()
        {
            var a = _container.Resolve<A>();
            a.C(new D {E = "e"});
            var target = (MemoryTarget)((AsyncTargetWrapper)LogManager.Configuration.FindTargetByName("memory")).WrappedTarget;
            target.Logs.Count.ShouldBe(3);
        }
    }
}
