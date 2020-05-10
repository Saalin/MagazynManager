using Dapper;
using MagazynManager.Domain.Entities.Uzytkownicy;
using MagazynManager.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MagazynManager.Infrastructure.Specifications
{
    public class PrzedsiebiorstwoSpecification : Specification<User>
    {
        private readonly string _email;

        public PrzedsiebiorstwoSpecification(string email)
        {
            _email = email;
        }

        public override IEnumerable<Action<DynamicParameters>> GetDynamicParameters()
        {
            yield return x => x.Add("@Email", _email);
        }

        public override Expression<Func<User, bool>> ToExpression()
        {
            return entity => entity.Email == _email;
        }

        public override string ToSql()
        {
            return "Email = @Email";
        }
    }
}