using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MagazynManager.Domain.Specification.Technical
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
            var leftExpression = _left.ToExpression();
            var rightExpression = _right.ToExpression();
            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.OrElse(leftExpression.Body, rightExpression.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            return Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
        }

        public override IEnumerable<Action<DynamicParameters>> GetDynamicParameters()
        {
            return _right.GetDynamicParameters().Concat(_left.GetDynamicParameters());
        }

        public override string ToSql()
        {
            return $"({_left.ToSql()} OR {_right.ToSql()})";
        }
    }
}