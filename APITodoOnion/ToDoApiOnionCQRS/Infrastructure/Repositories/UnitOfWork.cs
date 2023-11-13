﻿using Domain.Dtos.User.RefreshToken;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private IDbContextTransaction _currentTransaction;
        public UnitOfWork(IRepository<User> userRepository, IRepository<Todo> todoRepository, ApplicationDbContext context, ILogger<UnitOfWork> logger, IRepository<RefreshToken> refreshTokenRepository)
        {
            UserRepository = userRepository;
            TodoRepository = todoRepository;
            RefreshTokenRepository = refreshTokenRepository;
            _context = context;
            _logger = logger;
        }

        public IRepository<User> UserRepository { get; }
        public IRepository<Todo> TodoRepository  { get; }
        public IRepository<RefreshToken> RefreshTokenRepository { get; set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public bool SaveChanges()
        {
            try
            {
                _logger.LogInformation("Commiting changes.");

                var saved = _context.SaveChanges();

                _logger.LogInformation("Saved changes: {saved}", saved);

                return saved > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception has ocurred {Message}.", ex.Message);

        
                throw new InfrastructureException(ex.Message);
            }
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation("Commiting changes.");

                var saved = await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Saved changes: {saved}", saved);

                return saved > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception has ocurred {Message}.", ex.Message);

             
                throw new InfrastructureException(ex.Message);
            }
        }
        public async Task BeginTransactionAsync()
        {
            _currentTransaction ??= await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            await _context.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                try
                {
                    await SaveChangesAsync(cancellationToken);
                    _currentTransaction?.Commit();
                }
                finally
                {
                    if (_currentTransaction != null)
                    {
                        _currentTransaction.Dispose();
                        _currentTransaction = null;
                    }
                }
            });
        }
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        public async Task<T> ExecuteWithinTransactionAsync<T>(Func<Task<T>> action)
        {
            return await _context.Database.CreateExecutionStrategy().ExecuteAsync(action);
        }




    }
}
