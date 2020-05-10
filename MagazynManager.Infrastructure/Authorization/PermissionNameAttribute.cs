using System;

namespace MagazynManager.Infrastructure.Authorization
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PermissionNameAttribute : Attribute
    {
        public string Name { get; set; }

        public PermissionNameAttribute(string name)
        {
            Name = name;
        }
    }
}