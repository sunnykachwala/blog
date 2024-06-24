namespace Np.DAL.Repository
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Np.DAL.Context;
    using System.Linq.Expressions;

    public class BaseRepository<T> : IDisposable, IBaseRepository<T> where T : class
    {
        private readonly NewsPortalContext context;
        private readonly DbSet<T> dbSet;

        public BaseRepository(NewsPortalContext entities)
        {
            this.context = entities;
            this.dbSet = this.context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll() => this.dbSet.AsEnumerable<T>();

        public IQueryable<T> GetAllCustom() => this.dbSet.AsQueryable<T>();

        public IQueryable<T> GetAllIncluding(
          params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> source = this.dbSet.AsQueryable<T>();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                source = source.Include<T, object>(includeProperty);
            return source;
        }

        public IEnumerable<T> GetFindBy(Expression<Func<T, bool>> predicate) => this.dbSet.Where<T>(predicate).AsNoTracking().AsEnumerable<T>();

        public virtual async Task<T?> GetFindByColumnAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T?> GetFindByColumnWithTrackAsync(Expression<Func<T, bool>> predicate) => await this.dbSet.FirstOrDefaultAsync<T>(predicate);
        public async Task<bool> DataExists(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.AnyAsync(predicate);
        }

        public virtual T GetById(object id)
        {
            return dbSet.Find(new object[1] { id });
        }
        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(new object[1] { id });
        }
        public IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters) => (IEnumerable<T>)this.context.Database.SqlQueryRaw<T>(query, parameters);

        public virtual void Insert(T entity) => this.dbSet.Add(entity);

        public virtual void Delete(T entity) => this.dbSet.Remove(entity);

        public virtual void Edit(T entity) => this.context.Entry<T>(entity).State = EntityState.Modified;

        public virtual int ExecuteSqlQuery(string sql)
        {
            SqlConnection connection = new SqlConnection(this.context.Database.GetConnectionString());
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            sqlCommand.Connection.Open();
            int num = (int)sqlCommand.ExecuteScalar();
            sqlCommand.Connection.Close();
            return num;
        }

        public virtual void Save() => this.context.SaveChanges();

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            this.context.Dispose();
        }

        public void SaveAudited(Guid userId, int ActivityLogId)
        {
            this.context.AuditSaveChanges(userId, ActivityLogId);
        }
    }
}
