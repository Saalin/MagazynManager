using MediatR;
using System;

namespace MagazynManager.Application.Commands.Slowniki
{
    public class ProduktCreateCommand : IRequest<Guid>
    {
        public ProduktCreateCommand(string shortName, string name, string jednostkaMiary, string kategoria, Guid magazynId, Guid przedsiebiorstwoId)
        {
            ShortName = shortName;
            Name = name;
            JednostkaMiary = jednostkaMiary;
            Kategoria = kategoria;
            MagazynId = magazynId;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }

        public string ShortName { get; }
        public string Name { get; }
        public string JednostkaMiary { get; }
        public string Kategoria { get; }
        public Guid MagazynId { get; }
        public Guid PrzedsiebiorstwoId { get; }
    }
}