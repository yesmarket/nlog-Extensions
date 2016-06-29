using System.Threading;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using Xunit;
using Shouldly;

namespace nlog.Extensions.Tests
{
    public class LogContextTests
    {
        private readonly MemoryTarget _memoryTarget;
        private readonly Logger _logger;

        public LogContextTests()
        {
            var config = new LoggingConfiguration();
            _memoryTarget = new MemoryTarget
            {
                Layout = new SimpleLayout("${message} ${mdlc:item=context1} ${mdlc:item=context2}")
            };
            config.AddTarget("memory", _memoryTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, _memoryTarget));
            LogManager.Configuration = config;

            _logger = LogManager.GetCurrentClassLogger();
        }

        [Fact]
        public async Task LogContextAsyncReentrancy()
        {
            using (new LogContext().With("context1", "x"))
            {
                _logger.Debug("a");
                await Task.Delay(1);
                _logger.Debug("b");
            }

            var logs = _memoryTarget.Logs;
            logs[0].ShouldBe("a x ");
            logs[1].ShouldBe("b x ");
        }

        [Fact]
        public async Task LogContextNestedAsyncReentrancy()
        {
            using (new LogContext().With("context1", "x"))
            {
                _logger.Debug("a");
                await Task.Run(async () =>
                {
                    using (new LogContext().With("context2", "y"))
                    {
                        _logger.Debug("c");
                        await Task.Delay(1);
                        _logger.Debug("d");
                    }
                });
                _logger.Debug("b");
            }

            var logs = _memoryTarget.Logs;
            logs[0].ShouldBe("a x ");
            logs[1].ShouldBe("c x y");
            logs[2].ShouldBe("d x y");
            logs[3].ShouldBe("b x ");
        }

        [Fact]
        public void LogContextMultiThreaded()
        {
            var autoResetEvent1 = new AutoResetEvent(false);
            var autoResetEvent2 = new AutoResetEvent(false);

            var thread1 =
                new Thread(() =>
                {
                    using (new LogContext().With("context1", "x"))
                    {
                        autoResetEvent2.WaitOne();
                        _logger.Debug("a");
                        autoResetEvent1.Set();
                    }
                });

            var thread2 =
                new Thread(() =>
                {
                    using (new LogContext().With("context2", "y"))
                    {
                        autoResetEvent2.Set();
                        autoResetEvent1.WaitOne();
                        _logger.Debug("b");
                    }
                });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            var logs = _memoryTarget.Logs;
            logs[0].ShouldBe("a x ");
            logs[1].ShouldBe("b  y");
        }
    }
}
