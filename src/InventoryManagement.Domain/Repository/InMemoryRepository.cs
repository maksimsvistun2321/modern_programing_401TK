using InventoryManagement.Domain.Interfaces;

namespace InventoryManagement.Domain.Repository
{
    public class InMemoryRepository<T> : IRepository<T>
    {
        private readonly List<T> _items = [];

        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Remove(T item)
        {
            _items.Remove(item);
        }

        public IReadOnlyCollection<T> GetAll()
        {
            return _items;
        }
    }
}
