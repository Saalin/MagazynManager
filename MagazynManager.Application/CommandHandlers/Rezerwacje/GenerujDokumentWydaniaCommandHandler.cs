using MagazynManager.Application.Commands.Ewidencja;
using MagazynManager.Application.Commands.Rezerwacje;
using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Domain.Entities.Rezerwacje;
using MagazynManager.Infrastructure.InputModel.Ewidencja;
using MagazynManager.Infrastructure.Specifications;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Rezerwacje
{
    [CommandHandler]
    public class GenerujDokumentWydaniaCommandHandler : IRequestHandler<GenerujDokumentWydaniaCommand, Guid>
    {
        private readonly IRezerwacjaRepository _rezerwacjaRepository;
        private readonly IMediator _mediator;
        private readonly ISlownikRepository<Produkt> _produktRepository;

        public GenerujDokumentWydaniaCommandHandler(IRezerwacjaRepository rezerwacjaRepository, IMediator mediator, ISlownikRepository<Produkt> produktRepository)
        {
            _rezerwacjaRepository = rezerwacjaRepository;
            _mediator = mediator;
            _produktRepository = produktRepository;
        }

        public async Task<Guid> Handle(GenerujDokumentWydaniaCommand request, CancellationToken cancellationToken)
        {
            var rezerwacje = await _rezerwacjaRepository.GetList(request.PrzedsiebiorstwoId);
            var rezerwacja = rezerwacje.Single(x => x.Id == request.RezerwacjaId);
            var produkty = await _produktRepository.GetList(new PrzedsiebiorstwoSpecification<Produkt>(request.PrzedsiebiorstwoId));

            var magazynId = produkty.First(x => x.Id == rezerwacja.PozycjeRezerwacji.First().ProduktId).MagazynId;

            return await _mediator.Send(new WydajCommand(request.PrzedsiebiorstwoId, new WydanieCreateModel
            {
                Data = DateTime.Now,
                MagazynId = magazynId,
                Pozycje = rezerwacja.PozycjeRezerwacji.Select(x => new PozycjaWydaniaModel
                {
                    ProduktId = x.ProduktId,
                    Ilosc = x.Ilosc
                }).ToList()
            }));
        }
    }
}