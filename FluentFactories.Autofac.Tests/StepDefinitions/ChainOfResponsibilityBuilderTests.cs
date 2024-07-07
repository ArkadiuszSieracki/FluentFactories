using  ContainerBuilder =Autofac.ContainerBuilder;
using FluentFactories.Sut;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;
using FluentFactories.Autofac;
using Autofac;

namespace FluentFactories.Autofac.Tests.StepDefinitions
{
    [Binding]
    public class ChainOfResponsibilityBuilderTests
    {
        ISentenceEnricher sentenceEnricher;
        string enrichResult;
        public ChainOfResponsibilityBuilderTests()
        {
            
        }
        [Given(@"Hallo world enrichers manual assemblies")]
        public void GivenHalloWorldEnrichersManualAssemblies()
        {
            //This is a way how it should not be done
            sentenceEnricher = new HelloEnricher(new SpaceEnricher(new WorldEnricher(new ExclaimEnricher())));
        }

        [Given(@"Hallo world enricher with Autofac")]
        public void GivenHalloWorldEnricherWithAutofac()
        {
            ContainerBuilder container = new ContainerBuilder();

            container.RegisterTransientChain<ISentenceEnricher>()
                .WithNext<HelloEnricher>()
                .WithNext<SpaceEnricher>()
                .WithNext<WorldEnricher>().WithNext<ExclaimEnricher>();
            var c= container.Build();
            sentenceEnricher = c.Resolve<ISentenceEnricher>();
        }

        [When(@"Executing enrich method")]
        public void WhenExecutingEnrichMethod()
        {
            enrichResult = sentenceEnricher.Enrich();
        }

        [Then(@"the result should be ""([^""]*)""")]
        public void ThenTheResultShouldBe(string expectedResult)
        {
             enrichResult.Should().Be(expectedResult);
        }
    }
}
