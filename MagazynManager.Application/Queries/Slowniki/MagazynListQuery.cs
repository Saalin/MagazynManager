using MagazynManager.Domain.Entities.StukturaOrganizacyjna;
using MediatR;
using System;
using System.Collections.Generic;

namespace MagazynManager.Application.Queries.Slowniki
{
    public class MagazynListQuery : IRequest<List<Magazyn>>
    {
        public Guid PrzedsiebiorstwoId { get; }

        public MagazynListQuery(Guid przedsiebiorstwoId)
        {
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}