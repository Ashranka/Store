using Store.Controllers;
using Store.RequestHelpers;
using System.Text.Json;

namespace Store.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPagiantionHeader (this HttpResponse response, MetaData metaData)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.Headers.Append("Paginations", JsonSerializer.Serialize(metaData));
            response.Headers.Append("Access-Control-Expose-Headers", "Paginations");
        }
    }
}
