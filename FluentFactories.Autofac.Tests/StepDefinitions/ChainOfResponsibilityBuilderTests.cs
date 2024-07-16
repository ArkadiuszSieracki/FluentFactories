using ContainerBuilder = Autofac.ContainerBuilder;
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
        private IInstanceCounter instanceCounter;
        string enrichResult;
        public ChainOfResponsibilityBuilderTests()
        {

        }
        [Given(@"Hallo world enrichers manual assemblies")]
        public void GivenHalloWorldEnrichersManualAssemblies()
        {
            var instanceCouner = new InstanceCounter();
            //This is a way how it should not be done
            sentenceEnricher = new HelloEnricher(new SpaceEnricher(new WorldEnricher(new ExclaimEnricher(instanceCouner), instanceCouner), instanceCouner), instanceCouner);
        }

        [Given(@"Hallo world enricher with Autofac")]
        public void GivenHalloWorldEnricherWithAutofac()
        {
            ContainerBuilder container = new ContainerBuilder();
            container.RegisterType<InstanceCounter>().As<IInstanceCounter>();
            container.RegisterChain<ISentenceEnricher>()
                .WithNext<HelloEnricher>()
                .WithNext<SpaceEnricher>()
                .WithNext<WorldEnricher>().WithNext<ExclaimEnricher>();
            var c = container.Build();
            sentenceEnricher = c.Resolve<ISentenceEnricher>();
        }


        [Given(@"Hallo world enricher with Autofac and custom lifetime scopes")]
        public void GivenHalloWorldEnricherWithAutofacAndCustomLifetimeScopes()
        {
            ContainerBuilder container = new ContainerBuilder();
            container.RegisterType<InstanceCounter>().As<IInstanceCounter>().SingleInstance();
            container.RegisterChain<ISentenceEnricher>()
                .WithNext<HelloEnricher>().InstancePerDependency()
                .WithNext<SpaceEnricher>().InstancePerLifetieScope()
                .WithNext<WorldEnricher>().SingleInstance()
                .WithNext<ExclaimEnricher>().SingleInstance();

            var c = container.Build();
            var ls1 = c.BeginLifetimeScope();
            var ls2 = c.BeginLifetimeScope();
            sentenceEnricher = ls1.Resolve<ISentenceEnricher>();
            sentenceEnricher = ls2.Resolve<ISentenceEnricher>();
            sentenceEnricher = ls2.Resolve<ISentenceEnricher>();
            instanceCounter = ls1.Resolve<IInstanceCounter>();
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

        [Then(@"Scope of registerations should be respected")]
        public void ThenScopeOfRegisterationsShouldBeRespected()
        {
            instanceCounter.GetCount(typeof(HelloEnricher)).Should().Be(3);
            instanceCounter.GetCount(typeof(SpaceEnricher)).Should().Be(2);
            instanceCounter.GetCount(typeof(WorldEnricher)).Should().Be(1);
            instanceCounter.GetCount(typeof(ExclaimEnricher)).Should().Be(1);
        }

    }
}
