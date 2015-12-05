using FluentAssertions;
using System.Net;
using LiveSpec.Extensions.MSpec;
using Machine.Specifications;
using xpf.Http.Spec.TestModels;

namespace xpf.Http.Spec
{
    [Specification("Posting data from a URL using the POST verb")]
    public class HttpPostSpec
    {
        public static NancyTesting NancyTesting;

        Establish context = () =>
        {
            NancyTesting = new NancyTesting();
            NancyTesting.Start();
        };

        Cleanup cleanup = () => NancyTesting.Stop();

        [Given(@"The the URL accepts a json document
                '''
                /basic-url-that-accepts-json-document
                '''")]
        public class When_posting_data_to_a_basic_url_the_data_is_received_by_the_endpoint
        {
            static LiveDocScenario Scenario;

            Establish given = () =>
            {
                Scenario = new LiveDocScenario(typeof(When_posting_data_to_a_basic_url_the_data_is_received_by_the_endpoint));
            };

            static HttpResponse<string> Result;
            static string Url;

            Because when = () =>
            {
                Url = Scenario.Given.DocString;
                var http = new Http(NancyTesting.Server.Handler);
                var c = new Customer {Name = "John", Phone = "12424"};

                Result = http.Navigate(Url)
                    .RequestContentType.Json
                    .PostAsync<string>(c).Result;
            };

            It should_have_status_code_of_OK = () => Result.StatusCode.Should().Be(HttpStatusCode.OK);
            It should_not_have_an_error_set = () => Result.Error.Should().BeEmpty();
        }
    }
}
