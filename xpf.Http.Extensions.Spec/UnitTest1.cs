﻿using System;
using System.Net;
using FluentAssertions;
using LiveDoc.Extensions;
using Machine.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace xpf.Http.Extensions.Spec
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var http = new Http();
            var result = http.Url("http://twitter.com").GetClassification().Result;
        }
    }

    [Specification("Get the classification detail of a web site")]
    public class WebSiteClassificationSpec
    {

        [Given(@"The the URL below which is a social media site
                '''
                http://twitter.com/
                '''")]
        public class When_requesting_its_classification
        {
            static LiveDocScenario Scenario;

            Establish given = () =>
            {
                Scenario = new LiveDocScenario(typeof(When_requesting_its_classification));
            };

            static WebSiteClassification Result;
            static string Url;

            Because when = () =>
            {
                Url = Scenario.Given.DocString;
                var http = new Http();
                Result = http.Url(Url).GetClassification().Result;
            };

            It should_have_a_valid_result = () => Result.Should().NotBeNull();
            It should_have_a_security_risk_of_safe = () => Result.SecurityRisk.Should().Be("Safe");
            It should_have_two_categories = () => Result.Categories.Count.Should().Be(2);
            It should_have_the_category_social = () => Result.Categories[1].Should().Be("Social");
            It should_have_the_category_blogs= () => Result.Categories[0].Should().Be("Blogs");
        }
    }
}
