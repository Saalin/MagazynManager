using System;
using System.Collections.Generic;

namespace MagazynManager.Domain.Entities.Rezerwacje
{
    public class Rezerwacja : BaseEntity<Guid>
    {
        public Guid PrzedsiebiorstwoId { get; set; }
        public Guid UzytkownikRezerwujacyId { get; set; }
        public DateTime DataRezerwacji { get; set; }
        public DateTime DataWaznosci { get; set; }
        public string Opis { get; set; }
        public List<PozycjaRezerwacji> PozycjeRezerwacji { get; }
        public Guid? DokumentWydaniaId { get; set; }

        public Rezerwacja()
        {
            Id = Guid.NewGuid();
            PozycjeRezerwacji = new List<PozycjaRezerwacji>();
        }

        public Rezerwacja(Guid id)
        {
            Id = id;
            PozycjeRezerwacji = new List<PozycjaRezerwacji>();
        }

        public void DodajPozycjeRezerwacji(PozycjaRezerwacji pozycjaRezerwacji)
        {
            PozycjeRezerwacji.Add(pozycjaRezerwacji);
        }

        public bool CzyPrzedawniona(DateTime aktualnyCzas)
        {
            return DataWaznosci < aktualnyCzas && !DokumentWydaniaId.HasValue;
        }

        public bool Zreazlizowana => DokumentWydaniaId.HasValue;
    }
}