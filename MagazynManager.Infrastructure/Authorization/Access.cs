using System;

namespace MagazynManager.Infrastructure.Authorization
{
    [Flags]
    public enum Access
    {
        [AccessLetter('n')]
        None = 0,

        [AccessLetter('l')]
        List = 0b1,

        [AccessLetter('r')]
        Read = 0b10,

        [AccessLetter('c')]
        Create = 0b100,

        [AccessLetter('u')]
        Update = 0b1000,

        [AccessLetter('d')]
        Delete = 0b10000,

        [AccessLetter('a')]
        Admin = 0b100000
    }
}