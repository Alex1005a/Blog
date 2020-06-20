using Blog.Contracts;
using Blog.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        public RepositoryBase(ApplicationDbContext ApplicationDbContext)
        {
            this.ApplicationDbContext = ApplicationDbContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.ApplicationDbContext.Set<T>();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.ApplicationDbContext.Set<T>().Where(expression);
        }

        public void Create(T entity)
        {
            this.ApplicationDbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.ApplicationDbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.ApplicationDbContext.Set<T>().Remove(entity);
        }
    }
}
