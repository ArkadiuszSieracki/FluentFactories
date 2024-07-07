using System;

namespace FluentFactories.Sut
{
    public class ChainProcessor: ISentenceEnricher
    {
        public ChainProcessor() { }
        public ChainProcessor(ISentenceEnricher chainProcessor)
        {
            NextProcessor = chainProcessor;
        }

        public ISentenceEnricher? NextProcessor { get; }

        public virtual string Enrich()
        {
            return NextProcessor?.Enrich()??string.Empty;
        }
    }

    public class HelloEnricher : ChainProcessor
    {
        public HelloEnricher(ISentenceEnricher chainProcessor) : base(chainProcessor)
        {
        }

        public override string Enrich()
        {
            return "Hello" + base.Enrich();
        }
    }

    public class WorldEnricher : ChainProcessor
    {
        public WorldEnricher(ISentenceEnricher chainProcessor) : base(chainProcessor)
        {
        }

        public override string Enrich()
        {
            return "World" +  base.Enrich();
        }
    }

    public class ExclaimEnricher : ChainProcessor
    {
        public ExclaimEnricher()
        {
            
        }

        public override string Enrich()
        {
            return "!" +  base.Enrich();
        }
    }

    public class SpaceEnricher : ChainProcessor
    {
        public SpaceEnricher(ISentenceEnricher chainProcessor) : base(chainProcessor)
        {
        }

        public override string Enrich()
        {
            return " " + base.Enrich() ;
        }
    }
    public class QuoteEnricher : ChainProcessor
    {
        public QuoteEnricher(ISentenceEnricher chainProcessor) : base(chainProcessor)
        {
        }

        public override string Enrich()
        {
            return "\"" + base.Enrich()+ "\"";
        }
    }
}
