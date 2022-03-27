using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Notes.Api.Middleware
{
    public static class JsonSerializationSettings
    {
        public static JsonSerializerSettings CamelCase()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
                Formatting = Formatting.Indented
            };
        }
    }
}
