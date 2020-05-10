using System;

namespace MagazynManager.Infrastructure.InputModel.Slowniki
{
    public class ProduktCreateModel
    {
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string JednostkaMiary { get; set; }
        public string Kategoria { get; set; }
        public Guid MagazynId { get; set; }
    }
}