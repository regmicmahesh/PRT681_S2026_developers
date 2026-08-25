using MiniCartSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCartSystem.Repositries
{
    internal class InMemoryRepository<T> where T:IEntity
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
            foreach(T item in _items)
            {
                if(item.Id == id)
                {
                    return item;
                }
                
            }
            return default;
        }

        public void RemoveById(int id)
        {
            T itemToRemove = GetById(id);
            Console.WriteLine($"\nItem to remove {itemToRemove}");

            if (itemToRemove == null)
            {
                throw new ArgumentNullException("There is no item to remove.");
            }
            else
            {
                _items.Remove(itemToRemove);
                Console.WriteLine("Item removed Successfully\n");
            }       
        }

        

        public void PrintAll()
        {
            foreach(T item in _items)
            {
                Console.WriteLine($"{item.ToString()}");
            }
        }
    }
}
