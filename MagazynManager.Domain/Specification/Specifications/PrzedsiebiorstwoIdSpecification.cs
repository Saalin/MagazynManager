using Dapper;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Domain.Specification.Technical;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MagazynManager.Domain.Specification.Specifications
{
    public class PrzedsiebiorstwoIdSpecification<T> : Specification<T> where T : IPrzedsiebiorstwo
    {
        private readonly Guid _przedsiebiorstwoId;

        public PrzedsiebiorstwoIdSpecification(Guid id)
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