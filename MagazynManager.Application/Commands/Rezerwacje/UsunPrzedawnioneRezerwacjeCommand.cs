using MediatR;
using System;

namespace MagazynManager.Application.Commands.Rezerwacje
{
    public class UsunPrzedawnioneRezerwacjeCommand : IRequest<Unit>
    {
        public Guid PrzedsiebiorstwoId { get; }

        public UsunPrzedawnioneRezerwacjeCommand(Guid przedsiebiorstwoId)
        {
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}