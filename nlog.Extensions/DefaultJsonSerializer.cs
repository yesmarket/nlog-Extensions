using Newtonsoft.Json;

namespace nlog.Extensions
{
    public class DefaultJsonSerializer : IJsonSerializer
    {
        public static string SerializeObject(object request, Formatting formatting)
        {
            var serializer = new DefaultJsonSerializer();
            return ((IJsonSerializer)serializer).SerializeObject(request, formatting);
        }

        string IJsonSerializer.SerializeObject(object request, Formatting formatting)
        {
            return JsonConvert.SerializeObject(request, formatting);
        }
    }
}