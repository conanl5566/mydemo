namespace HttpClientSample.Framework
{
    using CorrelationId;
    using Microsoft.Extensions.Options;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class CorrelationIdDelegatingHandler : DelegatingHandler
    {
        private readonly ICorrelationContextAccessor correlationContextAccessor;
        private readonly IOptions<CorrelationIdOptions> options;

        public CorrelationIdDelegatingHandler(
            ICorrelationContextAccessor correlationContextAccessor,
            IOptions<CorrelationIdOptions> options)
        {
            this.correlationContextAccessor = correlationContextAccessor;
            this.options = options;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(this.options.Value.Header))
            {
                request.Headers.Add(this.options.Value.Header, correlationContextAccessor.CorrelationContext.CorrelationId);
            }

            // Else the header has already been added due to a retry.

            return base.SendAsync(request, cancellationToken);
        }
    }
}