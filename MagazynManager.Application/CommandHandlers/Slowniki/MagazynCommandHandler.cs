using MagazynManager.Application.Commands.Slowniki;
using MagazynManager.Domain.Entities.StukturaOrganizacyjna;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Slowniki
{
    [CommandHandler]
    public class MagazynCommandHandler : IRequestHandler<MagazynCreateCommand, Guid>,
        IRequestHandler<MagazynDeleteCommand, Unit>
    {
        private readonly IMagazynRepository _repository;

        public MagazynCommandHandler(IMagazynRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(MagazynDeleteCommand request, CancellationToken cancellationToken)
        {
            await _repository.Delete(request.MagazynId);
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