using System.Linq.Expressions;
using Infrastructure.DataBase;
using Infrastructure.DataBase.Models;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BaseRepository<TEntity>
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

        public virtual TEntity? Get(int id) => Context.Find<TEntity>(id);

        public IEnumerable<TEntity> Get()
        {
            return WriteEntity;
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate) =>
            WriteEntity.Where(predicate);

        public virtual async Task<TEntity?> GetAsync(int id) =>
            await Context.FindAsync<TEntity>(id);

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
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
            Context.Detach(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
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
    }
}