using System;

namespace MagazynManager.Domain.DomainServices
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DomainServiceAttribute : Attribute
    {
    }
}
