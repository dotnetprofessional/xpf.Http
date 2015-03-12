using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using Owin;

namespace xpf.Http.Spec
{
    public class NancyTesting
    {
        public TestServer Server;

        public class Startup
        {
            public void Configuration(IAppBuilder appBuilder)
            {
                appBuilder.UseNancy();
            }
        }

        public void Start()
        {
            Server = TestServer.Create<Startup>();
        }

        public HttpMessageHandler Handler
        {
            get { return new OwinClientHandler(this.Server.Invoke); }
        }

        // Use ClassCleanup to run code after all tests in a class have run

        public void Stop()
        {
            Server.Dispose();
        }
    }

    //public class TestServerMessageHandler : OwinClientHandler
    //{
    //    public TestServerMessageHandler(Func<IDictionary<string, object>, Task> next) : base(next)
    //    {
    //    }

    //    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //    {
    //        // Setup any cookies that are present
    //        foreach(var c in this.CookieContainer.)

    //        return base.SendAsync(request, cancellationToken);
    //    }

    //    public CookieContainer CookieContainer { get; set; }
    //}
}