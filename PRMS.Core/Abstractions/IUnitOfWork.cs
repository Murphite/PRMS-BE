namespace PRMS.Core.Abstractions;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync();
}