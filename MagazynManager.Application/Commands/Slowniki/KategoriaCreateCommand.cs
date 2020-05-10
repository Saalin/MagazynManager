using MagazynManager.Infrastructure.InputModel.Slowniki;
using MediatR;
using System;

namespace MagazynManager.Application.Commands.Slowniki
{
    public class KategoriaCreateCommand : IRequest<Guid>
    {
        public string Name { get; }
        public Guid PrzedsiebiorstwoId { get; }

        public KategoriaCreateCommand(KategoriaCreateModel inputModel, Guid przedsiebiorstwoId)
        {
            Name = inputModel.Name;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}