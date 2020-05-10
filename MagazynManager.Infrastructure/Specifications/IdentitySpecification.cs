using MagazynManager.Domain.Specification;
using System;
using System.Linq.Expressions;

namespace MagazynManager.Infrastructure.Specifications
{
    public class IdentitySpecification<T> : Specification<T>
    {
        public override Expression<Func<T, bool>> ToExpression()
        {
            return _ => true;
        }

        public override string ToSql()
        {
            return "1 = 1";
        }
    }
}