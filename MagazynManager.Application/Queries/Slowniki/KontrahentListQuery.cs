using MagazynManager.Domain.Entities.Kontrahent;
using MediatR;
using System;
using System.Collections.Generic;

namespace MagazynManager.Application.Queries.Slowniki
{
    public class KontrahentListQuery : IRequest<List<Kontrahent>>
    {
        public Guid PrzedsiebiorstwoId { get; }

        public KontrahentListQuery(Guid id)
        {
            PrzedsiebiorstwoId = id;
        }
    }
}