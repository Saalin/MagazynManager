using System;
using System.Linq;
using System.Linq.Expressions;

namespace MagazynManager.Domain.Specification
{
    public class OrSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public OrSpecification(Specification<T> left, Specification<T> right)
        {
            _right = right;
            _left = left;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = _left.ToExpression();
            Expression<Func<T, bool>> rightExpression = _right.ToExpression();

            BinaryExpression orExpression = Expression.Or(
                leftExpression.Body, rightExpression.Body);

            return Expression.Lambda<Func<T, bool>>(
                orExpression, leftExpression.Parameters.Single());
        }

        public override string ToSql()
        {
            return $"{_left.ToSql()} OR {_right.ToSql()}";
        }
    }
}