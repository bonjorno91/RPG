using System;
using System.Collections.Generic;

namespace Code.Services.QueryBus
{
    public sealed class QueryBusService : IQueryBus
    {
        private readonly Dictionary<Type,Dictionary<Type,object>> _queryResultHandlers = new();

        public void Bind<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler) where TQuery : IQuery<TResult>
        {
            if (!_queryResultHandlers.TryGetValue(typeof(TQuery), out var results))
            {
                results = new Dictionary<Type, object>();
                _queryResultHandlers[typeof(TQuery)] = results;
            }

            results[typeof(TResult)] = handler;
        }

        public void Unbind<TQuery, TResult>() where TQuery : IQuery<TResult>
        {
            if(_queryResultHandlers.TryGetValue(typeof(TQuery),out var results)) 
                results[typeof(TResult)] = default;
        }
        
        public void Unbind<TQuery, TResult>(IQueryHandler<TQuery,TResult> handler) where TQuery : IQuery<TResult>
        {
            Unbind<TQuery,TResult>();
        }
        
        public TResult Resolve<TQuery,TResult>(TQuery query) where TQuery :  IQuery<TResult>
        {
            if (_queryResultHandlers.TryGetValue(typeof(TQuery), out var results))
            {
                if (results.TryGetValue(typeof(TResult), out var handler))
                {
                    return ((IQueryHandler<TQuery, TResult>) handler).Handle(query);
                }
            }

            return default;
        }
    }
}