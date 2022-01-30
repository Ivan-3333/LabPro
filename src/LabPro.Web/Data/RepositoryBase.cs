using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LabPro.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace LabPro.Web.Data
{
    public class RepositoryBase<T, U> where U : struct where T : DatabaseEntity<U>
    {
        private readonly LabProContext context;

        public RepositoryBase(LabProContext context)
        {
            this.context = context;
        }

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedEnumerable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                return orderBy(query).AsQueryable();
            }

            return query;
        }

        public virtual IQueryable<T> GetActive(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedEnumerable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = context.Set<T>()
                .Where(q => q.Status == DatabaseEntityStatus.Active);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                return orderBy(query).AsQueryable();
            }

            return query;
        }

        public virtual IQueryable<T> ReadAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedEnumerable<T>> orderBy = null)
        {
            IQueryable<T> query = context.Set<T>().AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).AsQueryable();
            }

            return query;
        }

        public virtual IQueryable<T> ReadActive(Expression<Func<T, bool>> filter = null)
        {
            var query = context.Set<T>()
                .Where(e => e.Status == DatabaseEntityStatus.Active)
                .AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public virtual T FindSingle(Guid id)
        {
            return context.Set<T>().Find(id);
        }

        public virtual T FindSingle(int id)
        {
            return context.Set<T>().Find(id);
        }

        public virtual T FindBy(Expression<Func<T, bool>> predicate, string includeProperties = "")
        {
            IQueryable<T> query = context.Set<T>();
            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }
            return query.Where(predicate).FirstOrDefault();
        }

        public virtual void Add(T toAdd)
        {
            context.Set<T>().Add(toAdd);
        }

        public virtual void AddRange(List<T> toAdd)
        {
            context.Set<T>().AddRange(toAdd);
        }

        public virtual void Update(T toUpdate)
        {
            context.Set<T>().Update(toUpdate);
        }

        public virtual void Delete(Guid id)
        {
            T entity = FindSingle(id);
            context.Set<T>().Remove(entity);
        }

        public virtual void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public void Delete(int id)
        {
            T entity = FindSingle(id);
            context.Set<T>().Remove(entity);
        }

        public T FindSingle(U id)
        {
            return context.Set<T>().Find(id);
        }

        public void Delete(U id)
        {
            T entity = FindSingle(id);
            context.Set<T>().Remove(entity);
        }

        public void DeleteBulk(IEnumerable<U> ids)
        {
            var entitiesToDelete = GetActive(e => ids.Contains(e.Id));
            context.Set<T>().RemoveRange(entitiesToDelete);
        }

        public void DeleteBulk(IEnumerable<T> entitiesToDelete)
        {
            context.Set<T>().RemoveRange(entitiesToDelete);
        }
    }
}