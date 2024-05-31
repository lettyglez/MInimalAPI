using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Xunit;

namespace books_APITestsMinimalAPI.Repository
{
    [TestClass()]
    public class TestContainersRuns : IAsyncLifetime
    {
        private readonly IContainer container;
        public TestContainersRuns()
        {
            container = new ContainerBuilder()
                
                .WithImage("booksapi")
                .WithPortBinding(8080, true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080)))
                .Build();
        }

        [Fact]
        public async Task Can_Call_Endpoint()
        {
            var httpClient = new HttpClient();
            var requestUri =
                new UriBuilder(
                    Uri.UriSchemeHttp,
                    container.Hostname,
                    container.GetMappedPublicPort(8080),
                    "uuid"
                ).Uri;
            var guid = await httpClient.GetStringAsync(requestUri);
            Xunit.Assert.True(Guid.TryParse(guid, out _));
        }
        public Task InitializeAsync()
            => container.StartAsync();
        public Task DisposeAsync()
            => container.DisposeAsync().AsTask();
    }
}
