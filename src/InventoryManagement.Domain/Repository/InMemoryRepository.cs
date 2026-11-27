using InventoryManagement.Domain.Interfaces;

namespace InventoryManagement.Domain.Repository
{
    public class InMemoryRepository<T> : IRepository<T> where T : IEntity
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

        public T? GetById(Guid id)
        {
            return _items.Find(item => item.Id == id);
        }

        public void Update(T item)
        {
            var index = _items.FindIndex(x => x.Id == item.Id);

            if (index == -1)
                throw new InvalidOperationException(
                    $"Item with ID {item.Id} was not found.");

            _items[index] = item;
        }
    }
}
