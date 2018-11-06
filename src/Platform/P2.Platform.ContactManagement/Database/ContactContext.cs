namespace P2.Platform.ContactManagement.Database
{
    using Infrastructure.Sql;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class ContactContext : DbContext
    {
        public ContactContext(DbContextOptions<ContactContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<Address> Address { get; set; }

        public T Find<T>(Guid id) where T : class
        {
            return this.Set<T>().Find(id);
        }

        public T Find<T>(int id) where T : class
        {
            return this.Set<T>().Find(id);
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return this.Set<T>();
        }

        public void Save<T>(T entity) where T : class
        {
            var entry = this.Entry(entity);

            if (entry.State == EntityState.Detached)
                this.Set<T>().Add(entity);

            this.SaveChanges();
        }

        public void Delete<T>(T entity) where T : class
        {
            this.Set<T>().Remove(entity);

            this.SaveChanges();
        }

        public IQueryable<T> Filter<T>(Expression<Func<T, bool>> filter, string orderBy,
            out int count, out int total, bool ascending = true, int skip = 0, int top = 50) where T : class
        {
            //int skipCount = index * size;
            int skipCount = skip;
            var resetSet = filter != null ? this.Set<T>().Where(filter).AsQueryable() :
                this.Set<T>().AsQueryable();

            total = resetSet.Count();

            resetSet = skipCount == 0 ? resetSet.OrderByField(orderBy, ascending).Take(top) :
                resetSet.OrderByField(orderBy, ascending).Skip(skipCount).Take(top);

            count = resetSet.Count();

            return resetSet.AsNoTracking().AsQueryable();
        }
    }
}
