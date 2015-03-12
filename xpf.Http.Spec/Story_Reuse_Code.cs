using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;

namespace xpf.Http.Spec
{
    /// <Story>
    /// As a mobile developer 
    /// I want to be able to reuse as much code between platforms
    /// So that I can deliver faster with less bugs
    /// </Story>
    [Story(@"
        As a mobile developer 
        I want to be able to reuse as much code between platforms
        So that I can deliver faster with less bugs")]
    public partial class Reuse_Code
    {
        It story = () => { };

        public static class Features
        {
            public const string Accessing_Http_Content = "Accessing Http Content";
            public const string Accessing_Http_Content2 = "Some other feature";
        }
    }

}
