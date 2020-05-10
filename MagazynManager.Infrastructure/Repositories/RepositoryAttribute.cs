using System;

namespace MagazynManager.Infrastructure.Repositories
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class RepositoryAttribute : Attribute
    {
    }
}
