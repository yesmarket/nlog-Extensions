using NLog;

namespace nlog.Extensions.Tests.Helpers
{
    public class A
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public virtual void B()
        {
            Logger.Debug("test");
        }

        public virtual F C(D d)
        {
            Logger.Debug("test");
            return new F {G = "g" };
        }
    }
}