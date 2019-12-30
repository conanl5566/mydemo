namespace HttpClientSample.Clients
{
    using HttpClientSample.Models;
    using System.Threading.Tasks;

    public interface IRocketClient
    {
        Task<TakeoffStatus> GetStatus(bool working);
    }
}