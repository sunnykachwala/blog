namespace Np.DAL.Repository
{
    using System.Linq.Expressions;
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        IQueryable<T> GetAllCustom();

        IEnumerable<T> GetFindBy(Expression<Func<T, bool>> predicate);
        Task<T?> GetFindByColumnAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAllIncluding(
          params Expression<Func<T, object>>[] includeProperties);

        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        Task<T?> GetFindByColumnWithTrackAsync(Expression<Func<T, bool>> predicate);
        IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters);

        void Insert(T entity);

        void Delete(T entity);

        void Edit(T entity);

        int ExecuteSqlQuery(string sql);

        void Save();
        void SaveAudited(Guid userId, int ActivityLogId);
    }
}
