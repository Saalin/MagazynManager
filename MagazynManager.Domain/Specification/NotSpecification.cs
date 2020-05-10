using System;
using System.Linq;
using System.Linq.Expressions;

namespace MagazynManager.Domain.Specification
{
    public class NotSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _spec;

        public NotSpecification(Specification<T> spec)
        {
            _spec = spec;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> expression = _spec.ToExpression();

            UnaryExpression andExpression = Expression.Not(expression);

            return Expression.Lambda<Func<T, bool>>(
                andExpression, expression.Parameters.Single());
        }

        public override string ToSql()
        {
            return $"NOT ({_spec.ToSql()})";
        }
    }
}