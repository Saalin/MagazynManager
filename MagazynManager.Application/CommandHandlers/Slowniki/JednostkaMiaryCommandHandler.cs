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
    public class JednostkaMiaryCommandHandler : IRequestHandler<JednostkaMiaryCreateCommand, Guid>,
        IRequestHandler<JednostkaMiaryDeleteCommand, Unit>
    {
        private readonly ISlownikRepository<JednostkaMiary> _repository;

        public JednostkaMiaryCommandHandler(ISlownikRepository<JednostkaMiary> repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(JednostkaMiaryCreateCommand request, CancellationToken cancellationToken)
        {
            return await _repository.Save(new JednostkaMiary(request.Name, request.PrzedsiebiorstwoId));
        }

        public async Task<Unit> Handle(JednostkaMiaryDeleteCommand request, CancellationToken cancellationToken)
        {
            var jednostkiMiary = await _repository.GetList(new IdSpecification<JednostkaMiary, Guid>(request.JednostkaMiaryId));
            await _repository.Delete(jednostkiMiary.Single());
            return Unit.Value;
        }
    }
}