using Newtonsoft.Json;

namespace nlog.Extensions
{
    public interface IJsonSerializer<in T> : IJsonSerializer
    {
        string SerializeObject(T request, Formatting formatting);
    }
}
