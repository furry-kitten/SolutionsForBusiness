using System.Linq.Expressions;
using Infrastructure.DataBase;
using Infrastructure.DataBase.Models;
using Infrastructure.Extensions;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity>
        where TEntity : BaseDbModel, new()
    {
        protected readonly EfCoreContext Context;
        //protected readonly EfCoreContext Context = EfCoreContext.Context;
        protected readonly DbSet<TEntity> WriteEntity;

        protected BaseRepository(EfCoreContext context)
        {
            Context = context;
            WriteEntity = Context.Set<TEntity>();
        }

        public virtual TEntity? Get(int id) =>
            WriteEntity.AsNoTracking().FirstOrDefault(model => model.Id == id);

        public virtual IEnumerable<TEntity> Get()
        {
            return WriteEntity.AsNoTracking();
        }

        public virtual IEnumerable<TEntity> Get<TType>(DataFilter<TEntity, TType> filter) =>
            WriteEntity.AsNoTracking().AsEnumerable().Where(filter.Filter);

        public virtual IEnumerable<TEntity> Get<TType>(Expression<Func<TEntity, bool>> predicate) =>
            WriteEntity.AsNoTracking().Where(predicate);

        public virtual TEntity? GetAsync(int id) =>
            Context.Find<TEntity>(id);

        public virtual TEntity Add(TEntity entity)
        {
            WriteEntity.Add(entity);
            Context.SaveChanges();
            Context.Detach(entity);

            return entity;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await WriteEntity.AddAsync(entity);
            await Context.SaveChangesAsync();
            Context.Detach(entity);

            return entity;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }

            Context.SaveChanges();
            Context.DetachAllEntries(entities);
        }

        public virtual void Update(TEntity entity)
        {
            var entityEntry = Context.Entry(entity);
            if (entityEntry.State != EntityState.Modified)
            {
                entityEntry.State = EntityState.Modified;
            }

            Context.SaveChanges();
            Context.Detach(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            var entityEntry = Context.Entry(entity);
            entityEntry.State = EntityState.Modified;
            await Context.SaveChangesAsync();
            Context.Detach(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            WriteEntity.AddRange(entities);
            Context.SaveChanges();
            Context.DetachAllEntries(entities);
        }

        public virtual async Task<int> Remove(TEntity entity)
        {
            WriteEntity.Remove(entity);
            var count = await Context.SaveChangesAsync();
            Context.Detach(entity);
            return count;
        }

        public virtual int RemoveRange(IEnumerable<TEntity> entities)
        {
            WriteEntity.RemoveRange(entities);
            var count = Context.SaveChanges();
            Context.DetachAllEntries(entities);
            return count;
        }

        public virtual async Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            WriteEntity.RemoveRange(entities);
            var count = await Context.SaveChangesAsync();
            Context.DetachAllEntries(entities);
            return count;
        }

        //public abstract TEntity GetByFilter(Expression<Func<Order, bool>> predicate);
    }
}