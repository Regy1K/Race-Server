namespace MPP_Server.repo.db
{
    public abstract class DBRepo<ID, T> : Repository<ID, T>
    {
        protected readonly string url;

        protected DBRepo(string url)
        {
            this.url = url ?? throw new ArgumentNullException(nameof(url));
        }
    }
}