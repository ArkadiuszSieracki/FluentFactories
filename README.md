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

##Flyweight
Now provided generic flyweight factory implementation.
Assume that you want to get Circle with shared brush factory instance
Example registeration
```
        _containerBuilder.RegisterType<RenderContext>().SingleInstance();
        _containerBuilder.RegisterFlyWeight<Circle>().WithShared<BrushFactory>();
        _containerBuilder.RegisterFlyWeight<Rectangle>().WithShared<BrushFactory>();
```
Later usage is simple, just inject factory 'IFlyWeightFactory<Circle>' into the constructor of your class:
```
    class Scene
    {
         public Scene(IFlyWeightFactory<Circle> circleFactory)
         ...

         //somewere in the render loop:
         Color c = GetRandomColor();
         Circle circle = circleFactory.Get(c); //<= this will get instance of circle indexedd by the key(c)
         //in this example each indexed circle will get shared version of the resoure
         // each circle will be built in the separate lifetime scope
         circle.SetX(GetRandomX());
         circle.SetY(GetRandomY());
         circle.SetRadius(100);
         circle.Draw();
    }
```