namespace Code.Services.QueryBus
{
    public interface IQueryResolver
    {
        TResult Resolve<TQuery,TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}