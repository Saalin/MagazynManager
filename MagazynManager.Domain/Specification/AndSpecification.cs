using System;
using System.Linq;
using System.Linq.Expressions;

namespace MagazynManager.Domain.Specification
{
    public class AndSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public AndSpecification(Specification<T> left, Specification<T> right)
        {
            _right = right;
            _left = left;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = _left.ToExpression();
            Expression<Func<T, bool>> rightExpression = _right.ToExpression();

            BinaryExpression andExpression = Expression.AndAlso(
                leftExpression.Body, rightExpression.Body);

            return Expression.Lambda<Func<T, bool>>(
                andExpression, leftExpression.Parameters.Single());
        }

        public override string ToSql()
        {
            return $"{_left.ToSql()} AND {_right.ToSql()}";
        }
    }
}