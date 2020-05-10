using MagazynManager.Application.Commands.Slowniki;
using MagazynManager.Domain.Entities.Produkty;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Slowniki
{
    [CommandHandler]
    public class KategoriaCommandHandler : IRequestHandler<KategoriaCreateCommand, Guid>,
        IRequestHandler<KategoriaDeleteCommand, Unit>
    {
        private readonly IKategoriaRepository _repository;

        public KategoriaCommandHandler(IKategoriaRepository repository)
        {
            _repository = repository;
        }

        public Task<Guid> Handle(KategoriaCreateCommand request, CancellationToken cancellationToken)
        {
            return _repository.Save(new Kategoria(request.Name, request.PrzedsiebiorstwoId));
        }

        public async Task<Unit> Handle(KategoriaDeleteCommand request, CancellationToken cancellationToken)
        {
            await _repository.Delete(request.Id);

            return Unit.Value;
        }
    }
}