namespace FluentFactories.Autofac
{
    public interface IFlyWeightFactory<TKey>
    {
        void Dispose();
        //TKey Get(object key);
        TKey Get<TVaue>(TVaue key);
    }
}