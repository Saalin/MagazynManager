using MagazynManager.Infrastructure.Dto.Slowniki;
using MediatR;
using System;
using System.Collections.Generic;

namespace MagazynManager.Application.Queries.Slowniki
{
    public class KategoriaListQuery : IRequest<List<KategoriaDto>>
    {
        public Guid PrzedsiebiorstwoId { get; }

        public KategoriaListQuery(Guid przedsiebiorstwoId)
        {
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}