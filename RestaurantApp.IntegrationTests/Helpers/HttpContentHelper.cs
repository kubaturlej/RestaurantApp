using Newtonsoft.Json;
using System.Text;

namespace RestaurantApp.IntegrationTests.Helpers
{
    public static class HttpContentHelper
    {
        public static HttpContent ToJsonHttpContent(this object obj)
        {
            var jsonBody = JsonConvert.SerializeObject(obj);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            return content;
        }
    }
}
