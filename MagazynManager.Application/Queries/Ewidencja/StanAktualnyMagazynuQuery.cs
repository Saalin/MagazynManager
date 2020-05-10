using MagazynManager.Infrastructure.Dto.Ewidencja;
using MediatR;
using System;
using System.Collections.Generic;

namespace MagazynManager.Application.Queries.Ewidencja
{
    public class StanAktualnyMagazynuQuery : IRequest<List<StanAktualnyDto>>
    {
        public Guid MagazynId { get; }
        public Guid PrzedsiebiorstwoId { get; }

        public StanAktualnyMagazynuQuery(Guid magazynId, Guid przedsiebiorstwoId)
        {
            MagazynId = magazynId;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}