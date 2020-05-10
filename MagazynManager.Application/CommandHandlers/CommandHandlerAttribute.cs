using System;

namespace MagazynManager.Application.CommandHandlers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandHandlerAttribute : Attribute
    {
    }
}
