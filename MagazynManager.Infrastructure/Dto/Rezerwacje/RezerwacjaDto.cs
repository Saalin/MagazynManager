using System;
using System.Collections.Generic;

namespace MagazynManager.Infrastructure.Dto.Rezerwacje
{
    public class RezerwacjaDto : BaseDto<Guid>
    {
        public Guid PrzedsiebiorstwoId { get; set; }
        public Guid UzytkownikRezerwujacyId { get; set; }
        public DateTime DataRezerwacji { get; set; }
        public DateTime DataWaznosci { get; set; }
        public string Opis { get; set; }
        public List<PozycjaRezerwacjiDto> PozycjeRezerwacji { get; set; }
        public Guid? DokumentWydaniaId { get; set; }
    }
}