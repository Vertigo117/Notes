using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Notes.Core.Tests.Models;

namespace Notes.Core.Tests.Helpers
{
    internal static class TokenHelper
    {
        private const int HeaderIndex = 0;
        private const int PayloadIndex = 1;
        private const int SignatureIndex = 2;

        public static Header GetHeaderObject(string token)
        {
            string[] parts = token.Split('.');
            string header = Base64UrlEncoder.Decode(parts[HeaderIndex]);
            return JsonConvert.DeserializeObject<Header>(header);
        }

        public static string GetHeader(string token)
        {
            string[] parts = token.Split('.');
            return parts[HeaderIndex];
        }

        public static Payload GetPayloadObject(string token)
        {
            string[] parts = token.Split('.');
            string payload = Base64UrlEncoder.Decode(parts[PayloadIndex]);
            return JsonConvert.DeserializeObject<Payload>(payload);
        }

        public static string GetPayload(string token)
        {
            string[] parts = token.Split('.');
            return parts[PayloadIndex];
        }


        public static string GetSignature(string token)
        {
            string[] parts = token.Split(".");
            return parts[SignatureIndex];
        }
    }
}
