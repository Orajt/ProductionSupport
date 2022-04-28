namespace Application.Interfaces
{
    public interface IStuffRepository : IGenericRepository<Domain.Stuff>
    {
         Task<bool> IsStuffNameUnique(string name);
    }
}