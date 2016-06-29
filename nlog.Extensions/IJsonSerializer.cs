using Newtonsoft.Json;

namespace nlog.Extensions
{
    public interface IJsonSerializer
    {
        string SerializeObject(object request, Formatting formatting);
    }
}