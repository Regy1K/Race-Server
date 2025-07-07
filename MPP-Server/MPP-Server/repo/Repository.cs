using log4net;

namespace MPP_Server.repo
{
    public abstract class Repository<ID, T> : IRepository<ID, T>
    {
        protected static readonly ILog log = LogManager.GetLogger("DBRepo");
        
        public abstract bool Add(ID id, T entity);
        public abstract bool Remove(ID id);
        public abstract T? Find(ID id);
        public abstract ICollection<T> GetAll();
        public abstract bool Update(ID id, T newEntity);
    }
}