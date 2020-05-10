using System;

namespace MagazynManager.Domain.Entities.Slowniki
{
    public enum StawkaVat
    {
        [StawkaVatWartosc(0)]
        ZeroProcent = 1,

        [StawkaVatWartosc(5)]
        PiecProcent = 2,

        [StawkaVatWartosc(8)]
        OsiemProcent = 3,

        [StawkaVatWartosc(23)]
        DwadziesciaTrzyProcent = 4,

        [StawkaVatWartosc(0)]
        Zwolniony = 5
    }

    public static class StawkaVatExtensions
    {
        private static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static decimal GetStawkaVat(this StawkaVat stawkaVatEnum)
        {
            return GetAttributeOfType<StawkaVatWartoscAttribute>(stawkaVatEnum).Stawka;
        }
    }
}