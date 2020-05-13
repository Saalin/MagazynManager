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
    public class KategoriaCommandHandler : IRequestHandler<KategoriaCreateCommand, Guid>,
        IRequestHandler<KategoriaDeleteCommand, Unit>
    {
        private readonly ISlownikRepository<Kategoria> _repository;

        public KategoriaCommandHandler(ISlownikRepository<Kategoria> repository)
        {
            _repository = repository;
        }

        public Task<Guid> Handle(KategoriaCreateCommand request, CancellationToken cancellationToken)
        {
            return _repository.Save(new Kategoria(request.Name, request.PrzedsiebiorstwoId));
        }

        public async Task<Unit> Handle(KategoriaDeleteCommand request, CancellationToken cancellationToken)
        {
            var kategoria = await _repository.GetList(new IdSpecification<Kategoria, Guid>(request.Id));
            await _repository.Delete(kategoria.Single());

            return Unit.Value;
        }
    }
}