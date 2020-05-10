using Dapper;
using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MagazynManager.Infrastructure.Specifications
{
    public class IdSpecification<T, TKey> : Specification<T> where T : BaseEntity<TKey>
    {
        private readonly TKey _id;

        public IdSpecification(TKey id)
        {
            _id = id;
        }

        public override IEnumerable<Action<DynamicParameters>> GetDynamicParameters()
        {
            yield return x => x.Add("@Id", _id);
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            return entity => entity.Id.Equals(_id);
        }

        public override string ToSql()
        {
            return "Id = @Id";
        }
    }
}