using MagazynManager.Application.Commands.Slowniki;
using MagazynManager.Application.QueryHandlers;
using MagazynManager.Domain.Entities.Produkty;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Slowniki
{
    [CommandHandler]
    public class JednostkaMiaryCommandHandler : IRequestHandler<JednostkaMiaryCreateCommand, Guid>,
        IRequestHandler<JednostkaMiaryDeleteCommand, Unit>
    {
        private readonly IJednostkaMiaryRepository _repository;

        public JednostkaMiaryCommandHandler(IJednostkaMiaryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(JednostkaMiaryCreateCommand request, CancellationToken cancellationToken)
        {
            return await _repository.Save(new JednostkaMiary(request.Name, request.PrzedsiebiorstwoId));
        }

        public async Task<Unit> Handle(JednostkaMiaryDeleteCommand request, CancellationToken cancellationToken)
        {
            await _repository.Delete(request.JednostkaMiaryId);
            return Unit.Value;
        }
    }
}