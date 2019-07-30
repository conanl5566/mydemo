namespace HttpClientSample.Clients
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using HttpClientSample.Models;
    //https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/http-requests?view=aspnetcore-2.2
    public class RocketClient : IRocketClient
    {
        private readonly HttpClient httpClient;

        public RocketClient(HttpClient httpClient) => this.httpClient = httpClient;

        public async Task<TakeoffStatus> GetStatus(bool working)
        {
            ss s = new ss() { RequestId="11111111111111111"};
            this.httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample"); // GitHub requires a user-agent
          //  var response = await this.httpClient.PostAsJsonAsync<ss>(working ? "status-working" : "status-failing",s);

            var response = await this.httpClient.GetAsync(working ? "status-working?RequestId="+22222 : "status-failing");

            response.EnsureSuccessStatusCode();
            var r= await response.Content.ReadAsAsync<TakeoffStatus>();
            r.Status = response.StatusCode.ToString();
            return r;
        }
    }


    public class ss
    {
        public string RequestId { get; set; }

    }


}
