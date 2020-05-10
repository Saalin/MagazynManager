using Dapper;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MagazynManager.Infrastructure.Specifications
{
    public class PrzedsiebiorstwoSpecification<T> : Specification<T> where T : IPrzedsiebiorstwo
    {
        private readonly Guid _przedsiebiorstwoId;

        public PrzedsiebiorstwoSpecification(Guid id)
        {
            _przedsiebiorstwoId = id;
        }

        public override IEnumerable<Action<DynamicParameters>> GetDynamicParameters()
        {
            yield return x => x.Add("@PrzedsiebiorstwoId", _przedsiebiorstwoId);
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            return entity => entity.PrzedsiebiorstwoId == _przedsiebiorstwoId;
        }

        public override string ToSql()
        {
            return "PrzedsiebiorstwoId = @PrzedsiebiorstwoId";
        }
    }
}