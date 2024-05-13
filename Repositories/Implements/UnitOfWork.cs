using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repositories.Interfaces;

namespace Repositories.Implements;

public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : BeanFastContext
{
    public TContext Context { get; }
    private Dictionary<Type, object>? _repositories;
    private IMapper _mapper { get; set; }
    public UnitOfWork(TContext context, IMapper mapper)
    {
        Context = context;
        _mapper = mapper;
    }

    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
    {
        _repositories ??= new Dictionary<Type, object>();
        if (_repositories.TryGetValue(typeof(TEntity), out var repository))
        {
            return (IGenericRepository<TEntity>)repository;
        }

        repository = new GenericRepository<TEntity>(Context, _mapper);
        _repositories.Add(typeof(TEntity), repository);
        return (IGenericRepository<TEntity>)repository;
    }

    public void Dispose()
    {
        Context?.Dispose();
    }

    public int Commit()
    {
        TrackChanges();
        return Context.SaveChanges();
    }

    public async Task<int> CommitAsync()
    {
        TrackChanges();
        return await Context.SaveChangesAsync();
    }

    private void TrackChanges()
    {
        var validationErrors = Context.ChangeTracker.Entries<IValidatableObject>()
            .SelectMany(e => e.Entity.Validate(null))
            .Where(e => e != ValidationResult.Success)
            .ToArray();
        if (validationErrors.Any())
        {
            var exceptionMessage = string.Join(Environment.NewLine,
                validationErrors.Select(error => $"Properties {error.MemberNames} Error: {error.ErrorMessage}"));
            throw new Exception(exceptionMessage);
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        var transaction = await Context.Database.BeginTransactionAsync();
        
        return transaction;
    }
    public async Task CommitTransactionAsync()
    {
        await Context.Database.CommitTransactionAsync();
    }
}