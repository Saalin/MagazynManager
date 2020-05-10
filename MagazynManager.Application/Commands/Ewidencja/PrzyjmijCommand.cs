using MagazynManager.Infrastructure.InputModel.Ewidencja;
using MediatR;
using System;

namespace MagazynManager.Application.Commands.Ewidencja
{
    public class PrzyjmijCommand : IRequest<Guid>
    {
        public PrzyjecieCreateModel Model { get; }
        public Guid PrzedsiebiorstwoId { get; }

        public PrzyjmijCommand(Guid przedsiebiorstwoId, PrzyjecieCreateModel model)
        {
            Model = model;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}