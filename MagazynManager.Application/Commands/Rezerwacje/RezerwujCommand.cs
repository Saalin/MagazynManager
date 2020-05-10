using MagazynManager.Infrastructure.InputModel.Rezerwacje;
using MediatR;
using System;

namespace MagazynManager.Application.Commands.Rezerwacje
{
    public class RezerwujCommand : IRequest<Guid>
    {
        public Guid PrzedsiebiorstwoId { get; }
        public RezerwacjaCreateModel Model { get; }
        public Guid UserId { get; set; }

        public RezerwujCommand(Guid przedsiebiorstwoId, Guid userId, RezerwacjaCreateModel model)
        {
            PrzedsiebiorstwoId = przedsiebiorstwoId;
            UserId = userId;
            Model = model;
        }
    }
}