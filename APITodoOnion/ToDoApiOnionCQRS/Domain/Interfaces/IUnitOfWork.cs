using Domain.Dtos.User.RefreshToken;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User>  UserRepository { get; }
        IRepository<Todo> TodoRepository { get; }
        IRepository<RefreshToken> RefreshTokenRepository { get; }
        bool SaveChanges();
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
        //
        public Task BeginTransactionAsync();
        public  Task CommitTransactionAsync(CancellationToken cancellationToken);
        public void RollbackTransaction();
        public Task<T> ExecuteWithinTransactionAsync<T>(Func<Task<T>> action);

    }
}
