using System;

namespace MagazynManager.Application.QueryHandlers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class QueryHandlerAttribute : Attribute
    {
    }
}