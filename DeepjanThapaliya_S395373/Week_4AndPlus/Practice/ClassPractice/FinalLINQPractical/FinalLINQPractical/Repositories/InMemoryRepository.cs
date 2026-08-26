using System;
using System.Collections.Generic;
using System.Text;
using FinalLINQPractical.Models;
using FinalLINQPractical.Interfaces;
namespace FinalLINQPractical.Repositories
{
    internal class InMemoryRepository<T> where T: IEntity
    {
        private readonly List<T> _products = new List<T>();

        public  void Add(T item)
        {
            _products.Add(item);
        }
        
        public T? GetById(int id)
        {
            return _products.FirstOrDefault(product => product.Id == id);
        }

        public bool Exists(int id)
        {
            return (_products.Any(product => product.Id == id));
            
        }

        public IReadOnlyList<T> GetAll()
        {
            return _products;
        }

        public int Count()
        {
            return _products.Count();
        }public bool RemoveById(int id)
        {
            T? itemtoRemove = GetById(id);
            if(itemtoRemove == null)
            {
                return false;
            }
            else
            {
                _products.Remove(itemtoRemove);
                return true;
            }
        }


    }

    
}
