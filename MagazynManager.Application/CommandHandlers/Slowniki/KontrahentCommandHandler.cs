using MagazynManager.Application.Commands.Slowniki;
using MagazynManager.Domain.Entities.Kontrahent;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Slowniki
{
    [CommandHandler]
    public class KontrahentCommandHandler : IRequestHandler<KontrahentCreateCommand, Guid>,
        IRequestHandler<KontrahentDeleteCommand, Unit>
    {
        private readonly IKontrahentRepository _kontrahentRepository;

        public KontrahentCommandHandler(IKontrahentRepository kontrahentRepository)
        {
            _kontrahentRepository = kontrahentRepository;
        }

        public async Task<Unit> Handle(KontrahentDeleteCommand request, CancellationToken cancellationToken)
        {
            await _kontrahentRepository.Delete(request.KontrahentId);
            return Unit.Value;
        }

        public async Task<Guid> Handle(KontrahentCreateCommand request, CancellationToken cancellationToken)
        {
            var kontrahent = new Kontrahent
            {
                Id = Guid.NewGuid(),
                DaneAdresowe = new DaneAdresowe
                {
                    Miejscowosc = request.Model.DaneAdresowe.Miejscowosc,
                    Ulica = request.Model.DaneAdresowe.Ulica,
                    KodPocztowy = request.Model.DaneAdresowe.KodPocztowy
                },
                Nazwa = request.Model.Nazwa,
                Nip = request.Model.Nip,
                Skrot = request.Model.Skrot,
                TypKontrahenta = request.Model.TypKontrahenta,
                PrzedsiebiorstwoId = request.PrzedsiebiorstwoId
            };

            return await _kontrahentRepository.Save(kontrahent);
        }
    }
}