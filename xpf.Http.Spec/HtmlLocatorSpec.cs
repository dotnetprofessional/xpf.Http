using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LiveDoc.Extensions;
using Machine.Specifications;

namespace xpf.Http.Spec
{
    [Specification("Extracting values from HTML")]
    [Given(@"The the following HTML
                '''
                <input id=""abc1"" text=""some text""/>
                <input name=""abc2"" text=""some text""/>
                <input text=""some text"" name=""abc3"" class=""some class"" />
                <input text=""some text"" id=""abc4"" />

                <input name=""abc5"" text=""some text"">hello</input>
                <input class=""abcclss"" name=""abc6"" text=""some text"">hello</input>

                <b><input id=""xxx"" /></b>
                '''")]
    public class HtmlLocatorSpec
    {
        static LiveDocScenario Scenario;

        Establish context = () =>
        {
            Scenario = new LiveDocScenario(typeof(HtmlLocatorSpec));
        };


        public class When_getting_element_abc1_by_name_that_exists_should_return_element
        {

            Establish given = () =>
            {
            };

            static HttpElement Result;
            static string Url;

            Because when = () =>
            {
                string html = Scenario.Given.DocString;
                Result = new HtmlLocator<string>(new HttpResponse<string> {RawContent = html, StatusCode = HttpStatusCode.OK}).Name("abc1");
            };

            It should_not_have_a_null_result = () => Result.Should().NotBeNull();
            It should_have_a_type = () => Result.Type.Should().Be("input");

            It should_have_an_id = () => Result.Id.Should().Be("abc1");
            It should_not_have_a_text_value= () => Result.Text.Should().Be("");
            It should_one_attribute = () => Result.Attributes.Count.Should().Be(1);
        }

        public class When_getting_element_abc2_by_name_that_exists_should_return_element
        {

            Establish given = () =>
            {
            };

            static HttpElement Result;
            static string Url;

            Because when = () =>
            {
                string html = Scenario.Given.DocString;
                Result = new HtmlLocator<string>(new HttpResponse<string> { RawContent = html, StatusCode = HttpStatusCode.OK }).Name("abc2");
            };

            It should_not_have_a_null_result = () => Result.Should().NotBeNull();
            It should_have_a_type = () => Result.Type.Should().Be("input");

            It should_have_an_id = () => Result.Id.Should().Be("abc2");
            It should_not_have_a_text_value = () => Result.Text.Should().Be("");
            It should_one_attribute = () => Result.Attributes.Count.Should().Be(1);
        }

        public class When_getting_element_abc3_by_name_that_exists_should_return_element
        {

            Establish given = () =>
            {
            };

            static HttpElement Result;
            static string Url;

            Because when = () =>
            {
                string html = Scenario.Given.DocString;
                Result = new HtmlLocator<string>(new HttpResponse<string> { RawContent = html, StatusCode = HttpStatusCode.OK }).Name("abc3");
            };

            It should_not_have_a_null_result = () => Result.Should().NotBeNull();
            It should_have_a_type = () => Result.Type.Should().Be("input");

            It should_have_an_id = () => Result.Id.Should().Be("abc3");
            It should_not_have_a_text_value = () => Result.Text.Should().Be("");
            It should_one_attribute = () => Result.Attributes.Count.Should().Be(2);
        }

        public class When_getting_element_abc6_by_name_that_exists_should_return_element
        {

            Establish given = () =>
            {
            };

            static HttpElement Result;
            static string Url;

            Because when = () =>
            {
                string html = Scenario.Given.DocString;
                Result = new HtmlLocator<string>(new HttpResponse<string> { RawContent = html, StatusCode = HttpStatusCode.OK }).Name("abc6");
            };

            It should_not_have_a_null_result = () => Result.Should().NotBeNull();
            It should_have_a_type = () => Result.Type.Should().Be("input");

            It should_have_an_id = () => Result.Id.Should().Be("abc6");
            It should_not_have_a_text_value = () => Result.Text.Should().Be("hello");
            It should_one_attribute = () => Result.Attributes.Count.Should().Be(2);
        }
    }

}
