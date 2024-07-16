# FluentFactories
Target for the repository is to provide robust IOC factories for various injection frameworks

For autofac use:
```
 container.RegisterChain<ISentenceEnricher>()
     .WithNext<HelloEnricher>()
     .WithNext<SpaceEnricher>()
     .WithNext<WorldEnricher>()
	 .WithNext<ExclaimEnricher>();
 var c= container.Build();
 sentenceEnricher = c.Resolve<ISentenceEnricher>();
```
This will create ordered chain of responsibily. (See tests of project in repo to dig into example

Now it is possible as well to manage lifetime scopes of registerations in factory

```
   container.RegisterChain<ISentenceEnricher>()
       .WithNext<HelloEnricher>().InstancePerDependency()
       .WithNext<SpaceEnricher>().InstancePerLifetieScope()
       .WithNext<WorldEnricher>().SingleInstance()
       .WithNext<ExclaimEnricher>().SingleInstance();
```
Scope of previous element is always affected by scope definition, default scope is instance per dependency