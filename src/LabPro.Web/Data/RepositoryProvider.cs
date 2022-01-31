
using LabPro.Web.Models;

namespace LabPro.Web.Data
{
    public class RepositoryProvider
    {
        private LabProContext _context { get; set; }

        private Dictionary<Type, object> _repositories { get; set; }

        public RepositoryProvider(LabProContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }


        public virtual RepositoryBase<T, U> GetCustomRepository<T, U>() where U : struct where T : DatabaseEntity<U>
        {
            _repositories.TryGetValue(typeof(T), out object repository);

            if (repository != null)
            {
                return (RepositoryBase<T, U>)repository;
            }

            return CreateRepository<T, U>();
        }

        private RepositoryBase<T, U> CreateRepository<T, U>() where U : struct where T : DatabaseEntity<U>
        {

            var repository = new RepositoryBase<T, U>(_context);
            _repositories[typeof(T)] = repository;

            return repository;
        }
    }
}
