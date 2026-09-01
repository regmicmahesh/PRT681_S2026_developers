using CleanCartSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using CleanCartSystem.Common;

namespace CleanCartSystem.Repositories
{
    internal class InMemoryRepository<T> where T : IEntity
    {
        private readonly List<T> _items = new List<T>();

        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Add(item);
        }

        public T? GetById(int id)
        {
            return _items.FirstOrDefault(item => item.Id == id);
        }

        public bool RemoveById(int id)
        {
            T? itemToRemove = GetById(id);
            if (itemToRemove == null)
            {
                return false;
            }

            return _items.Remove(itemToRemove);
        }

        public void PrintAll() //for now print is done by repo..
                               //later we can make another service if needed.
        {
            foreach (T item in _items)
            {
                Console.WriteLine($"{item}");
            }

        }
    }
}
