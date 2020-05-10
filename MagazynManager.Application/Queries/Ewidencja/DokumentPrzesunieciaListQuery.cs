using MagazynManager.Domain.Entities.Dokumenty;
using MediatR;
using System;
using System.Collections.Generic;

namespace MagazynManager.Application.Queries.Ewidencja
{
    public class DokumentPrzesunieciaListQuery : IRequest<List<Dokument>>
    {
        public Guid PrzedsiebiorstwoId { get; }

        public DokumentPrzesunieciaListQuery(Guid przedsiebiorstwoId)
        {
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}