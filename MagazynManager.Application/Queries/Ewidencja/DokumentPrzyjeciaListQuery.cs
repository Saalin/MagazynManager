using MagazynManager.Domain.Entities.Dokumenty;
using MediatR;
using System;
using System.Collections.Generic;

namespace MagazynManager.Application.Queries.Ewidencja
{
    public class DokumentPrzyjeciaListQuery : IRequest<List<Dokument>>
    {
        public Guid PrzedsiebiorstwoId { get; }

        public DokumentPrzyjeciaListQuery(Guid przedsiebiorstwoId)
        {
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}