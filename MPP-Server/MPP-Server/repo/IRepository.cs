namespace MPP_Server.repo
{
    public interface IRepository<ID, T>
    {
        bool Add(ID id, T entity);
        bool Update(ID id, T newEntity);
        bool Remove(ID id);
        T? Find(ID id);
        ICollection<T> GetAll();
    }
}