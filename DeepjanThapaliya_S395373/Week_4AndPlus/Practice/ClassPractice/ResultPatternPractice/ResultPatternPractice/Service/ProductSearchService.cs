using System;
using System.Collections.Generic;
using System.Text;
using ResultPatternPractice.Common;
using ResultPatternPractice.Interfaces;

namespace ResultPatternPractice.Service
{
    internal class ProductSearchService<T> where T: IEntity
    {
        public Result<T> FindById(List<T> items, int id)
        {
            if(items == null)
            {
                throw new ArgumentNullException("nameof(products");
            }
            if(id < 1)
            {
                return Result<T>.Failure($"{typeof(T).Name} Id must be greater than zero.");
            }

            T? item = items.FirstOrDefault(item => item.Id == id);

            if(item == null)
            {
                return Result<T>.Failure($"No {typeof(T).Name} is found ");
            }

            return Result<T>.Success(item, "Product Found");

        }
    }
}
