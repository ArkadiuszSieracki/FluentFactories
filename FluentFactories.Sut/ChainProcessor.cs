using System;

namespace FluentFactories.Sut
{
    public class ChainProcessor : ISentenceEnricher
    {
        public ChainProcessor(IInstanceCounter instanceCounter)
        {
            instanceCounter.Confirm(this);
        }
        public ChainProcessor(ISentenceEnricher chainProcessor, IInstanceCounter instanceCounter) : this(instanceCounter)
        {
            NextProcessor = chainProcessor;
        }

        public ISentenceEnricher? NextProcessor { get; }

        public virtual string Enrich()
        {
            return NextProcessor?.Enrich() ?? string.Empty;
        }
    }

    public class HelloEnricher : ChainProcessor
    {
        public HelloEnricher(ISentenceEnricher chainProcessor, IInstanceCounter instanceCounter) : base(chainProcessor, instanceCounter)
        {
        }

        public override string Enrich()
        {
            return "Hello" + base.Enrich();
        }
    }

    public class WorldEnricher : ChainProcessor
    {
        public WorldEnricher(ISentenceEnricher chainProcessor, IInstanceCounter instanceCounter) : base(chainProcessor, instanceCounter)
        {
        }

        public override string Enrich()
        {
            return "World" + base.Enrich();
        }
    }

    public class ExclaimEnricher : ChainProcessor
    {
        public ExclaimEnricher(IInstanceCounter instanceCounter):base(instanceCounter)
        {

        }

        public override string Enrich()
        {
            return "!" + base.Enrich();
        }
    }

    public class SpaceEnricher : ChainProcessor
    {
        public SpaceEnricher(ISentenceEnricher chainProcessor, IInstanceCounter instanceCounter) : base(chainProcessor,instanceCounter)
        {
        }

        public override string Enrich()
        {
            return " " + base.Enrich();
        }
    }
    public class QuoteEnricher : ChainProcessor
    {
        public QuoteEnricher(ISentenceEnricher chainProcessor, IInstanceCounter instanceCounter) : base(chainProcessor, instanceCounter)
        {
        }

        public override string Enrich()
        {
            return "\"" + base.Enrich() + "\"";
        }
    }
}
