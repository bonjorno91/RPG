namespace Code.Services.QueryBus
{
    public interface IQueryBus : IQueryResolver
    {
        void Bind<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler) where TQuery : IQuery<TResult>;
        void Unbind<TQuery, TResult>() where TQuery : IQuery<TResult>;
        void Unbind<TQuery, TResult>(IQueryHandler<TQuery,TResult> handler) where TQuery : IQuery<TResult>;
    }
}