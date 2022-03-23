using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace UmniahUsers_UI.Helpers
{
    public static class WebApiHelper
    {
        public static HttpClient webApiClient = new HttpClient();

        static WebApiHelper()
        {
            webApiClient.BaseAddress = new Uri("https://localhost:44334/api/");
            webApiClient.DefaultRequestHeaders.Clear();
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("admin" + ":" + "admin@123"));
            webApiClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Basic", credentials);
            webApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
    }
}
