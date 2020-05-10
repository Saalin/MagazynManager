using MagazynManager.Infrastructure.Dto.Slowniki;
using MediatR;
using System;
using System.Collections.Generic;

namespace MagazynManager.Application.Queries.Slowniki
{
    public class ProduktListQuery : IRequest<List<ProduktDto>>
    {
        public Guid PrzedsiebiorstwoId { get; }

        public ProduktListQuery(Guid id)
        {
            PrzedsiebiorstwoId = id;
        }
    }
}