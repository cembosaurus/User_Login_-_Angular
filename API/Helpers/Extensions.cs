using Microsoft.AspNetCore.Http;

namespace API.Helpers
{
    public static class Extensions
    {
        
        public static void AddMyCustomError(this HttpResponse response, string message)
        {

            response.Headers.Add("My-Custom-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "My-Custom-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");

        }

    }
}