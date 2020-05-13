using MagazynManager.Application.Commands.Slowniki;
using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Domain.Specification.Specifications;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Slowniki
{
    [CommandHandler]
    public class ProduktCommandHandler : IRequestHandler<ProduktCreateCommand, Guid>,
        IRequestHandler<ProduktDeleteCommand, Unit>
    {
        private readonly ISlownikRepository<Produkt> _produktRepository;
        private readonly ISlownikRepository<Kategoria> _kategoriaRepository;
        private readonly ISlownikRepository<JednostkaMiary> _jednostkaMiaryRepository;

        public ProduktCommandHandler(ISlownikRepository<Produkt> produktRepository, ISlownikRepository<Kategoria> kategoriaRepository, ISlownikRepository<JednostkaMiary> jednostkaMiaryRepository)
        {
            _produktRepository = produktRepository;
            _kategoriaRepository = kategoriaRepository;
            _jednostkaMiaryRepository = jednostkaMiaryRepository;
        }

        public async Task<Guid> Handle(ProduktCreateCommand request, CancellationToken cancellationToken)
        {
            var kategorie = await _kategoriaRepository.GetList(new PrzedsiebiorstwoIdSpecification<Kategoria>(request.PrzedsiebiorstwoId));
            var kategoria = kategorie.FirstOrDefault(x => x.Nazwa == request.Kategoria);

            if (kategoria == null)
            {
                kategoria = new Kategoria(request.Kategoria, request.PrzedsiebiorstwoId);
                await _kategoriaRepository.Save(kategoria);
            }

            var jednostkiMiary = await _jednostkaMiaryRepository.GetList(new PrzedsiebiorstwoIdSpecification<JednostkaMiary>(request.PrzedsiebiorstwoId));
            var jednostkaMiary = jednostkiMiary.FirstOrDefault(x => x.Nazwa == request.JednostkaMiary);

            if (jednostkaMiary == null)
            {
                jednostkaMiary = new JednostkaMiary(request.JednostkaMiary, request.PrzedsiebiorstwoId);
                await _jednostkaMiaryRepository.Save(jednostkaMiary);
            }

            var produkt = new Produkt
            {
                Id = Guid.NewGuid(),
                Skrot = request.ShortName,
                Nazwa = request.Name,
                Kategoria = kategoria,
                JednostkaMiary = jednostkaMiary,
                MagazynId = request.MagazynId
            };

            return await _produktRepository.Save(produkt);
        }

        public async Task<Unit> Handle(ProduktDeleteCommand request, CancellationToken cancellationToken)
        {
            var produkt = await _produktRepository.GetList(new IdSpecification<Produkt, Guid>(request.ProduktId));
            await _produktRepository.Delete(produkt.Single());
            return Unit.Value;
        }
    }
}