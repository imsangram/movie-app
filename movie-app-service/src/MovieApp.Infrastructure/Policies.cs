using Polly;
using Polly.Extensions.Http;

namespace MovieApp.Infrastructure
{
    internal class Policies
    {
        public static readonly IAsyncPolicy<HttpResponseMessage> RetryPolicy =
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => (int)msg.StatusCode == 429)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
