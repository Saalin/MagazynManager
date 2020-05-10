namespace MagazynManager.Infrastructure.Dto
{
    public abstract class BaseDto<T>
    {
        public T Id { get; set; }
    }
}