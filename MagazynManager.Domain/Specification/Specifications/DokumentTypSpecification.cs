using Dapper;
using MagazynManager.Domain.Entities.Dokumenty;
using MagazynManager.Domain.Specification.Technical;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MagazynManager.Domain.Specification.Specifications
{
    public class DokumentTypSpecification : Specification<Dokument>
    {
        private readonly TypDokumentu _typDokumentu;

        public DokumentTypSpecification(TypDokumentu typ)
        {
            _typDokumentu = typ;
        }

        public override IEnumerable<Action<DynamicParameters>> GetDynamicParameters()
        {
            yield return x => x.Add("@TypDokumentu", (int)_typDokumentu);
        }

        public override Expression<Func<Dokument, bool>> ToExpression()
        {
            return entity => entity.TypDokumentu == _typDokumentu;
        }

        public override string ToSql()
        {
            return "TypDokumentu = @TypDokumentu";
        }
    }
}