using MagazynManager.Application.Commands.Slowniki;
using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.StukturaOrganizacyjna;
using MagazynManager.Domain.Specification.Specifications;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Slowniki
{
    [CommandHandler]
    public class MagazynCommandHandler : IRequestHandler<MagazynCreateCommand, Guid>,
        IRequestHandler<MagazynDeleteCommand, Unit>
    {
        private readonly ISlownikRepository<Magazyn> _repository;

        public MagazynCommandHandler(ISlownikRepository<Magazyn> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(MagazynDeleteCommand request, CancellationToken cancellationToken)
        {
            var magazyn = await _repository.GetList(new IdSpecification<Magazyn, Guid>(request.MagazynId));
            await _repository.Delete(magazyn.Single());
            return Unit.Value;
        }

        public async Task<Guid> Handle(MagazynCreateCommand request, CancellationToken cancellationToken)
        {
            var magazyn = new Magazyn
            {
                Id = Guid.NewGuid(),
                PrzedsiebiorstwoId = request.PrzedsiebiorstwoId,
                Nazwa = request.Nazwa,
                Skrot = request.Skrot
            };

            await _repository.Save(magazyn);

            return magazyn.Id;
        }
    }
}