using MagazynManager.Infrastructure.Dto.Rezerwacje;
using MediatR;
using System;
using System.Collections.Generic;

namespace MagazynManager.Application.Queries.Rezerwacje
{
    public class RezerwacjaListQuery : IRequest<List<RezerwacjaDto>>
    {
        public Guid PrzedsiebiorstwoId { get; }

        public RezerwacjaListQuery(Guid przedsiebiorstwoId)
        {
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}