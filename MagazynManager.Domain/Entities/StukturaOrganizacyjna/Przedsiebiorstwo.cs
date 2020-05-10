using System;
using System.Collections.Generic;

namespace MagazynManager.Domain.Entities.StukturaOrganizacyjna
{
    public class Przedsiebiorstwo : BaseEntity<Guid>
    {
        public string Nazwa { get; set; }
        public List<Magazyn> Magazyny { get; set; }

        public Przedsiebiorstwo(Guid id, string nazwa)
        {
            Id = id;
            Nazwa = nazwa;
            Magazyny = new List<Magazyn>();
        }

        public Przedsiebiorstwo(Guid id, string nazwa, List<Magazyn> magazyny)
        {
            Id = id;
            Nazwa = nazwa;
            Magazyny = magazyny;
        }

        public void DodajMagazyn(Magazyn magazyn)
        {
            Magazyny.Add(magazyn);
        }
    }
}