using MagazynManager.Infrastructure.Dto.Slowniki;
using MediatR;
using System;
using System.Collections.Generic;

namespace MagazynManager.Application.Queries.Slowniki
{
    public class JednostkaMiaryListQuery : IRequest<List<JednostkaMiaryDto>>
    {
        public Guid PrzedsiebiorstwoId { get; }

        public JednostkaMiaryListQuery(Guid przedsiebiorstwoId)
        {
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}