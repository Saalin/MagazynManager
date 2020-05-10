using System;

namespace MagazynManager.Infrastructure.Authorization
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class AccessLetterAttribute : Attribute
    {
        public char Letter { get; set; }

        public AccessLetterAttribute(char letter)
        {
            Letter = letter;
        }
    }
}