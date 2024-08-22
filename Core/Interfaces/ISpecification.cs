using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; } // To use as a where clause
        Expression<Func<T, object>>? OrderBy { get; } // To use as a order by
        Expression<Func<T, object>>? OrderByDesc { get; } // To use as a order by desc
        bool IsDistinct { get; } 
    }

    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>>? Select { get; }
    }
}