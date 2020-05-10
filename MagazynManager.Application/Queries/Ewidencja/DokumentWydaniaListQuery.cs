using MagazynManager.Domain.Entities.Dokumenty;
using MediatR;
using System;
using System.Collections.Generic;

namespace MagazynManager.Application.Queries.Ewidencja
{
    public class DokumentWydaniaListQuery : IRequest<List<Dokument>>
    {
        public Guid PrzedsiebiorstwoId { get; }

        public DokumentWydaniaListQuery(Guid przedsiebiorstwoId)
        {
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}