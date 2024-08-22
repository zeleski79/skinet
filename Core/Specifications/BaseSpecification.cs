using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specifications
{
    public class BaseSpecification<T>(Expression <Func<T, bool>>? criteria) : ISpecification<T>
    {
        protected BaseSpecification() : this(null) {}

        // Var Criteria ot type Expression that will contain our where clause expression
        public Expression<Func<T, bool>>? Criteria => criteria;
        // Var orderBy of type Expression will contain our order by expression
        public Expression<Func<T, object>>? OrderBy {get; private set;}
        // Var orderBy of type Expression will contain our order by desc expression
        public Expression<Func<T, object>>? OrderByDesc {get; private set;}
        // Var specifying if a distinct need to be performed
        public bool IsDistinct {get; private set;}

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression) {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression) {
            OrderByDesc = orderByDescExpression;
        }

        protected void ApplyDistinct() {
            IsDistinct = true; // IsDinstinct will be false by default but will be set to true if this method is called
        }
    }

    public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria)
        : BaseSpecification<T>(criteria), ISpecification<T, TResult> {
            
        protected BaseSpecification() : this(null) {}

        public Expression<Func<T, TResult>>? Select {get; private set;}

        protected void AddSelect(Expression<Func<T, TResult>> selectExpression) {
            Select = selectExpression;
        }
    }
}