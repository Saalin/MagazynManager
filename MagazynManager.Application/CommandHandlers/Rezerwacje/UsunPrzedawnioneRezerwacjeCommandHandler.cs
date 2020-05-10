using MagazynManager.Application.Commands.Rezerwacje;
using MagazynManager.Domain.Entities.Rezerwacje;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Rezerwacje
{
    [CommandHandler]
    public class UsunPrzedawnioneRezerwacjeCommandHandler : IRequestHandler<UsunPrzedawnioneRezerwacjeCommand, Unit>
    {
        private readonly IRezerwacjaRepository _rezerwacjaRepository;

        public UsunPrzedawnioneRezerwacjeCommandHandler(IRezerwacjaRepository rezerwacjaRepository)
        {
            _rezerwacjaRepository = rezerwacjaRepository;
        }

        public async Task<Unit> Handle(UsunPrzedawnioneRezerwacjeCommand request, CancellationToken cancellationToken)
        {
            var list = await _rezerwacjaRepository.GetList(request.PrzedsiebiorstwoId);

            await Task.WhenAll(list.Where(x => x.CzyPrzedawniona(DateTime.Now)).Select(x => _rezerwacjaRepository.Delete(x)));

            return Unit.Value;
        }
    }
}