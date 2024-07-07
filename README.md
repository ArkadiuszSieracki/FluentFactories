# FluentFactories
Target for the repository is to provide robust IOC factories for various injection frameworks

For autofac use:
```
 container.RegisterTransientChain<ISentenceEnricher>()
     .WithNext<HelloEnricher>()
     .WithNext<SpaceEnricher>()
     .WithNext<WorldEnricher>()
	 .WithNext<ExclaimEnricher>();
 var c= container.Build();
 sentenceEnricher = c.Resolve<ISentenceEnricher>();
```
This will create ordered chain of responsibily. (See tests of project in repo to dig into example