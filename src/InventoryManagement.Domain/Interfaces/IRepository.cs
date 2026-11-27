namespace InventoryManagement.Domain.Interfaces
{
    public interface IRepository<T>
    {
        public void Add(T item);
        public void Remove(T item);
        public IReadOnlyCollection<T> GetAll();
        public T? GetById(Guid id);
        public void Update(T item);
    }
}
