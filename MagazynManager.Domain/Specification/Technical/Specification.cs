using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MagazynManager.Domain.Specification.Technical
{
    /// <summary>
    /// https://enterprisecraftsmanship.com/posts/specification-pattern-c-implementation/
    /// </summary>
    public abstract class Specification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        public bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = ToExpression().Compile();
            return predicate(entity);
        }

        public abstract string ToSql();

        public virtual IEnumerable<Action<DynamicParameters>> GetDynamicParameters()
        {
            return Enumerable.Empty<Action<DynamicParameters>>();
        }

        public Specification<T> And(Specification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public Specification<T> Or(Specification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }
    }
}