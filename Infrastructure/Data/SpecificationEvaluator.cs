using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery,
                              ISpecification<T> spec)
        {
            var query = inputQuery;
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null) {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDesc != null) {
                query = query.OrderByDescending(spec.OrderByDesc);
            }      
            if (spec.IsDistinct) {
                query = query.Distinct();
            }        
            return query;
        }

        public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> inputQuery,
                              ISpecification<T, TResult> spec)
        {
            var query = inputQuery;
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null) {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDesc != null) {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            var selectQuery = query as IQueryable<TResult>;

            if (spec.Select != null) {
                selectQuery = query.Select(spec.Select);
            }     

            if (spec.IsDistinct) {
                selectQuery = selectQuery?.Distinct();
            }  

            return selectQuery ?? query.Cast<TResult>(); // ?? means "if null then" 
        }
    }
}