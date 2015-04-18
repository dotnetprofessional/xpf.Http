using System.IO;
using System.Text;
using FluentAssertions;
using LiveDoc.Extensions;
using Machine.Specifications;

namespace xpf.Http.Spec
{
    [Specification("Support Gzip compression/decompression")]
    public class GzipSpec
    {

        [Given(@"The following text
                '''
                This is a string!
                '''")]
        public class When_compressing_using_gzip
        {
            static LiveDocScenario Scenario;

            Establish given = () =>
            {
                Scenario = new LiveDocScenario(typeof(When_compressing_using_gzip));
            };

            static string Result;

            Because when = () =>
            {
                var value = Scenario.Given.DocString;
                var encoder = new GzipEncoder();
                using (var stream = encoder.Encode(value).Result)
                {
                    stream.Position = 0;
                    var reader = new StreamReader(stream, Encoding.UTF8);

                    Result = reader.ReadToEnd();
                }
            };

            It should_have_compressed_the_value = () =>
            {
                Result.Length.Should().BeGreaterThan(1);
            };
        }

    }
}
