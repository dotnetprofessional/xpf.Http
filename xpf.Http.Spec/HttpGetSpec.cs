using System;
using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using LiveDoc.Extensions;
using Machine.Specifications;
using Nancy.Routing.Constraints;
using xpf.Http.Spec.TestModels;

namespace xpf.Http.Spec
{
    [Specification("Retrieving data from a URL using the GET verb")]
    public class HttpGetSpec
    {
        public static NancyTesting NancyTesting;

        Establish context = () =>
        {
            NancyTesting = new NancyTesting();
            NancyTesting.Start();
        };
        Cleanup cleanup = () => NancyTesting.Stop();

        [Given(@"The the URL below which is valid
                '''
                /basic-url-that-returns-a-simple-string
                '''")]
        public class When_requesting_a_basic_url_that_returns_a_simple_string
        {
            static LiveDocScenario Scenario;
            Establish given = () =>
            {
                Scenario = new LiveDocScenario(typeof(When_requesting_a_basic_url_that_returns_a_simple_string));
            };

            static HttpResponse<string> Result;
            static string Url;

            Because when = () =>
            {
                Url = Scenario.Given.DocString;
                var http = new Http(NancyTesting.Server.Handler);
                Result = http.Url(Url).GetAsync<string>().Result;
            };

            It should_have_status_code_of_OK = () => Result.StatusCode.Should().Be(HttpStatusCode.OK);
            It should_not_have_an_error_set = () => Result.Error.Should().BeEmpty();
            It should_return_the_contents_of_the_Url = () => Result.Content.Should().Be("this is simple text");
            It should_not_have_any_headers = () => Result.Headers.Count.Should().Be(0);
            It should_have_the_correct_Url_in_response = () => Result.Url.Should().BeEquivalentTo(Url);

            It should_have_a_valid_details = () => Result.Detail.Should().NotBeNull();
            It should_have_correct_Url_in_detail = () => Result.Detail.Url.Should().BeEquivalentTo(Url);
            It should_have_no_title_in_detail = () => Result.Detail.Title.Should().BeEmpty();
            It should_have_no_description_in_detail = () => Result.Detail.Description.Should().BeEmpty();
            It should_have_no_keywords_in_detail = () => Result.Detail.Keywords.Should().BeEmpty();
            It should_not_support_flash_in_detail = () => Result.Detail.SupportsFlash.Should().BeFalse();
        }


        [Given(@"The the URL below which is valid
                '''
                /basic-url-that-returns-a-simple-json-document
                '''")]
        public class When_requesting_a_basic_url_that_returns_a_simple_json_document
        {
            static LiveDocScenario Scenario;
            Establish given = () =>
            {
                Scenario = new LiveDocScenario(typeof(When_requesting_a_basic_url_that_returns_a_simple_json_document));
            };

            static HttpResponse<Customer> Result;
            static string Url;

            Because when = () =>
            {
                Url = Scenario.Given.DocString;
                var http = new Http(NancyTesting.Server.Handler);
                Result = http.Url(Url).ResponseContentType.Json.GetAsync<Customer>().Result;
            };

            It should_deserialize_into_class_instance = () => Result.Should().NotBeNull();
            It should_have_correct_values_set_for_instance = () =>
            {
                Result.Content.Name.Should().Be("Test");
                Result.Content.Phone.Should().Be("12345678");
            };
        }

        [Given(@"The the URL below which is valid
                '''
                /basic-url-that-returns-a-simple-xml-document
                '''")]
        public class When_requesting_a_basic_url_that_returns_a_simple_xml_document
        {
            static LiveDocScenario Scenario;
            Establish given = () =>
            {
                Scenario = new LiveDocScenario(typeof(When_requesting_a_basic_url_that_returns_a_simple_xml_document));
            };

            static HttpResponse<Customer> Result;
            static string Url;

            Because when = () =>
            {
                Url = Scenario.Given.DocString;
                var http = new Http(NancyTesting.Server.Handler);
                Result = http.Url(Url).ResponseContentType.Xml.GetAsync<Customer>().Result;
            };

            It should_deserialize_into_class_instance = () => Result.Should().NotBeNull();
            It should_have_correct_values_set_for_instance = () =>
            {
                Result.Content.Name.Should().Be("Test");
                Result.Content.Phone.Should().Be("12345678");
            };
        }

        [Given(@"The the URL below which is valid
                '''
                /basic-url-that-returns-a-simple-base64-document
                '''")]
        public class When_requesting_a_basic_url_that_returns_a_simple_base64_document
        {
            static LiveDocScenario Scenario;

            Establish given = () =>
            {
                Scenario = new LiveDocScenario(typeof (When_requesting_a_basic_url_that_returns_a_simple_base64_document));
            };

            static HttpResponse<string> Result;
            static string Url;

            Because when = () =>
            {
                Url = Scenario.Given.DocString;
                var http = new Http(NancyTesting.Server.Handler);
                Result = http.Url(Url).Encoding.Base64.GetAsync<string>().Result;
            };

            It should_deserialize_into_class_instance = () => Result.Should().NotBeNull();

            It should_have_correct_values_set_for_instance = () => Result.Content.Should().Be("base64 Text");

            It should_have_a_base64_encoded_raw_content = () => Result.RawContent.Should().EndWith("==");
        }

        [Given(@"The the URL below which is valid
                '''
                /basic-url-that-returns-headers-as-keyvalue-pairs
                '''")]
        public class When_requesting_a_basic_url_with_headers
        {
            static LiveDocScenario Scenario;
            Establish given = () =>
            {
                Scenario = new LiveDocScenario(typeof(When_requesting_a_basic_url_with_headers));
            };

            static HttpResponse<List<string>> Result;
            static string Url;

            Because when = () =>
            {
                Url = Scenario.Given.DocString;
                var http = new Http(NancyTesting.Server.Handler);
                Result = http.Url(Url).WithHeader("key1", new []{"value1"}).ResponseContentType.Json.GetAsync<List<string>>().Result;
            };

            It should_have_status_code_of_OK = () => Result.StatusCode.Should().Be(HttpStatusCode.OK);
            It should_include_all_headers = () => Result.Content.Count.Should().Be(4);
            It should_include_additional_header = () => Result.Content.Should().Contain("key1:System.String[]");
        }

        [Given(@"The the URL below which is valid
                '''
                /basic-url-recieves-and-sets-cookies
                '''")]
        public class When_requesting_a_basic_url_with_cookies
        {
            static LiveDocScenario Scenario;
            Establish given = () =>
            {
                Scenario = new LiveDocScenario(typeof(When_requesting_a_basic_url_with_cookies));
            };

            static HttpResponse<string> Result;
            static string Url;

            Because when = () =>
            {
                Url = Scenario.Given.DocString;
                var http = new Http(NancyTesting.Server.Handler);
               Result = http.Url(Url).WithCookie("key1","value1", expiry: DateTime.Parse("1900/1/1")).GetAsync<string>().Result;
            };

            It should_have_status_code_of_OK = () => Result.StatusCode.Should().Be(HttpStatusCode.OK);
            It should_include_all_headers = () => Result.Cookies.Count.Should().Be(4);
            It should_include_additional_header = () => Result.Content.Should().Contain("set cookies");
        }
    }
}
