﻿using Dapper;
using MagazynManager.Domain.Entities.Uzytkownicy;
using MagazynManager.Domain.Specification.Technical;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MagazynManager.Domain.Specification.Specifications
{
    public class EmailSpecification : Specification<User>
    {
        private readonly string _email;

        public EmailSpecification(string email)
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