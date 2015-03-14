using FluentAssertions;
using LiveDoc.Extensions;
using Machine.Specifications;
using xpf.Http.Original;

namespace xpf.Http.Extensions.Spec
{
    [Specification("Get the classification detail of a web site")]
    public class ShrinkTheWebSpec
    {

        [Given(@"The the URL below which is a social media site
                '''
                http://twitter.com/
                '''")]
        public class When_requesting_its_Thumbnail_url
        {
            static LiveDocScenario Scenario;

            Establish given = () =>
            {
                Scenario = new LiveDocScenario(typeof(When_requesting_its_Thumbnail_url));
            };

            static string Result;
            static string Url;

            Because when = () =>
            {
                Url = Scenario.Given.DocString;
                var http = new Http();
                Result = http.Navigate(Url).GetThumbnailUrl(ThumbnailSize.Micro75x56);
            };

            It should_have_a_valid_result = () => Result.Should().NotBeNull();
            It should_have_a_valid_url = () => Result.Should().Be("http://images.shrinktheweb.com/xino.php?stwembed=1&stwaccesskeyid=3ab2d75e4b4621d&stwhash=22817fa5af&stwinside=1&stwsize=mcr&stwurl=" + Url);
        }
    }

}
