using System;

namespace MagazynManager.Domain.Entities.Slowniki
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class StawkaVatWartoscAttribute : Attribute
    {
        public decimal Stawka { get; }

        public StawkaVatWartoscAttribute(int stawka)
        {
            Stawka = (decimal)stawka / 100;
        }
    }
}