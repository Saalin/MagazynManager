namespace MagazynManager.Infrastructure.Authorization
{
    public enum AppArea
    {
        [PermissionName("Administracja")]
        Administracja,

        [PermissionName("Slowniki")]
        Slowniki,

        [PermissionName("Ewidencjonowanie")]
        Ewidencjonowanie,

        [PermissionName("Rezerwacje")]
        Rezerwacje
    }
}