using MediatR;
using System;

namespace MagazynManager.Application.Commands.Slowniki
{
    public class KategoriaDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; }

        public KategoriaDeleteCommand(Guid id)
        {
            Id = id;
        }
    }
}