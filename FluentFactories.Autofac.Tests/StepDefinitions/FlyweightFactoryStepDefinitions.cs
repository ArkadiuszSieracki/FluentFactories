using Autofac;
using System;
using TechTalk.SpecFlow;

namespace FluentFactories.Autofac.Tests.StepDefinitions
{
    [Binding]
    public class FlyweightFactoryStepDefinitions
    {
        ContainerBuilder _containerBuilder;
        [Given(@"Container builder")]
        public void GivenContainerBuilder()
        {
            _containerBuilder = new ContainerBuilder();
            _containerBuilder.RegisterType<BrushFactory>().SingleInstance();
        }
        [When(@"registering Flyweight circle with shared resources")]
        public void WhenRegisteringFlyweightCircleWithSharedResources()
        {
            _containerBuilder.RegisterFlyWeight<Circle>().WithShared<BrushFactory>();
        }

        [When(@"registering Other flyweight square with other shared resources")]
        public void WhenRegisteringOtherFlyweightSquareWithOtherSharedResources()
        {
            _containerBuilder.RegisterFlyWeight<Rectangle>().WithShared<BrushFactory>();
        }

     


        [Then(@"It is possible to render both shapes")]
        public void ThenBothAreWorkingAsExpected()
        {
            IContainer container = _containerBuilder.Build();
            var circleFactory=    container.Resolve<IFlyWeightFactory<Circle>>();
            var rectangleFactory=    container.Resolve<IFlyWeightFactory<Rectangle>>();
            Dictionary < Color, Circle> cc = new Dictionary<Color, Circle>();
            Dictionary < Color, Rectangle> cr = new Dictionary<Color, Rectangle>();
          
            for (int i = 0; i < 20; ++i)
            {
                Color c = GetRandomColor();
                Circle circle = circleFactory.Get(c);
                if (cc.ContainsKey(c) == false)
                {
                    cc.Add(c, circle);
                }
                circle.Equals(cc[c]).Should().BeTrue();
                circle.SetX(GetRandomX());
                circle.SetY(GetRandomY());
                circle.SetRadius(100);
                circle.Draw();
                var rect = rectangleFactory.Get(c);
                if (cr.ContainsKey(c) == false)
                {
                    cr.Add(c, rect);
                }
                rect.Equals(cr[c]).Should().BeTrue();
                rect.SetX(GetRandomX());
                rect.SetY(GetRandomY());
                rect.SetA(100);
                rect.Draw();
            }
        }

        private static Color GetRandomColor()
        {
            string[] colors = { "Red", "Green", "Blue", "White", "Black" };
            Random random = new Random();
            return new Color(colors[random.Next(colors.Length)]);
        }

        private static int GetRandomX()
        {
            Random random = new Random();
            return random.Next(100);
        }

        private static int GetRandomY()
        {
            Random random = new Random();
            return random.Next(100);
        }
    }
}
